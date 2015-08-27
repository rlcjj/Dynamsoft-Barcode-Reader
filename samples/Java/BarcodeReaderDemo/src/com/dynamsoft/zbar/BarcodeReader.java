package com.dynamsoft.zbar;

public class BarcodeReader {
	static {
		System.loadLibrary("zbarjni");
	}
	
	public native void decode(String fileName);
}
