import os.path
import DynamsoftBarcodeReader

formats = {
    0x1FFL : "OneD",
    0x1L   : "CODE_39",
    0x2L : "CODE_128",
    0x4L   : "CODE_93",
    0x8L : "CODABAR",
    0x10L   : "ITF",
    0x20L : "EAN_13",
    0x40L   : "EAN_8",
    0x80L : "UPC_A",
    0x100L   : "UPC_E",
}

def initLicense(license):
    DynamsoftBarcodeReader.initLicense(license)

def decodeFile(fileName):
    results = DynamsoftBarcodeReader.decodeFile(fileName)
    for result in results:
        print "barcode format: " + formats[result[0]]
        print "barcode value: " + result[1]

if __name__ == "__main__":
    barcode_image = input("Enter the barcode file: ");
    if not os.path.isfile(barcode_image):
        print "It is not a valid file."
    else:
        decodeFile(barcode_image);
