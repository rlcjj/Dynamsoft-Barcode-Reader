using System;
using System.Collections.Generic;
using System.Text;

namespace BarcodeDLL
{
    public class BarcodeException:ApplicationException
    {
        public BarcodeException():base(){}
        public BarcodeException(string strMessage) : base(strMessage) { }
        public BarcodeException(string strMessage, Exception innerException) :
            base(strMessage, innerException) { }
    }
}
