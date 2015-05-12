function _Function_AjaxGetXMLHttpRequest() {
    var xmlHttpRequestObject = null;
    try {
        xmlHttpRequestObject = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            xmlHttpRequestObject = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (e) {
            xmlHttpRequestObject = new XMLHttpRequest();
        }
    }

    return xmlHttpRequestObject;
}

function _Function_AjaxPOST(serverURL, body, parseResponseFunction, objClientObject) {
    var xmlHttpRequestObject = _Function_AjaxGetXMLHttpRequest();
    xmlHttpRequestObject.open("POST", serverURL, true);
    xmlHttpRequestObject.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    xmlHttpRequestObject.onreadystatechange = function() {
        if (xmlHttpRequestObject.readyState == 4) {
            if (xmlHttpRequestObject.status == 200) {
                if (parseResponseFunction != null && typeof (parseResponseFunction) == "function") {
                    parseResponseFunction(xmlHttpRequestObject, objClientObject);
                }
            }
            else if (xmlHttpRequestObject.status == 404) {
                //alert("Requested URL is not found.");
            }
            else if (xmlHttpRequestObject.status == 403) {
                //alert("Access denied.");
            }
            else {
                //alert("Ajax Status is "+ xmlHttpRequestObject.status);
            }
        }
    };


    xmlHttpRequestObject.send(body);
}



function _Function_AddEvent(element, eventType, fn, useCapture) {
    if (element.addEventListener) { element.addEventListener(eventType, fn, useCapture); return true; }
    else if (element.attachEvent) {
        element["e" + eventType + fn] = fn;
        element[eventType + fn] = function() { element["e" + eventType + fn](window.event); };
        var r = element.attachEvent("on" + eventType, element[eventType + fn]);
        return r;
    }
    else { element["on" + eventType] = fn; }
}

function _Function_RemoveEvent(element, eventType, fn, useCapture) {
    if (element.removeEventListener) {
        element.removeEventListener(eventType, fn, useCapture);
        return true;
    }
    else if (element.detachEvent) {
        var r = element.detachEvent("on" + eventType, element[eventType + fn]);
        element["e" + eventType + fn] = null;
        element[eventType + fn] = null;
        return r;
    }
    else { element["on" + eventType] = null; }
}

function _Function_GetElementById(strControlId) {
    if (document.getElementById) {
        return document.getElementById(strControlId);
    }
    else {
        return document.all[strControlId];
    }
}

function _Function_IsDigit(strString) {
    var strPattern = /^[+-]{0,1}[0-9]{1,20}$/;
    return strPattern.exec(strString);
}

function _Function_IsDouble(strString, iDigitNum) {
    var str = "^[+-]{0,1}[0-9]{0," + (18 - iDigitNum) + "}(.[0-9]{1," + (iDigitNum) + "}){0,1}$";
    var reg = new RegExp(str, "ig");
    return reg.exec(strString);
}

function _Function_GetBoolean(value) {
    if (typeof (value) === "string") {
        return (value.toLowerCase() === "true");
    }
    else if (typeof (value) === "boolean") {
        return value;
    }
    else {
        return false;
    }
}


/* ITA_ExplorerType Object */
function _ExplorerType() {
    var ua = (navigator.userAgent.toLowerCase());

    this.isFirefox = ua.indexOf("gecko") != -1;
    this.isOpera = ua.indexOf("opera") != -1;
    this.isIE = !this.isOpera && ua.indexOf("msie") != -1;
}

_ExplorerType.prototype.IsIE = function() { return this.isIE; }


/* Base 64 */
var _ConstValue_Base64EncodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
var _ConstValue_Base64DecodeChars = new Array(
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63,
    52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1,
    -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
    15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1,
    -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
    41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1);

