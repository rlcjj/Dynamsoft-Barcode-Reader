// php_dbr.cpp : Defines the exported functions for the DLL application.
#include "php_dbr.h"

#include "If_DBR.h"
#include "BarcodeFormat.h"
#include "BarcodeStructs.h"
#include "ErrorCode.h"

#ifdef _WIN64
#pragma comment(lib, "DBRx64.lib")
#else
#pragma comment(lib, "DBRx86.lib")
#endif

ZEND_FUNCTION(DecodeBarcodeFile);

zend_function_entry CustomExtModule_functions[] = {
    ZEND_FE(DecodeBarcodeFile, NULL)
    {NULL, NULL, NULL}
};

zend_module_entry CustomExtModule_module_entry = {
    STANDARD_MODULE_HEADER,
    "Dynamsoft Barcode Reader",
    CustomExtModule_functions,
    NULL, NULL, NULL, NULL, NULL,
    NO_VERSION_YET, STANDARD_MODULE_PROPERTIES
};

ZEND_GET_MODULE(CustomExtModule)

ZEND_FUNCTION(DecodeBarcodeFile){
	array_init(return_value);

	// Get Barcode image path
	char* pFileName = NULL;
	int iLen = 0;

    if (zend_parse_parameters(ZEND_NUM_ARGS() TSRMLS_CC, "s", &pFileName, &iLen) == FAILURE) {
        RETURN_STRING("Invalid parameters", true);
    }

	// Dynamsoft Barcode Reader: init
	__int64 llFormat = (OneD | QR_CODE | PDF417 | DATAMATRIX);
	int iMaxCount = 0x7FFFFFFF;
	int iIndex = 0;
	ReaderOptions ro = {0};
	pBarcodeResultArray pResults = NULL;
	int iRet = -1;
	char * pszTemp = NULL;

	DBR_InitLicense("license");
	ro.llBarcodeFormat = llFormat;
	ro.iMaxBarcodesNumPerPage = iMaxCount;

	// decode barcode image file
	int ret = DBR_DecodeFile(pFileName, &ro, &pResults);
	printf("ret = %d", ret);
	if (ret == DBR_OK)
	{
		int count = pResults->iBarcodeCount;
		pBarcodeResult* ppBarcodes = pResults->ppBarcodes;
		pBarcodeResult tmp = NULL;
		printf("count = %d", count);
		// loop all results
		for (int i = 0; i < count; i++)
		{
			tmp = ppBarcodes[i];

			// convert format type to string
			char format[64];
			sprintf (format, "%d", tmp->llFormat);

			// (barcode type, result)
			add_assoc_string(return_value, format, tmp->pBarcodeData, 1);
		}

		// Dynamsoft Barcode Reader: release memory
		DBR_FreeBarcodeResults(&pResults);
	}
	else
	{
		RETURN_STRING("No Barcode detected", true);
	}

}
