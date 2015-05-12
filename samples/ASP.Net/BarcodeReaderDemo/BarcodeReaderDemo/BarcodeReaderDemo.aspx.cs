using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BarcodeDLL;
using System.Drawing;
using System.IO;

namespace BarcodeWeb
{//http://www.chinfun.com/bbs/attachments/month_0902/20090210_bcb6a9a306c4b072a8c6d85ehCwBVKPW.jpg
    public partial class BarcodePage : System.Web.UI.Page
    {
        public string SessionID = "";
        public int iWidth = 0;
        public int iHeight = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SessionID = Session["SessionID"].ToString();

            if (!IsPostBack)
            {
                hide_showInputURLImage.Value = "0";
                Initial();
                Image1.Style.Add("border-width","1px");
                hide_ShowType.Value = "0";

            }

        }
        private void Initial()
        {
            string strFilesPaht = "";
            string strSelectPath = "";
            string strFilePath = Server.MapPath("/") + "..\\..\\..\\..\\Images\\";  
            SetFilesPaht(strFilePath, "AllSupportedBarcodeTypes.tif", true, ref strFilesPaht);
            strSelectPath = strFilesPaht;
            SetFilesPaht(strFilePath, "Check#Code_128.tif", false, ref strFilesPaht);
            SetFilesPaht(strFilePath, "DamagedBarcodes.tif", false, ref strFilesPaht);
            SetFilesPaht(strFilePath, "EAN_13.jpg", false, ref strFilesPaht);
            SetFilesPaht(strFilePath, "MutliBarcodes.tif", false, ref strFilesPaht);
            SetFilesPaht(strFilePath, "Quote#Code_39.tif", false, ref strFilesPaht);
            SetFilesPaht(strFilePath, "UPC_A.jpg", false, ref strFilesPaht);

            hide_allImgURL.Value = strFilesPaht;
            hide_ImgFileName.Value = strSelectPath;
            Image1.ImageUrl = strSelectPath;
        }

        public bool SetFilesPaht(string strFilePath, string strFileName, bool bFirst, ref string strFilesPaht)
        {
            string strReturnPath = BarcodeMode.LoadImage(strFilePath + strFileName, SessionID);
            if (strReturnPath != "")
            {
                string strPath = BarcodeAccess.GetUploadFolder() + System.IO.Path.DirectorySeparatorChar + SessionID + System.IO.Path.DirectorySeparatorChar + strReturnPath;
                strPath = strPath.Replace("\\", "/");
                if (bFirst)
                {
                    strFilesPaht = "Images/Upload/" + SessionID + "/" + strReturnPath;
                }
                else
                {
                    strFilesPaht = strFilesPaht + ":Images/Upload/" + SessionID + "/" + strReturnPath;
                }
                return true;
            }
            return false;
        }

    }
}
