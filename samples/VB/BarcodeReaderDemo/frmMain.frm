VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form frmMain 
   Caption         =   "Barcode Reader Demo"
   ClientHeight    =   8325
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   8205
   LinkTopic       =   "Form1"
   ScaleHeight     =   8325
   ScaleWidth      =   8205
   StartUpPosition =   3  'Windows Default
   Begin MSComDlg.CommonDialog cdOpenFileDlg 
      Left            =   0
      Top             =   7920
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.ListBox lbResults 
      Height          =   3570
      Left            =   360
      TabIndex        =   17
      Top             =   3960
      Width           =   7575
   End
   Begin VB.CommandButton btnReadBarcode 
      Caption         =   "Read Barcodes"
      Height          =   495
      Left            =   360
      TabIndex        =   16
      Top             =   3240
      Width           =   1455
   End
   Begin VB.TextBox tbMaxNum 
      BeginProperty DataFormat 
         Type            =   1
         Format          =   "0"
         HaveTrueFalseNull=   0
         FirstDayOfWeek  =   0
         FirstWeekOfYear =   0
         LCID            =   1033
         SubFormatType   =   1
      EndProperty
      Height          =   375
      Left            =   1920
      TabIndex        =   15
      Text            =   "100"
      Top             =   2640
      Width           =   6015
   End
   Begin VB.CommandButton btnBrowse 
      Caption         =   "Browse..."
      Height          =   375
      Left            =   6960
      TabIndex        =   2
      Top             =   240
      Width           =   975
   End
   Begin VB.TextBox tbFileName 
      Height          =   375
      Left            =   1440
      TabIndex        =   1
      Top             =   240
      Width           =   5415
   End
   Begin VB.Frame frmTypes 
      Caption         =   "Barcode Types"
      Height          =   1455
      Left            =   240
      TabIndex        =   3
      Top             =   960
      Width           =   7695
      Begin VB.CommandButton btnSelect 
         Caption         =   "Unselect All"
         Height          =   375
         Left            =   5880
         TabIndex        =   13
         Top             =   840
         Width           =   1335
      End
      Begin VB.CheckBox cbCode93 
         Caption         =   "Code93"
         Height          =   375
         Left            =   1800
         TabIndex        =   6
         Top             =   360
         Value           =   1  'Checked
         Width           =   855
      End
      Begin VB.CheckBox cbCode39 
         Caption         =   "Code39"
         Height          =   375
         Left            =   480
         TabIndex        =   4
         Top             =   360
         Value           =   1  'Checked
         Width           =   975
      End
      Begin VB.CheckBox cbITF 
         Caption         =   "ITF"
         Height          =   375
         Left            =   3240
         TabIndex        =   12
         Top             =   360
         Value           =   1  'Checked
         Width           =   855
      End
      Begin VB.CheckBox cbEAN13 
         Caption         =   "EAN13"
         Height          =   375
         Left            =   3240
         TabIndex        =   11
         Top             =   840
         Value           =   1  'Checked
         Width           =   855
      End
      Begin VB.CheckBox cbEAN8 
         Caption         =   "EAN8"
         Height          =   375
         Left            =   4560
         TabIndex        =   10
         Top             =   360
         Value           =   1  'Checked
         Width           =   855
      End
      Begin VB.CheckBox cbUPCA 
         Caption         =   "UPCA"
         Height          =   375
         Left            =   4560
         TabIndex        =   9
         Top             =   840
         Value           =   1  'Checked
         Width           =   855
      End
      Begin VB.CheckBox cbUPCE 
         Caption         =   "UPCE"
         Height          =   375
         Left            =   5880
         TabIndex        =   8
         Top             =   360
         Value           =   1  'Checked
         Width           =   855
      End
      Begin VB.CheckBox cbCodabar 
         Caption         =   "Codabar"
         Height          =   375
         Left            =   1800
         TabIndex        =   7
         Top             =   840
         Value           =   1  'Checked
         Width           =   975
      End
      Begin VB.CheckBox cbCode128 
         Caption         =   "Code128"
         Height          =   375
         Left            =   480
         TabIndex        =   5
         Top             =   840
         Value           =   1  'Checked
         Width           =   975
      End
   End
   Begin VB.Label Label2 
      Caption         =   "Maximum Number:"
      Height          =   375
      Left            =   360
      TabIndex        =   14
      Top             =   2640
      Width           =   1455
   End
   Begin VB.Label Label1 
      Caption         =   "Image File:"
      Height          =   375
      Left            =   360
      TabIndex        =   0
      Top             =   240
      Width           =   1215
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim bUnselectFlag As Boolean

Private Sub btnBrowse_Click()
    
    On Error GoTo ErrLabel
    
    cdOpenFileDlg.CancelError = True
    cdOpenFileDlg.Filter = "BMP(*.bmp)|*.bmp|JPEG(*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG(*.png)|*.png|TIFF(*.tif;*.tiff)|*.tif;*.tiff|All Files|*.*"
    cdOpenFileDlg.FilterIndex = 5
    cdOpenFileDlg.ShowOpen
    
    tbFileName.Text = cdOpenFileDlg.FileName
    
    Exit Sub
ErrLabel:
    Exit Sub
    
End Sub

