#include "Python.h"

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

static PyObject *
initLicense(PyObject *self, PyObject *args)
{
	char *license;

	if (!PyArg_ParseTuple(args, "s", &license)) {
		return NULL;
	}

	printf("information: %s\n", license);

	int ret = DBR_InitLicense(license);

	printf("return value = %d", ret);

	return Py_None;
}

static PyObject *
decodeFile(PyObject *self, PyObject *args)
{
	char *pFileName;
	int option_iMaxBarcodesNumPerPage = -1;
	int option_llBarcodeFormat = -1;

	if (!PyArg_ParseTuple(args, "s", &pFileName)) {
		return NULL;
	}

	pBarcodeResultArray pResults = NULL;
	ReaderOptions option;
	SetOptions(&option, option_iMaxBarcodesNumPerPage, option_llBarcodeFormat);

	int ret = DBR_DecodeFile(
		pFileName,
		&option,
		&pResults
		);

	if (ret == DBR_OK){
		int count = pResults->iBarcodeCount;
		pBarcodeResult* ppBarcodes = pResults->ppBarcodes;
		pBarcodeResult tmp = NULL;

		PyObject* list = PyList_New(count);
		PyObject* result = NULL;

		for (int i = 0; i < count; i++)
		{
			tmp = ppBarcodes[i];
			result = PyString_FromString(tmp->pBarcodeData);

			PyList_SetItem(list, i, Py_BuildValue("iN", (int)tmp->llFormat, result));
		}

		// release memory
		DBR_FreeBarcodeResults(&pResults);

		return list;
	}

	return Py_None;
}

static PyMethodDef methods[] = {
	{ "initLicense", initLicense, METH_VARARGS, NULL },
	{ "decodeFile", decodeFile, METH_VARARGS, NULL },
	{ NULL, NULL }
};

PyMODINIT_FUNC
initDynamsoftBarcodeReader(void)
{
	Py_InitModule("DynamsoftBarcodeReader", methods);
}