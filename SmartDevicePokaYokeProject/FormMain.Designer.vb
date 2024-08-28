<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormMain
    Inherits System.Windows.Forms.Form

    'Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
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
    'コード エディタでこのプロシージャを変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnKUBOTA = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.lblTANCD = New System.Windows.Forms.Label
        Me.txtTANCD = New System.Windows.Forms.TextBox
        Me.LabelMenu = New System.Windows.Forms.Label
        Me.btnYANMAR = New System.Windows.Forms.Button
        Me.btnHITATI = New System.Windows.Forms.Button
        Me.lblVersion = New System.Windows.Forms.Label
        Me.chkBuzzer = New System.Windows.Forms.CheckBox
        Me.btnTANA = New System.Windows.Forms.Button
        Me.btnORIENT = New System.Windows.Forms.Button
        Me.btnRestart = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnKUBOTA
        '
        Me.btnKUBOTA.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnKUBOTA.Location = New System.Drawing.Point(14, 76)
        Me.btnKUBOTA.Name = "btnKUBOTA"
        Me.btnKUBOTA.Size = New System.Drawing.Size(213, 36)
        Me.btnKUBOTA.TabIndex = 1
        Me.btnKUBOTA.Text = "1.クボタ　"
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnClose.Location = New System.Drawing.Point(138, 240)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 36)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "9.終了"
        '
        'lblTANCD
        '
        Me.lblTANCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblTANCD.Location = New System.Drawing.Point(11, 39)
        Me.lblTANCD.Name = "lblTANCD"
        Me.lblTANCD.Size = New System.Drawing.Size(137, 28)
        Me.lblTANCD.Text = "担当者ｺｰﾄﾞ："
        '
        'txtTANCD
        '
        Me.txtTANCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.txtTANCD.Location = New System.Drawing.Point(138, 31)
        Me.txtTANCD.Name = "txtTANCD"
        Me.txtTANCD.Size = New System.Drawing.Size(89, 42)
        Me.txtTANCD.TabIndex = 0
        Me.txtTANCD.WordWrap = False
        '
        'LabelMenu
        '
        Me.LabelMenu.BackColor = System.Drawing.Color.MediumBlue
        Me.LabelMenu.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.LabelMenu.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.LabelMenu.Location = New System.Drawing.Point(0, 0)
        Me.LabelMenu.Name = "LabelMenu"
        Me.LabelMenu.Size = New System.Drawing.Size(240, 28)
        Me.LabelMenu.Text = "品番照合メニュー"
        Me.LabelMenu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnYANMAR
        '
        Me.btnYANMAR.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnYANMAR.Location = New System.Drawing.Point(14, 117)
        Me.btnYANMAR.Name = "btnYANMAR"
        Me.btnYANMAR.Size = New System.Drawing.Size(213, 36)
        Me.btnYANMAR.TabIndex = 3
        Me.btnYANMAR.Text = "2.ヤンマー"
        '
        'btnHITATI
        '
        Me.btnHITATI.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnHITATI.Location = New System.Drawing.Point(14, 158)
        Me.btnHITATI.Name = "btnHITATI"
        Me.btnHITATI.Size = New System.Drawing.Size(213, 36)
        Me.btnHITATI.TabIndex = 4
        Me.btnHITATI.Text = "3.日立建機"
        '
        'lblVersion
        '
        Me.lblVersion.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 12.0!, System.Drawing.FontStyle.Regular)
        Me.lblVersion.Location = New System.Drawing.Point(116, 276)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(110, 22)
        Me.lblVersion.Text = "ver 24.08.20"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkBuzzer
        '
        Me.chkBuzzer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkBuzzer.Checked = True
        Me.chkBuzzer.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBuzzer.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 10.0!, System.Drawing.FontStyle.Regular)
        Me.chkBuzzer.Location = New System.Drawing.Point(14, 279)
        Me.chkBuzzer.Name = "chkBuzzer"
        Me.chkBuzzer.Size = New System.Drawing.Size(110, 20)
        Me.chkBuzzer.TabIndex = 8
        Me.chkBuzzer.TabStop = False
        Me.chkBuzzer.Text = "照合時ブザー"
        '
        'btnTANA
        '
        Me.btnTANA.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnTANA.Location = New System.Drawing.Point(14, 199)
        Me.btnTANA.Name = "btnTANA"
        Me.btnTANA.Size = New System.Drawing.Size(212, 36)
        Me.btnTANA.TabIndex = 5
        Me.btnTANA.Text = "5.自動倉庫"
        '
        'btnORIENT
        '
        Me.btnORIENT.Enabled = False
        Me.btnORIENT.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnORIENT.Location = New System.Drawing.Point(205, 230)
        Me.btnORIENT.Name = "btnORIENT"
        Me.btnORIENT.Size = New System.Drawing.Size(32, 27)
        Me.btnORIENT.TabIndex = 12
        Me.btnORIENT.Text = "4.オリエント"
        Me.btnORIENT.Visible = False
        '
        'btnRestart
        '
        Me.btnRestart.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnRestart.Location = New System.Drawing.Point(14, 240)
        Me.btnRestart.Name = "btnRestart"
        Me.btnRestart.Size = New System.Drawing.Size(118, 36)
        Me.btnRestart.TabIndex = 6
        Me.btnRestart.Text = "8.再起動"
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(240, 320)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnRestart)
        Me.Controls.Add(Me.btnTANA)
        Me.Controls.Add(Me.chkBuzzer)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnHITATI)
        Me.Controls.Add(Me.btnYANMAR)
        Me.Controls.Add(Me.LabelMenu)
        Me.Controls.Add(Me.txtTANCD)
        Me.Controls.Add(Me.lblTANCD)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnKUBOTA)
        Me.Controls.Add(Me.btnORIENT)
        Me.Font = New System.Drawing.Font("メイリオ", 14.0!, System.Drawing.FontStyle.Regular)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormMain"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnKUBOTA As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblTANCD As System.Windows.Forms.Label
    Friend WithEvents LabelMenu As System.Windows.Forms.Label
    Friend WithEvents btnYANMAR As System.Windows.Forms.Button
    Friend WithEvents btnHITATI As System.Windows.Forms.Button
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Public WithEvents txtTANCD As System.Windows.Forms.TextBox
    Friend WithEvents chkBuzzer As System.Windows.Forms.CheckBox
    Friend WithEvents btnTANA As System.Windows.Forms.Button
    Friend WithEvents btnORIENT As System.Windows.Forms.Button
    Friend WithEvents btnRestart As System.Windows.Forms.Button

End Class
