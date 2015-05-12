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

	    int iMaxCount = -1;
	    long lFormat = -1;
	    String pszImageFile = null;
		for (int iIndex = 0; iIndex < args.length; iIndex ++)
		{
			if (compareCmd(args[iIndex], "-help") == 0)
		{
			printUsage();
			return;
		}
	
		if (compareCmd(args[iIndex], "-f") == 0)
		{
			// parse format
			iIndex ++;
			if (iIndex >= args.length)
			{
				System.out.println(CMD_ERR);
				return;
			}
			lFormat = GetFormat(args[iIndex]);
			if (lFormat == 0)
			{
				System.out.println(CMD_ERR);
				return;
			}
			continue;
		}
	
		if (compareCmd(args[iIndex], "-n") == 0)
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
		if(iret == -10003){
			System.out.println("Invalid License.");
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
			pszTemp = String.format("    Type: %s", GetFormatStr(result.lFormat));
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
		System.out.println("-f format: supported barcode formats include {CODE_39;CODE_128;CODE_93;CODABAR;ITF;UPC_A;UPC_E;EAN_13;EAN_8;OneD}. ");
		System.out.println();
		System.out.println("-n number: maximum barcodes to read per page.");
	}

	private static int compareCmd(String cmd, String dest) {
		return (cmd.compareTo(dest));
	}

	private static long GetFormat(String formats) {
		long lFormat = 0;
		String[] aryFormats = formats.split("\\|");
		
		for(int i=0; i<aryFormats.length; i++){
			String cmd = aryFormats[i].toUpperCase();
			
			if (compareCmd(cmd, "CODE_39") == 0)
				lFormat |= EnumBarCode.CODE_39;
			else if (compareCmd(cmd, "CODE_128") == 0)
				lFormat |= EnumBarCode.CODE_128;
			else if (compareCmd(cmd, "CODE_93") == 0)
				lFormat |= EnumBarCode.CODE_93;
			else if (compareCmd(cmd, "CODABAR") == 0)
				lFormat |= EnumBarCode.CODABAR;
			else if (compareCmd(cmd, "ITF") == 0)
				lFormat |= EnumBarCode.ITF;
			else if (compareCmd(cmd, "UPC_A") == 0)
				lFormat |= EnumBarCode.UPC_A;
			else if (compareCmd(cmd, "UPC_E") == 0)
				lFormat |= EnumBarCode.UPC_E;
			else if (compareCmd(cmd, "EAN_13") == 0)
				lFormat |= EnumBarCode.EAN_13;
			else if (compareCmd(cmd, "EAN_8") == 0)
				lFormat |= EnumBarCode.EAN_8;
			else if (compareCmd(cmd, "ONED") == 0)
				lFormat = EnumBarCode.OneD;
			
		}
		return lFormat;
	}


	private static String GetFormatStr(long format)
	{
		if (format == EnumBarCode.OneD)	
			return "OneD";
		if (format == EnumBarCode.CODE_39)
			return "CODE_39";
		if (format == EnumBarCode.CODE_128)
			return "CODE_128";
		if (format == EnumBarCode.CODE_93)
			return "CODE_93";
		if (format == EnumBarCode.CODABAR)	
			return "CODABAR";
		if (format == EnumBarCode.ITF)	
			return "ITF";
		if (format == EnumBarCode.UPC_A)	
			return "UPC_A";
		if (format == EnumBarCode.UPC_E)	
			return "UPC_E";
		if (format == EnumBarCode.EAN_13)	
			return "EAN_13";
		if (format == EnumBarCode.EAN_8)	
			return "EAN_8";

		return "Unknown";
	}
}
