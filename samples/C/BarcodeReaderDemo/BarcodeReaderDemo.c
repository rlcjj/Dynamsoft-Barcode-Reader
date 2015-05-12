// BarcodeReaderDemo.cpp : Defines the entry point for the console application.

#include <windows.h>
#include <stdio.h>
#include <conio.h>
#include "../../../Include/If_DBR.h"
#ifdef _WIN64
#pragma comment(lib, "../../../Lib/DBRx64.lib")
#else
#pragma comment(lib, "../../../Lib/DBRx86.lib")
#endif

__int64 GetFormat(const char * pstr)
{
	__int64 llFormat = 0;
	// convert string to a lowercase string
	int iStrlen = strlen(pstr);
	char * pszFormat = (char*)malloc(iStrlen + 1);
	memset(pszFormat, 0, iStrlen + 1);
	strcpy(pszFormat, pstr);
	strlwr(pszFormat);
	
	if (strstr(pszFormat, "code_39") != NULL)
		llFormat |= CODE_39;
	if (strstr(pszFormat, "code_128") != NULL)
		llFormat |= CODE_128;
	if (strstr(pszFormat, "code_93") != NULL)
		llFormat |= CODE_93;
	if (strstr(pszFormat, "codabar") != NULL)
		llFormat |= CODABAR;
	if (strstr(pszFormat, "itf") != NULL)
		llFormat |= ITF;
	if (strstr(pszFormat, "upc_a") != NULL)
		llFormat |= UPC_A;
	if (strstr(pszFormat, "upc_e") != NULL)
		llFormat |= UPC_E;
	if (strstr(pszFormat, "ean_13") != NULL)
		llFormat |= EAN_13;
	if (strstr(pszFormat, "ean_8") != NULL)
		llFormat |= EAN_8;
	if (strstr(pszFormat, "oned") != NULL)
		llFormat = OneD;
	
	free(pszFormat);
	return llFormat;
}

const char * GetFormatStr(__int64 format)
{
	if (format == CODE_39)
		return "CODE_39";
	if (format == CODE_128)
		return "CODE_128";
	if (format == CODE_93)
		return "CODE_93";
	if (format == CODABAR)	
		return "CODABAR";
	if (format == ITF)	
		return "ITF";
	if (format == UPC_A)	
		return "UPC_A";
	if (format == UPC_E)	
		return "UPC_E";
	if (format == EAN_13)	
		return "EAN_13";
	if (format == EAN_8)	
		return "EAN_8";
	
	return "UNKNOWN";
}

void PrintHelp()
{
	printf("\r\nUsage: BarcodeReaderDemo_C.exe [-f format] [-n number] ImageFilePath\r\n\r\n\
-f format: supported barcode formats include {CODE_39;CODE_128;CODE_93;CODABAR;ITF;UPC_A;UPC_E;EAN_13;EAN_8;OneD}.\r\n\r\n\
-n number: maximum barcodes to read per page.\r\n\r\n\
Press any key to continue . . .");
	getch();
}

int main(int argc, const char* argv[])
{
	// Parse command
	__int64 llFormat = OneD;
	const char * pszImageFile = NULL;
	int iMaxCount = 0x7FFFFFFF;
	int iIndex = 0;
	ReaderOptions ro = {0};
	pBarcodeResultArray paryResult = NULL;
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
	DBR_InitLicense("<Put your license key here>");

	// Read barcode
	ullTimeBegin = GetTickCount();
	ro.llBarcodeFormat = llFormat;
	ro.iMaxBarcodesNumPerPage = iMaxCount;
	iRet = DBR_DecodeFile(pszImageFile, &ro, &paryResult);
	ullTimeEnd = GetTickCount();
	
	// Output barcode result
	pszTemp = (char*)malloc(4096);
	if (iRet != DBR_OK)
	{
		sprintf(pszTemp, "Failed to read barcode: %s\r\n", GetErrorString(iRet));
		printf(pszTemp);
		free(pszTemp);
		return 1;
	}
	
	if (paryResult->iBarcodeCount == 0)
	{
		sprintf(pszTemp, "No barcode found. Total time spent: %.3f seconds.\r\n", ((float)(ullTimeEnd - ullTimeBegin)/1000));
		printf(pszTemp);
		free(pszTemp);
		DBR_FreeBarcodeResults(&paryResult);
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
	DBR_FreeBarcodeResults(&paryResult);

	return 0;
}

