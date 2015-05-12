<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadMode.aspx.cs" Inherits="BarcodeWeb.UploadMode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <link type="text/css" rel="Stylesheet" href="Css/style.css" />
     <script type="text/javascript" language="javascript" src="Scripts/kissy-min.js"></script>
     <script type="text/javascript" language="javascript" src="Scripts/OnlineDemoAjax.js"></script>
     <script type="text/javascript" language="javascript" src="Scripts/DemoCommon.js"></script>
    <script type="text/javascript">
     function CheckLocalPath() {
            var objImgURL = document.getElementById("<%=upLoadFile.ClientID%>");
            return CheckLocalPathInner(objImgURL);
        }

        function CheckFileExist() {
            var objImgURL = document.getElementById("<%=txtImgURL.ClientID%>");
            return CheckFileExistInner(objImgURL);
        }

        function ClickUploadImage() {
            var objHide = document.getElementById("<%=hide_State.ClientID %>");
            if (CheckLocalPath()) {
            
                parent.ClickUploadFile();
                objHide.value = "1";
                return true;
            }
            else {
                ClearUpLoadFile();
                return false;
            }
        }
        function ClickCopyFromURL() {
            var objHide = document.getElementById("<%=hide_State.ClientID %>");
 
            var varReturn = "false";

            if (CheckFileExist()) {
                parent.ClickUploadFile();
                objHide.value = "2";
                varReturn = true;
            }
            else {
                ClearImgURL();
                varReturn = false;
            }

            return varReturn;
        }
        

        function DoNotShowWaitDDialog() {
            parent.DoNotShowWaitDDialog();
        }
        
        function ShowImages()
        {
            var objHide = document.getElementById("<%=hide_State.ClientID %>");
            if(objHide.value == "1" || objHide.value == "2" ) {
            
            if("<%=strReturnPath%>" != "")
                parent.AppendImage("<%=strReturnPath%>", "<%=iWidth%>", "<%=iHeight%>");
            }
        }

        function SetState(strValue) { 
            var objHide = document.getElementById("<%=hide_State.ClientID %>");
            objHide.value = strValue;
        }

        function ClearImgURL() {
             var objImgURL = document.getElementById("<%=txtImgURL.ClientID%>");
             objImgURL.value = "";
         }

         function ClearUpLoadFile() {
             var objImgLoadFile = document.getElementById("<%=upLoadFile.ClientID%>");
             objImgLoadFile.value = "";

             var objImgLoadFileText = document.getElementById("txtUploadFileName");
             objImgLoadFileText.value = "";

         }

         window.onload = function() {
             Init();
             DoNotShowWaitDDialog();
             ShowImages();
             SetState("0");
             ClearImgURL();
             ClearUpLoadFile();
         }
     </script>
</head>
<body oncontextmenu=self.event.returnValue=false>
    <form id="form1" runat="server">
     <div class="body_Broad_width" style="background-color:#ffffff; border:0;">
      <table style="width: 94%; float: left; margin-left: 0px; top:5px;background-color:#ffffff; border:0;">
        <tr>
            <td style="text-align: left;padding:0px; height:28px; font-weight:bold">
                Load&nbsp;image&nbsp;from&nbsp;local&nbsp;disk:
            </td>
             <td>
                <div style="width:638px;"><asp:FileUpload CssClass="ImgLocalPath"  ID="upLoadFile"  size="115%" 
                        style="width:645px;  filter:alpha(opacity=0);-moz-opacity:0;opacity:0;" runat="server"/></div>
                <div style="width:638px; float:left;position:relative;">
                <asp:TextBox ID="txtUploadFileName" CssClass="ImgURL" ReadOnly=true runat="server"></asp:TextBox>
                <img src="Images/Browse.png" id="btnUploadFile" style="position:absolute;cursor:pointer;margin-left:5px;margin-top:0px;float:left;" /></div>
                <div style="float:left; width:80px; margin-left:5px;*margin-left:6px;margin-left:6px\9; margin-top:0px;"><img src="Images/OpenImage.png" id="Img1" style="cursor:pointer;margin-left:0px; float:left;" onclick="javascript:document.getElementById('AddImage').click();"/></div>                 
                <div style="display:none;"><input id="AddImage" type="submit" onclick="return ClickUploadImage();" style="display:none;height:22px; width:1px;margin-left:0px; float:left;" value="Open Image" name="AddImage"></div>
            </td>
        </tr>
        
        <tr >
            <td style="text-align: left;padding:0px;font-weight:bold">
                Copy image from remote url:
            </td>
            <td>
                <div style="width:610px; height:30px;float:left;position:relative;">
                <asp:TextBox ID="txtImgURL" CssClass="ImgURL" runat="server"></asp:TextBox>
                <img src="Images/open image from URL.png" id="GetFileFromURL" style="position:absolute;cursor:pointer;margin-left:5px; float:left;margin-top:0px;" onclick="javascript:document.getElementById('btnGetFileFromURL').click();"/></div>
                <div style="display:none;"><input id="btnGetFileFromURL" type="submit" onclick="return ClickCopyFromURL();" style="display:none;height:22px; width:1px; margin-left:0px;"  value="Open Image From URL" name="btnUploadFile">
                </div>
            </td>
        </tr>                  
    </table>
    </div>
    <asp:HiddenField ID="hide_State" Value= "0" runat="server" />
    </form>
</body>
</html>
