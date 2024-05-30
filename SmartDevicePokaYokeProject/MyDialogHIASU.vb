Imports System.Threading
Imports Bt.SysLib
Imports Bt

Public Class MyDialogHIASU

    ' メンバー変数
    Private mHIASU As String
    Private mCOLOR As String
    Private mSU As String

    ' ユーザー定義コンストラクタ
    Public Sub New(ByVal _HIASU As String, ByVal _COLOR As String, ByVal _SU As String)
        Me.New() ' 一番最初に呼び出さなくてはならない
        mHIASU = _HIASU
        mCOLOR = _COLOR
        mSU = _SU
    End Sub

    ' 引数なしのデフォルトコンストラクタ
    Private Sub New()
        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub MyDialogHIASU_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        ' ブザー制御構造体(Set)
        ' 「500msオン、500msオフ」を3回繰り返す設定
        Dim stBuzzerSet As New LibDef.BT_BUZZER_PARAM()
        stBuzzerSet.dwOn = 100      ' 鳴動時間[ms] （1～5000）
        stBuzzerSet.dwOff = 100     ' 停止時間[ms] （0～5000）
        stBuzzerSet.dwCount = 1     ' 鳴動回数[回] （0～100）
        stBuzzerSet.bTone = 16      ' 音階 （1～16）
        stBuzzerSet.bVolume = 2     ' ブザー音量 （1～3）

        ' バイブレータ制御構造体(Set)
        ' 「100msオン、100msオフ」を1回繰り返す設定
        Dim stVibSet As New LibDef.BT_VIBRATOR_PARAM()
        stVibSet.dwOn = 200         ' 鳴動時間[ms] （1～5000）
        stVibSet.dwOff = 200        ' 停止時間[ms] （0～5000）
        stVibSet.dwCount = 1        ' 鳴動回数[回] （0～100）

        ' LED制御構造体(Set)
        ' 「500msオン、500msオフ」を3回繰り返す設定
        Dim stLedSet As New LibDef.BT_LED_PARAM()
        stLedSet.dwOn = 500         ' 鳴動時間[ms] （1～5000）
        stLedSet.dwOff = 500        ' 停止時間[ms] （0～5000）
        stLedSet.dwCount = 1        ' 鳴動回数[回] （0～100）
        stLedSet.bColor = LibDef.BT_LED_GREEN ' 点灯色

        Try
            If FormMain.chkBuzzer.Checked Then ' 開発時ブザーは鳴らしたくない
                ' btBuzzer 鳴動
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

        '
        txtHIASU.Text = vbCrLf & mHIASU & vbCrLf
        Select Case mCOLOR
            Case "ｱｶ"
                txtHIASU.BackColor = Color.Red
                txtHIASU.ForeColor = Color.Yellow
            Case "ｱｵ", "ｱｵﾑﾗ"
                txtHIASU.BackColor = Color.Blue
                txtHIASU.ForeColor = Color.Yellow
            Case "ﾑﾗｻｷ", "ﾑﾗ", "ﾑﾗﾑﾗ"
                txtHIASU.BackColor = Color.MediumOrchid
                txtHIASU.ForeColor = Color.Yellow
            Case "ﾐﾄﾞﾘ", "ﾐﾄﾞ", "ﾐﾄﾞﾑﾗ"
                txtHIASU.BackColor = Color.ForestGreen
                txtHIASU.ForeColor = Color.Gold
            Case "ｼﾛ", "ｼﾛﾑﾗ"
                txtHIASU.BackColor = Color.White
                txtHIASU.ForeColor = Color.Gray
            Case "ｷｲﾛ", "ｷ", "ｷﾑﾗ"
                txtHIASU.BackColor = Color.Yellow
                txtHIASU.ForeColor = Color.Red
            Case "ﾋﾟﾝｸ"
                txtHIASU.BackColor = Color.HotPink
                txtHIASU.ForeColor = Color.MidnightBlue
        End Select

        btnOK.Focus()

    End Sub

End Class