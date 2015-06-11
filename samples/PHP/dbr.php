<?php

$filename = "F:\\git\\Dynamsoft-Barcode-Reader\\Images\\AllSupportedBarcodeTypes.tif";

if (file_exists($filename)) {
  echo "Barcode file: $filename \n";
  $resultArray = DecodeBarcodeFile($filename);

  if (is_array($resultArray)) {
    foreach($resultArray as $key => $value) {
      print "format:$key, result: $value \n";
      print "*******************\n";
    }
  }
  else {
    print "$resultArray";
  }

} else {
    echo "The file $filename does not exist";
}

?>
