#include <node.h>

#include "If_DBR.h"
#include "BarcodeFormat.h"
#include "BarcodeStructs.h"
#include "ErrorCode.h"


#ifdef _WIN64
#pragma comment(lib, "DBRx64.lib")
#else
#pragma comment(lib, "DBRx86.lib")
#endif

using namespace v8;

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

void DecodeFile(const FunctionCallbackInfo<Value>& args) {

	Isolate* isolate = Isolate::GetCurrent();
	HandleScope scope(isolate);

	// convert v8 string to char *
	String::Utf8Value utfStr(args[0]->ToString());
	char *pFileName = *utfStr;

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

	if (ret == DBR_OK){
		int count = pResults->iBarcodeCount;
		pBarcodeResult* ppBarcodes = pResults->ppBarcodes;
		pBarcodeResult tmp = NULL;

		// javascript callback function
		Local<Function> cb = Local<Function>::Cast(args[1]);
		const unsigned argc = 1;

		// array for storing barcode results
		Local<Array> barcodeResults = Array::New(isolate);

		for (int i = 0; i < count; i++)
		{
			tmp = ppBarcodes[i];

			Local<Object> result = Object::New(isolate);
			result->Set(String::NewFromUtf8(isolate, "format"), Number::New(isolate, tmp->llFormat));
			result->Set(String::NewFromUtf8(isolate, "value"), String::NewFromUtf8(isolate, tmp->pBarcodeData));

			barcodeResults->Set(Number::New(isolate, i), result);
		}

		// release memory
		DBR_FreeBarcodeResults(&pResults);

		Local<Value> argv[argc] = { barcodeResults };
		cb->Call(isolate->GetCurrentContext()->Global(), argc, argv);
	}
}

void Init(Handle<Object> exports) {
	NODE_SET_METHOD(exports, "decodeFile", DecodeFile);
}

NODE_MODULE(dbr, Init)
