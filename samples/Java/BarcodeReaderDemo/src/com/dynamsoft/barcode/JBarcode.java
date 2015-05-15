package com.dynamsoft.barcode;

import java.util.Properties;

public class JBarcode {

	static {

		try {
			// get arch
			Properties props = System.getProperties();
			String bits = String.valueOf(props.get("sun.arch.data.model"));
			if (bits.equals("32")) {
				bits = "86";
			}

			String jniLib = "DynamsoftBarcodeJNIx" + bits;

			// load dll
			System.loadLibrary(jniLib);
		} catch (Exception e) {
			System.err.println("load jni error!");
		}
    	
}
  
    public native static int DBR_InitLicense(
		String pLicense	//License Key
	);

    public native static int DBR_DecodeFile(
		String pFileName,
		
		int option_iMaxBarcodesNumPerPage,
		long option_lBarcodeFormat,
		
		tagBarcodeResultArray ppResults	//Barcode Results
	);	

    public native static int DBR_DecodeFileRect(
		String pFileName,

		int option_iMaxBarcodesNumPerPage,
		long option_lBarcodeFormat,
		
		int iRectLeft,			//Rectangle Left
		int iRectTop,			//Rectangle Top
		int iRectWidth,			//Rectangle 
		int iRectHeight,		//Rectangle 
		tagBarcodeResultArray ppResults	// Barcode Results
	);

    public native static int DBR_DecodeBuffer(
		byte[] pDIBBuffer,	//Buffer
        int iDIBSize,
        
		int option_iMaxBarcodesNumPerPage,
		long option_lBarcodeFormat,
		
        tagBarcodeResultArray ppResults	//Barcode Results
	);

    public native static int DBR_DecodeBufferRect(
		byte[] pDIBBuffer,	//Buffer
        int iDIBSize,	
        
		int option_iMaxBarcodesNumPerPage,
		long option_lBarcodeFormat,
		
        int iRectLeft,			//Rectangle Left
        int iRectTop,			//Rectangle Top
        int iRectWidth,			//Rectangle 
        int iRectHeight,		//Rectangle 
        tagBarcodeResultArray ppResults	//Barcode Results
	);


    public native String GetErrorString(int iErrorCode);

}