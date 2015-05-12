using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Dynamsoft.Barcode;
using System.Drawing;
using System.Drawing.Imaging;

namespace BarcodeReaderDemo_CSharp
{
    public partial class Form1 : Form
    {

        private bool m_bFitWindow = false;
        private List<Image> m_data = new List<Image>();
        private Dictionary<Image, Image> m_results = new Dictionary<Image, Image>();
        private int m_index = -1;
        private string filePath = null;
        private String lastOpenedDirectory = Application.ExecutablePath;
        private int iCheckedFormatCount = 9;

        public bool FitWindow
        {
            get
            {
                return m_bFitWindow;
            }
            set
            {
                if (m_bFitWindow != value)
                {
                    m_bFitWindow = value;
                    if (m_bFitWindow)
                    {
                        panel1.AutoScroll = false;
                        imageViewer.SizeMode = PictureBoxSizeMode.Zoom;
                        imageViewer.Dock = DockStyle.Fill;
                    }
                    else
                    {
                        panel1.AutoScroll = true;
                        imageViewer.SizeMode = PictureBoxSizeMode.AutoSize;
                        imageViewer.Dock = DockStyle.None;
                    }
                    panel1.Refresh();
                }
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
                if (value >= 0 && value < m_data.Count && m_index != value)
                {
                    m_index = value;
                    tbxCurrentImageIndex.Text = (value + 1).ToString();                  
                }
                SetImageViewerImage();
            }
        }

        public Form1()
        {
            InitializeComponent();
            chkFitWindow.Checked = true;
            int index = -1;
            lastOpenedDirectory.Replace('/', '\\');
            if ((index = lastOpenedDirectory.LastIndexOf("Samples")) > 0)
            {
                lastOpenedDirectory = lastOpenedDirectory.Substring(0, index);
                lastOpenedDirectory += "Images\\";
            }
            if (!System.IO.Directory.Exists(lastOpenedDirectory))
                lastOpenedDirectory = "";
        }

        private void chkFitWindow_CheckedChanged(object sender, EventArgs e)
        {
            FitWindow = chkFitWindow.Checked;
            //imageViewer.FitWindow = chkFitWindow.Checked;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            bool selectall = false;
            int count = gbBarcodeType.Controls.Count;
            for (int i = 0; i < count; i++)
                if (gbBarcodeType.Controls[i] is CheckBox)
                    if (!((CheckBox)gbBarcodeType.Controls[i]).Checked)
                    {
                        selectall = true;
                        break;
                    }

            for (int i = 0; i < count; i++)
                if (gbBarcodeType.Controls[i] is CheckBox)
                    ((CheckBox)gbBarcodeType.Controls[i]).Checked = selectall;

            if (selectall)
                btnSelectAll.Text = "Unselect All";
            else
                btnSelectAll.Text = "Select All";
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "All Supported Images(*.BMP,*.PNG,*.JPEG,*.JPG,*.JPE,*.JFIF,*.TIF,*.TIFF)|*.BMP;*.PNG;*.JPEG;*.JPG;*.JPE;*.JFIF;*.TIF;*.TIFF|JPEG|*.JPG;*.JPEG;*.JPE;*.JFIF|BMP|*.BMP|PNG|*.PNG|TIFF|*.TIF;*.TIFF";
                //dlg.Multiselect = true;
                dlg.InitialDirectory = lastOpenedDirectory;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    lastOpenedDirectory = System.IO.Directory.GetParent(dlg.FileName).FullName;
                    for (int i = 0; i < m_data.Count; i++)
                        if (m_data[i] != null)
                        {
                            if (m_results.ContainsKey(m_data[i]) && m_results[m_data[i]] != null)
                                m_results[m_data[i]].Dispose();
                            m_data[i].Dispose();
                        }
                    m_data.Clear();
                    m_results.Clear();
                    tbResults.Clear();

                    //foreach (string file in dlg.FileNames)
                    //{
                    filePath = dlg.FileName;
                    Image img = null;
                    try
                    {
                        img = Image.FromFile(filePath);
                        Guid frameGuid = img.FrameDimensionsList[0];
                        FrameDimension demension = new FrameDimension(frameGuid);
                        int pageCount = img.GetFrameCount(demension);
                        System.Collections.ArrayList lastExceptionalIndice = new System.Collections.ArrayList(pageCount);
                        for (int i = 0; i < pageCount; i++)
                        {
                            try
                            {
                                img.SelectActiveFrame(demension, i);
                            }
                            catch (Exception exp)
                            {
                                img.Dispose();
                                img = Image.FromFile(filePath);
                                if (!lastExceptionalIndice.Contains(i))
                                {
                                    lastExceptionalIndice.Add(i);
                                    if (i > 0)
                                    {
                                        i--;
                                        if (m_data[i] != null)
                                            m_data[i].Dispose();
                                        m_data.RemoveAt(i);
                                    }
                                }
                                img.SelectActiveFrame(demension, i);
                            }
                            m_data.Add(new Bitmap(img));
                        }

                        img.Dispose();
                        img = null;
                    }
                    finally
                    {
                        if (img != null)
                            img.Dispose();
                    }
                    //}
                    if (m_data.Count > 0 && iCheckedFormatCount > 0)
                        btnRead.Enabled = true;
                    CurrentIndex = m_data.Count - 1;
                    tbxTotalImageNum.Text = m_data.Count.ToString();
                    if (m_data.Count > 1)
                    {
                        EnableControls(picboxFirst);
                        EnableControls(picboxPrevious);
                        //EnableControls(picboxNext);
                        //EnableControls(picboxLast);
                    }
                    else
                    {
                        DisableControls(picboxFirst);
                        DisableControls(picboxPrevious);
                        DisableControls(picboxNext);
                        DisableControls(picboxLast);
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Barcode Reader Demo", MessageBoxButtons.OK);
            }
        }

