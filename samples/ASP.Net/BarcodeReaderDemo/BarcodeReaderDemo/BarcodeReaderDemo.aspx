<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarcodeOnLineDemo.aspx.cs"
    Inherits="BarcodeWeb.BarcodePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
    <title>Barcode Reader Online Demo | Read barcodes in browsers</title>
    <link type="text/css" rel="Stylesheet" href="Css/style.css" />
    <meta name="viewport" content="width=device-width, maximum-scale=1.0"/>
    <link rel="stylesheet" href="Css/basis.css?ver=2.0"/>
   
    <script type="text/javascript" language="javascript" src="Scripts/jquery-1.11.2.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/ds-jquery.js"></script>
    <meta name="viewport" content="width=device-width, maximum-scale=1.0"/>
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
        
        var MaxHeight = 710;
        var MaxWidth = 560;
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

        window.setInterval(function () {
            var AjaxFunctionUrl = "Ajaxfunctions.aspx";
            _Function_AjaxPOST(AjaxFunctionUrl, null, null, null);
        }, 900000);
        
    </script>
    </head>
    <body oncontextmenu=self.event.returnValue=false onselectstart="return true">
    <div id="wrapper">
      <div id="container" class="pr"> 
        <!--begin header-->
        <div id="header"> 
          <!-- #include file=overall-header.aspx --> 
          <!--begin subNav--> 
          

          <!--end subNav--> 
        </div>
        <!--end header-->
        <div id="main">
        <div class="inner">
          <div class="D-dailog" id="J_waiting">
            <div id = "strBody"> </div>
          </div>
          <form id="form1" class="form-content" runat="server">
            <div class="body_Broad_width" style="margin:0 auto;">
              <div id="main-content">
                <div id="content-left">
                  <div id="content-nav">
                    <iframe width='100%' height="190" frameborder='0' scrolling="no" src="UploadMode.aspx"></iframe>
                  </div>
                  <div id="sample-image">
                    <div class="divThumbnail">
                      <div id="pre" class="tc"><img src="Images/icn-pre.png"/></div>
                      <div id="imgCenterDiv"> </div>
                      <div id="next" class="tc"><img src="Images/icn-next.png"/></div>
                    </div>
                  </div>
                  <div id="tool-bar"> <img alt="Fit" title="Fit" onclick="FixSize();" src="Images/FitWindow.png" /> <img alt="1:1" title="1:1" onclick="OriginSize();" src="Images/1To1.png" /> </div>
                  <div id="image-box">
                    <div class="divShowBarcode">
                      <asp:Image ID="Image1" CssClass="ImageBarcode" runat="server" />
                    </div>
                  </div>
                </div>
                <div id="content-right">
                  <div class="divThumbnail">
                    <div class="title"> Barcode Types </div>
                    <div class="barcode-types">
                      <label class="lblBarcodeType">
                        <input id="chkCode39" name="BarcodeType" type="checkbox" value = "0x1" onclick = "ClickCheckBox(this);"/>
                        Code 39</label>
                      <label class="lblBarcodeType">
                        <input id="chkCode128" name="BarcodeType" type="checkbox" value = "0x2"  onclick = "ClickCheckBox(this);"/>
                        Code 128</label>
                      <label class="lblBarcodeType">
                        <input id="chkCode93" name="BarcodeType" type="checkbox" value = "0x4"  onclick = "ClickCheckBox(this);"/>
                        Code 93</label>
                      <label class="lblBarcodeType">
                        <input id="chkCodabar" name="BarcodeType" type="checkbox" value = "0x8"  onclick = "ClickCheckBox(this);"/>
                        Codabar</label>
                      <label class="lblBarcodeType">
                        <input id="chkUPCA" name="BarcodeType" type="checkbox" value = "0x80"  onclick = "ClickCheckBox(this);"/>
                        UPC-A</label>
                      <label class="lblBarcodeType">
                        <input id="chkUPCE" name="BarcodeType" type="checkbox" value = "0x100"  onclick = "ClickCheckBox(this);"/>
                        UPC-E</label>
                      <label class="lblBarcodeType">
                        <input id="chkEAN13" name="BarcodeType" type="checkbox" value = "0x20"  onclick = "ClickCheckBox(this);"/>
                        EAN-13</label>
                      <label class="lblBarcodeType">
                        <input id="chkEAN8" name="BarcodeType" type="checkbox" value = "0x40"  onclick = "ClickCheckBox(this);"/>
                        EAN-8</label>
                      <label class="lblBarcodeType lblBarcodeType-long">
                        <input id="chkITF" name="BarcodeType" type="checkbox" value = "0x10"  onclick = "ClickCheckBox(this);"/>
                        Interleaved 2 of 5</label>
                      <label class="lblBarcodeType lblBarcodeType-long">
                        <input id="chkIndustrial25" name="BarcodeType" type="checkbox" value = "0x200"  onclick = "ClickCheckBox(this);"/>
                        Industrial 2 of 5</label>
                      <label class="lblBarcodeType">
                        <input id="chkQRCode" name="BarcodeType" type="checkbox" value = "0x4000000"  onclick = "ClickCheckBox(this);"/>
                        QRCode</label>
                    </div>
                    
                    <div class="UnSelect-all"><a id="ImgSelectAll"  onclick="javascript:document.getElementById('btnSelectAndUnSelectedAll').click();">Select All</a>
              </div>
                    
                    <div style="display:none;">
                      <input id="btnSelectAndUnSelectedAll" type="button" onclick="ClickSelectAndUnSelectedAll();" style="display:none;height:22px; width:1px;margin-left:0px; float:left;" value="SelectAll" name="AddImage">
                    </div>
                  </div>
                  <div class="recognize-barcode"> <a  id="RecgabtnCssBarcode" name="RecgabtnCssBarcode"  onclick = "ClickDoBarcode();"></a> </div>
                  <div class="msg-box">
                    <div class="title">Barcode Results</div>
                    <div id="DWTemessage"></div>
                  </div>
                </div>
              </div>
            </div>
            <asp:HiddenField ID="hide_ImgFileName" runat="server" />
            <asp:HiddenField ID="hide_allImgURL" runat="server" />
            <asp:HiddenField ID="hide_showInputURLImage" runat="server" />
            <asp:HiddenField ID="hide_ShowType" Value="2" runat="server" />
          </form>
          </div>
        </div>
        <!--begin footer-->
        <div id="footer"> 
            <!-- #include file=overall-footer.aspx --> 
        </div>
        <!--end footer--> 
      </div>
    </div>
