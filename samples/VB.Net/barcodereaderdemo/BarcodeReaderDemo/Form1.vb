Imports System.Collections
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports Dynamsoft.Barcode
Imports System

Public Class Form1

    Private m_bFitWindow As Boolean = False
    Private m_data As List(Of Image) = New List(Of Image)()
    Private m_results As Dictionary(Of Image, Image) = New Dictionary(Of Image, Image)()
    Private m_index As Integer = -1
    Private filePath As String = Nothing
    Private lastOpenedDirectory As String = Application.ExecutablePath
    Private iCheckedFormatCount As Integer = 0

    Public Property FitWindow() As Boolean
        Get
            Return m_bFitWindow
        End Get
        Set(ByVal value As Boolean)
            If (Not m_bFitWindow = value) Then
                m_bFitWindow = value
                If (m_bFitWindow) Then
                    panel1.AutoScroll = False
                    imageViewer.SizeMode = PictureBoxSizeMode.Zoom
                    imageViewer.Dock = DockStyle.Fill
                Else
                    panel1.AutoScroll = True
                    imageViewer.SizeMode = PictureBoxSizeMode.AutoSize
                    imageViewer.Dock = DockStyle.None
                End If
                panel1.Refresh()
            End If
        End Set
    End Property

    Public Property CurrentIndex() As Integer
        Get
            Return m_index
        End Get
        Set(ByVal value As Integer)
            If (value >= 0 And value < m_data.Count And (Not m_index = value)) Then
                m_index = value
                tbxCurrentImageIndex.Text = (value + 1).ToString()
            End If
            SetImageViewerImage() 'update Image property of imageviewer when Open an file(under the case, m_index may equal to value)
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
        chkFitWindow.Checked = True
        lastOpenedDirectory.Replace("/", "\")
        Dim index As Integer = lastOpenedDirectory.LastIndexOf("Samples")
        If (index > 0) Then
            lastOpenedDirectory = lastOpenedDirectory.Substring(0, index)
            lastOpenedDirectory += "Images\"
        End If
        If (Not System.IO.Directory.Exists(lastOpenedDirectory)) Then
            lastOpenedDirectory = ""
        End If
    End Sub

    Private Sub chkFitWindow_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkFitWindow.CheckedChanged
        FitWindow = chkFitWindow.Checked
    End Sub

    Private Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectAll.Click
        Dim selectall As Boolean = False
        Dim count As Integer = gbBarcodeType.Controls.Count
        Dim i As Integer
        For i = 0 To count - 1 Step 1
            If (TypeOf gbBarcodeType.Controls(i) Is CheckBox) Then
                If (Not DirectCast(gbBarcodeType.Controls(i), CheckBox).Checked) Then
                    selectall = True
                    Exit For
                End If
            End If
        Next

        For i = 0 To count - 1 Step 1
            If (TypeOf gbBarcodeType.Controls(i) Is CheckBox) Then
                DirectCast(gbBarcodeType.Controls(i), CheckBox).Checked = selectall
            End If
        Next

        If selectall Then
            btnSelectAll.Text = "Unselect All"
        Else
            btnSelectAll.Text = "Select All"
        End If
    End Sub

    Private Sub btnOpenImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpenImage.Click
        Try
            Dim dlg As OpenFileDialog = New OpenFileDialog()
            dlg.Filter = "All Supported Images(*.BMP,*.PNG,*.JPEG,*.JPG,*.JPE,*.JFIF,*.TIF,*.TIFF)|*.BMP;*.PNG;*.JPEG;*.JPG;*.JPE;*.JFIF;*.TIF;*.TIFF|JPEG|*.JPG;*.JPEG;*.JPE;*.JFIF|BMP|*.BMP|PNG|*.PNG|TIFF|*.TIF;*.TIFF"
            'dlg.Multiselect = true;
            dlg.InitialDirectory = lastOpenedDirectory
            If (dlg.ShowDialog() = DialogResult.OK) Then
                lastOpenedDirectory = System.IO.Directory.GetParent(dlg.FileName).FullName
                Dim i As Integer
                For i = 0 To m_data.Count - 1
                    If Not m_data(i) Is Nothing Then
                        If m_results.ContainsKey(m_data(i)) Then
                            If Not m_results(m_data(i)) Is Nothing Then
                                m_results(m_data(i)).Dispose()
                            End If
                        End If
                        m_data(i).Dispose()
                    End If
                Next
                m_data.Clear()
                m_results.Clear()
                tbResults.Clear()

                'foreach (string file in dlg.FileNames)
                '{
                filePath = dlg.FileName
                Dim img As Image = Nothing
                Try
                    img = Image.FromFile(filePath)
                    Dim frameGuid As Guid = img.FrameDimensionsList(0)
                    Dim demension As FrameDimension = New FrameDimension(frameGuid)
                    Dim pageCount As Integer = img.GetFrameCount(demension)
                    Dim k As Integer
                    Dim lastExceptionalIndice As System.Collections.ArrayList = New System.Collections.ArrayList(pageCount)
                    For k = 0 To pageCount - 1
                        Try
                            img.SelectActiveFrame(demension, k)
                        Catch ex As Exception
                            img.Dispose()
                            img = Image.FromFile(filePath)
                            If (Not lastExceptionalIndice.Contains(k)) Then
                                lastExceptionalIndice.Add(k)
                                If k > 0 Then
                                    k = k - 1
                                    If Not m_data(k) Is Nothing Then
                                        m_data(k).Dispose()
                                    End If
                                    m_data.RemoveAt(k)
                                End If
                            End If
                            img.SelectActiveFrame(demension, k)
                        End Try
                        m_data.Add(New Bitmap(img))
                    Next

                    img.Dispose()
                    img = Nothing
                Finally
                    If Not img Is Nothing Then
                        img.Dispose()
                    End If
                End Try
                '}
                If m_data.Count > 0 And iCheckedFormatCount > 0 Then
                    btnRead.Enabled = True
                End If
                CurrentIndex = m_data.Count - 1
                tbxTotalImageNum.Text = m_data.Count.ToString()
                If (m_data.Count > 1) Then
                    EnableControls(picboxFirst)
                    EnableControls(picboxPrevious)
                    'EnableControls(picboxNext);
                    'EnableControls(picboxLast);
                Else
                    DisableControls(picboxFirst)
                    DisableControls(picboxPrevious)
                    DisableControls(picboxNext)
                    DisableControls(picboxLast)
                End If
            End If
        Catch exp As Exception
            MessageBox.Show(exp.Message, "Barcode Reader Demo", MessageBoxButtons.OK)
        End Try
    End Sub

    Private Sub SetImageViewerImage()
        If (m_results.ContainsKey(m_data(m_index))) Then
            If (Not m_results(m_data(m_index)) Is Nothing) Then
                imageViewer.Image = m_results(m_data(m_index))
                Return
            End If
        End If

        imageViewer.Image = m_data(m_index)
    End Sub

    Private Function GetFormats() As BarcodeFormat
        Dim formats As BarcodeFormat? = Nothing
        If (chkCode39.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.CODE_39
            Else
                formats = formats Or BarcodeFormat.CODE_39
            End If
        End If
        If (chkCode128.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.CODE_128
            Else
                formats = formats Or BarcodeFormat.CODE_128
            End If
        End If
        If (chkCode93.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.CODE_93
            Else
                formats = formats Or BarcodeFormat.CODE_93
            End If
        End If
        If (chkCodabar.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.CODABAR
            Else
                formats = formats Or BarcodeFormat.CODABAR
            End If
        End If
        If (chkITF.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.ITF
            Else
                formats = formats Or BarcodeFormat.ITF
            End If
        End If
        If (chkUPCA.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.UPC_A
            Else
                formats = formats Or BarcodeFormat.UPC_A
            End If
        End If
        If (chkUPCE.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.UPC_E
            Else
                formats = formats Or BarcodeFormat.UPC_E
            End If
        End If
        If (chkEAN8.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.EAN_8
            Else
                formats = formats Or BarcodeFormat.EAN_8
            End If
        End If
        If (chkEAN13.Checked) Then
            If (Not formats.HasValue) Then
                formats = BarcodeFormat.EAN_13
            Else
                formats = formats Or BarcodeFormat.EAN_13
            End If
        End If

        If Not formats.HasValue Then
            Return BarcodeFormat.OneD
        Else
            Return formats.Value
        End If
    End Function

    Private Sub btnRead_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRead.Click
        If (Not imageViewer.Image Is Nothing) Then
            'Rectangle rect = new Rectangle();
            Dim reader As BarcodeReader = New Dynamsoft.Barcode.BarcodeReader()
            Try
                reader.LicenseKeys = "<Input your license key here>"
                Dim ro As ReaderOptions = New ReaderOptions()
                ro.BarcodeFormats = GetFormats()
                ro.MaxBarcodesToReadPerPage = Integer.Parse(tbMaximumNum.Text)
                reader.ReaderOptions = ro
                Dim beforeRead As DateTime = DateTime.Now
                Dim barcodes As BarcodeResult() = reader.DecodeFile(filePath)
                Dim afterRead As DateTime = DateTime.Now
                Dim timeElapsed As Integer = (afterRead - beforeRead).Milliseconds
                ShowBarcodeResults(barcodes, timeElapsed)
            Catch exp As Exception
                MessageBox.Show(exp.Message, "Barcode Reader Demo", MessageBoxButtons.OK)
            Finally
                reader.Dispose()
            End Try
        End If
    End Sub

    Private Sub ShowBarcodeResults(ByVal barcodeResults As BarcodeResult(), ByVal timeElapsed As Single)
        tbResults.Clear()
        Dim imgs As List(Of Image) = New List(Of Image)(m_results.Keys)
        For Each img As Image In imgs
            If Not m_results(img) Is Nothing Then
                m_results(img).Dispose()
                m_results(img) = Nothing
            End If
        Next

        If (Not barcodeResults Is Nothing) Then
            If barcodeResults.Length > 0 Then
                tbResults.AppendText(String.Format("Total barcode(s) found: {0}. Total cost time: {1} seconds{2}", barcodeResults.Length, (timeElapsed / 1000), vbCrLf))
                Dim i As Integer
                For i = 0 To barcodeResults.Length - 1
                    tbResults.AppendText(String.Format("  Barcode: {0}{1}", (i + 1).ToString(), vbCrLf))
                    tbResults.AppendText(String.Format("    Page: {0}{1}", (barcodeResults(i).PageNumber).ToString(), vbCrLf))
                    tbResults.AppendText(String.Format("    Type: {0}{1}", barcodeResults(i).BarcodeFormat.ToString(), vbCrLf))
                    tbResults.AppendText(String.Format("    Value: {0}{1}", barcodeResults(i).BarcodeText, vbCrLf))
                    tbResults.AppendText(String.Format("    Region: {{Left: {0}, Top: {1}, Width: {2}, Height: {3}}}{4}", barcodeResults(i).BoundingRect.Left.ToString(), _
                                                       barcodeResults(i).BoundingRect.Top.ToString(), barcodeResults(i).BoundingRect.Width.ToString(), barcodeResults(i).BoundingRect.Height.ToString(), vbCrLf))
                    tbResults.AppendText(vbCrLf)
                Next

                Dim pen As Pen = New Pen(Color.Red, 1)
                Dim textBrush As Brush = New SolidBrush(Color.Blue)
                Dim img As Image = Nothing
                Dim g As Graphics = Nothing
                Dim index As Integer = -1
                For i = barcodeResults.Length - 1 To 0 Step -1
                    If (Not barcodeResults(i) Is Nothing) Then
                        If (Not barcodeResults(i).PageNumber = (index + 1)) Then
                            If (Not g Is Nothing) Then
                                g.Dispose()
                            End If
                            If (Not img Is Nothing And index >= 0 And index < m_data.Count) Then
                                If (m_results.ContainsKey(m_data(index))) Then
                                    If Not m_results(m_data(index)) Is Nothing Then
                                        m_results(m_data(index)).Dispose()
                                    End If
                                    m_results(m_data(index)) = img
                                Else
                                    m_results.Add(m_data(index), img)
                                End If
                            End If

                            index = barcodeResults(i).PageNumber - 1
                            img = DirectCast(m_data(index).Clone(), Image)
                            g = Graphics.FromImage(img)
                        End If
                        Dim rect As Rectangle = barcodeResults(i).BoundingRect
                        g.DrawRectangle(pen, rect)
                        Dim textFont As Font = SystemFonts.DefaultFont
                        Dim iHeight As Integer = textFont.Height * 3 / 2
                        Dim iTop As Integer
                        If rect.Top - iHeight < 0 Then
                            iTop = 0
                        Else
                            iTop = rect.Top - iHeight
                        End If
                        g.DrawString("[" + i.ToString() + "] {" + barcodeResults(i).BarcodeText + "}", textFont, textBrush, New Rectangle(rect.Left, iTop, 0, iHeight), New StringFormat(StringFormatFlags.NoClip Or StringFormatFlags.NoWrap))
                    End If
                Next
                pen.Dispose()
                textBrush.Dispose()
                If (Not g Is Nothing) Then
                    g.Dispose()
                End If
                If (Not img Is Nothing And index >= 0 And index < m_data.Count) Then
                    If (m_results.ContainsKey(m_data(index))) Then
                        If Not m_results(m_data(index)) Is Nothing Then
                            m_results(m_data(index)).Dispose()
                        End If
                        m_results(m_data(index)) = img
                    Else
                        m_results.Add(m_data(index), img)
                    End If
                End If

                SetImageViewerImage()
            Else
                tbResults.AppendText("No barcode found. Total time spent: " + (timeElapsed / 1000).ToString() + " seconds" + vbCrLf)
            End If
        Else
            tbResults.AppendText("No barcode found. Total time spent: " + (timeElapsed / 1000).ToString() + " seconds" + vbCrLf)
        End If
    End Sub

    Private Sub tbMaximumNum_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If (Not e.KeyChar = "\b") Then
            Dim array As Byte() = System.Text.Encoding.Default.GetBytes(e.KeyChar.ToString())
            If ((Not Char.IsDigit(e.KeyChar)) Or array.LongLength = 2) Then
                e.Handled = True
            End If
            If ((tbMaximumNum.Text.Length = 0 Or tbMaximumNum.SelectionLength = tbMaximumNum.Text.Length) And e.KeyChar = "0") Then
                e.Handled = True
            End If
        End If
    End Sub

#Region "regist Event For All PictureBox Buttons"
    Private Sub picbox_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles picboxFirst.MouseEnter, picboxLast.MouseEnter, picboxNext.MouseEnter, picboxPrevious.MouseEnter
        If (TypeOf sender Is PictureBox) Then
            Dim picBox As PictureBox = DirectCast(sender, PictureBox)
            If (picBox.Enabled) Then
                picBox.Image = DirectCast(My.Resources.ResourceManager.GetObject(picBox.Name + "_Enter"), Image)
            End If
        End If
    End Sub

    Private Sub picbox_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles picboxFirst.MouseLeave, picboxLast.MouseLeave, picboxNext.MouseLeave, picboxPrevious.MouseLeave
        If (TypeOf sender Is PictureBox) Then
            Dim picBox As PictureBox = DirectCast(sender, PictureBox)
            If (picBox.Enabled) Then
                picBox.Image = DirectCast(My.Resources.ResourceManager.GetObject(picBox.Name + "_Leave"), Image)
            End If
        End If
    End Sub

    Private Sub picbox_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles picboxFirst.MouseDown, picboxLast.MouseDown, picboxNext.MouseDown, picboxPrevious.MouseDown
        If (TypeOf sender Is PictureBox) Then
            Dim picBox As PictureBox = DirectCast(sender, PictureBox)
            If (picBox.Enabled) Then
                picBox.Image = DirectCast(My.Resources.ResourceManager.GetObject(picBox.Name + "_Down"), Image)
            End If
        End If
    End Sub

    Private Sub picbox_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles picboxFirst.MouseUp, picboxLast.MouseUp, picboxNext.MouseUp, picboxPrevious.MouseUp
        If (TypeOf sender Is PictureBox) Then
            Dim picBox As PictureBox = DirectCast(sender, PictureBox)
            If (picBox.Enabled) Then
                picBox.Image = DirectCast(My.Resources.ResourceManager.GetObject(picBox.Name + "_Enter"), Image)
            End If
        End If
    End Sub

    Private Sub DisableControls(ByVal sender As Object)
        If (TypeOf sender Is PictureBox) Then
            Dim picBox As PictureBox = DirectCast(sender, PictureBox)
            picBox.Image = DirectCast(My.Resources.ResourceManager.GetObject(picBox.Name + "_Disabled"), Image)
            picBox.Enabled = False
        Else
            DirectCast(sender, Control).Enabled = False
        End If
    End Sub

    Private Sub EnableControls(ByVal sender As Object)
        If (TypeOf sender Is PictureBox) Then
            Dim picBox As PictureBox = DirectCast(sender, PictureBox)
            picBox.Image = DirectCast(My.Resources.ResourceManager.GetObject(picBox.Name + "_Leave"), Image)
            picBox.Enabled = True
        Else
            DirectCast(sender, Control).Enabled = True
        End If
    End Sub

    Private Sub picboxFirst_Click(ByVal sender As Object, ByVal e As EventArgs) Handles picboxFirst.Click
        If (picboxFirst.Enabled) Then
            CurrentIndex = 0
            DisableControls(picboxFirst)
            DisableControls(picboxPrevious)
            EnableControls(picboxNext)
            EnableControls(picboxLast)
        End If
    End Sub

    Private Sub picboxLast_Click(ByVal sender As Object, ByVal e As EventArgs) Handles picboxLast.Click
        If (picboxLast.Enabled) Then
            CurrentIndex = m_data.Count - 1
            DisableControls(picboxLast)
            DisableControls(picboxNext)
            EnableControls(picboxPrevious)
            EnableControls(picboxFirst)
        End If
    End Sub

    Private Sub picboxPrevious_Click(ByVal sender As Object, ByVal e As EventArgs) Handles picboxPrevious.Click
        If (picboxPrevious.Enabled) Then
            CurrentIndex = CurrentIndex - 1
            If (CurrentIndex = 0) Then
                DisableControls(picboxFirst)
                DisableControls(picboxPrevious)
            End If
            EnableControls(picboxNext)
            EnableControls(picboxLast)
        End If
    End Sub

    Private Sub picboxNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles picboxNext.Click
        CurrentIndex = CurrentIndex + 1
        If (picboxNext.Enabled) Then
            If (CurrentIndex = m_data.Count - 1) Then
                DisableControls(picboxLast)
                DisableControls(picboxNext)
            End If
            EnableControls(picboxPrevious)
            EnableControls(picboxFirst)
        End If
    End Sub

#End Region

    Private Sub chkFormat_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkCodabar.CheckedChanged, chkCode128.CheckedChanged, chkCode39.CheckedChanged, chkCode93.CheckedChanged, chkEAN13.CheckedChanged, chkEAN8.CheckedChanged, chkITF.CheckedChanged, chkUPCA.CheckedChanged, chkUPCE.CheckedChanged
        Dim chkbox As CheckBox = DirectCast(sender, CheckBox)
        If (chkbox.Checked) Then
            iCheckedFormatCount = iCheckedFormatCount + 1
        Else
            iCheckedFormatCount = iCheckedFormatCount - 1
        End If

        If (m_data.Count > 0 And iCheckedFormatCount > 0) Then
            btnRead.Enabled = True
        Else
            btnRead.Enabled = False
        End If

        If (iCheckedFormatCount < 9) Then
            btnSelectAll.Text = "Select All"
        Else
            btnSelectAll.Text = "Unselect All"
        End If
    End Sub
End Class
