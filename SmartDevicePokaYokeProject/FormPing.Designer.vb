<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FormPing
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
        Me.btnPing = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.txtOutput = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'btnPing
        '
        Me.btnPing.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnPing.Location = New System.Drawing.Point(3, 281)
        Me.btnPing.Name = "btnPing"
        Me.btnPing.Size = New System.Drawing.Size(128, 36)
        Me.btnPing.TabIndex = 0
        Me.btnPing.Text = "Ping の実行"
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.Location = New System.Drawing.Point(137, 281)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(98, 36)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "閉じる"
        '
        'txtOutput
        '
        Me.txtOutput.BackColor = System.Drawing.SystemColors.InfoText
        Me.txtOutput.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular)
        Me.txtOutput.ForeColor = System.Drawing.SystemColors.Info
        Me.txtOutput.Location = New System.Drawing.Point(3, 3)
        Me.txtOutput.Multiline = True
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.Size = New System.Drawing.Size(232, 272)
        Me.txtOutput.TabIndex = 2
        Me.txtOutput.TabStop = False
        '
        'FormPing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(238, 320)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtOutput)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnPing)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FormPing"
        Me.Text = "FormPing"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnPing As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
End Class
