Wrapping C/C++ Methods of Dynamsoft Barcode SDK for Python
=======================================================================

The sample shows how to wrap native C/C++ methods of [Dynamsoft Barcode Reader][1] (DBR) SDK to create a Barcode extension for Python on Windows.

Download & Installation
-----------------------
* [Dynamsoft Barcode Reader SDK][2]
![image](http://www.codepool.biz/wp-content/uploads/2015/05/dbr_folder.png)

How to Run
-----------
1. Import the project to Visual Studio. And then correctly configure directories of DBR header files and libraries, as well as add Python dependencies.
![image](http://www.codepool.biz/wp-content/uploads/2015/05/python_include-1024x463.png)
![image](http://www.codepool.biz/wp-content/uploads/2015/05/python_lib-1024x460.png)
![image](http://www.codepool.biz/wp-content/uploads/2015/05/python_dependency-1024x465.png)
![image](http://www.codepool.biz/wp-content/uploads/2015/05/python_pyd-1024x459.png)
2. Build the project to generate **DynamsoftBarcodeReader.pyd**.
3. Run **DynamsoftBarcodeReader.py** which located under Release folder.
```cmd
> Python DynamsoftBarcodeReader.py
```
![image](http://www.codepool.biz/wp-content/uploads/2015/05/python_dbr_test.png)

Blog
-----------
[Wrapping C/C++ Methods of Dynamsoft Barcode SDK for Python][3]

[1]:http://www.dynamsoft.com/Products/Dynamic-Barcode-Reader.aspx
[2]:http://www.dynamsoft.com/Downloads/Dynamic-Barcode-Reader-Download.aspx
[3]:http://www.codepool.biz/wrap-c-barcode-sdk-for-python/
