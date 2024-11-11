Imports System.Threading
Imports System.Net
Imports Bt.CommLib
Imports Bt

Public Class FormWiFiCheck

    Private cnt As Integer

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        cnt = cnt + 1
        lblMessage.Text = lblMessage.Text.Replace(".", "") + New String(".", cnt)
        Refresh()
        If cnt > 5 Then cnt = 0
    End Sub

    Private Sub FormWiFiCheck_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        cnt = 0
        Timer1.Enabled = True
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim statusGet As UInt32 = 0
        Dim typeSet As UInt32 = 0
        Dim levelRssiGet As UInt32 = 0

        Thread.Sleep(100)

        ' HTの無線LAN状態取得
        ret = Wlan.btWLANGetStatus(statusGet)
        If ret = LibDef.BT_OK And statusGet = LibDef.BT_WLAN_STS_CONNECTED Then
            lblHT.BackColor = Color.LimeGreen
            lblHT.ForeColor = Color.Black
            lblHT.Text = "この端末はOK"
        Else
            lblHT.BackColor = Color.Red
            lblHT.ForeColor = Color.Yellow
            Timer1.Enabled = False
            lblMessage.Text = "デバイス未オープン．"
            GoTo WIFICHECKEND
        End If
        Refresh()
        Thread.Sleep(100)

        ' HTの電波レベルの取得
        typeSet = LibDef.BT_WLAN_TYPE_RSSI
        ret = Wlan.btWLANGetSignalLevel(typeSet, levelRssiGet)
        If ret <> LibDef.BT_OK Then
            lblMessage.Text = "電波状態取得異常．"
            GoTo WIFICHECKEND
        Else
            If levelRssiGet >= 2 Then
                lblAP.BackColor = Color.LimeGreen
                lblAP.ForeColor = Color.Black
                lblAP.Text = "電波状態は良"
            Else
                lblAP.BackColor = Color.Yellow
                lblAP.ForeColor = Color.Black
                lblAP.Text = "電波状態は弱"
            End If
        End If
        Refresh()
        Thread.Sleep(100)

        ' SQL Server 2008 とのping疎通確認
        'If pingSQLServer() Then
        If checkSQLServer() Then
            lblSV.BackColor = Color.LimeGreen
            lblSV.ForeColor = Color.Black
        Else
            lblSV.BackColor = Color.Red
            lblSV.ForeColor = Color.Yellow
            lblSV.Text = "物流事務所×"
            lblMessage.Text = "WHap01を確認してください．"
            GoTo WIFICHECKEND
        End If

        lblMessage.Text = "チェック終了．"
        Me.DialogResult = System.Windows.Forms.DialogResult.OK

WIFICHECKEND:
        Refresh()
        Thread.Sleep(500)
        Timer1.Enabled = False
    End Sub

    Private Function pingSQLServer() As Boolean

        Dim fn As String = mAppPath & "\" & "ping.log"

        Dim proc As New Process()
        proc.StartInfo.FileName = "cmd.exe"
        proc.StartInfo.Arguments = "/c ping -n 3 -w 1000 " & mSQLServer & " > " & fn
        proc.StartInfo.UseShellExecute = False
        proc.Start()
        proc.WaitForExit()

        Dim sr As New System.IO.StreamReader(fn, System.Text.Encoding.GetEncoding("shift_jis"))
        Dim s As String = sr.ReadToEnd()
        sr.Close()

        If s.IndexOf("Reply from") > 0 Then
            pingSQLServer = True
        Else
            pingSQLServer = False
        End If

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub

    ' 端末ライブラリリファレンス【通信制御編】マニュアル
    ' btWLANGetStatus
    '   引数
    '       status 無線LAN 状態 (O)
    '           BT_WLAN_STS_DISABLE         デバイス無効状態
    '           BT_WLAN_STS_CONNECTED       接続状態
    '           BT_WLAN_STS_LINKLOST        圏外状態
    '           BT_WLAN_STS_PROCESSING      接続試み中
    '           BT_WLAN_STS_DISCONNECTED    切断状態
    '   戻り値
    '       BT_OK                           正常終了
    '       BT_ERR                          異常終了
    '       BT_ERR_ARG_1                    第1 引数エラー（NULL 指定）
    '       BT_ERR_COMM_NOTOPEN             対象の通信デバイスがオープンされていない

    ' btWLANGetSignalLevel
    '   引数
    '       type 取得タイプ (I)
    '           BT_WLAN_TYPE_RSSI           RSSI の取得
    '           BT_WLAN_TYPE_SNR            SNR の取得
    '       value 電波レベル (O)
    '                                       4 段階（0～3）
    '   戻り値
    '       BT_OK                           正常終了
    '       BT_ERR                          異常終了
    '       BT_ERR_ARG_1                    第1 引数エラー（範囲外指定）
    '       BT_ERR_ARG_2                    第2 引数エラー（NULL 指定）
    '       BT_ERR_COMM_NOTOPEN             対象の通信デバイスがオープンされていない

End Class