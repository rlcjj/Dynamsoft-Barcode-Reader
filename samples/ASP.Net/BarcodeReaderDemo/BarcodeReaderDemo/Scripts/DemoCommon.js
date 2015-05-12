var nLen;
var dlgDoBarcode;
var varCurrentImageWidth = 0;
var varCurrentImageHeight = 0;

function showWaitDialog(waitDialogType) {
    var varInformation = "";
    if (waitDialogType == "Barcode Recognize") {
        varInformation = "Recognition in progress...";
    }
    else if (waitDialogType == "Upload Image") {
        varInformation = "Uploading Image...";
    }
    else if (waitDialogType == "Upload Image URL") {
        varInformation = "Uploading Image From URL...";
    }
    else if (waitDialogType == "Engine Changed") {
        varInformation = "Please Wait while Updating Language List...";
    }

    var ObjString = "<div class=\"D-dailog-body-Recognition\">";
    ObjString += "<p>" + varInformation + "</p>";
    ObjString += "<img src='Images/loading.gif'  style='width:160px; height:160px' /></div>";
    document.getElementById("strBody").innerHTML = ObjString;

    ShowWaitDialog(200, 232); 
}

function DoNotShowWaitDDialogInner() {
    if (dlgDoBarcode) {
        dlgDoBarcode.hide();
    }
}

function ShowWaitDialog(varWidth, varHeight) {
    S.use("overlay", function(S, o) {

    dlgDoBarcode = new o.Dialog({
            srcNode: "#J_waiting",
            width: varWidth,
            height: varHeight,
            closable: false,
            mask: true,
            align: {
                points: ['cc', 'cc']
            }
        });
        dlgDoBarcode.show();
    });
}
        

function CloseDownLoadFile_onclick() {
    DoNotShowWaitDDialog();
}

function Init() {
    if (document.getElementById('upLoadFile')) {
        if (!document.getElementById('upLoadFile').onchange)
            document.getElementById('upLoadFile').onchange = function() {
                document.getElementById('txtUploadFileName').value = document.getElementById('upLoadFile').value;
            };
    }
}

function InitialPreviewIMGInner(allImgListObj, objImgage) {
    var allImgList = allImgListObj.value.split(":");
    nLen = allImgList.length;
    var centerDiv = document.getElementById("imgCenterDiv");
    var children = centerDiv.childNodes;
    var childrenLength = children.length;
    for (i = childrenLength-1; i >= 0; i--) {
        centerDiv.removeChild(children[i]);
    }
    
    for (var index = 0; index < nLen; ++index) {
        var aObj = document.createElement("a");
        aObj.id = "a_id_" + (index);
        aObj.href = "javascript:ShowImg('a_id_" + (index) + "','" + allImgList[index] + "');";
        var ImgObj = document.createElement("img");
        ImgObj.alt = "preview image";
        ImgObj.src = allImgList[index];
        ImgObj.id = "img_id_" + (index);
        aObj.appendChild(ImgObj);
        centerDiv.appendChild(aObj);
    }
    
    var tmpLoad = document.createElement("img");
    tmpLoad.src = "Images/loading.gif";
    
    var _divMessageContainer = document.getElementById("DWTemessage");
    _divMessageContainer.ondblclick = function() {
        this.innerHTML = "";
        _strTempStr = "";
    }
}

function ShowImgSizeInner() {
    var objShowType = document.getElementById("hide_ShowType");

    if (objShowType.value == "1") {
        FixWidth();
    }
    else if (objShowType.value == "2") {
        OriginSize();
    }
    else if (objShowType.value == "3") {
        FullSize();
    }
    else {
        FixSize();
    }
} 

function ShowImgInner(a_Id, ImgSrc, objImgage, objHide) {
    objImgage.src = ImgSrc;
    tmpIMG.src = ImgSrc;
    objHide.value = ImgSrc;

    for (var i = 0; ; ++i) {
        var idd = "a_id_" + i;
        var ss = document.getElementById(idd);
        if (ss) {
            if (idd == a_Id)
                ss.className = "CurrentSelected";
            else {
                ss.className = "Normal";
            }
        }
        else
            break;
    }

    
}

function SetCurrentSelectInner(objHide) {
    var ImgSrc = objHide.value;
    for (var i = 0; ; ++i) {
        var idd = "img_id_" + i;
        var ss = document.getElementById(idd);
        if (ss) {
            if (ss.src.indexOf(ImgSrc) > 0) {
                var aa = document.getElementById("a_id_" + i);
                aa.className = "CurrentSelected";
                try {
                    var objScroll = document.getElementById("imgCenterDiv");
                    objScroll.scrollTop = objScroll.scrollHeight * (i / nLen);
                }
                catch (e) { ; }
                break;
            }
        }
        else
            break;
    }
}

function GetFileName(strURL) {
    var index = strURL.lastIndexOf('/');
    if (index < 0) index = -1;
    return strURL.substring(index + 1);
}

var tmpIMG = document.createElement("img");
tmpIMG.onload = function() {
    var strIMGSRC = GetFileName(this.src);
    var objImgage = document.getElementById("<%=Image1.ClientID%>");
}


