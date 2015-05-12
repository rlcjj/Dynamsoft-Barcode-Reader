<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarcodeOnLineDemo.aspx.cs"
    Inherits="BarcodeWeb.BarcodePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <link type="text/css" rel="Stylesheet" href="Css/style.css" />
     <script type="text/javascript" language="javascript" src="Scripts/kissy-min.js"></script>
     <script type="text/javascript" language="javascript" src="Scripts/OnlineDemoAjax.js"></script>
     <script type="text/javascript" language="javascript" src="Scripts/DemoCommon.js"></script>
     <script type="text/javascript">

        // Assign the page onload fucntion.
        function S_get(id) {
            return document.getElementById(id);
        }

        var S = KISSY;
        
        
        function ClickDoBarcode() {
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            var ss = objImgage.src;
            if (ss.indexOf('Barcoding.aspx') > 0) {
                alert("Please change image.");
                return false;
            }

            showWaitDialog("Barcode Recognize");
            DoBarcode();
            return true;
        }

        function DoBarcode() {
            var objHide = document.getElementById("<%=hide_ImgFileName.ClientID %>");
            var vFileName = objHide.value;
            vFileName = vFileName.substring(vFileName.lastIndexOf("/") + 1);
            var vType = 0;
            var   barcodeType   =   document.getElementsByName("BarcodeType");  
            for(i=0;i<barcodeType.length;i++){  
                
                if(barcodeType[i].checked == true)
                vType = vType | (barcodeType[i].value *1);
            } 
            
            var AjaxFunctionUrl = "Ajaxfunctions.aspx";
            var body = "DW_AjaxMethod=DoBarcode&FileName=" + _Function_EncodeXmlString(vFileName) +
            "&BarcodeFormat=" + _Function_EncodeXmlString(vType.toString()) +
            "&SessionID=" + _Function_EncodeXmlString("<%=SessionID %>");
           
            _Function_AjaxPOST(AjaxFunctionUrl, body, _Function_DoBarcode, null);
        }


        function _Function_DoBarcode(xmlHttpRequestObject, objFieldObject) {
           DoNotShowWaitDDialog();
            var responseText = xmlHttpRequestObject.responseText;
            if (responseText != "") {
                var tmpState = responseText.split(';');
                if (tmpState[0] == "OK") {
                    var objImgage = document.getElementById("<%=Image1.ClientID%>");
                    objImgage.src = tmpState[1];
                    var message = tmpState[2];
                    var r, re;
                    re = /&nbsp/g;
                    r = message.replace(re, "&nbsp;");
                    appendMessage(r);
                }
                else if (tmpState[0] == "EXP") {
                    var message = tmpState[1];
                    appendMessage(message);
                    alert(message);
                }
            }
        }

     
        function SetCurrentSelect() {
            var objHide = document.getElementById("<%=hide_ImgFileName.ClientID %>");      
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            objImgage.src = objHide.value;
            ShowImgSize(objHide.value);
            SetCurrentSelectInner(objHide);              
        }

        function FixSize() {
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            var objShowType = document.getElementById("hide_ShowType");
            objShowType.value = 0;
            FixSizeInner(objImgage);
        }

        function OriginSize() {
            var objShowType = document.getElementById("hide_ShowType");
            objShowType.value = 2;
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            OriginSizeInner(objImgage);
        }

        function FullSize() {
            var objShowType = document.getElementById("hide_ShowType");
            objShowType.value = 3;
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            FullSizeInner(objImgage);
        }

        function FixWidth() {
            var objShowType = document.getElementById("hide_ShowType");
            objShowType.value = 1;
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            FixWidthInner(objImgage);
        }
        

        function ClickUploadFile() {
            showWaitDialog("Upload Image");
        }

        function ClickUploadImageURL() {
            showWaitDialog("Upload Image URL");
        }
        
        function DoNotShowWaitDDialog() {
            DoNotShowWaitDDialogInner();
        }
        
        var MaxHeight = 620;
        var MaxWidth = 650;
        var Per = 0.98;
        var OriginWidth = 0;

        function InitialPreviewIMG() {
            var allImgListObj = document.getElementById("<%=hide_allImgURL.ClientID%>");
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            InitialPreviewIMGInner(allImgListObj, objImgage);
        }

        function ShowImg(a_Id, ImgSrc) {
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            var objHide = document.getElementById("<%=hide_ImgFileName.ClientID %>");
            ShowImgInner(a_Id, ImgSrc, objImgage, objHide);
            ShowImgSize(objHide.value);
        }

        function AppendImage(fileName,iWidth, iHeight) {
            var aryFileName = fileName.split(':');            
            var objHide = document.getElementById("<%=hide_ImgFileName.ClientID %>");
            
            var objImgage = document.getElementById("<%=Image1.ClientID%>");
            if(aryFileName.length > 0)
            {
                objHide.value = aryFileName[0];
                objImgage.src = aryFileName[0];
            }
            else
            {
                objHide.value = fileName;
                objImgage.src = fileName;
            }
            var objAllImgURL = document.getElementById("<%=hide_allImgURL.ClientID%>");
            objAllImgURL.value += ":" + fileName;

            varCurrentImageWidth = iWidth;
            varCurrentImageHeight = iHeight;
            InitialPreviewIMG();
            SetCurrentSelect();
        }

        window.onload = function() {
            InitialPreviewIMG();
            SetCurrentSelect();
            ClickSelectAll();
        }
        
    </script>