</body>

<script>

$(document).ready(function(){
	
var iCurrentFirst = 0;
	
        $("#pre").click(function(){
			var imgCenterDivbottom = $("#imgCenterDiv").css('bottom');
			 var imglist = $("#imgCenterDiv a")
			 if (imglist.length == 0) {
                 return false;
             } else if(imglist.length > 6 && iCurrentFirst < (imglist.length -6) ){
				 imgCenterDivbottom = parseInt(imgCenterDivbottom) - 120 ;
				 $("#imgCenterDiv").css('bottom',imgCenterDivbottom);
				 $("#next img").css('display','inline-block');
				 iCurrentFirst++;
				 } else {
					 $("#pre img").css('display', 'none');
                     return false;
                 }           
         });
		 
         $("#next").click(function () {
			 var imgCenterDivbottom = $("#imgCenterDiv").css('bottom');
			 var imglist = $("#imgCenterDiv a")
			 if (imglist.length == 0) {
                 return false;
             } else if(imglist.length > 6 && iCurrentFirst >0){
				 imgCenterDivbottom = parseInt(imgCenterDivbottom) + 120 ;
				 $("#imgCenterDiv").css('bottom',imgCenterDivbottom);
				 $("#pre img").css('display', 'inline-block');
				 iCurrentFirst--;
				 } else {
					 $("#next img").css('display','none');
                     return false;
                 }
         });
	
	
});
</script>

</html>
