<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormDialog
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
        Me.lblMessage = New System.Windows.Forms.Label
        Me.Timer1 = New System.Windows.Forms.Timer
        Me.SuspendLayout()
        '
        'lblMessage
        '
        Me.lblMessage.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lblMessage.Location = New System.Drawing.Point(18, 26)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(177, 33)
        Me.lblMessage.Text = "しばらくお待ち下さい"
        '
        'Timer1
        '
        Me.Timer1.Interval = 200
        '
        'FormDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(198, 75)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblMessage)
        Me.Location = New System.Drawing.Point(22, 110)
        Me.Name = "FormDialog"
        Me.Text = "マスタファイル受信"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
