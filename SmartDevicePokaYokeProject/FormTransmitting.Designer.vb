<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormTransmitting
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
        Me.Button1 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer
        Me.SuspendLayout()
        '
        'lblMessage
        '
        Me.lblMessage.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 11.0!, System.Drawing.FontStyle.Regular)
        Me.lblMessage.Location = New System.Drawing.Point(3, 12)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(192, 43)
        Me.lblMessage.Text = "Label1"
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("TTヒラギノUD丸ゴ Mono StdN W4", 14.0!, System.Drawing.FontStyle.Regular)
        Me.Button1.Location = New System.Drawing.Point(55, 59)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(92, 33)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "閉じる"
        '
        'Timer1
        '
        Me.Timer1.Interval = 200
        '
        'FormTransmitting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(198, 95)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblMessage)
        Me.Location = New System.Drawing.Point(22, 110)
        Me.Name = "FormTransmitting"
        Me.Text = "送信ダイアログ"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblMessage As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