        private void SetImageViewerImage()
        {
            if (m_results.ContainsKey(m_data[m_index]) && m_results[m_data[m_index]] != null)
                imageViewer.Image = m_results[m_data[m_index]];
            else
                imageViewer.Image = m_data[m_index];
            //if (imageViewer.Image != null)
            //    imageViewer.Image.Dispose();
            //imageViewer.Image = null;

            //if (img != null)
            //    imageViewer.Image = (Image)img.Clone();
        }

        private BarcodeFormat GetFormats()
        {
            BarcodeFormat? formats = null;
            if (chkCode39.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.CODE_39;
                else
                    formats = formats | BarcodeFormat.CODE_39;
            if (chkCode128.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.CODE_128;
                else
                    formats = formats | BarcodeFormat.CODE_128;
            if (chkCode93.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.CODE_93;
                else
                    formats = formats | BarcodeFormat.CODE_93;
            if (chkCodabar.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.CODABAR;
                else
                    formats = formats | BarcodeFormat.CODABAR;
            if (chkITF.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.ITF;
                else
                    formats = formats | BarcodeFormat.ITF;
            if (chkUPCA.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.UPC_A;
                else
                    formats = formats | BarcodeFormat.UPC_A;
            if (chkUPCE.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.UPC_E;
                else
                    formats = formats | BarcodeFormat.UPC_E;
            if (chkEAN8.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.EAN_8;
                else
                    formats = formats | BarcodeFormat.EAN_8;
            if (chkEAN13.Checked)
                if (!formats.HasValue)
                    formats = BarcodeFormat.EAN_13;
                else
                    formats = formats | BarcodeFormat.EAN_13;
            return !formats.HasValue ? BarcodeFormat.OneD : formats.Value;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (imageViewer.Image != null)
            {
                //Rectangle rect = new Rectangle();
                BarcodeReader reader = new Dynamsoft.Barcode.BarcodeReader();
                try
                {
                    ReaderOptions ro = new ReaderOptions();
                    ro.BarcodeFormats = GetFormats();
                    ro.MaxBarcodesToReadPerPage = int.Parse(tbMaximumNum.Text);
                    reader.ReaderOptions = ro;
                    reader.LicenseKeys = "<Input your license key here>";
                    DateTime beforeRead = DateTime.Now;
                    BarcodeResult[] barcodes = reader.DecodeFile(filePath);//, rect);             
                    DateTime afterRead = DateTime.Now;
                    int timeElapsed = (afterRead - beforeRead).Milliseconds;
                    ShowBarcodeResults(barcodes, timeElapsed);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message, "Barcode Reader Demo", MessageBoxButtons.OK);
                }
                finally
                {
                    reader.Dispose();
                }
            }
        }

        private void ShowBarcodeResults(BarcodeResult[] barcodeResults, float timeElapsed)
        {
            tbResults.Clear();
            List<Image> imgs = new List<Image>(m_results.Keys);
            foreach (Image img in imgs)
            {
                if (m_results[img] != null)
                {
                    m_results[img].Dispose();
                    m_results[img] = null;
                }
            }

            if (barcodeResults != null && barcodeResults.Length > 0)
            {
                tbResults.AppendText(String.Format("Total barcode(s) found: {0}. Total time spent: {1} seconds\r\n", barcodeResults.Length, (timeElapsed / 1000)));
                for (int i = 0; i < barcodeResults.Length; i++)
                {
                    tbResults.AppendText(String.Format("  Barcode {0}:\r\n", i + 1));
                    tbResults.AppendText(String.Format("    Page: {0}\r\n", barcodeResults[i].PageNumber));
                    tbResults.AppendText(String.Format("    Type: {0}\r\n", barcodeResults[i].BarcodeFormat));
                    tbResults.AppendText(String.Format("    Value: {0}\r\n", barcodeResults[i].BarcodeText));
                    tbResults.AppendText(String.Format("    Region: {{Left: {0}, Top: {1}, Width: {2}, Height: {3}}}\r\n", barcodeResults[i].BoundingRect.Left, barcodeResults[i].BoundingRect.Top, barcodeResults[i].BoundingRect.Width, barcodeResults[i].BoundingRect.Height));
                    tbResults.AppendText("\r\n");
                }

                Pen pen = new Pen(Color.Red, 1);
                Brush textBrush = new SolidBrush(Color.Blue);
                Image img = null;
                Graphics g = null;
                int index = -1;
                for (int i = barcodeResults.Length - 1; i >= 0; i--)
                {
                    if (barcodeResults[i] != null)
                    {
                        if (barcodeResults[i].PageNumber != (index+1))
                        {
                            if (g != null)
                                g.Dispose();
                            if (img != null && index >= 0 && index < m_data.Count)
                                if (m_results.ContainsKey(m_data[index]))
                                {
                                    if (m_results[m_data[index]] != null)
                                        m_results[m_data[index]].Dispose();
                                    m_results[m_data[index]] = img;
                                }
                                else
                                    m_results.Add(m_data[index], img);

                            index = barcodeResults[i].PageNumber-1;
                            img = (Image)m_data[index].Clone();
                            g = Graphics.FromImage(img);
                        }
                        Rectangle rect = barcodeResults[i].BoundingRect;
                        g.DrawRectangle(pen, rect);
                        Font textFont = SystemFonts.DefaultFont;
                        int iHeight = textFont.Height * 3 / 2;
                        g.DrawString("[" + i + "] {" + barcodeResults[i].BarcodeText + "}", textFont, textBrush, new Rectangle(rect.Left, rect.Top - iHeight < 0 ? 0 : rect.Top - iHeight, 0, iHeight), new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap));
                    }
                }
                pen.Dispose();
                textBrush.Dispose();
                if (g != null)
                    g.Dispose();
                if (img != null && index >= 0 && index < m_data.Count)
                    if (m_results.ContainsKey(m_data[index]))
                    {
                        if (m_results[m_data[index]] != null)
                            m_results[m_data[index]].Dispose();
                        m_results[m_data[index]] = img;
                    }
                    else
                        m_results.Add(m_data[index], img);

                SetImageViewerImage();
            }
            else
                tbResults.AppendText(String.Format("No barcode found. Total time spent: {0} seconds\r\n", (timeElapsed / 1000)));
        }

        private void tbMaximumNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(e.KeyChar.ToString());
                if (!char.IsDigit(e.KeyChar) || array.LongLength == 2) e.Handled = true;
                if ((tbMaximumNum.Text.Length == 0 || tbMaximumNum.SelectionLength == tbMaximumNum.Text.Length) && e.KeyChar == '0')
                    e.Handled = true;
            }
        }



        #region regist Event For All PictureBox Buttons
        private void picbox_MouseEnter(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                if ((sender as PictureBox).Enabled == true)
                {
                    (sender as PictureBox).Image =
                        (Image)Properties.Resources.ResourceManager.GetObject((sender as PictureBox).Name + "_Enter");
                }
            }
        }

        private void picbox_MouseLeave(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                if ((sender as PictureBox).Enabled == true)
                {
                    (sender as PictureBox).Image =
                        (Image)Properties.Resources.ResourceManager.GetObject((sender as PictureBox).Name + "_Leave");
                }
            }
        }

        private void picbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is PictureBox)
            {
                if ((sender as PictureBox).Enabled == true)
                {
                    (sender as PictureBox).Image =
                        (Image)Properties.Resources.ResourceManager.GetObject((sender as PictureBox).Name + "_Down");
                }
            }
        }

