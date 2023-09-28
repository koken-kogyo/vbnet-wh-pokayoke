Imports System.Text
Imports Bt.CommLib
Imports Bt

Public Class FormTransmitting

    Private dialogFlg As Boolean = False
    Private accPath As [String] = "\FlashDisk\BT_FILES\drv1"
    Private tblName As String
    Private cnt As Integer

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        cnt = cnt + 1
        lblMessage.Text = lblMessage.Text.Replace(".", "") + New String(".", cnt)
        Refresh()
        If cnt > 5 Then cnt = 0
    End Sub

    Private Sub FormTransmitting_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        cnt = 0
        Timer1.Enabled = True
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Try
            ' 通信経路設定
            lblMessage.Text = "通信経路設定中"
            Refresh()
            System.Threading.Thread.Sleep(200)
            Dim route As UInt32 = LibDef.BT_COMM_DEVICE_USBCOM
            ret = KProtocol2.btComm2SetDev(route)
            If ret <> LibDef.BT_OK Then
                Timer1.Enabled = False
                lblMessage.Text = "通信経路設定エラー" & vbCrLf & " ret[" & ret & "]"
                Exit Sub
            End If

            ' PCへコネクト (タイムアウト10秒、キャンセル＝Cキー)
            lblMessage.Text = "PCへコネクト中"
            Refresh()
            System.Threading.Thread.Sleep(200)
            ret = KProtocol2.btComm2Connect(10)
            If ret <> LibDef.BT_OK Then
                Timer1.Enabled = False
                If ret = Bt.LibDef.BT_ERR_COMM_KP_TIMEOUT Then
                    lblMessage.Text = "通信相手との接続タイムアウト ret[" & ret & "]"
                Else
                    lblMessage.Text = "PCへのコネクトエラー" & vbCrLf & " ret[" & ret & "]"
                End If
                Exit Sub
            End If


            ' PCへファイルを送信
            lblMessage.Text = "ファイル送信中"
            Refresh()
            System.Threading.Thread.Sleep(200)
            Dim week As Int32 = DateTime.Now.DayOfWeek
            Dim jpName As String = ""
            If tblName = "Poka1" Then jpName = "品番照合クボタ"
            If tblName = "Poka2" Then jpName = "品番照合ヤンマー"
            If tblName = "Poka3" Then jpName = "品番照合ティエラ"
            If tblName = "Poka4" Then jpName = "品番照合オリエント"

            Dim localFileName As String = accPath & "\" & tblName & "_" & week & ".csv"
            Dim remoteFileName As String = Format(Now, "yyyyMMdd") & "_" & _
                mHtName & "_" & _
                jpName & ".csv"

            Dim localFile = New StringBuilder(localFileName)
            Dim remoteFile = New StringBuilder(remoteFileName)
            ret = KProtocol2.btComm2PutFile(localFile, remoteFile, CType(10, UInteger))
            If ret <> LibDef.BT_OK Then
                Timer1.Enabled = False
                lblMessage.Text = "ファイル送信エラー" & vbCrLf & " ret[" & ret & "]"
                Exit Sub
            End If



            ' 切断
            ret = KProtocol2.btComm2Disconnect(10)
            If ret <> LibDef.BT_OK Then
                Timer1.Enabled = False
                lblMessage.Text = "切断エラー" & vbCrLf & " ret[" & ret & "]"
                Exit Sub
            End If

            Timer1.Enabled = False
            lblMessage.Text = "ファイル送信完了"
            Refresh()
            dialogFlg = True

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            Close()
        End Try

    End Sub

    Private Sub FormTransmitting_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Timer1.Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If dialogFlg Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.Abort
        End If
        Me.Close()
    End Sub

    Public Sub New(ByVal _tblName As String)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        tblName = _tblName

    End Sub
End Class