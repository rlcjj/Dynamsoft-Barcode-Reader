Making Java Barcode Reader with Dynamsoft Barcode SDK
=======================================================================

The sample shows how to call native C/C++ methods of [Dynamsoft Barcode Reader][1] (DBR) SDK via JNI to create a Java Barcode Reader on Windows.

Download & Installation
-----------------------
* [Dynamsoft Barcode Reader SDK][2]
![image](http://www.codepool.biz/wp-content/uploads/2015/05/dbr_folder.png)

How to Run
-----------
1. configure directories of DBR header files and libraries in Visual Studio.
![image](http://www.codepool.biz/wp-content/uploads/2015/05/dbr_include-1024x462.png)
![image](http://www.codepool.biz/wp-content/uploads/2015/05/dbr_lib-1024x462.png)
2. build the JNI project.
3. copy generated DLL and DBR DLL to Java Barcode Reader Project.
![image](http://www.codepool.biz/wp-content/uploads/2015/05/dbr_java.png)
4. import the Java project into Eclipse.
5. specify an image file path as argument, and then run the Java project.
![image](http://www.codepool.biz/wp-content/uploads/2015/05/dbr_args.png)
![image](http://www.codepool.biz/wp-content/uploads/2015/05/dbr_results1.png)

[1]:http://www.dynamsoft.com/Products/Dynamic-Barcode-Reader.aspx
[2]:http://www.dynamsoft.com/Downloads/Dynamic-Barcode-Reader-Download.aspx
