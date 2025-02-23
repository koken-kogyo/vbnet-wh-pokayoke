<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormPoka1Kubota
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
        Me.btnF1 = New System.Windows.Forms.Button
        Me.btnF2 = New System.Windows.Forms.Button
        Me.btnF3 = New System.Windows.Forms.Button
        Me.btnF4 = New System.Windows.Forms.Button
        Me.txtHMCD = New System.Windows.Forms.TextBox
        Me.txtTANCD = New System.Windows.Forms.TextBox
        Me.lblTANCD = New System.Windows.Forms.Label
        Me.lblHMCDTitle = New System.Windows.Forms.Label
        Me.LabelMenu = New System.Windows.Forms.Label
        Me.lblTKHMCDTitle = New System.Windows.Forms.Label
        Me.txtTKHMCD = New System.Windows.Forms.TextBox
        Me.lblCount = New System.Windows.Forms.Label
        Me.lblHMCD = New System.Windows.Forms.Label
        Me.lblTKHMCD = New System.Windows.Forms.Label
        Me.chkQR = New System.Windows.Forms.CheckBox
        Me.chkTe = New System.Windows.Forms.CheckBox
        Me.lblQTY = New System.Windows.Forms.Label
        Me.txtQTY = New System.Windows.Forms.TextBox
        Me.lblHIASU = New System.Windows.Forms.Label
        Me.TimerWiFiUpdater = New System.Windows.Forms.Timer
        Me.TimerServerChecker = New System.Windows.Forms.Timer
        Me.txtTotalQty = New System.Windows.Forms.TextBox
        Me.lblTotalQty = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'btnF1
        '
        Me.btnF1.BackColor = System.Drawing.Color.Red
        Me.btnF1.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF1.Location = New System.Drawing.Point(0, 285)
        Me.btnF1.Name = "btnF1"
        Me.btnF1.Size = New System.Drawing.Size(62, 34)
        Me.btnF1.TabIndex = 108
        Me.btnF1.Text = "戻る"
        '
        'btnF2
        '
        Me.btnF2.BackColor = System.Drawing.Color.Blue
        Me.btnF2.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF2.Location = New System.Drawing.Point(59, 285)
        Me.btnF2.Name = "btnF2"
        Me.btnF2.Size = New System.Drawing.Size(62, 34)
        Me.btnF2.TabIndex = 109
        Me.btnF2.Text = "送信"
        '
        'btnF3
        '
        Me.btnF3.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnF3.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF3.Location = New System.Drawing.Point(118, 285)
        Me.btnF3.Name = "btnF3"
        Me.btnF3.Size = New System.Drawing.Size(62, 34)
        Me.btnF3.TabIndex = 110
        Me.btnF3.Text = "履歴"
        '
        'btnF4
        '
        Me.btnF4.BackColor = System.Drawing.Color.Gold
        Me.btnF4.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF4.Location = New System.Drawing.Point(178, 285)
        Me.btnF4.Name = "btnF4"
        Me.btnF4.Size = New System.Drawing.Size(62, 34)
        Me.btnF4.TabIndex = 111
        Me.btnF4.Text = "ｸﾘｱ"
        '
        'txtHMCD
        '
        Me.txtHMCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 20.0!, System.Drawing.FontStyle.Regular)
        Me.txtHMCD.Location = New System.Drawing.Point(11, 89)
        Me.txtHMCD.Name = "txtHMCD"
        Me.txtHMCD.Size = New System.Drawing.Size(216, 46)
        Me.txtHMCD.TabIndex = 112
        '
        'txtTANCD
        '
        Me.txtTANCD.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtTANCD.Enabled = False
        Me.txtTANCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.txtTANCD.Location = New System.Drawing.Point(138, 31)
        Me.txtTANCD.Name = "txtTANCD"
        Me.txtTANCD.Size = New System.Drawing.Size(89, 34)
        Me.txtTANCD.TabIndex = 114
        '
        'lblTANCD
        '
        Me.lblTANCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblTANCD.Location = New System.Drawing.Point(11, 33)
        Me.lblTANCD.Name = "lblTANCD"
        Me.lblTANCD.Size = New System.Drawing.Size(121, 28)
        Me.lblTANCD.Text = "担当者："
        '
        'lblHMCDTitle
        '
        Me.lblHMCDTitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblHMCDTitle.Location = New System.Drawing.Point(11, 61)
        Me.lblHMCDTitle.Name = "lblHMCDTitle"
        Me.lblHMCDTitle.Size = New System.Drawing.Size(133, 35)
        Me.lblHMCDTitle.Text = "社内品番："
        '
        'LabelMenu
        '
        Me.LabelMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LabelMenu.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.LabelMenu.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.LabelMenu.Location = New System.Drawing.Point(0, 0)
        Me.LabelMenu.Name = "LabelMenu"
        Me.LabelMenu.Size = New System.Drawing.Size(240, 28)
        Me.LabelMenu.Text = "クボタ照合"
        Me.LabelMenu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTKHMCDTitle
        '
        Me.lblTKHMCDTitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblTKHMCDTitle.Location = New System.Drawing.Point(11, 135)
        Me.lblTKHMCDTitle.Name = "lblTKHMCDTitle"
        Me.lblTKHMCDTitle.Size = New System.Drawing.Size(143, 26)
        Me.lblTKHMCDTitle.Text = "メーカー品番："
        '
        'txtTKHMCD
        '
        Me.txtTKHMCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 20.0!, System.Drawing.FontStyle.Regular)
        Me.txtTKHMCD.Location = New System.Drawing.Point(11, 162)
        Me.txtTKHMCD.Name = "txtTKHMCD"
        Me.txtTKHMCD.Size = New System.Drawing.Size(216, 46)
        Me.txtTKHMCD.TabIndex = 121
        '
        'lblCount
        '
        Me.lblCount.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.lblCount.Location = New System.Drawing.Point(181, 252)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(46, 28)
        Me.lblCount.Text = "00"
        Me.lblCount.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblHMCD
        '
        Me.lblHMCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 9.0!, System.Drawing.FontStyle.Regular)
        Me.lblHMCD.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.lblHMCD.Location = New System.Drawing.Point(11, 249)
        Me.lblHMCD.Name = "lblHMCD"
        Me.lblHMCD.Size = New System.Drawing.Size(171, 18)
        Me.lblHMCD.Text = "xxxxxxxxxx"
        '
        'lblTKHMCD
        '
        Me.lblTKHMCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 9.0!, System.Drawing.FontStyle.Regular)
        Me.lblTKHMCD.Location = New System.Drawing.Point(11, 267)
        Me.lblTKHMCD.Name = "lblTKHMCD"
        Me.lblTKHMCD.Size = New System.Drawing.Size(216, 22)
        Me.lblTKHMCD.Text = "xxxxxxxxxx"
        '
        'chkQR
        '
        Me.chkQR.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.chkQR.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular)
        Me.chkQR.Location = New System.Drawing.Point(161, 140)
        Me.chkQR.Name = "chkQR"
        Me.chkQR.Size = New System.Drawing.Size(77, 20)
        Me.chkQR.TabIndex = 128
        Me.chkQR.TabStop = False
        Me.chkQR.Text = "QRだけ"
        '
        'chkTe
        '
        Me.chkTe.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular)
        Me.chkTe.Location = New System.Drawing.Point(160, 68)
        Me.chkTe.Name = "chkTe"
        Me.chkTe.Size = New System.Drawing.Size(77, 20)
        Me.chkTe.TabIndex = 136
        Me.chkTe.TabStop = False
        Me.chkTe.Text = "手入力"
        '
        'lblQTY
        '
        Me.lblQTY.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblQTY.Location = New System.Drawing.Point(154, 216)
        Me.lblQTY.Name = "lblQTY"
        Me.lblQTY.Size = New System.Drawing.Size(30, 29)
        Me.lblQTY.Text = "/"
        '
        'txtQTY
        '
        Me.txtQTY.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 20.0!, System.Drawing.FontStyle.Regular)
        Me.txtQTY.Location = New System.Drawing.Point(173, 211)
        Me.txtQTY.Name = "txtQTY"
        Me.txtQTY.Size = New System.Drawing.Size(54, 46)
        Me.txtQTY.TabIndex = 145
        '
        'lblHIASU
        '
        Me.lblHIASU.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 9.0!, System.Drawing.FontStyle.Regular)
        Me.lblHIASU.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.lblHIASU.Location = New System.Drawing.Point(11, 219)
        Me.lblHIASU.Name = "lblHIASU"
        Me.lblHIASU.Size = New System.Drawing.Size(100, 20)
        Me.lblHIASU.Text = "ｱｵ 5"
        '
        'TimerWiFiUpdater
        '
        Me.TimerWiFiUpdater.Interval = 120000
        '
        'TimerServerChecker
        '
        Me.TimerServerChecker.Interval = 1000
        '
        'txtTotalQty
        '
        Me.txtTotalQty.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtTotalQty.Enabled = False
        Me.txtTotalQty.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.txtTotalQty.Location = New System.Drawing.Point(100, 211)
        Me.txtTotalQty.Name = "txtTotalQty"
        Me.txtTotalQty.Size = New System.Drawing.Size(49, 34)
        Me.txtTotalQty.TabIndex = 155
        '
        'lblTotalQty
        '
        Me.lblTotalQty.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 9.0!, System.Drawing.FontStyle.Regular)
        Me.lblTotalQty.Location = New System.Drawing.Point(57, 219)
        Me.lblTotalQty.Name = "lblTotalQty"
        Me.lblTotalQty.Size = New System.Drawing.Size(54, 20)
        Me.lblTotalQty.Text = "指示数"
        '
        'FormPoka1Kubota
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(240, 320)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtTotalQty)
        Me.Controls.Add(Me.lblTotalQty)
        Me.Controls.Add(Me.txtQTY)
        Me.Controls.Add(Me.lblQTY)
        Me.Controls.Add(Me.txtHMCD)
        Me.Controls.Add(Me.chkTe)
        Me.Controls.Add(Me.chkQR)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.txtTKHMCD)
        Me.Controls.Add(Me.LabelMenu)
        Me.Controls.Add(Me.txtTANCD)
        Me.Controls.Add(Me.lblTANCD)
        Me.Controls.Add(Me.btnF4)
        Me.Controls.Add(Me.btnF3)
        Me.Controls.Add(Me.btnF2)
        Me.Controls.Add(Me.btnF1)
        Me.Controls.Add(Me.lblHMCD)
        Me.Controls.Add(Me.lblTKHMCD)
        Me.Controls.Add(Me.lblTKHMCDTitle)
        Me.Controls.Add(Me.lblHMCDTitle)
        Me.Controls.Add(Me.lblHIASU)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FormPoka1Kubota"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnF1 As System.Windows.Forms.Button
    Friend WithEvents btnF2 As System.Windows.Forms.Button
    Friend WithEvents btnF3 As System.Windows.Forms.Button
    Friend WithEvents btnF4 As System.Windows.Forms.Button
    Friend WithEvents txtTANCD As System.Windows.Forms.TextBox
    Friend WithEvents lblTANCD As System.Windows.Forms.Label
    Friend WithEvents lblHMCDTitle As System.Windows.Forms.Label
    Friend WithEvents LabelMenu As System.Windows.Forms.Label
    Friend WithEvents lblTKHMCDTitle As System.Windows.Forms.Label
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Public WithEvents txtHMCD As System.Windows.Forms.TextBox
    Public WithEvents txtTKHMCD As System.Windows.Forms.TextBox
    Friend WithEvents lblHMCD As System.Windows.Forms.Label
    Friend WithEvents lblTKHMCD As System.Windows.Forms.Label
    Friend WithEvents chkQR As System.Windows.Forms.CheckBox
    Friend WithEvents chkTe As System.Windows.Forms.CheckBox
    Friend WithEvents lblQTY As System.Windows.Forms.Label
    Friend WithEvents txtQTY As System.Windows.Forms.TextBox
    Friend WithEvents lblHIASU As System.Windows.Forms.Label
    Friend WithEvents TimerWiFiUpdater As System.Windows.Forms.Timer
    Friend WithEvents TimerServerChecker As System.Windows.Forms.Timer
    Friend WithEvents txtTotalQty As System.Windows.Forms.TextBox
    Friend WithEvents lblTotalQty As System.Windows.Forms.Label
End Class