function _Function_Base64Encode(str) {
    var out, i, len;
    var c1, c2, c3;

    len = str.length;
    i = 0;
    out = "";
    while (i < len) {
        c1 = str.charCodeAt(i++) & 0xff;
        if (i == len) {
            out += _ConstValue_Base64EncodeChars.charAt(c1 >> 2);
            out += _ConstValue_Base64EncodeChars.charAt((c1 & 0x3) << 4);
            out += "==";
            break;
        }
        c2 = str.charCodeAt(i++);
        if (i == len) {
            out += _ConstValue_Base64EncodeChars.charAt(c1 >> 2);
            out += _ConstValue_Base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
            out += _ConstValue_Base64EncodeChars.charAt((c2 & 0xF) << 2);
            out += "=";
            break;
        }
        c3 = str.charCodeAt(i++);
        out += _ConstValue_Base64EncodeChars.charAt(c1 >> 2);
        out += _ConstValue_Base64EncodeChars.charAt(((c1 & 0x3) << 4) | ((c2 & 0xF0) >> 4));
        out += _ConstValue_Base64EncodeChars.charAt(((c2 & 0xF) << 2) | ((c3 & 0xC0) >> 6));
        out += _ConstValue_Base64EncodeChars.charAt(c3 & 0x3F);
    }
    return out;
}

function _Function_Base64Decode(str) {
    var c1, c2, c3, c4;
    var i, len, out;

    len = str.length;
    i = 0;
    out = "";
    while (i < len) {
        /* c1 */
        do {
            c1 = _ConstValue_Base64DecodeChars[str.charCodeAt(i++) & 0xff];
        } while (i < len && c1 == -1);
        if (c1 == -1)
            break;

        /* c2 */
        do {
            c2 = _ConstValue_Base64DecodeChars[str.charCodeAt(i++) & 0xff];
        } while (i < len && c2 == -1);
        if (c2 == -1)
            break;

        out += String.fromCharCode((c1 << 2) | ((c2 & 0x30) >> 4));

        /* c3 */
        do {
            c3 = str.charCodeAt(i++) & 0xff;
            if (c3 == 61)
                return out;
            c3 = _ConstValue_Base64DecodeChars[c3];
        } while (i < len && c3 == -1);
        if (c3 == -1)
            break;

        out += String.fromCharCode(((c2 & 0XF) << 4) | ((c3 & 0x3C) >> 2));

        /* c4 */
        do {
            c4 = str.charCodeAt(i++) & 0xff;
            if (c4 == 61)
                return out;
            c4 = _ConstValue_Base64DecodeChars[c4];
        } while (i < len && c4 == -1);
        if (c4 == -1)
            break;
        out += String.fromCharCode(((c3 & 0x03) << 6) | c4);
    }
    return out;
}

function _Function_UTF16To8(str) {
    var out, i, len, c;

    out = "";
    len = str.length;
    for (i = 0; i < len; i++) {
        c = str.charCodeAt(i);
        if ((c >= 0x0001) && (c <= 0x007F)) {
            out += str.charAt(i);
        } else if (c > 0x07FF) {
            out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
            out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        } else {
            out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        }
    }
    return out;
}

function _Function_UTF8To16(str) {
    var out, i, len, c;
    var char2, char3;

    out = "";
    len = str.length;
    i = 0;
    while (i < len) {
        c = str.charCodeAt(i++);
        switch (c >> 4) {
            case 0: case 1: case 2: case 3: case 4: case 5: case 6: case 7:
                // 0xxxxxxx
                out += str.charAt(i - 1);
                break;
            case 12: case 13:
                // 110x xxxx   10xx xxxx
                char2 = str.charCodeAt(i++);
                out += String.fromCharCode(((c & 0x1F) << 6) | (char2 & 0x3F));
                break;
            case 14:
                // 1110 xxxx  10xx xxxx  10xx xxxx
                char2 = str.charCodeAt(i++);
                char3 = str.charCodeAt(i++);
                out += String.fromCharCode(((c & 0x0F) << 12) |
                       ((char2 & 0x3F) << 6) |
                       ((char3 & 0x3F) << 0));
                break;
        }
    }

    return out;
}


function _Function_EncodeXmlString(strSrcString) {
    return _Function_Base64Encode(_Function_UTF16To8(strSrcString));
}

function _Function_DecodeXmlString(strSrcString) {
    return _Function_UTF8To16(_Function_Base64Decode(strSrcString));
}