Private Function GetSelectedBarcodeTypes() As Variant
    Dim oFormat As New BarcodeFormat
    Dim vFormat As Variant
    vFormat = 0
    
    If cbCode39.Value = Checked Then
        vFormat = vFormat Or oFormat.CODE_39
    End If
        
    If cbCode128.Value = Checked Then
        vFormat = vFormat Or oFormat.CODE_128
    End If
     
    If cbCode93.Value = Checked Then
        vFormat = vFormat Or oFormat.CODE_93
    End If
    
    If cbCodabar.Value = Checked Then
        vFormat = vFormat Or oFormat.CODABAR
    End If
    
    If cbITF.Value = Checked Then
        vFormat = vFormat Or oFormat.ITF
    End If
    
    If cbEAN13.Value = Checked Then
        vFormat = vFormat Or oFormat.EAN_13
    End If
    
    If cbEAN8.Value = Checked Then
        vFormat = vFormat Or oFormat.EAN_8
    End If
    
    If cbUPCA.Value = Checked Then
        vFormat = vFormat Or oFormat.UPC_A
    End If
   
    If cbUPCE.Value = Checked Then
        vFormat = vFormat Or oFormat.UPC_E
    End If
    
    GetSelectedBarcodeTypes = vFormat
    
End Function
Private Sub btnReadBarcode_Click()
    On Error GoTo ErrLabel
    
    lbResults.Clear
        
    Dim oBarcodeReader As New BarcodeReader
    oBarcodeReader.InitLicense "<Put your license key here>"
    
    Dim oOptions As New ReaderOptions
    oOptions.MaxBarcodesNumPerPage = tbMaxNum.Text
    oOptions.BarcodeFormats = GetSelectedBarcodeTypes
    
    oBarcodeReader.ReaderOptions = oOptions
    
    Dim dtBeg, dtEnd As Double
    dtBeg = Timer
    oBarcodeReader.DecodeFile tbFileName.Text
    dtEnd = Timer
    
    
    Dim oBarcodeArray As IBarcodeResultArray
    Set oBarcodeArray = oBarcodeReader.Barcodes
    
    
    Dim oTempStr As String
    If oBarcodeArray.Count = 0 Then
        oTempStr = "No barcode found."
    Else
        oTempStr = "Total barcode(s) found: " & oBarcodeArray.Count & "."
    End If
    
    oTempStr = oTempStr & " Total time spent: " & Format(dtEnd - dtBeg, "0.000") & " seconds."
    lbResults.AddItem oTempStr
    lbResults.AddItem ""
    
    Dim iIndex As Long
    Dim oBarcode As IBarcodeResult
    For iIndex = 0 To oBarcodeArray.Count - 1
        Set oBarcode = oBarcodeArray.Item(iIndex)
        
        oTempStr = "    Barcode " & iIndex + 1 & ":"
        lbResults.AddItem oTempStr
        
        oTempStr = "        Page: " & oBarcode.PageNum
        lbResults.AddItem oTempStr
        
        oTempStr = "        Type: " & oBarcode.BarcodeFormat.TypeString
        lbResults.AddItem oTempStr
        
        oTempStr = "        Value: " & oBarcode.BarcodeText
        lbResults.AddItem oTempStr
        
        oTempStr = "        Region: {Left: " & oBarcode.Left & _
                                ", Top: " & oBarcode.Top & _
                                ", Width: " & oBarcode.Width & _
                                ", Height: " & oBarcode.Height & "}"
        lbResults.AddItem oTempStr
        lbResults.AddItem ""
    Next
    
    Set oBarcodeArray = Nothing
    Set oOption = Nothing
    Set oBarcodeReader = Nothing
    
    Exit Sub
ErrLabel:
    MsgBox Err.Description, vbCritical
End Sub

Private Sub btnSelect_Click()
    If Not bUnselectFlag Then
        btnSelect.Caption = "Unselect All"
        bUnselectFlag = True
        cbCode39.Value = Checked
        cbCode128.Value = Checked
        cbCode93.Value = Checked
        cbCodabar.Value = Checked
        cbITF.Value = Checked
        cbEAN13.Value = Checked
        cbEAN8.Value = Checked
        cbUPCA.Value = Checked
        cbUPCE.Value = Checked
    Else
        btnSelect.Caption = "Select All"
        bUnselectFlag = False
        cbCode39.Value = Unchecked
        cbCode128.Value = Unchecked
        cbCode93.Value = Unchecked
        cbCodabar.Value = Unchecked
        cbITF.Value = Unchecked
        cbEAN13.Value = Unchecked
        cbEAN8.Value = Unchecked
        cbUPCA.Value = Unchecked
        cbUPCE.Value = Unchecked
    End If
    
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub cbCodabar_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub cbCode128_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub cbCode39_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
    
End Sub

Private Sub cbCode93_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub cbEAN13_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub cbEAN8_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub cbITF_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub cbUPCA_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub cbUPCE_Click()
    If GetSelectedBarcodeTypes = 0 Then
        btnReadBarcode.Enabled = False
    Else
        btnReadBarcode.Enabled = True
    End If
End Sub

Private Sub Form_Load()
    bUnselectFlag = True
End Sub
