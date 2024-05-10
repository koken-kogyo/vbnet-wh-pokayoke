Imports System.Text
Imports Bt.CommLib
Imports Bt.SysLib
Imports Bt

Public Class FormPoka5Tana

    ' このWindowのインスタンス
    Public Shared FormPoka5Instance As FormPoka5Tana

    Private Const PROCESS_EXIT_WAIT_TIME As Integer = 200
    Private Const MASTER_PATH As String = "\FlashDisk\BT_FILES\drv1\"
    Private Const SS_MASTER As String = "shelfstock.pkdat"

    Private Sub FormPoka5Tana_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtTANCD.Text = FormMain.txtTANCD.Text
        txtClearAll()

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormPoka5Instance = Me

    End Sub

    Private Sub FormPoka5Tana_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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

    ' F1キー
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F2キー
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click

        If getRecordCount(tblNamePoka5) = 0 Then
            MessageBox.Show("送信するデータが存在しません", "警告")
            Exit Sub
        End If

        ' CSVファイル作成
        If SQLite2CSV(tblNamePoka5) <> 0 Then
            MessageBox.Show("CSVファイルの作成に失敗しました")
            Exit Sub
        End If

        ' ファイル転送
        Dim dialog As New FormTransmitting(tblNamePoka5)
        If (System.Windows.Forms.DialogResult.OK = dialog.ShowDialog()) Then

            'DB Delete
            If deletePokaX(tblNamePoka5) = 0 Then
                txtClear()
            Else
                MessageBox.Show("CSVファイルの作成に失敗しました")
                Exit Sub
            End If
        End If

    End Sub

    ' F3キー
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        If MessageBox.Show("少々時間がかかります．" & vbCrLf & "実行してよろしいですか？", "マスターファイル受信", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            Dim frm As Form = New FormDialog()
            frm.Show()
            Call receiveShelfStock(frm)
            frm.Close()
        End If
    End Sub

    '************************************************************
    ' ロケーションマスタ受信 (shelfstock.pkdat)
    '************************************************************
    Private Sub receiveShelfStock(ByRef frm As FormDialog)
        Dim ret As Integer

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

        ' PCからファイルを受信 (タイムアウト10秒)
        frm.lblMessage.Text = "マスタ受信中"
        Dim localFile As StringBuilder
        localFile = New StringBuilder(MASTER_PATH & SS_MASTER)
        ret = KProtocol2.btComm2GetFile(New StringBuilder(SS_MASTER), localFile, 10)
        If ret <> LibDef.BT_OK Then
            Dim msg As String
            Select Case ret
                Case LibDef.BT_ERR_COMM_KP_FILENOTFOUND
                    msg = "マスタファイルが見つかりません．" & vbCrLf & "[" & SS_MASTER & "]"
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
            Exit Sub
        End If

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

        MessageBox.Show("マスタファイルを受信しました．")
    End Sub

    ' F4キー
    Private Sub btnF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF4.Click
        If txtHMCD.Text <> "" Then
            Call txtClear()
        Else
            Call txtClearAll()
        End If
    End Sub

    Private Sub txtClearAll()
        txtTANACD.Text = ""
        txtHMCD.Text = ""
        lblStatus.Text = ""
        lblStatus.BackColor = Color.Silver
        lblCount.Text = getRecordCount(tblNamePoka5)
        txtTANACD.Focus()
    End Sub

    Private Sub txtClear()
        txtHMCD.Text = ""
        lblStatus.Text = ""
        lblStatus.BackColor = Color.Silver
        lblCount.Text = getRecordCount(tblNamePoka5)
        txtHMCD.Focus()
    End Sub

    Private Sub txtTANACD_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTANACD.GotFocus
        Dim modeSet As UInt32 = Bt.LibDef.BT_KEYINPUT_DIRECT
        Dim ret As Int32 = Bt.SysLib.Display.btSetKeyCharacter(modeSet)
        If ret <> 0 Then
            MessageBox.Show("キー入力設定に失敗しました:" & ret)
        End If
        'Call setScanProperty(2) ' Code系, JAN系 のみ読み取り可能に設定
        txtTANACD.SelectionStart = 0
        txtTANACD.SelectionLength = txtTANACD.TextLength
        txtTANACD.BackColor = Color.Aqua
    End Sub

    Private Sub txtTANACD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTANACD.TextChanged
        If txtTANACD.TextLength > 8 Then
            txtTANACD.Text = Strings.Left(txtTANACD.Text, 8)
            txtTANACD.SelectionStart = 8
        End If
    End Sub

    Private Sub txtTANACD_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTANACD.LostFocus
        If txtTANACD.Text = "" Then
            txtTANACD.BackColor = Color.White
        Else
            If checkBUCD(txtTANACD.Text) Then
                txtTANACD.BackColor = Color.White
            Else
                MessageBox.Show("棚番はマスタに存在しません．", "エラー")
                txtTANACD.Focus()
            End If
        End If
    End Sub

    Private Sub txtTANACD_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTANACD.KeyDown
        Select Case e.KeyCode
            Case System.Windows.Forms.Keys.Up
                txtHMCD.Focus()
            Case System.Windows.Forms.Keys.Down
                txtHMCD.Focus()
            Case System.Windows.Forms.Keys.Enter
                If txtTANACD.Text <> "" Then
                    txtHMCD.Focus()
                End If
        End Select
    End Sub

    Private Sub txtHMCD_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHMCD.GotFocus
        Dim modeSet As UInt32 = Bt.LibDef.BT_KEYINPUT_DIRECT
        Dim ret As Int32 = Bt.SysLib.Display.btSetKeyCharacter(modeSet)
        If ret <> 0 Then
            MessageBox.Show("キー入力設定に失敗しました:" & ret)
        End If
        'Call setScanProperty(1)
        txtHMCD.SelectionStart = 0
        txtHMCD.SelectionLength = txtHMCD.TextLength
        txtHMCD.BackColor = Color.Aqua
    End Sub

    Private Sub txtHMCD_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHMCD.LostFocus
        txtHMCD.BackColor = Color.White
    End Sub

    Private Sub txtHMCD_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtHMCD.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                txtTANACD.Focus()
            Case Keys.Down
                txtTANACD.Focus()
            Case Keys.Back
                If txtHMCD.Text = "" Then
                    txtTANACD.Focus()
                End If

            Case Keys.Enter

                ' 入力チェック
                If txtHMCD.Text = "" Then
                    'MessageBox.Show("社内品番をスキャンして下さい")
                    'txtHMCD.Focus()
                    Return
                ElseIf txtTANCD.Text = "" Then
                    MessageBox.Show("担当者ｺｰﾄﾞを確認してください")
                    Return
                ElseIf txtTANACD.Text = "" Then
                    MessageBox.Show("先に倉庫棚番をスキャンしてください")
                    txtTANACD.Focus()
                    Return
                ElseIf txtHMCD.TextLength > 24 Then
                    MessageBox.Show("正しい社内品番を読み取ってください")
                    txtHMCD.Focus()
                    Return
                End If

                ' ロケーション照合
                Call Judge()

        End Select
    End Sub

    ' ロケーション照合
    Private Sub Judge()
        Dim ret As Int32

        ' 照合処理
        Dim isOK As Boolean = checkJIDO(txtTANACD.Text, txtHMCD.Text)

        ' 照合結果出力
        Dim rec As DBTanaRecord
        rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
        rec.TANCD = txtTANCD.Text
        rec.HMCD = txtHMCD.Text
        rec.BUCD = ""
        rec.TANACD = txtTANACD.Text
        If isOK Then

            ' SQLite Insert
            rec.RESULT = "OK"
            ret = insertTana(tblNamePoka5, rec)
            If ret <> SQLITE_OK Then
                MessageBox.Show(sqliteErrorString & vbCrLf & _
                    "データベースの登録に失敗しました" & vbCrLf & _
                    "システム担当者に連絡してください")
                Return
            End If

            Call MyDialogOK_Activated()

            ' 次の照合へ
            Call txtClear()

            ' OKダイアログ表示
            lblStatus.Text = "OK"
            lblStatus.BackColor = Color.LimeGreen
            lblStatus.ForeColor = Color.Snow

        Else ' 照合ERROR

            ' SQLite Insert
            rec.RESULT = "NG"
            ret = insertTana(tblNamePoka5, rec)
            If ret <> SQLITE_OK Then
                MessageBox.Show(sqliteErrorString & vbCrLf & _
                    "データベースの登録に失敗しました" & vbCrLf & _
                    "システム担当者に連絡してください")
                Return
            End If

            ' 照合エラー
            Call MyDialogError_Activated()

            lblCount.Text = getRecordCount(tblNamePoka5)

            lblStatus.Text = "NG"
            lblStatus.BackColor = Color.Red
            lblStatus.ForeColor = Color.Yellow

            txtHMCD.SelectionStart = 0
            txtHMCD.SelectionLength = txtHMCD.TextLength
            txtHMCD.Focus()
        End If
    End Sub

    Private Sub MyDialogOK_Activated()
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
        ' 「500msオン、500msオフ」を3回繰り返す設定
        Dim stVibSet As New LibDef.BT_VIBRATOR_PARAM()
        stVibSet.dwOn = 500         ' 鳴動時間[ms] （1～5000）
        stVibSet.dwOff = 500        ' 停止時間[ms] （0～5000）
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

    End Sub

    Private Sub MyDialogError_Activated()

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

    End Sub

End Class
