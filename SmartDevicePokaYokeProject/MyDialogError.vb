Imports System.Threading
Imports Bt.SysLib
Imports Bt

Public Class MyDialogError

    Dim wCnt As Integer = 0

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub MyDialogError_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated

        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        ' ブザー制御構造体(Set)
        ' 「500msオン、500msオフ」を3回繰り返す設定
        Dim stBuzzerSet As New LibDef.BT_BUZZER_PARAM()
        stBuzzerSet.dwOn = 100      ' 鳴動時間[ms] （1～5000）
        stBuzzerSet.dwOff = 100     ' 停止時間[ms] （0～5000）
        stBuzzerSet.dwCount = 10    ' 鳴動回数[回] （0～100）
        stBuzzerSet.bTone = 2       ' 音階 （1～16）
        stBuzzerSet.bVolume = 3     ' ブザー音量 （1～3）

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
        stLedSet.bColor = LibDef.BT_LED_RED ' 点灯色

        Try
            ' btBuzzer 鳴動
            If FormMain.chkBuzzer.Checked Then ' 開発時ブザーは鳴らしたくない
                ret = Device.btBuzzer(1, stBuzzerSet)
                If ret <> LibDef.BT_OK Then
                    disp = "btBuzzer error ret[" & ret & "]"
                    MessageBox.Show(disp, "エラー")
                    Return
                End If
            End If
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

    Private Sub MyDialogError_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Timer1.Enabled = False
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'Thread.Sleep(500)
        If wCnt Mod 2 = 0 Then
            Me.BackColor = Color.Aqua
            Me.Label1.ForeColor = Color.Gray
            Me.Label2.ForeColor = Color.Gray
            Me.Label1.BackColor = Color.Aqua
            Me.Label2.BackColor = Color.Aqua
        Else
            Me.BackColor = Color.Red
            Me.Label1.ForeColor = Color.Snow
            Me.Label2.ForeColor = Color.Snow
            Me.Label1.BackColor = Color.Red
            Me.Label2.BackColor = Color.Red
        End If
        'Me.Refresh()
        wCnt = wCnt + 1
    End Sub

End Class