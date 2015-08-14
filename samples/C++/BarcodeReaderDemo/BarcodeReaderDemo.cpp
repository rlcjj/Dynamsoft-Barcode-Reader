// BarcodeReaderDemo.cpp : Defines the entry point for the console application.

#include <windows.h>
#include <stdio.h>
#include <conio.h>
#include "../../../Include/If_DBRP.h"
#ifdef _WIN64
#pragma comment(lib, "../../../Lib/DBRx64.lib")
#else
#pragma comment(lib, "../../../Lib/DBRx86.lib")
#endif

struct barcode_format
{
	const char * pszFormat;
	__int64 llFormat;
};

static struct barcode_format Barcode_Formats[] = 
{
	{"CODE_39", CODE_39},
	{"CODE_128", CODE_128},
	{"CODE_93", CODE_93},
	{"CODABAR", CODABAR},
	{"ITF", ITF},
	{"UPC_A", UPC_A},
	{"UPC_E", UPC_E},
	{"EAN_13", EAN_13},
	{"EAN_8", EAN_8},
	{"INDUSTRIAL_25",INDUSTRIAL_25},
	{"OneD", OneD},
	{"QR_CODE", QR_CODE}
};

static struct barcode_format Barcode_Formats_Lowcase[] = 
{
	{"code_39", CODE_39},
	{"code_128", CODE_128},
	{"code_93", CODE_93},
	{"codabar", CODABAR},
	{"itf", ITF},
	{"upc_a", UPC_A},
	{"upc_e", UPC_E},
	{"ean_13", EAN_13},
	{"ean_8", EAN_8},
	{"industrial_25", INDUSTRIAL_25},
	{"oned", OneD},
	{"qr_code", QR_CODE}
};
__int64 GetFormat(const char * pstr)
{
	__int64 llFormat = 0;
	int iCount = sizeof(Barcode_Formats_Lowcase)/sizeof(Barcode_Formats_Lowcase[0]);

	// convert string to a lowercase string
	int iStrlen = strlen(pstr);
	char * pszFormat = new char[iStrlen + 1];
	memset(pszFormat, 0, iStrlen + 1);
	strcpy(pszFormat, pstr);
	strlwr(pszFormat);
	
	for (int index = 0; index < iCount; index ++)
	{
		if (strstr(pszFormat, Barcode_Formats_Lowcase[index].pszFormat) != NULL)
			llFormat |= Barcode_Formats_Lowcase[index].llFormat;
	}
	
	delete[] pszFormat;
	return llFormat;
}

const char * GetFormatStr(__int64 format)
{
	int iCount = sizeof(Barcode_Formats)/sizeof(Barcode_Formats[0]);
	
	for (int index = 0; index < iCount; index ++)
	{
		if (Barcode_Formats[index].llFormat == format)
			return Barcode_Formats[index].pszFormat;
	}
	
	return "UNKNOWN";
}

void PrintHelp()
{
	printf("\r\nUsage: BarcodeReaderDemo_C++.exe [-f format] [-n number] ImageFilePath\r\n\r\n\
-f format: supported barcode formats include {CODE_39;CODE_128;CODE_93;CODABAR;ITF;UPC_A;UPC_E;EAN_13;EAN_8;INDUSTRIAL_25;OneD;QR_CODE}.\r\n\r\n\
-n number: maximum barcodes to read per page.\r\n\r\n\
Press any key to continue . . .");
	getch(); 
}

