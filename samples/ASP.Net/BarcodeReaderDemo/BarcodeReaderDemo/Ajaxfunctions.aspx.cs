using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BarcodeDLL;
using System.Text;
using System.Drawing;

namespace BarcodeWeb
{
    public partial class  Ajaxfunctions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(this.Request.Form["DW_AjaxMethod"] != null)
			{
                String strMethod = this.Request.Form["DW_AjaxMethod"];
                if (strMethod == "DoBarcode")
                {
                    string strReturnValue = "";

                    string strImgID = Ajaxfunctions.DecodeValueInXml(this.Request.Form["FileName"]);
                    string strFormat = Ajaxfunctions.DecodeValueInXml(this.Request.Form["BarcodeFormat"]);
                    string strSessionID = Ajaxfunctions.DecodeValueInXml(this.Request.Form["SessionID"]);
                    Int64 iFormat = Convert.ToInt64(strFormat);
                    strReturnValue = this.DoBarcodeInner(strImgID, iFormat, strSessionID);
                    Response.Write(strReturnValue);
                }
                else if (strMethod == "GetImageSize")
                {
                    string strReturnValue = "";

                    string strFileName = Ajaxfunctions.DecodeValueInXml(this.Request.Form["FileName"]);

                    strReturnValue = this.GetImageSize(strFileName);
                    Response.Write(strReturnValue);
                }
            }
            else
            {
                Response.Write("Error.");
            }
        }

        private string DoBarcodeInner(string strImgID, Int64 iFormat, string strSessionID)
        {
            string strReturnValue = "";
            string strResult = "";
            try
            {
                string strBarcodeInfo = BarcodeMode.Barcode(strImgID, iFormat, strSessionID, ref strResult);
                strReturnValue = "OK;" + strBarcodeInfo + ";" + strResult;
            }

            catch (Exception exp)
            {
                strReturnValue = "EXP;" + exp.Message.ToString() + ";" + strResult;
            }
            finally
            {

            }
            return strReturnValue;
        }


        private string GetImageSize(string strFileName)
        {
            string strReturnValue = "";
            int iWidth = 0;
            int iHeight = 0;
            Bitmap objImage = null;
            try
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + strFileName;
                strPath = strPath.Replace("\\", "/");
                objImage = new Bitmap(strPath);
                iWidth = objImage.Width;
                iHeight = objImage.Height;
                strReturnValue = "OK;" + iWidth + ";" + iHeight; 
            }
            catch (Exception exp)
            {
                strReturnValue = "EXP;" + exp.Message.ToString();
            }
            finally
            {
                if (objImage != null)
                {
                    objImage.Dispose();
                }  
            }
            return strReturnValue;
        }


        public static string DecodeValueInXml(string sourceString)
        {
            if (sourceString == null || sourceString == "")
            {
                return "";
            }

            string src = sourceString.Replace(" ", "+");

            return Encoding.UTF8.GetString(Convert.FromBase64String(src));
        }
    }
}
