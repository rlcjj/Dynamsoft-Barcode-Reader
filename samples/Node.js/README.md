Making Dynamsoft Barcode SDK an Addon for Node.js
=======================================================================

The sample shows how to implement a JavaScript Barcode application with the custom addon, wrapped with DBR ([Dynamsoft Barcode SDK][1]) , for Node.js on Windows.

Download & Installation
-----------------------
* [Dynamsoft Barcode Reader SDK][2]
* ```npm install -g node-gyp```

How to Run
-----------
1. Import the project to Visual Studio. And then correctly configure directories of DBR header files and libraries. In addition, add the custom build event: ```copy "{installation directory}\Dynamsoft\Barcode Reader 2.0 Trial\Redist\C_C++\*.dll" "$(OutDir)"```
2. Build the project to generate **dbr.node**.
3. Run **dbr.js** with the following command.
```cmd
> node dbr.js
```
![image](http://www.codepool.biz/wp-content/uploads/2015/05/node_barcode.png)

[1]:http://www.dynamsoft.com/Products/Dynamic-Barcode-Reader.aspx
[2]:http://www.dynamsoft.com/Downloads/Dynamic-Barcode-Reader-Download.aspx
