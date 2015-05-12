using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace BarcodeDLL
{
    public class BarcodeAccess
    {
        private static readonly int MaxSize=1024*1024*5;
        internal static readonly string[] FileExt = { 
                               "bmp","dib",
                               "jpg","jpeg","jpe","jfif",
                               "tif","tiff",
                               "gif",
                               "png",
                               
                                                    };
        public static readonly string[] FileExtMimeType = { 
                               "bmp","image/bmp",
                               "bmp","application/x-MS-bmp",
                               "dib","application/x-dib",
                               "jpg","image/jpeg",
                               "jpg","application/x-jpg",
                               "jpg","image/pjpeg",
                               "jpg","image/jpg",
                               "jpeg","image/jpeg",
                               "jpeg","image/pjpeg",
                               "jpe","image/jpeg",
                               "jpe","application/x-jpe",
                               "jfif","image/jpeg",
                               "jfif","image/pipeg",
                               "tif","image/tiff",
                               "tif","application/x-tif",
                               "tiff","image/tiff",
                               "gif","image/gif",
                               "png","image/png",
                               "png","application/x-png",
                              // "pdf","application/pdf",
                               };
        private static char cSep=System.IO.Path.DirectorySeparatorChar;
        private static string StrPath = AppDomain.CurrentDomain.BaseDirectory+"Images";

        private static string UploadFolder = StrPath + cSep + "Upload";
        private static string DemoFolder = StrPath + cSep + "Demo";

        public static string GetUploadFolder()
        {
            return UploadFolder;
        }

        public static string GetNextFileIndex(string strFileExt, string strSessionID)
        {
            if (strFileExt.ToLower() == "tif" || strFileExt.ToLower() == "tiff")
            {
                strFileExt = "jpg";
            }
            DateTime now = DateTime.Now;
            string strFile = now.ToString("yyyyMMdd_HHmmss_")+now.Millisecond+"_"+(new Random().Next()%1000).ToString();
            string strFileName = strFile + "." + strFileExt.ToLower();
            while (File.Exists(UploadFolder + System.IO.Path.DirectorySeparatorChar + strSessionID + System.IO.Path.DirectorySeparatorChar + strFileName))
            {
                strFile = now.ToString("yyyyMMdd_HHmmss_") + now.Millisecond +"_"+ (new Random().Next() % 1000).ToString();
                strFileName = strFile + "." + strFileExt.ToLower();
            }
            File.Create(UploadFolder + System.IO.Path.DirectorySeparatorChar + strSessionID + System.IO.Path.DirectorySeparatorChar  + strFileName).Close();
            return strFileName;
        }

        internal static string SaveUploadFile(string strFileExt, byte[] data, string strSessionID)
        {
            string strFileName = GetNextFileIndex(strFileExt, strSessionID);
            if (strFileExt.ToLower() == "tif" || strFileExt.ToLower() == "tiff")
            {
                using (MemoryStream memdata = new MemoryStream(data))
                {
                    string strAryFileNmae = "";
                    using (Bitmap map = new Bitmap(memdata))
                    { 
                        string strFileNameTemp = strFileName;
                        int pages = map.GetFrameCount(FrameDimension.Page);
                        for(int i= 0; i< pages; i++)
                        {
                            if (strAryFileNmae.Length == 0)
                                strAryFileNmae = strAryFileNmae + strFileNameTemp;
                            else
                                strAryFileNmae = strAryFileNmae + ":" + strFileNameTemp;
                            map.SelectActiveFrame(FrameDimension.Page, i);
                            Bitmap bitmap = new Bitmap(map);
                            bitmap.Save(UploadFolder + System.IO.Path.DirectorySeparatorChar + strSessionID + System.IO.Path.DirectorySeparatorChar + strFileNameTemp, System.Drawing.Imaging.ImageFormat.Jpeg);
                            if (bitmap != null)
                                bitmap.Dispose();
                            strFileNameTemp = (i + 1).ToString() + strFileName;
                        }
                    }
                    strFileName = strAryFileNmae;
                }
            }
            else
                using (FileStream fs = File.Open(UploadFolder + System.IO.Path.DirectorySeparatorChar + strSessionID + System.IO.Path.DirectorySeparatorChar + strFileName,
                      FileMode.Truncate, FileAccess.Write))
                {
                    fs.Write(data, 0, data.Length);
                }
            return strFileName;
        }

        internal static string SaveLoadeFile(string strFileExt, Bitmap map, string strSessionID)
        {
            string strFileName = GetNextFileIndex(strFileExt, strSessionID);

            map.Save(UploadFolder + System.IO.Path.DirectorySeparatorChar + strSessionID + System.IO.Path.DirectorySeparatorChar + strFileName, System.Drawing.Imaging.ImageFormat.Jpeg);
          
            return strFileName;
        }

        internal static string FetchImageFromURL(string strImgURL, string strSessionID)
        {
           HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strImgURL);
           string strFileName = "";
           using (WebResponse response = req.GetResponse())
           {
               if(response.ContentLength> BarcodeAccess.MaxSize)
               {
                   throw new BarcodeException(" Picture size is too big. Max size: 5MB.");
               }
               using (BufferedStream reader=new BufferedStream( response.GetResponseStream()))
               {
                   byte[] data = new byte[response.ContentLength];
                   int index = 0;
                   while (index < response.ContentLength)
                   {
                       index += reader.Read(data, index, (int)response.ContentLength - index);
                   }
                   int nLen= FileExtMimeType.Length;
                   string strContentType=response.ContentType;
                   string strExt = "";
                   for (int i = 1; i <nLen; i+=2)
                   {
                       if (FileExtMimeType[i] == strContentType)
                       {
                           strExt = FileExtMimeType[i - 1];
                           break;
                       }
                   }
                   if (strExt == "")
                       throw new BarcodeException("URL is invalid image.");
                   strFileName = GetNextFileIndex(strExt, strSessionID);
                   using (FileStream fs = File.Open(UploadFolder + System.IO.Path.DirectorySeparatorChar + strSessionID + System.IO.Path.DirectorySeparatorChar + strFileName,
                         FileMode.Truncate, FileAccess.Write))
                   {
                       fs.Write(data, 0,(int) response.ContentLength);
                   }
               }
           }
           return strFileName;
        }

        internal static string GetImgPathByName(string strImgID, string strSessionID)
        {
             return UploadFolder + System.IO.Path.DirectorySeparatorChar + strSessionID + System.IO.Path.DirectorySeparatorChar + strImgID;
        }


        internal static void DeleteTempFiles(string strPath)
        {
            try
            {
                string[] aryFiles = System.IO.Directory.GetFiles(strPath);
                foreach (string strFileName in aryFiles)
                {
                    try
                    {
                        int index = strFileName.LastIndexOf("\\");
                        if (index > 0)
                        {
                            string strFileNameTemp = strFileName.Substring(index + 1, strFileName.Length - index - 1);
                            if (strFileNameTemp.StartsWith("Demo_") == false)
                            {
                                File.Delete(strFileName);
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }

        }

        internal static void DeleteFolder(string strSessionID)
        {
            try
            {
                string strFullPath = BarcodeAccess.GetUploadFolder() + System.IO.Path.DirectorySeparatorChar + strSessionID;

                if (Directory.Exists(strFullPath))
                {
                    try
                    {
                        DeleteTempFiles(strFullPath);
                    }
                    catch { }
                    Directory.Delete(strFullPath);
                }
            }
            catch { }
        }

        internal static void CreateFolder(string strSessionID)
        {
            string strDir = BarcodeAccess.GetUploadFolder() + System.IO.Path.DirectorySeparatorChar + strSessionID;
            if (!Directory.Exists(strDir))
                 Directory.CreateDirectory(strDir);
        }
    }
}
