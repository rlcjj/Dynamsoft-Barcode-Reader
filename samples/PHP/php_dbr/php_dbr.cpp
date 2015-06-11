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

void SetOptions(pReaderOptions pOption, int option_iMaxBarcodesNumPerPage, int option_llBarcodeFormat){

	if (option_llBarcodeFormat > 0)
		pOption->llBarcodeFormat = option_llBarcodeFormat;
	else
		pOption->llBarcodeFormat = OneD;

	if (option_iMaxBarcodesNumPerPage > 0)
		pOption->iMaxBarcodesNumPerPage = option_iMaxBarcodesNumPerPage;
	else
		pOption->iMaxBarcodesNumPerPage = INT_MAX;

}

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
	int option_iMaxBarcodesNumPerPage = -1;
	int option_llBarcodeFormat = -1;
	pBarcodeResultArray pResults = NULL;
	ReaderOptions option;

	SetOptions(&option, option_iMaxBarcodesNumPerPage, option_llBarcodeFormat);

	// decode barcode image file
	int ret = DBR_DecodeFile(
		pFileName,
		&option,
		&pResults
		);

	if (ret == DBR_OK)
	{
		int count = pResults->iBarcodeCount;
		pBarcodeResult* ppBarcodes = pResults->ppBarcodes;
		pBarcodeResult tmp = NULL;

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