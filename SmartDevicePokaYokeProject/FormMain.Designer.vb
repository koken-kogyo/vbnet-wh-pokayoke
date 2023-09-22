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
        Me.btnORIENT = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnKUBOTA
        '
        Me.btnKUBOTA.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnKUBOTA.Location = New System.Drawing.Point(14, 78)
        Me.btnKUBOTA.Name = "btnKUBOTA"
        Me.btnKUBOTA.Size = New System.Drawing.Size(213, 36)
        Me.btnKUBOTA.TabIndex = 1
        Me.btnKUBOTA.Text = "1.クボタ　"
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnClose.Location = New System.Drawing.Point(14, 274)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(97, 38)
        Me.btnClose.TabIndex = 6
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
        Me.btnYANMAR.Location = New System.Drawing.Point(14, 119)
        Me.btnYANMAR.Name = "btnYANMAR"
        Me.btnYANMAR.Size = New System.Drawing.Size(213, 36)
        Me.btnYANMAR.TabIndex = 3
        Me.btnYANMAR.Text = "2.ヤンマー"
        '
        'btnHITATI
        '
        Me.btnHITATI.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnHITATI.Location = New System.Drawing.Point(14, 160)
        Me.btnHITATI.Name = "btnHITATI"
        Me.btnHITATI.Size = New System.Drawing.Size(213, 36)
        Me.btnHITATI.TabIndex = 4
        Me.btnHITATI.Text = "3.日立建機"
        '
        'lblVersion
        '
        Me.lblVersion.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 12.0!, System.Drawing.FontStyle.Regular)
        Me.lblVersion.Location = New System.Drawing.Point(117, 288)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(110, 22)
        Me.lblVersion.Text = "ver 23.09.22"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkBuzzer
        '
        Me.chkBuzzer.Checked = True
        Me.chkBuzzer.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBuzzer.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.chkBuzzer.Location = New System.Drawing.Point(14, 250)
        Me.chkBuzzer.Name = "chkBuzzer"
        Me.chkBuzzer.Size = New System.Drawing.Size(213, 20)
        Me.chkBuzzer.TabIndex = 8
        Me.chkBuzzer.TabStop = False
        Me.chkBuzzer.Text = "照合時ブザー音あり"
        '
        'btnORIENT
        '
        Me.btnORIENT.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.btnORIENT.Location = New System.Drawing.Point(14, 201)
        Me.btnORIENT.Name = "btnORIENT"
        Me.btnORIENT.Size = New System.Drawing.Size(213, 36)
        Me.btnORIENT.TabIndex = 5
        Me.btnORIENT.Text = "4.オリエント"
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(240, 320)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnORIENT)
        Me.Controls.Add(Me.chkBuzzer)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.btnHITATI)
        Me.Controls.Add(Me.btnYANMAR)
        Me.Controls.Add(Me.LabelMenu)
        Me.Controls.Add(Me.txtTANCD)
        Me.Controls.Add(Me.lblTANCD)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnKUBOTA)
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
    Friend WithEvents btnORIENT As System.Windows.Forms.Button

End Class
