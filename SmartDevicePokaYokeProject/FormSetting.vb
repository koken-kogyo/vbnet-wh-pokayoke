Public Class FormSetting

    Private Sub FormSetting_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True
        txtWaitTime.Text = mWaitTime
    End Sub

    ' F1キー (戻る)
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F4キー (更新)
    Private Sub btnF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF4.Click
        If IsNumeric(txtWaitTime.Text) = False Then
            MessageBox.Show("数値を入力してください．", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MsgBoxStyle.DefaultButton1)
            Return
        End If
        ' INIファイル更新
        mWaitTime = txtWaitTime.Text
        saveSettingWaitTime(mWaitTime)
        Dim msg As String = "遅延更新時間を " & txtWaitTime.Text & "秒に設定しました．"
        MessageBox.Show(msg, "確認", MessageBoxButtons.OK, MessageBoxIcon.None, MsgBoxStyle.DefaultButton1)
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        Close()
    End Sub

    Private Sub FormSetting_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyValue
            Case Bt.LibDef.BT_VK_F1
                Call btnF1_Click(sender, e)
            Case Bt.LibDef.BT_VK_F4
                Call btnF4_Click(sender, e)
        End Select
    End Sub

    Private Sub txtWaitTime_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWaitTime.GotFocus
        Dim modeSet As UInt32 = Bt.LibDef.BT_KEYINPUT_DIRECT
        Dim ret As Int32 = Bt.SysLib.Display.btSetKeyCharacter(modeSet)
        If ret <> 0 Then
            MessageBox.Show("キー入力設定に失敗しました:" & ret)
        End If

        txtWaitTime.SelectionStart = 0
        txtWaitTime.SelectionLength = txtWaitTime.TextLength
        txtWaitTime.BackColor = Color.Aqua
    End Sub

    Private Sub txtWaitTime_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWaitTime.KeyDown
        Select e.KeyCode
            Case System.Windows.Forms.Keys.Up
                txtWaitTime.Focus()
            Case System.Windows.Forms.Keys.Down
                txtWaitTime.Focus()
            Case System.Windows.Forms.Keys.Enter
                Call btnF4_Click(sender, e)
        End Select
    End Sub

    Private Sub txtWaitTime_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWaitTime.LostFocus
        txtWaitTime.BackColor = Color.White
    End Sub

End Class