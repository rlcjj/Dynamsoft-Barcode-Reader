Making PHP Barcode Extension with Dynamsoft Barcode SDK
=======================================================================

The sample shows how to implement a PHP Barcode extension, wrapped with DBR ([Dynamsoft Barcode SDK][1]) , on Windows. With this extension, PHP developers could quickly create Barcode Reader components for their PHP Web projects.

Download & Installation
-----------------------
* [Dynamsoft Barcode Reader SDK][2]
* [PHP 5.6 source code and VC11 x86 Thread Safe][3]

How to Build and Run
-----------
1. You have to import the project to **Visual Studio 2012**.
2. Configure directories of PHP header files and libraries.
3. Configure directories of DBR header files and libraries. In addition, add the custom build event: ```copy "{installation directory}\Dynamsoft\Barcode Reader 2.0 Trial\Redist\C_C++\*.dll" "$(OutDir)"```
4. Add preprosessor definitions:

    ```
    ZEND_DEBUG=0
    ZEND_WIN32
    PHP_WIN32
    ZTS=1
    ```
5. Build the project to generate **php_dbr.dll**
6. Copy **php_dbr.dll** to ```{PHP installation directory}\ext```
7. Copy **DynamsoftBarcodeReaderx86.dll** to ```{PHP installation directory}```
8. Open **php.ini**, and add:

    ``` 
    [Dynamsoft Barcode Reader]
    extension=php_dbr.dll
    ```
9. Run **dbr.php** with the following command:

    ```cmd
    > php dbr.php
    ```
![image](http://www.codepool.biz/wp-content/uploads/2015/06/php_barcode_extension.png)

[1]:http://www.dynamsoft.com/Products/Dynamic-Barcode-Reader.aspx
[2]:http://www.dynamsoft.com/Downloads/Dynamic-Barcode-Reader-Download.aspx
[3]:http://windows.php.net/download/
