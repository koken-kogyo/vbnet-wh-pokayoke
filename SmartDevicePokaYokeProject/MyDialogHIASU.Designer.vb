<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MyDialogHIASU
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
        Me.btnOK = New System.Windows.Forms.Button
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblHIASU = New System.Windows.Forms.Label
        Me.txtHIASU = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 24.0!, System.Drawing.FontStyle.Regular)
        Me.btnOK.Location = New System.Drawing.Point(59, 271)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(122, 46)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "OK"
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTitle.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 18.0!, System.Drawing.FontStyle.Regular)
        Me.lblTitle.ForeColor = System.Drawing.Color.Cornsilk
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(240, 30)
        Me.lblTitle.Text = "出荷情報あり"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblHIASU
        '
        Me.lblHIASU.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.lblHIASU.Location = New System.Drawing.Point(11, 48)
        Me.lblHIASU.Name = "lblHIASU"
        Me.lblHIASU.Size = New System.Drawing.Size(209, 32)
        Me.lblHIASU.Text = "出荷ヒモ色＆数："
        '
        'txtHIASU
        '
        Me.txtHIASU.BackColor = System.Drawing.Color.HotPink
        Me.txtHIASU.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 36.0!, System.Drawing.FontStyle.Regular)
        Me.txtHIASU.ForeColor = System.Drawing.Color.MidnightBlue
        Me.txtHIASU.Location = New System.Drawing.Point(3, 83)
        Me.txtHIASU.Multiline = True
        Me.txtHIASU.Name = "txtHIASU"
        Me.txtHIASU.Size = New System.Drawing.Size(234, 182)
        Me.txtHIASU.TabIndex = 6
        Me.txtHIASU.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ｱｵ 10" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.txtHIASU.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'MyDialogHIASU
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(240, 320)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtHIASU)
        Me.Controls.Add(Me.lblHIASU)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "MyDialogHIASU"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblHIASU As System.Windows.Forms.Label
    Friend WithEvents txtHIASU As System.Windows.Forms.TextBox
End Class
