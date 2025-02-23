<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormReceive
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkShelf = New System.Windows.Forms.CheckBox
        Me.chkMaster = New System.Windows.Forms.CheckBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(240, 28)
        Me.Label1.Text = "マスタ受信"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnF4
        '
        Me.btnF4.BackColor = System.Drawing.Color.Gold
        Me.btnF4.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF4.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF4.Location = New System.Drawing.Point(178, 286)
        Me.btnF4.Name = "btnF4"
        Me.btnF4.Size = New System.Drawing.Size(62, 34)
        Me.btnF4.TabIndex = 146
        Me.btnF4.Text = "ｸﾘｱ"
        '
        'btnF3
        '
        Me.btnF3.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnF3.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF3.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF3.Location = New System.Drawing.Point(118, 286)
        Me.btnF3.Name = "btnF3"
        Me.btnF3.Size = New System.Drawing.Size(62, 34)
        Me.btnF3.TabIndex = 147
        Me.btnF3.TabStop = False
        Me.btnF3.Text = "受信"
        '
        'btnF2
        '
        Me.btnF2.BackColor = System.Drawing.Color.Blue
        Me.btnF2.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF2.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF2.Location = New System.Drawing.Point(59, 286)
        Me.btnF2.Name = "btnF2"
        Me.btnF2.Size = New System.Drawing.Size(62, 34)
        Me.btnF2.TabIndex = 145
        Me.btnF2.Text = "全て"
        '
        'btnF1
        '
        Me.btnF1.BackColor = System.Drawing.Color.Red
        Me.btnF1.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 16.0!, System.Drawing.FontStyle.Regular)
        Me.btnF1.ForeColor = System.Drawing.SystemColors.ControlLightLight
        Me.btnF1.Location = New System.Drawing.Point(0, 286)
        Me.btnF1.Name = "btnF1"
        Me.btnF1.Size = New System.Drawing.Size(62, 34)
        Me.btnF1.TabIndex = 144
        Me.btnF1.Text = "戻る"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.DarkGray
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.chkShelf)
        Me.Panel1.Controls.Add(Me.chkMaster)
        Me.Panel1.Location = New System.Drawing.Point(14, 31)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(213, 204)
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular)
        Me.Label5.Location = New System.Drawing.Point(45, 167)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 20)
        Me.Label5.Text = "目安：30秒"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular)
        Me.Label4.Location = New System.Drawing.Point(45, 76)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 20)
        Me.Label4.Text = "目安：3分"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular)
        Me.Label3.Location = New System.Drawing.Point(45, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(138, 20)
        Me.Label3.Text = "KokenMaster.DB"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular)
        Me.Label2.Location = New System.Drawing.Point(45, 147)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(138, 20)
        Me.Label2.Text = "shelfstock.pkdat"
        '
        'chkShelf
        '
        Me.chkShelf.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.chkShelf.Location = New System.Drawing.Point(14, 112)
        Me.chkShelf.Name = "chkShelf"
        Me.chkShelf.Size = New System.Drawing.Size(182, 32)
        Me.chkShelf.TabIndex = 152
        Me.chkShelf.Text = "棚番マスタ"
        '
        'chkMaster
        '
        Me.chkMaster.Checked = True
        Me.chkMaster.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMaster.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.chkMaster.Location = New System.Drawing.Point(14, 21)
        Me.chkMaster.Name = "chkMaster"
        Me.chkMaster.Size = New System.Drawing.Size(182, 32)
        Me.chkMaster.TabIndex = 151
        Me.chkMaster.Text = "品目マスタ"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular)
        Me.Label6.Location = New System.Drawing.Point(14, 263)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(213, 20)
        Me.Label6.Text = " 151:ｵｰﾄﾊﾟﾜｰｵﾌ設定、142:ﾊﾞｯｸﾗｲﾄ設定"
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular)
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(3, 244)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(234, 20)
        Me.Label7.Text = "  受信中は以下の設定を解除して実行します."
        '
        'FormReceive
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 320)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnF4)
        Me.Controls.Add(Me.btnF3)
        Me.Controls.Add(Me.btnF2)
        Me.Controls.Add(Me.btnF1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FormReceive"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnF4 As System.Windows.Forms.Button
    Friend WithEvents btnF3 As System.Windows.Forms.Button
    Friend WithEvents btnF2 As System.Windows.Forms.Button
    Friend WithEvents btnF1 As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkShelf As System.Windows.Forms.CheckBox
    Friend WithEvents chkMaster As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
