using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dynamsoft.Barcode;

namespace BarcodeReaderDemo_CSharp
{
    public partial class ImageViewer : PictureBox
    {
        private bool m_bFitWindow = true;
        //private List<KeyValuePair<Image, BarcodeResult[]>> m_data = new List<KeyValuePair<Image,BarcodeResult[]>>();
        private List<Image> m_data = new List<Image>();
        private int m_index = 0;

        public ImageViewer()
        {
            InitializeComponent();
            //this.Paint += new PaintEventHandler(ImageViewer_Paint);
            this.MouseDown += new MouseEventHandler(ImageViewer_MouseDown);
            this.MouseMove += new MouseEventHandler(ImageViewer_MouseMove);
            this.MouseUp += new MouseEventHandler(ImageViewer_MouseUp);
        }

        private bool hasClicked = false;
        private Point m_mousePosition;
        private Pen pen = new Pen(Color.Blue, 1);
        void ImageViewer_MouseUp(object sender, MouseEventArgs e)
        {
            hasClicked = false;
        }

        void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (hasClicked)
            {
                Refresh();
                Graphics g = Graphics.FromHwnd(this.Handle);
                g.DrawRectangle(pen, new Rectangle(m_mousePosition, new Size(e.Location.X - m_mousePosition.X, e.Location.Y - m_mousePosition.Y)));
            }
        }

        void ImageViewer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                hasClicked = true;
                m_mousePosition = e.Location;
            }
        }

        //void ImageViewer_Paint(object sender, PaintEventArgs e)
        //{
        //    if (m_data != null && m_data.Count > 0)
        //    {
        //        float fact = 1;
        //        int currentIndex = m_index % m_data.Count;
        //        Image img = m_data[currentIndex].Key;
        //        if (img != null)
        //        {
        //            if (m_bFitWindow)
        //            {
        //            }
        //        }
        //    }
        //}

        public bool FitWindow
        {
            get
            {
                return m_bFitWindow;
            }
            set
            {
                m_bFitWindow = value;
                Refresh();
            }
        }

        public int CurrentIndex
        {
            get
            {
                return m_index;
            }
            set
            {
                if (value >= 0 && value < m_data.Count)
                {
                    m_index = value;
                    Refresh();
                }
            }
        }

        public void SetImages(List<Image> imgs)
        {
            for (int i = m_data.Count - 1; i >= 0; i--)
            {
                if (m_data[i] != null)
                    m_data[i].Dispose();
            }
            m_data.Clear();

            if (imgs != null)
            {
                m_data = imgs;
            }
        }

        public void SetBarcodeResults(int index, BarcodeResult[] results)
        {
            if (index >= 0 && index < m_data.Count)
            {
                if (m_data[index] != null && results.Length > 0)
                {
                    Pen pen = new Pen(Color.Red, 1);
                    Brush textBrush = new SolidBrush(Color.Blue);
                    Graphics g = Graphics.FromImage(m_data[index]);
                    for (int i = results.Length - 1; i >= 0; i--)
                    {
                        if (results[i] != null)
                        {
                            g.DrawRectangle(pen, results[i].BoundingRect);
                            g.DrawString("[" + i + "] {" + results[i].BarcodeText + "}", SystemFonts.DefaultFont, textBrush, results[i].BoundingRect);
                        }
                    }
                    pen.Dispose();
                    textBrush.Dispose();
                    g.Dispose();
                    if (index == m_index)
                        Refresh();
                }
            }
        }

        //public List<KeyValuePair<Image, BarcodeResult[]>> Data
        //{
        //    get
        //    {
        //        return m_data;
        //    }
        //    set
        //    {
        //        m_data = value;
        //        Refresh();
        //    }
        //}
    }
}
