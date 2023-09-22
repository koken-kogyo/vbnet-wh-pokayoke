Imports System.Threading
Imports Bt.SysLib
Imports Bt

Public Class MyDialogWarn

    Dim wCnt As Integer = 0

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub MyDialogWarn_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        ' バイブレータ制御構造体(Set)
        ' 「500msオン、500msオフ」を3回繰り返す設定
        Dim stVibSet As New LibDef.BT_VIBRATOR_PARAM()
        stVibSet.dwOn = 500         ' 鳴動時間[ms] （1～5000）
        stVibSet.dwOff = 500        ' 停止時間[ms] （0～5000）
        stVibSet.dwCount = 5        ' 鳴動回数[回] （0～100）

        ' LED制御構造体(Set)
        ' 「500msオン、500msオフ」を3回繰り返す設定
        Dim stLedSet As New LibDef.BT_LED_PARAM()
        stLedSet.dwOn = 500         ' 鳴動時間[ms] （1～5000）
        stLedSet.dwOff = 500        ' 停止時間[ms] （0～5000）
        stLedSet.dwCount = 5        ' 鳴動回数[回] （0～100）
        stLedSet.bColor = LibDef.BT_LED_YELLOW ' 点灯色

        Try
            ' btVibrator 鳴動
            ret = Device.btVibrator(1, stVibSet)
            If ret <> LibDef.BT_OK Then
                disp = "btVibrator error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
            ' btLED 点灯
            ret = Device.btLED(1, stLedSet)
            If ret <> LibDef.BT_OK Then
                disp = "btLED error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        ' 画面フラッシュ
        wCnt = 1
        Timer1.Enabled = True

    End Sub

    Private Sub MyDialogWarn_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Timer1.Enabled = False
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        ' ブザー制御構造体(Set)
        ' 「500msオン、500msオフ」を3回繰り返す設定
        Dim stBuzzerSet1 As New LibDef.BT_BUZZER_PARAM()
        stBuzzerSet1.dwOn = 100      ' 鳴動時間[ms] （1～5000）
        stBuzzerSet1.dwOff = 100     ' 停止時間[ms] （0～5000）
        stBuzzerSet1.dwCount = 1     ' 鳴動回数[回] （0～100）
        stBuzzerSet1.bTone = 7       ' 音階 （1～16）
        stBuzzerSet1.bVolume = 3     ' ブザー音量 （1～3）
        Dim stBuzzerSet2 As New LibDef.BT_BUZZER_PARAM()
        stBuzzerSet2.dwOn = 100      ' 鳴動時間[ms] （1～5000）
        stBuzzerSet2.dwOff = 100     ' 停止時間[ms] （0～5000）
        stBuzzerSet2.dwCount = 1     ' 鳴動回数[回] （0～100）
        stBuzzerSet2.bTone = 3       ' 音階 （1～16）
        stBuzzerSet2.bVolume = 3     ' ブザー音量 （1～3）

        'Thread.Sleep(500)
        If wCnt Mod 2 = 0 Then
            Me.BackColor = Color.Blue
            Me.Label1.ForeColor = Color.Aqua
            Me.Label2.ForeColor = Color.Aqua
            Me.Label3.ForeColor = Color.Aqua
            Me.Label4.ForeColor = Color.Aqua
            Me.Label5.ForeColor = Color.Aqua
            Me.Label1.BackColor = Color.Blue
            Me.Label2.BackColor = Color.Blue
            Me.Label3.BackColor = Color.Blue
            Me.Label4.BackColor = Color.Blue
            Me.Label5.BackColor = Color.Blue
        Else
            Me.BackColor = Color.Yellow
            Me.Label1.ForeColor = Color.Red
            Me.Label2.ForeColor = Color.Red
            Me.Label3.ForeColor = Color.Red
            Me.Label4.ForeColor = Color.Red
            Me.Label5.ForeColor = Color.Red
            Me.Label1.BackColor = Color.Yellow
            Me.Label2.BackColor = Color.Yellow
            Me.Label3.BackColor = Color.Yellow
            Me.Label4.BackColor = Color.Yellow
            Me.Label5.BackColor = Color.Yellow
            If wCnt < 8 Then
                ' btBuzzer 鳴動
                If FormMain.chkBuzzer.Checked Then ' 開発時ブザーは鳴らしたくない
                    ret = Device.btBuzzer(1, stBuzzerSet1)
                    If ret <> LibDef.BT_OK Then
                        disp = "btBuzzer error ret[" & ret & "]"
                        MessageBox.Show(disp, "エラー")
                        Return
                    End If
                End If
                Thread.Sleep(100)
                If FormMain.chkBuzzer.Checked Then ' 開発時ブザーは鳴らしたくない
                    ret = Device.btBuzzer(1, stBuzzerSet2)
                    If ret <> LibDef.BT_OK Then
                        disp = "btBuzzer error ret[" & ret & "]"
                        MessageBox.Show(disp, "エラー")
                        Return
                    End If
                End If
            End If
        End If
        'Me.Refresh()
        wCnt = wCnt + 1
    End Sub

End Class