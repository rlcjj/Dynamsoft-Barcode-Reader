using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using BarcodeDLL;

namespace BarcodeWeb
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["SessionID"] = System.Web.HttpContext.Current.Session.SessionID.ToString();
            try
            {
                BarcodeMode.DeleteFolder(Session["SessionID"].ToString());
                Session.Timeout = 20;
            }
            catch { }
            BarcodeMode.CreateFolder(Session["SessionID"].ToString());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
            BarcodeMode.DeleteFolder(Session["SessionID"].ToString());
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}