int main(int argc, const char* argv[])
{
	// Parse command
	__int64 llFormat = (OneD|QR_CODE);
	const char * pszImageFile = NULL;
	int iMaxCount = 0x7FFFFFFF;
	int iIndex = 0;
	ReaderOptions ro = {0};
	int iRet = -1;
	char * pszTemp = NULL;
	char * pszTemp1 = NULL;
	unsigned __int64 ullTimeBegin = 0;
	unsigned __int64 ullTimeEnd = 0;
	
	if (argc <= 1)
	{
		PrintHelp();
		return 1;
	}

	for (iIndex = 1; iIndex < argc; iIndex ++)
	{
		if (strcmpi(argv[iIndex], "-help") == 0)
		{
			PrintHelp();
			return 0;
		}

		if (strcmpi(argv[iIndex], "-f") == 0)
		{// parse format
			iIndex ++;
			if (iIndex >= argc)
			{
				printf("The syntax of the command is incorrect.\r\n");
				return 1;
			}
			llFormat = GetFormat(argv[iIndex]);
			if (llFormat == 0)
			{
				printf("The syntax of the command is incorrect.\r\n");
				return 1;
			}
			continue;
		}

		if (strcmpi(argv[iIndex], "-n") == 0)
		{// parse format
			iIndex ++;
			if (iIndex >= argc)
			{
				printf("The syntax of the command is incorrect.\r\n");
				return 1;
			}
			iMaxCount = atoi(argv[iIndex]);
			continue;
		}

		if (NULL == pszImageFile)
			pszImageFile = argv[iIndex];
	}

	if (NULL == pszImageFile)
	{
		printf("The syntax of the command is incorrect.\r\n");
		return 1;
	}

	// Set license
	CBarcodeReader reader;
	reader.InitLicense("<Put your license key here>");

	// Read barcode
	ullTimeBegin = GetTickCount();
	ro.llBarcodeFormat = llFormat;
	ro.iMaxBarcodesNumPerPage = iMaxCount;
	reader.SetReaderOptions(ro);
	iRet = reader.DecodeFile(pszImageFile);
	ullTimeEnd = GetTickCount();
		
	// Output barcode result
	pszTemp = (char*)malloc(4096);
	if (iRet != DBR_OK)
	{
		sprintf(pszTemp, "Failed to read barcode: %s\r\n", DBR_GetErrorString(iRet));
		printf(pszTemp);
		free(pszTemp);
		return 1;
	}

	pBarcodeResultArray paryResult = NULL;
	reader.GetBarcodes(&paryResult);
	
	if (paryResult->iBarcodeCount == 0)
	{
		sprintf(pszTemp, "No barcode found. Total time spent: %.3f seconds.\r\n", ((float)(ullTimeEnd - ullTimeBegin)/1000));
		printf(pszTemp);
		free(pszTemp);
		reader.FreeBarcodeResults(&paryResult);
		return 0;
	}
	
	sprintf(pszTemp, "Total barcode(s) found: %d. Total time spent: %.3f seconds\r\n\r\n", paryResult->iBarcodeCount, ((float)(ullTimeEnd - ullTimeBegin)/1000));
	printf(pszTemp);
	for (iIndex = 0; iIndex < paryResult->iBarcodeCount; iIndex++)
	{
		sprintf(pszTemp, "Barcode %d:\r\n", iIndex + 1);
		printf(pszTemp);
		sprintf(pszTemp, "    Page: %d\r\n", paryResult->ppBarcodes[iIndex]->iPageNum);
		printf(pszTemp);
		sprintf(pszTemp, "    Type: %s\r\n", GetFormatStr(paryResult->ppBarcodes[iIndex]->llFormat));
		printf(pszTemp);
		pszTemp1 = (char*)malloc(paryResult->ppBarcodes[iIndex]->iBarcodeDataLength + 1);
		memset(pszTemp1, 0, paryResult->ppBarcodes[iIndex]->iBarcodeDataLength + 1);
		memcpy(pszTemp1, paryResult->ppBarcodes[iIndex]->pBarcodeData, paryResult->ppBarcodes[iIndex]->iBarcodeDataLength);
		sprintf(pszTemp, "    Value: %s\r\n", pszTemp1);
		printf(pszTemp);
		free(pszTemp1);
		sprintf(pszTemp, "    Region: {Left: %d, Top: %d, Width: %d, Height: %d}\r\n\r\n", 
			paryResult->ppBarcodes[iIndex]->iLeft, paryResult->ppBarcodes[iIndex]->iTop, 
			paryResult->ppBarcodes[iIndex]->iWidth, paryResult->ppBarcodes[iIndex]->iHeight);
		printf(pszTemp);
	}	

	free(pszTemp);
	reader.FreeBarcodeResults(&paryResult);

	return 0;
}

