<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormWiFiCheck
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblHT = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblAP = New System.Windows.Forms.Label
        Me.lblMessage = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblSV = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblHT
        '
        Me.lblHT.BackColor = System.Drawing.Color.Gray
        Me.lblHT.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.lblHT.ForeColor = System.Drawing.Color.Black
        Me.lblHT.Location = New System.Drawing.Point(7, 9)
        Me.lblHT.Name = "lblHT"
        Me.lblHT.Size = New System.Drawing.Size(193, 39)
        Me.lblHT.Text = "この端末"
        Me.lblHT.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.Label1.Location = New System.Drawing.Point(79, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 21)
        Me.Label1.Text = "↓"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAP
        '
        Me.lblAP.BackColor = System.Drawing.Color.Gray
        Me.lblAP.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.lblAP.ForeColor = System.Drawing.Color.Black
        Me.lblAP.Location = New System.Drawing.Point(7, 69)
        Me.lblAP.Name = "lblAP"
        Me.lblAP.Size = New System.Drawing.Size(193, 39)
        Me.lblAP.Text = "ｱｸｾｽﾎﾟｲﾝﾄ"
        Me.lblAP.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblMessage.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblMessage.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblMessage.Location = New System.Drawing.Point(14, 224)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(186, 24)
        Me.lblMessage.Text = "しばらくお待ちください"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label3.Location = New System.Drawing.Point(7, 211)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(193, 40)
        '
        'Timer1
        '
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.Label2.Location = New System.Drawing.Point(79, 108)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 21)
        Me.Label2.Text = "↓"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSV
        '
        Me.lblSV.BackColor = System.Drawing.Color.Gray
        Me.lblSV.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.lblSV.ForeColor = System.Drawing.Color.Black
        Me.lblSV.Location = New System.Drawing.Point(7, 129)
        Me.lblSV.Name = "lblSV"
        Me.lblSV.Size = New System.Drawing.Size(193, 39)
        Me.lblSV.Text = "物流事務所"
        Me.lblSV.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnOK.Location = New System.Drawing.Point(60, 176)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(87, 29)
        Me.btnOK.TabIndex = 7
        Me.btnOK.Text = "OK"
        '
        'FormWiFiCheck
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(208, 255)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblSV)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.lblAP)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblHT)
        Me.Controls.Add(Me.Label3)
        Me.Location = New System.Drawing.Point(20, 10)
        Me.Name = "FormWiFiCheck"
        Me.Text = "Wi-Fi チェック"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblHT As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblAP As System.Windows.Forms.Label
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblSV As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
End Class
