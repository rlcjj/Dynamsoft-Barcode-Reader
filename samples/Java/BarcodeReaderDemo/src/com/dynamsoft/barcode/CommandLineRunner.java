package com.dynamsoft.barcode;

import java.util.Date;


public final class CommandLineRunner {

	private static String CMD_ERR = "The syntax of the command is incorrect.";

	private CommandLineRunner() {
	}


	public static void main(String[] args) throws Exception {
	    if (args.length <= 0) {
	      printUsage();
	      return;
	    }

	    int iMaxCount = Integer.MAX_VALUE;
	    long lFormat = -1;
	    String pszImageFile = null;
		for (int iIndex = 0; iIndex < args.length; iIndex ++)
		{
			if (DBRCommon.compareCmd(args[iIndex], "-help") == 0)
		{
			printUsage();
			return;
		}
	
		if (DBRCommon.compareCmd(args[iIndex], "-f") == 0)
		{
			// parse format
			iIndex ++;
			if (iIndex >= args.length)
			{
				System.out.println(CMD_ERR);
				return;
			}
			lFormat = DBRCommon.GetFormat(args[iIndex]);
			if (lFormat == 0)
			{
				System.out.println(CMD_ERR);
				return;
			}
			continue;
		}
	
		if (DBRCommon.compareCmd(args[iIndex], "-n") == 0)
		{// parse format
			iIndex ++;
			if (iIndex >= args.length)
			{
				System.out.println(CMD_ERR);
				return;
			}
			iMaxCount = Integer.parseInt(args[iIndex]);
			continue;
		}
	
		if (null == pszImageFile)
			pszImageFile = args[iIndex];
		}
		
		if (null == pszImageFile){
			System.out.println(CMD_ERR);
			return;
		}
		
		
		// Set license
		JBarcode.DBR_InitLicense("<Put your license key here>");
		
		// Read barcode
		long ullTimeBegin = new Date().getTime();
		
	
		tagBarcodeResultArray paryResults = new tagBarcodeResultArray();
		
		int iret = JBarcode.DBR_DecodeFile(pszImageFile, iMaxCount, lFormat, paryResults);
		if(iret != 0){
			System.out.println(JBarcode.DBR_GetErrorString(iret));
			return;
		}
		
		long ullTimeEnd =  new Date().getTime();
		
		// Output barcode result
		/*
			Total barcode(s) found: 2. Total time spent: 0.218 seconds. 

			  Barcode 1:
				  Page: 1
				  Type: CODE_128
				  Value: Zt=-mL-94
				  Region: {Left: 100, Top: 20, Width: 100, Height: 40}

			  Barcode 2:
				  Page: 1
				  Type: CODE_39
				  Value: Dynamsoft
				  Region: {Left: 100, Top: 200, Width: 180, Height: 30}
		*/

		String pszTemp;

		if(paryResults.iBarcodeCount <= 0){
			pszTemp = String.format("No barcode found. Total time spent: %.3f seconds.", ((float)(ullTimeEnd - ullTimeBegin)/1000));
		} else {
			pszTemp = String.format("Total barcode(s) found: %d. Total time spent: %.3f seconds.", paryResults.iBarcodeCount, ((float)(ullTimeEnd - ullTimeBegin)/1000));
		}
		System.out.println(pszTemp);
		
		for (int iIndex = 0; iIndex < paryResults.iBarcodeCount; iIndex++)
		{
			tagBarcodeResult result = paryResults.ppBarcodes[iIndex];
			pszTemp = String.format("  Barcode %d:", iIndex + 1);
			System.out.println(pszTemp);
			pszTemp = String.format("    Page: %d", result.iPageNum);
			System.out.println(pszTemp);
			pszTemp = String.format("    Type: %s", DBRCommon.GetFormatStr(result.lFormat));
			System.out.println(pszTemp);
			
			int barcodeDataLen = result.iBarcodeDataLength;
			
			byte[] pszTemp1 = new byte[barcodeDataLen];		
			for(int x = 0; x<barcodeDataLen; x++){
				pszTemp1[x] = result.pBarcodeData[x];
			}

			pszTemp = "    Value: " + new String(pszTemp1);
			System.out.println(pszTemp);
			
			pszTemp = String.format("    Region: {Left: %d, Top: %d, Width: %d, Height: %d}", 
					result.iLeft, result.iTop, 
					result.iWidth, result.iHeight);
			
			System.out.println(pszTemp);
			System.out.println();
		}
	}

	private static void printUsage() {

		System.out.println("Usage: java -jar BarcodeReaderDemo.jar [-f format] [-n number] ImageFilePath");
		System.out.println();
		System.out.println("-f format: supported barcode formats include {CODE_39;CODE_128;CODE_93;CODABAR;ITF;UPC_A;UPC_E;EAN_13;EAN_8;INDUSTRIAL_25;OneD;QR_CODE}. ");
		System.out.println();
		System.out.println("-n number: maximum barcodes to read per page.");
	}

}