function CheckLocalPathInner(objImgURL) {
    var strValue = objImgURL.value;
    if (strValue.length < 1) {
        alert("Local file path is invalid.");
        return false;
    }
    var a = strValue.split(".");
    if (a.length < 1) {
        alert("Only 'bmp','dib','jpg','jpeg','jpe','jfif','tif','tiff','gif','png' supported.");
        return false;
    }
    var ext = a[a.length - 1];
    ext = ext.toLowerCase();
    var allFileSupport = ['bmp', 'dib', 'jpg', 'jpeg', 'jpe', 'jfif', 'tif', 'tiff', 'gif', 'png'];
    var len = allFileSupport.length;
    var i = 0;
    for (i = 0; i < len; ++i) {
        if (allFileSupport[i] == ext)
            break;
    }
    if (i == len) {
        alert("Only 'bmp','dib','jpg','jpeg','jpe','jfif','tif','tiff','gif','png' supported.");
        return false;
    }
    return true;
}


function OnFileChange() {
    var objPath = document.getElementById("txtLocalPath.ClientID");
    var objUploadFile = document.getElementById("upLoadFile.ClientID");
    objPath.value = objUploadFile.value;
}
function ClickUpLoad() {
    var objUploadFile = document.getElementById("<%=upLoadFile.ClientID%>");
    objUploadFile.click();
}
function CheckFileExistInner(objImgURL) {
    var strValue = objImgURL.value;
    strValue = strValue.replace(/^\s+|\s+$/g, '');
    if (strValue.length < 1) {
        alert("URL is invalid.");
        return false;
    }
    return true;
}

function ShowImgSize(ImgSrc) {
    var img = new Image();
    img.src = ImgSrc;
    img.onload = function () {
        varCurrentImageWidth = img.width;
        varCurrentImageHeight = img.height;
        ShowImgSizeInner();
        img = null;
    }
}

function FixSizeInner(objImgage) {
    var height = varCurrentImageHeight;
    var width = varCurrentImageWidth; 
    var ppw = Math.round(MaxWidth * 100.00) / width;
    var pph = Math.round(MaxHeight * 100.00) / height;

    if (ppw > pph && pph > 0) {
        width = (width * pph) / 100;
        height = MaxHeight;
    }
    else if (ppw < pph && ppw > 0) {
        width = MaxWidth;
        height = (height * ppw) / 100;
    }
    else {
        height = MaxHeight;
        width = MaxWidth;
    }

    objImgage.style.width = width + "px";
    objImgage.style.height = height + "px";
}
function OriginSizeInner(objImgage) {
    var height = varCurrentImageHeight; 
    var width = varCurrentImageWidth; 
    if (width != 0 && height != 0) {
        objImgage.style.height = height + "px"; 
        objImgage.style.width = width + "px";
    }

}
function FullSizeInner(objImgage) {
    objImgage.style.width = MaxWidth + "px";
    objImgage.style.height = MaxHeight + "px";
}
function FixWidthInner(objImgage) {
    var height = varCurrentImageHeight;
    var width = varCurrentImageWidth; 
    var pp = 1.0 * MaxWidth / width;
    height = height * pp;
    objImgage.style.width = MaxWidth + "px";
    objImgage.style.height = height + "px";
} 

var _strTempStr = "";       // Store the temp string for display
function appendMessage(strMessage) {
    _strTempStr = strMessage;
    var _divMessageContainer = document.getElementById("DWTemessage");
    if (_divMessageContainer) {
        _divMessageContainer.innerHTML = _strTempStr;
        _divMessageContainer.scrollTop = _divMessageContainer.scrollHeight;
    }
}

var bSelectAll;
function ClickSelectAndUnSelectedAll() {
    if (bSelectAll == true) {
        ClickUnSelectAll();
    }
    else {
        ClickSelectAll();
    }
}

function ClickSelectAll() {
    bSelectAll = true;    
    var   barcodeType   =   document.getElementsByName("BarcodeType");  
    for(i=0;i<barcodeType.length;i++){  
        barcodeType[i].checked   =   true;
    }

    SetSelectButtonImage(false);
}

function ClickUnSelectAll() {
    bSelectAll = false;
    var barcodeType = document.getElementsByName("BarcodeType");
    for (i = 0; i < barcodeType.length; i++) {
        barcodeType[i].checked = false;
    }

    SetSelectButtonImage(true);
}

function SetSelectButtonImage(bSelectAll)
{
    var imageSelect = document.getElementById("ImgSelectAll");
    if(bSelectAll == true)
        imageSelect.src = "Images/SelectAll.png";
    else
        imageSelect.src = "Images/UnSelectAll.png";
}

function ClickCheckBox(obj) {
    var bSelect = obj.checked;
    var i = 0;
    var barcodeType = document.getElementsByName("BarcodeType");
    if (bSelect == true) {
        for (i = 0; i < barcodeType.length; i++) {
            if (barcodeType[i].checked == false)
                break;
        }
        if (i >= barcodeType.length) {
            bSelectAll = true;
            SetSelectButtonImage(false);
        }
    }
    else {
        for (i = 0; i < barcodeType.length; i++) {
            if (barcodeType[i].checked == true)
                break;
        }
        if (i >= barcodeType.length) {
            bSelectAll = false;
            SetSelectButtonImage(true);
        }
    }
}