Imports System.Text
Imports System.Threading
Imports Bt.CommLib
Imports Bt.SysLib
Imports Bt

Public Class FormReceive

    ' このWindowのインスタンス
    Public Shared FormMasterReceiveInstance As FormReceive

    Private Const MST_PATH As String = "\FlashDisk\BT_FILES\drv1\"
    Private Const MST_KOKEN As String = "KokenMaster.DB"
    Private Const MST_SHELF As String = "shelfstock.pkdat"

    Private gFlg As Boolean = False

    Private Sub FormReceive_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormMasterReceiveInstance = Me
    End Sub

    Private Sub FormReceive_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyValue
            Case Bt.LibDef.BT_VK_F1
                Call btnF1_Click(sender, e)
            Case Bt.LibDef.BT_VK_F2
                Call btnF2_Click(sender, e)
            Case Bt.LibDef.BT_VK_F3
                Call btnF3_Click(sender, e)
            Case Bt.LibDef.BT_VK_F4
                Call btnF4_Click(sender, e)
        End Select
    End Sub

    ' 戻る
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' 全て
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click
        gFlg = True
        chkMaster.Checked = True
        chkShelf.Checked = True
        gFlg = False
    End Sub

    ' 受信
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        If chkMaster.Checked = False And chkShelf.Checked = False Then
            MsgBox("受信するマスタを選択してください．", MsgBoxStyle.Exclamation)
            Exit Sub
        ElseIf MessageBox.Show("少々時間がかかります．" & vbCrLf & "実行してよろしいですか？", "マスターファイル受信", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MsgBoxStyle.DefaultButton1) = DialogResult.Yes Then
            Dim frm As Form = New FormDialog()
            frm.Show()
            Call receiveMaster(frm)
            frm.Close()
        End If
    End Sub

    ' ｸﾘｱ
    Private Sub btnF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF4.Click
        gFlg = True
        chkMaster.Checked = True
        chkShelf.Checked = False
        gFlg = False
    End Sub

    Private Sub chkMaster_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkMaster.KeyDown
        If gFlg Then Exit Sub
        If e.KeyCode = System.Windows.Forms.Keys.Enter Then
            chkMaster.Checked = Not chkMaster.Checked
        End If
    End Sub

    Private Sub chkShelf_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkShelf.KeyDown
        If gFlg Then Exit Sub
        If e.KeyCode = System.Windows.Forms.Keys.Enter Then
            chkShelf.Checked = Not chkShelf.Checked
        End If
    End Sub

    '************************************************************
    ' マスタファイル受信
    '************************************************************
    Private Sub receiveMaster(ByRef frm As FormDialog)
        Dim ret As Integer
        Dim gInterval As UInt32             ' オートパワーオフ設定値を保存

        ' 通信経路設定
        frm.lblMessage.Text = "通信経路設定"
        Dim route As UInt32 = LibDef.BT_COMM_DEVICE_USBCOM
        ret = KProtocol2.btComm2SetDev(route)
        If ret <> LibDef.BT_OK Then
            MessageBox.Show("通信経路設定エラー: ret[" & ret & "]")
            Exit Sub
        End If

        ' PCへコネクト (タイムアウト10秒、キャンセル＝Cキー)
        frm.lblMessage.Text = "コネクト中"
        ret = KProtocol2.btComm2Connect(10)
        If ret <> LibDef.BT_OK Then
            MessageBox.Show("PCへコネクトエラー: ret[" & ret & "]")
            Exit Sub
        End If

        ' SQLite データベースクローズ
        closeDB()

        ' オートパワーオフ設定を退避し、処理中はパワーオフなしで運用
        gInterval = getAutoPowerOFF()
        Call setAutoPowerOFF(0)

        If chkMaster.Checked Then
            ' PCからファイルを受信 (タイムアウト10秒)
            frm.lblMessage.Text = "マスタ1受信中"
            Dim localFile As StringBuilder
            localFile = New StringBuilder(MST_PATH & MST_KOKEN)
            ret = KProtocol2.btComm2GetFile(New StringBuilder(MST_KOKEN), localFile, 10)
            If ret <> LibDef.BT_OK Then
                Dim msg As String
                Select Case ret
                    Case LibDef.BT_ERR_COMM_KP_FILENOTFOUND
                        msg = "マスタファイルが見つかりません．" & vbCrLf & "[" & MST_KOKEN & "]"
                    Case LibDef.BT_ERR_COMM_KP_CANCELED
                        msg = "キャンセルされました．"
                    Case LibDef.BT_ERR_COMM_KP_INCOMPLETE
                        msg = "処理が完了しませんでした．"
                    Case LibDef.BT_ERR_COMM_KP_NETDOWN
                        msg = "通信経路が切断されました．" & vbCrLf & "（通信ユニット上に端末が無い等）"
                    Case LibDef.BT_ERR_COMM_KP_TIMEOUT
                        msg = "タイムアウトしました (10秒)．"
                    Case Else
                        msg = "PCファイルの受信に失敗しました: ret[" & ret & "]"
                End Select
                MessageBox.Show(msg)
                GoTo FUNCEND
            End If
        End If

        If chkShelf.Checked Then
            ' PCからファイルを受信 (タイムアウト10秒)
            frm.lblMessage.Text = "マスタ2受信中"
            Dim localFile As StringBuilder
            localFile = New StringBuilder(MST_PATH & MST_SHELF)
            ret = KProtocol2.btComm2GetFile(New StringBuilder(MST_SHELF), localFile, 10)
            If ret <> LibDef.BT_OK Then
                Dim msg As String
                Select Case ret
                    Case LibDef.BT_ERR_COMM_KP_FILENOTFOUND
                        msg = "マスタファイルが見つかりません．" & vbCrLf & "[" & MST_SHELF & "]"
                    Case LibDef.BT_ERR_COMM_KP_CANCELED
                        msg = "キャンセルされました．"
                    Case LibDef.BT_ERR_COMM_KP_INCOMPLETE
                        msg = "処理が完了しませんでした．"
                    Case LibDef.BT_ERR_COMM_KP_NETDOWN
                        msg = "通信経路が切断されました．" & vbCrLf & "（通信ユニット上に端末が無い等）"
                    Case LibDef.BT_ERR_COMM_KP_TIMEOUT
                        msg = "タイムアウトしました (10秒)．"
                    Case Else
                        msg = "PCファイルの受信に失敗しました: ret[" & ret & "]"
                End Select
                MessageBox.Show(msg)
                GoTo FUNCEND
            End If
        End If

        MessageBox.Show("マスタファイルを受信しました．")

FUNCEND:
        ' オートパワーオフ設定を元に戻す
        Call setAutoPowerOFF(gInterval)

        ' SQLite 再オープン
        frm.lblMessage.Text = "DB再オープン"
        ret = openDB()
        If ret <> 0 Then
            MessageBox.Show("受信後のオープンに失敗しました:" & ret & vbCrLf & "処理を終了します")
            Close()
        End If

        ' 切断
        frm.lblMessage.Text = "切断中"
        ret = KProtocol2.btComm2Disconnect(10)
        If ret <> LibDef.BT_OK Then
            MessageBox.Show("切断エラー: ret[" & ret & "]")
            Exit Sub
        End If

    End Sub

End Class