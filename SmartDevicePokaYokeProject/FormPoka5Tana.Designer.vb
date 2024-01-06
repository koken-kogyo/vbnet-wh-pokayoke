<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormPoka5Tana
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
        Me.LabelMenu = New System.Windows.Forms.Label
        Me.btnF4 = New System.Windows.Forms.Button
        Me.btnF1 = New System.Windows.Forms.Button
        Me.txtTANCD = New System.Windows.Forms.TextBox
        Me.lblCount = New System.Windows.Forms.Label
        Me.txtTANACD = New System.Windows.Forms.TextBox
        Me.txtHMCD = New System.Windows.Forms.TextBox
        Me.lblTKHMCDTitle = New System.Windows.Forms.Label
        Me.lblHMCDTitle = New System.Windows.Forms.Label
        Me.btnF3 = New System.Windows.Forms.Button
        Me.btnF2 = New System.Windows.Forms.Button
        Me.txtBUCD = New System.Windows.Forms.TextBox
        Me.lblTANCD = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'LabelMenu
        '
        Me.LabelMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.LabelMenu.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.LabelMenu.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.LabelMenu.Location = New System.Drawing.Point(0, 0)
        Me.LabelMenu.Name = "LabelMenu"
        Me.LabelMenu.Size = New System.Drawing.Size(240, 28)
        Me.LabelMenu.Text = "ﾛｹｰｼｮﾝ照合"
        Me.LabelMenu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnF4
        '
        Me.btnF4.BackColor = System.Drawing.Color.Gold
        Me.btnF4.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF4.Location = New System.Drawing.Point(178, 285)
        Me.btnF4.Name = "btnF4"
        Me.btnF4.Size = New System.Drawing.Size(62, 34)
        Me.btnF4.TabIndex = 5
        Me.btnF4.Text = "ｸﾘｱ"
        '
        'btnF1
        '
        Me.btnF1.BackColor = System.Drawing.Color.Red
        Me.btnF1.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF1.Location = New System.Drawing.Point(0, 285)
        Me.btnF1.Name = "btnF1"
        Me.btnF1.Size = New System.Drawing.Size(62, 34)
        Me.btnF1.TabIndex = 3
        Me.btnF1.Text = "戻る"
        '
        'txtTANCD
        '
        Me.txtTANCD.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtTANCD.Enabled = False
        Me.txtTANCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.txtTANCD.Location = New System.Drawing.Point(138, 31)
        Me.txtTANCD.Name = "txtTANCD"
        Me.txtTANCD.Size = New System.Drawing.Size(89, 42)
        Me.txtTANCD.TabIndex = 128
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
        'txtTANACD
        '
        Me.txtTANACD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 20.0!, System.Drawing.FontStyle.Regular)
        Me.txtTANACD.Location = New System.Drawing.Point(118, 79)
        Me.txtTANACD.Name = "txtTANACD"
        Me.txtTANACD.Size = New System.Drawing.Size(109, 46)
        Me.txtTANACD.TabIndex = 0
        Me.txtTANACD.Text = "1-10-10"
        '
        'txtHMCD
        '
        Me.txtHMCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 20.0!, System.Drawing.FontStyle.Regular)
        Me.txtHMCD.Location = New System.Drawing.Point(11, 153)
        Me.txtHMCD.Name = "txtHMCD"
        Me.txtHMCD.Size = New System.Drawing.Size(216, 46)
        Me.txtHMCD.TabIndex = 1
        '
        'lblTKHMCDTitle
        '
        Me.lblTKHMCDTitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblTKHMCDTitle.Location = New System.Drawing.Point(11, 86)
        Me.lblTKHMCDTitle.Name = "lblTKHMCDTitle"
        Me.lblTKHMCDTitle.Size = New System.Drawing.Size(124, 32)
        Me.lblTKHMCDTitle.Text = "倉庫棚番："
        '
        'lblHMCDTitle
        '
        Me.lblHMCDTitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblHMCDTitle.Location = New System.Drawing.Point(11, 127)
        Me.lblHMCDTitle.Name = "lblHMCDTitle"
        Me.lblHMCDTitle.Size = New System.Drawing.Size(121, 35)
        Me.lblHMCDTitle.Text = "社内品番："
        '
        'btnF3
        '
        Me.btnF3.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnF3.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF3.Location = New System.Drawing.Point(118, 285)
        Me.btnF3.Name = "btnF3"
        Me.btnF3.Size = New System.Drawing.Size(62, 34)
        Me.btnF3.TabIndex = 143
        Me.btnF3.TabStop = False
        '
        'btnF2
        '
        Me.btnF2.BackColor = System.Drawing.Color.Blue
        Me.btnF2.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF2.Location = New System.Drawing.Point(59, 285)
        Me.btnF2.Name = "btnF2"
        Me.btnF2.Size = New System.Drawing.Size(62, 34)
        Me.btnF2.TabIndex = 4
        Me.btnF2.Text = "送信"
        '
        'txtBUCD
        '
        Me.txtBUCD.Enabled = False
        Me.txtBUCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 20.0!, System.Drawing.FontStyle.Regular)
        Me.txtBUCD.Location = New System.Drawing.Point(11, 205)
        Me.txtBUCD.Name = "txtBUCD"
        Me.txtBUCD.Size = New System.Drawing.Size(216, 46)
        Me.txtBUCD.TabIndex = 2
        '
        'lblTANCD
        '
        Me.lblTANCD.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblTANCD.Location = New System.Drawing.Point(11, 38)
        Me.lblTANCD.Name = "lblTANCD"
        Me.lblTANCD.Size = New System.Drawing.Size(121, 29)
        Me.lblTANCD.Text = "担当者："
        '
        'FormPoka5Tana
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 320)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblTANCD)
        Me.Controls.Add(Me.txtBUCD)
        Me.Controls.Add(Me.txtTANCD)
        Me.Controls.Add(Me.btnF4)
        Me.Controls.Add(Me.btnF3)
        Me.Controls.Add(Me.btnF2)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.txtTANACD)
        Me.Controls.Add(Me.txtHMCD)
        Me.Controls.Add(Me.lblTKHMCDTitle)
        Me.Controls.Add(Me.lblHMCDTitle)
        Me.Controls.Add(Me.btnF1)
        Me.Controls.Add(Me.LabelMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FormPoka5Tana"
        Me.Text = "FormPoka5Location"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelMenu As System.Windows.Forms.Label
    Friend WithEvents btnF4 As System.Windows.Forms.Button
    Friend WithEvents btnF1 As System.Windows.Forms.Button
    Friend WithEvents txtTANCD As System.Windows.Forms.TextBox
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Public WithEvents txtTANACD As System.Windows.Forms.TextBox
    Public WithEvents txtHMCD As System.Windows.Forms.TextBox
    Friend WithEvents lblTKHMCDTitle As System.Windows.Forms.Label
    Friend WithEvents lblHMCDTitle As System.Windows.Forms.Label
    Friend WithEvents btnF3 As System.Windows.Forms.Button
    Friend WithEvents btnF2 As System.Windows.Forms.Button
    Public WithEvents txtBUCD As System.Windows.Forms.TextBox
    Friend WithEvents lblTANCD As System.Windows.Forms.Label
End Class