</head>
<body oncontextmenu=self.event.returnValue=false onselectstart="return false">  
<div class="D-dailog" id="J_waiting">
    <div id = "strBody">       
    </div>
 </div>
    <form id="form1" runat="server">
    <div class="body_Broad_width" style="margin:0 auto;">
        <div id="headcontainer" class="body_Broad_width" style="background-color:#3a3a3a; border:0; padding:0;">
        <br />
            <div class="body_Broad_width" style="background-image:url(Images/adtopbackground.gif); height:88px; width:987px; ">
            <div style="float:left; padding-top:15px; width:525px; margin-left:25px;">
                <span>
                    <a href="http://www.dynamsoft.com/">
                        <img src="Images/logo.gif" alt="Dynamsoft: provider of version control solution and TWAIN SDK" style='padding: 12px 0 0;'
                        name="logo" border="0" align="left" id="logo" title="Dynamsoft: provider of version control solution and TWAIN SDK" />
                    </a>
                </span>
                <span style='border-left:1px solid #CCC;margin: 0 0 0 10px;padding: 38px 0 0 10px;'>
                    <a href="http://www.dynamsoft.com/Products/Dynamic-Barcode-Reader.aspx">
                        <img alt = "Barcode logo" style="border:none; " src="Images/Barcode icon logo.png"/>
                    </a>
                </span>
                </div>
                <div style="float:left; border:0px; padding-top:15px; padding-left:0px; width:390px; text-align:right">
                    <div style='height:35px;'></div>
                    <div style='padding-top:15px;'><strong>ph:1-604-605-5491</strong></div>
                </div>
            </div>
            <div id="menu">
            <ul>
                <li style="float:left; width:30px; height:40px; line-height:40px; color:#FFF;"></li>
                <li style="float:left; width:680px;color:#FFF;height:40px;"></li>
                <li class="D_menu_item" style="width:180px;" title="Includes Source Code of Current Page">
                    <div class="menubar_split" >
                    </div>
                     <div class="menubar_split_last">
                    </div>
                    <a class="nohref" href="http://www.dynamsoft.com/Downloads/Dynamic-Barcode-Reader-Download.aspx"> Download Free Trial</a>
                    
                </li>
            </ul>
            </div>
        </div>
        
       <div class="body_Broad_width" style="background-color:#ffffff; border:0;clear:both;">
            <table style="margin-left:20px;width: 98%">
                 <tr>
                    <td colspan="2" >
                      <iframe width='96%' height='72' frameborder='0' scrolling="no" src="UploadMode.aspx"></iframe>
                    </td>
                </tr>
                <tr>
                <td  colspan="2" >
                    <div class="divThumbnail"  style ="width:93%">
                            <div class="title" style="width: 120px; position: relative; top: -2px;">
                                    Barcode Types
                              </div>
                            <div style=" padding:2px;" >
                                    <div  style="padding-left:0px;" > 
                                          <label class="lblBarcodeType"><input id="chkCode39" name="BarcodeType" type="checkbox" value = "0x1" onclick = "ClickCheckBox(this);"/>Code39</label>
                                          <label class="lblBarcodeType"><input id="chkCode128" name="BarcodeType" type="checkbox" value = "0x2"  onclick = "ClickCheckBox(this);"/>Code128</label>
                                          <label class="lblBarcodeType"><input id="chkCode93" name="BarcodeType" type="checkbox" value = "0x4"  onclick = "ClickCheckBox(this);"/>Code93</label>
                                          <label class="lblBarcodeType"><input id="chkCodabar" name="BarcodeType" type="checkbox" value = "0x8"  onclick = "ClickCheckBox(this);"/>Codabar</label>
                                          <label class="lblBarcodeType"><input id="chkUPCA" name="BarcodeType" type="checkbox" value = "0x80"  onclick = "ClickCheckBox(this);"/>UPCA</label>
                                          <label class="lblBarcodeType"><input id="chkUPCE" name="BarcodeType" type="checkbox" value = "0x100"  onclick = "ClickCheckBox(this);"/>UPCE</label>
                                          <label class="lblBarcodeType"><input id="chkEAN13" name="BarcodeType" type="checkbox" value = "0x20"  onclick = "ClickCheckBox(this);"/>EAN13</label>
                                          <label class="lblBarcodeType"><input id="chkEAN8" name="BarcodeType" type="checkbox" value = "0x40"  onclick = "ClickCheckBox(this);"/>EAN8</label>
                                          <label class="lblBarcodeType"><input id="chkITF" name="BarcodeType" type="checkbox" value = "0x10"  onclick = "ClickCheckBox(this);"/>ITF</label>
                                    </div>
                                   <div style="float:right; width:92px; margin-left:0px;*margin-left:6px;margin-left:6px\9; margin-top:-25px;"><img src="Images/UnSelectAll.png" id="ImgSelectAll" style="cursor:pointer;margin-left:0px; float:left;" onclick="javascript:document.getElementById('btnSelectAndUnSelectedAll').click();"/></div>                 
                                   <div style="display:none;"><input id="btnSelectAndUnSelectedAll" type="button" onclick="ClickSelectAndUnSelectedAll();" style="display:none;height:22px; width:1px;margin-left:0px; float:left;" value="SelectAll" name="AddImage"></div>
                                 </div>                 
                       </div>
                </td>  
                </tr>
                <tr>
                    <td>
                        <div  style="float: left; margin-left: 0px; margin-top:0px;" >
                          <div style=" height:60px;position:relative;" >
                          <img src= "images/RecognizeBarcode.png" style="cursor:pointer;margin-top:5px;" id="RecgabtnCssBarcode" name="RecgabtnCssBarcode"  onclick = "ClickDoBarcode();" />
                          </div>
                         <div class="divThumbnail">
                            <center>
                                <div id="imgCenterDiv" style="height: 750px">
                                </div>
                            </center>
                        </div>
                    </div>
                    </td>
                    <td>
                        <div style="width: 760px;position:relative;">
                            <div style="width: 700px; height: 690px; overflow: auto; border: 2px #E5E5E5 solid;
                                text-align: center; float: left; top: -2px;">
                                <div class="divShowBarcode">
                                    <asp:Image ID="Image1" CssClass="ImageBarcode" runat="server" />
                                </div>
                            </div>
                            <div id="DWTemessage" style="float:left; top:10px;width:700px;height:110px; overflow:scroll; background-color:#ffffff; border:1px #303030; border-style:solid; text-align:left; position:relative" ></div>
                            <div style="position:absolute;width: 36px; height: 186px; top:35px; right:20px;*right:20px;">
                                <img alt="Fit" title="Fit" onclick="FixSize();" src="Images/FitWindow.png" style="cursor:pointer;width: 36px;
                                    height: 36px" />
                                <img alt="Fit Width" title="Fit Width" onclick="FixWidth();" src="Images/FitWidth.png"
                                    style="cursor:pointer;width: 36px; height: 36px" />
                                <img alt="1:1" title="1:1" onclick="OriginSize();" src="Images/1To1.png" style="cursor:pointer;width: 36px;
                                    height: 36px" />
                                <img alt="Full" title="Full" onclick="FullSize();" src="Images/FullSize.png" style="cursor:pointer;width: 36px;
                                    height: 36px" />
                                <img alt="Recognize Barcode" title="Recognize Barcode" onclick="ClickDoBarcode();" src="Images/run.png" style="cursor:pointer;width: 36px;
                                    height: 36px" />
                            </div>
                         </div>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    
        <div id="tailcontainer" class="body_Broad_width" style="background-color:#ffffff; border:0;">

        <div class="body_Broad_width" style="height:4px; background-color:#303030"></div>
        <div class="body_Broad_width" style="height:6px; background-color:#ff8e13"></div>
        <br />
        <div style="width:10px; height:85px;float:right; border:0px; padding:0px;">
        <img alt = "&gt;" src="Images/bottomright.gif"/></div>
        <div class="body_Narrow_width" style="font-size:larger; z-index:100; position:relative;height:85px; border:0px; padding:0px; float:right; text-align:center; background-image: url(Images/bottommid.gif); background-repeat:repeat;">
        <br />
            <a class="fontcolor" href="http://www.dynamsoft.com/Products/Dynamic-Barcode-Reader.aspx" style="cursor:text">Barcode Reader SDK</a> powered by <a class="fontcolor" href="http://www.dynamsoft.com/" style="cursor:text">Dynamsoft</a>
        </div>
        <div style="width:10px; height:85px;float:right; border:0px; padding:0px;">
            <img alt = "&gt;" src="Images/bottomleft.gif"/></div>
        </div>    
      </div>       
    <asp:HiddenField ID="hide_ImgFileName" runat="server" />
    <asp:HiddenField ID="hide_allImgURL" runat="server" />
    <asp:HiddenField ID="hide_showInputURLImage" runat="server" />
    <asp:HiddenField ID="hide_ShowType" Value="2" runat="server" />
    </form>
</body>
</html>