        private void picbox_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is PictureBox)
            {
                if ((sender as PictureBox).Enabled == true)
                {
                    (sender as PictureBox).Image =
                        (Image)Properties.Resources.ResourceManager.GetObject((sender as PictureBox).Name + "_Enter");
                }
            }

        }

        private void DisableControls(object sender)
        {
            if (sender is PictureBox)
            {
                (sender as PictureBox).Image =
                    (Image)Properties.Resources.ResourceManager.GetObject((sender as PictureBox).Name + "_Disabled");
                (sender as PictureBox).Enabled = false;
            }
            else
            {
                (sender as Control).Enabled = false;
            }
        }

        private void EnableControls(object sender)
        {
            if (sender is PictureBox)
            {
                (sender as PictureBox).Image =
                    (Image)Properties.Resources.ResourceManager.GetObject((sender as PictureBox).Name + "_Leave");
                (sender as PictureBox).Enabled = true;
            }
            else
            {
                (sender as Control).Enabled = true;
            }
        }

        private void picboxFirst_Click(object sender, EventArgs e)
        {
            if (picboxFirst.Enabled)
            {
                CurrentIndex = 0;
                DisableControls(picboxFirst);
                DisableControls(picboxPrevious);
                EnableControls(picboxNext);
                EnableControls(picboxLast);
            }
        }

        private void picboxLast_Click(object sender, EventArgs e)
        {
            if (picboxLast.Enabled)
            {
                CurrentIndex = m_data.Count - 1;
                DisableControls(picboxLast);
                DisableControls(picboxNext);
                EnableControls(picboxPrevious);
                EnableControls(picboxFirst);
            }
        }

        private void picboxPrevious_Click(object sender, EventArgs e)
        {
            if (picboxPrevious.Enabled)
            {
                CurrentIndex--;
                if (CurrentIndex == 0)
                {
                    DisableControls(picboxFirst);
                    DisableControls(picboxPrevious);
                }
                EnableControls(picboxNext);
                EnableControls(picboxLast);
            }
        }

        private void picboxNext_Click(object sender, EventArgs e)
        {
            CurrentIndex++;
            if (picboxNext.Enabled)
            {
                if (CurrentIndex == m_data.Count - 1)
                {
                    DisableControls(picboxLast);
                    DisableControls(picboxNext);
                }
                EnableControls(picboxPrevious);
                EnableControls(picboxFirst);
            }
        }

        #endregion

        private void chkFormat_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            if (chkbox.Checked)
                iCheckedFormatCount++;
            else
                iCheckedFormatCount--;

            if (m_data.Count > 0 && iCheckedFormatCount > 0)
                btnRead.Enabled = true;
            else
            {
                btnRead.Enabled = false;
            }

            if (iCheckedFormatCount < 9)
                btnSelectAll.Text = "Select All";
            else
                btnSelectAll.Text = "Unselect All";
        }
    }
}
