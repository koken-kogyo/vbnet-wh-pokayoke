<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormPokaModify
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnF4 = New System.Windows.Forms.Button
        Me.btnF3 = New System.Windows.Forms.Button
        Me.btnF2 = New System.Windows.Forms.Button
        Me.btnF1 = New System.Windows.Forms.Button
        Me.lblHMCDTitle = New System.Windows.Forms.Label
        Me.txtHMCD = New System.Windows.Forms.TextBox
        Me.lblQYTTitle = New System.Windows.Forms.Label
        Me.txtQTYbefore = New System.Windows.Forms.TextBox
        Me.txtQTY = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblDLVRDTtitle = New System.Windows.Forms.Label
        Me.txtDLVRDT = New System.Windows.Forms.TextBox
        Me.lblODRNOTitle = New System.Windows.Forms.Label
        Me.txtODRNO = New System.Windows.Forms.TextBox
        Me.lblDBStatus = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(240, 28)
        Me.Label1.Text = "データ編集"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnF4
        '
        Me.btnF4.BackColor = System.Drawing.Color.Gold
        Me.btnF4.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF4.Location = New System.Drawing.Point(177, 286)
        Me.btnF4.Name = "btnF4"
        Me.btnF4.Size = New System.Drawing.Size(62, 34)
        Me.btnF4.TabIndex = 119
        Me.btnF4.Text = "更新"
        '
        'btnF3
        '
        Me.btnF3.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnF3.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF3.Location = New System.Drawing.Point(119, 286)
        Me.btnF3.Name = "btnF3"
        Me.btnF3.Size = New System.Drawing.Size(62, 34)
        Me.btnF3.TabIndex = 118
        Me.btnF3.Text = "消込"
        '
        'btnF2
        '
        Me.btnF2.BackColor = System.Drawing.Color.Blue
        Me.btnF2.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF2.Location = New System.Drawing.Point(58, 286)
        Me.btnF2.Name = "btnF2"
        Me.btnF2.Size = New System.Drawing.Size(62, 34)
        Me.btnF2.TabIndex = 117
        '
        'btnF1
        '
        Me.btnF1.BackColor = System.Drawing.Color.Red
        Me.btnF1.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF1.Location = New System.Drawing.Point(-1, 286)
        Me.btnF1.Name = "btnF1"
        Me.btnF1.Size = New System.Drawing.Size(62, 34)
        Me.btnF1.TabIndex = 116
        Me.btnF1.Text = "戻る"
        '
        'lblHMCDTitle
        '
        Me.lblHMCDTitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lblHMCDTitle.Location = New System.Drawing.Point(11, 35)
        Me.lblHMCDTitle.Name = "lblHMCDTitle"
        Me.lblHMCDTitle.Size = New System.Drawing.Size(133, 35)
        Me.lblHMCDTitle.Text = "社内品番："
        '
        'txtHMCD
        '
        Me.txtHMCD.Enabled = False
        Me.txtHMCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.txtHMCD.Location = New System.Drawing.Point(25, 57)
        Me.txtHMCD.Name = "txtHMCD"
        Me.txtHMCD.Size = New System.Drawing.Size(200, 34)
        Me.txtHMCD.TabIndex = 121
        '
        'lblQYTTitle
        '
        Me.lblQYTTitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lblQYTTitle.Location = New System.Drawing.Point(11, 208)
        Me.lblQYTTitle.Name = "lblQYTTitle"
        Me.lblQYTTitle.Size = New System.Drawing.Size(134, 36)
        Me.lblQYTTitle.Text = "数量変更："
        '
        'txtQTYbefore
        '
        Me.txtQTYbefore.Enabled = False
        Me.txtQTYbefore.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.txtQTYbefore.Location = New System.Drawing.Point(25, 230)
        Me.txtQTYbefore.Name = "txtQTYbefore"
        Me.txtQTYbefore.Size = New System.Drawing.Size(65, 42)
        Me.txtQTYbefore.TabIndex = 123
        '
        'txtQTY
        '
        Me.txtQTY.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 26.0!, System.Drawing.FontStyle.Regular)
        Me.txtQTY.Location = New System.Drawing.Point(151, 222)
        Me.txtQTY.Name = "txtQTY"
        Me.txtQTY.Size = New System.Drawing.Size(74, 58)
        Me.txtQTY.TabIndex = 124
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.Label4.Location = New System.Drawing.Point(96, 237)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 20)
        Me.Label4.Text = "→"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblDLVRDTtitle
        '
        Me.lblDLVRDTtitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lblDLVRDTtitle.Location = New System.Drawing.Point(11, 95)
        Me.lblDLVRDTtitle.Name = "lblDLVRDTtitle"
        Me.lblDLVRDTtitle.Size = New System.Drawing.Size(133, 35)
        Me.lblDLVRDTtitle.Text = "受注納期："
        '
        'txtDLVRDT
        '
        Me.txtDLVRDT.Enabled = False
        Me.txtDLVRDT.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.txtDLVRDT.Location = New System.Drawing.Point(25, 115)
        Me.txtDLVRDT.Name = "txtDLVRDT"
        Me.txtDLVRDT.Size = New System.Drawing.Size(200, 34)
        Me.txtDLVRDT.TabIndex = 130
        '
        'lblODRNOTitle
        '
        Me.lblODRNOTitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lblODRNOTitle.Location = New System.Drawing.Point(13, 151)
        Me.lblODRNOTitle.Name = "lblODRNOTitle"
        Me.lblODRNOTitle.Size = New System.Drawing.Size(133, 35)
        Me.lblODRNOTitle.Text = "注文番号："
        '
        'txtODRNO
        '
        Me.txtODRNO.Enabled = False
        Me.txtODRNO.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.txtODRNO.Location = New System.Drawing.Point(25, 172)
        Me.txtODRNO.Name = "txtODRNO"
        Me.txtODRNO.Size = New System.Drawing.Size(200, 34)
        Me.txtODRNO.TabIndex = 133
        '
        'lblDBStatus
        '
        Me.lblDBStatus.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 9.0!, System.Drawing.FontStyle.Regular)
        Me.lblDBStatus.Location = New System.Drawing.Point(101, 37)
        Me.lblDBStatus.Name = "lblDBStatus"
        Me.lblDBStatus.Size = New System.Drawing.Size(124, 19)
        Me.lblDBStatus.Text = "DBStatus"
        Me.lblDBStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'FormPokaModify
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(240, 320)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtHMCD)
        Me.Controls.Add(Me.lblDBStatus)
        Me.Controls.Add(Me.txtODRNO)
        Me.Controls.Add(Me.lblODRNOTitle)
        Me.Controls.Add(Me.txtDLVRDT)
        Me.Controls.Add(Me.lblDLVRDTtitle)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtQTY)
        Me.Controls.Add(Me.txtQTYbefore)
        Me.Controls.Add(Me.lblQYTTitle)
        Me.Controls.Add(Me.lblHMCDTitle)
        Me.Controls.Add(Me.btnF4)
        Me.Controls.Add(Me.btnF3)
        Me.Controls.Add(Me.btnF2)
        Me.Controls.Add(Me.btnF1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FormPokaModify"
        Me.Text = "FormPokaModify"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnF4 As System.Windows.Forms.Button
    Friend WithEvents btnF3 As System.Windows.Forms.Button
    Friend WithEvents btnF2 As System.Windows.Forms.Button
    Friend WithEvents btnF1 As System.Windows.Forms.Button
    Friend WithEvents lblHMCDTitle As System.Windows.Forms.Label
    Friend WithEvents txtHMCD As System.Windows.Forms.TextBox
    Friend WithEvents lblQYTTitle As System.Windows.Forms.Label
    Friend WithEvents txtQTYbefore As System.Windows.Forms.TextBox
    Friend WithEvents txtQTY As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblDLVRDTtitle As System.Windows.Forms.Label
    Friend WithEvents txtDLVRDT As System.Windows.Forms.TextBox
    Friend WithEvents lblODRNOTitle As System.Windows.Forms.Label
    Friend WithEvents txtODRNO As System.Windows.Forms.TextBox
    Friend WithEvents lblDBStatus As System.Windows.Forms.Label
End Class
