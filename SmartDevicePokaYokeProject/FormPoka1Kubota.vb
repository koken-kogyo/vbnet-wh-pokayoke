Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Data

Public Class FormPoka1Kubota

    ' このWindowのインスタンス
    Public Shared FormPoka1Instance As FormPoka1Kubota

    Dim flgSCAN As Boolean = False ' 社内品番のキー入力禁止フラグ
    Dim flgConfirm As Boolean = False ' 照合済みフラグ
    Dim gTKCD As String = "" ' 社内品番Enter時に得意先コードを保持
    Dim gWaitRec() As DBPokaRecord
    Dim gRetry As Integer = 0 ' Timerイベント回数
    Dim gInterval As UInt32 ' オートパワーオフ設定値を保存

    Private Sub FormPoka1Kubota_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtTANCD.Text = FormMain.txtTANCD.Text
        chkTe.Checked = If(mHandOK = "1", True, False)
        chkQR.Checked = If(mQROnly = "1", True, False)
        txtClear()

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormPoka1Instance = Me

    End Sub

    Private Sub FormPoka1Kubota_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        Call saveSettingHandOK(chkTe.Checked)
        Call saveSettingQROnly(chkQR.Checked)
    End Sub

    ' F1キー
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F2キー
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click

        If getRecordCount(tblNamePoka1) = 0 Then
            MessageBox.Show("送信するデータが存在しません", "警告")
            Exit Sub
        End If

        ' CSVファイル作成
        If SQLite2CSV(tblNamePoka1) <> 0 Then
            MessageBox.Show("CSVファイルの作成に失敗しました")
            Exit Sub
        End If

        ' ファイル転送
        Dim dialog As New FormTransmitting(tblNamePoka1)
        If (System.Windows.Forms.DialogResult.OK = dialog.ShowDialog()) Then

            'DB Delete
            If deletePokaX(tblNamePoka1) = 0 Then
                txtClear()
            Else
                MessageBox.Show("CSVファイルの作成に失敗しました")
                Exit Sub
            End If
        End If

    End Sub

    ' F3キー (履歴表示)
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        Dim frm As Form = New FormPokaHistory(tblNamePoka1)
        frm.ShowDialog()
        lblCount.Text = getRecordCount(tblNamePoka1) ' 24.05.30 add y.w
    End Sub

    ' F4キー
    Private Sub btnF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF4.Click
        Call txtClear()
    End Sub

    Private Sub txtClear()
        txtHMCD.Text = ""
        lblHMCD.Text = ""
        txtTKHMCD.Text = ""
        lblTKHMCD.Text = ""
        txtQTY.Text = "" ' 24.05.10 add y.w
        lblHIASU.Text = "" '24.05.20 add y.w
        lblCount.Text = getRecordCount(tblNamePoka1)
        txtHMCD.Focus()
        flgConfirm = False
    End Sub

    Private Sub FormPoka1Kubota_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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

    Private Sub txtHMCD_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHMCD.GotFocus
        Dim modeSet As UInt32 = Bt.LibDef.BT_KEYINPUT_DIRECT
        Dim ret As Int32 = Bt.SysLib.Display.btSetKeyCharacter(modeSet)
        If ret <> 0 Then
            MessageBox.Show("キー入力設定に失敗しました:" & ret)
        End If
        Call setScanProperty(1)
        txtHMCD.SelectionStart = 0
        txtHMCD.SelectionLength = txtHMCD.TextLength
        txtHMCD.BackColor = Color.Aqua
    End Sub

    Private Sub txtHMCD_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHMCD.LostFocus

        ' 社内品番は [ハイフン, 空白] を除去
        ' (SATOのラベルプリンターは品番24桁空白パディングでバーコードが作成されている)
        txtHMCD.Text = Trim(txtHMCD.Text)
        lblHMCD.Text = txtHMCD.Text.Replace("-", "")

        ' エラーチェック
        If txtHMCD.TextLength > 24 Then
            MessageBox.Show("24桁以下の品番を読み取ってください")
            txtHMCD.Focus()
            Return
        End If
        If Strings.Left(txtHMCD.Text, 2) = "**" Then
            MessageBox.Show("社内品番を読み取ってください[**]")
            txtHMCD.Focus()
            Return
        End If

        'If txtHMCD.Text <> "" Then

        '    ' 得意先コードを取得
        '    Dim tkcd As String = getTKCD(txtHMCD.Text)

        '    ' C0101:堺 C0104:臨海
        '    If tkcd = "C0101" Or tkcd = "C0104" Then
        '        chkQR.Checked = True
        '    Else
        '        chkQR.Checked = False
        '    End If

        'End If

        ' 入力待機色を解除
        txtHMCD.BackColor = Color.White

    End Sub

    Private Sub txtHMCD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtHMCD.KeyDown
        Select Case e.KeyCode
            Case System.Windows.Forms.Keys.Up
                txtQTY.Focus() ' 24.05.10 mod y.w
            Case System.Windows.Forms.Keys.Down
                txtTKHMCD.Focus()
            Case System.Windows.Forms.Keys.Enter
                If txtHMCD.Text <> "" Then
                    Dim wSKHIASU As String = ""
                    Dim wCOLOR As String = ""
                    Dim wSU As String = ""
                    Dim ret As Boolean
                    lblHIASU.Text = ""
                    gTKCD = "" ' 後々使う得意先コードをここで取得
                    ' 品目マスターチェック
                    ret = getM0500(txtHMCD.Text, gTKCD, wSKHIASU, wCOLOR, wSU)
                    If ret And wSKHIASU <> "" Then

                        ' ラベル表示
                        lblHIASU.Text = wSKHIASU

                        ' 色指定あり＆数が２本以上の場合、ポップアップ画面を出す
                        If wCOLOR <> "" And Convert.ToInt32(wSU) > 1 Then
                            Dim myDialog As MyDialogHIASU
                            myDialog = New MyDialogHIASU(wSKHIASU, wCOLOR, wSU)
                            myDialog.ShowDialog()
                        End If
                    End If
                    txtTKHMCD.Focus()
                End If
            Case 152, 153 'サイドボタンVK_TRG:0x98(152)、SCAN:VK_CTRG:0x99(153)ボタン 24.05.31 mod y.w
                flgSCAN = True
            Case Keys.NumPad0 To Keys.NumPad9   ' テンキーの0～9
                flgSCAN = False
            Case Keys.Subtract, Keys.Decimal    ' テンキーの減算"-", 小数点"." 
                flgSCAN = False
        End Select
    End Sub

    Private Sub txtHMCD_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtHMCD.KeyPress
        ' 通常時はキー入力禁止（チェックオンで社内品番を入力可能にする）
        If chkTe.Checked = False And flgSCAN = False Then e.Handled = True
    End Sub

    Private Sub txtTKHMCD_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTKHMCD.GotFocus
        Dim modeSet As UInt32 = Bt.LibDef.BT_KEYINPUT_DIRECT
        Dim ret As Int32 = Bt.SysLib.Display.btSetKeyCharacter(modeSet)
        If ret <> 0 Then
            MessageBox.Show("キー入力設定に失敗しました:" & ret)
        End If

        ' QR読取フラグの設定
        If chkQR.Checked Then
            Call setScanProperty(2) ' QRコードのみ読み取り可能に設定(OCRもオフ)
        Else
            Call setScanProperty(1) ' すべての読取を可能に設定
        End If

        txtTKHMCD.SelectionStart = 0
        txtTKHMCD.SelectionLength = txtTKHMCD.TextLength
        txtTKHMCD.BackColor = Color.Aqua
    End Sub

    Private Sub txtTKHMCD_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTKHMCD.LostFocus
        txtTKHMCD.BackColor = Color.White
        lblTKHMCD.Text = txtTKHMCD.Text
    End Sub

    Private Sub txtTKHMCD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTKHMCD.KeyDown
        Select Case e.KeyCode
            Case System.Windows.Forms.Keys.Up
                txtHMCD.Focus()
            Case System.Windows.Forms.Keys.Down
                txtQTY.Focus() ' 24.05.10 mod y.w
            Case System.Windows.Forms.Keys.Back
                If txtTKHMCD.Text = "" Then
                    txtHMCD.Focus()
                End If
            Case System.Windows.Forms.Keys.Enter
                If txtTANCD.Text = "" Then
                    MessageBox.Show("担当者ｺｰﾄﾞを確認してください")
                    Return
                ElseIf txtHMCD.Text = "" Then
                    MessageBox.Show("社内品番を入力してください")
                    txtHMCD.Focus()
                    Return
                ElseIf txtTKHMCD.Text = "" Then
                    MessageBox.Show("メーカー品番をスキャンしてください")
                    txtTKHMCD.Focus()
                    Return
                End If

                ' 品番照合
                Call Judge()

        End Select
    End Sub

    ' 品番照合
    ' lblHMCD(ハイフンが除去されたもの)とtxtTKHMCDを照合
    Private Sub Judge()
        Dim ret As Int32

        Dim _HMCD As String = txtHMCD.Text.Replace("-", "") ' ハイフンは除去
        Dim _TKHMCD As String = txtTKHMCD.Text              ' 23.09.28 得意先品番のハイフンは除去しない

        'If txtHMCD.Text = "V0531-68961-S" Then _HMCD = "V053168961" ' スリーブ品番特化対策 23.09.22
        'If txtHMCD.Text = "V0531-68971-S" Then _HMCD = "V053168971" ' スリーブ品番特化対策 23.09.28
        If Strings.Right(txtHMCD.Text, 2) = "-S" Then ' 23.10.04 スリーブ品番対応
            _HMCD = Strings.Left(txtHMCD.Text, txtHMCD.Text.Length - 2).Replace("-", "")
        End If

        Dim i As Int32 = _HMCD.Length
        Dim j As Int32 = _TKHMCD.Length

        Dim isOK As Boolean = False
        Dim isWN As Boolean = False

        ' 照合処理
        If txtHMCD.Text = txtTKHMCD.Text Then ' 同じものを読み取ったらワーニング
            isWN = True

        ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 1, i) Then ' 先頭から社内品番文字数分
            isOK = True

        ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 2, i) Then ' OCR対応 先頭"*"が入る 23.09.27
            isOK = True

        ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 3, i) Then ' 3バイト目から社内品番文字数分
            isOK = True

        ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 4, i) Then ' 4バイト目から社内品番文字数分 23.10.20
            isOK = True

        ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 6, i) Then ' 6バイト目から社内品番文字数分
            isOK = True

        ElseIf j > 30 And _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 18, i) Then ' 18バイト目から社内品番文字数分
            isOK = True ' 8/8 add メーカー品番 KQR_TagZ

        ElseIf j > 30 And _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 20, i) Then ' 20バイト目から社内品番文字数分
            isOK = True '

        ElseIf j > 70 And _HMCD = Strings.Mid(_TKHMCD, 59, 12).Replace("-", "") Then ' 素地品番を照合
            isOK = True ' 8/4 add 素地品番 「23RP85A7961186241411001                0000005TP3920000005RP851-7961-101910     20230731            」

        ElseIf i > 11 And Strings.Left(_HMCD, 11) = Strings.Mid(_TKHMCD, 1, 11) Then
            isOK = True ' 8/8 add 社内品番11桁 TA040333628000015

        ElseIf i > 11 And Strings.Left(_HMCD, 11) = Strings.Mid(_TKHMCD, 3, 11) Then
            isOK = True ' 8/8 add 社内品番11桁 TA040333628000015

        ElseIf _HMCD = "RD55192432" And Strings.Mid(_TKHMCD, 59, 12) = "RD551-9243-1" Then
            isOK = True ' 8/10 add 素地品番違い 臨時対応

        ElseIf _HMCD = "RD55192442" And Strings.Mid(_TKHMCD, 59, 12) = "RD551-9244-1" Then
            isOK = True ' 8/10 add 素地品番違い 臨時対応

        ElseIf _HMCD = "RP47168632" And Strings.Mid(_TKHMCD, 59, 12) = "RP47B-6863-2" Then
            isOK = True ' 8/10 add 素地品番違い 臨時対応

        ElseIf _HMCD = "RP47168643" And Strings.Mid(_TKHMCD, 59, 12) = "RP47B-6864-3" Then
            isOK = True ' 8/10 add 素地品番違い 臨時対応

        Else

            ' 得意先マスタ[M0600]の情報で再度照合をかける 23.10.04
            _HMCD = getTKHMCD(txtHMCD.Text, _TKHMCD).Replace("-", "") ' SQLiteのマスタサーチ

            If _HMCD <> "" Then

                i = _HMCD.Length

                If _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 1, i) Then ' 先頭から社内品番文字数分
                    isOK = True

                ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 2, i) Then ' OCR対応 先頭"*"が入る 23.09.27
                    isOK = True

                ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 3, i) Then ' 3バイト目から社内品番文字数分
                    isOK = True

                ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 4, i) Then ' 4バイト目から社内品番文字数分
                    isOK = True

                ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 6, i) Then ' 6バイト目から社内品番文字数分
                    isOK = True

                ElseIf j > 30 And _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 18, i) Then ' 18バイト目から社内品番文字数分
                    isOK = True ' 8/8 add メーカー品番 KQR_TagZ

                ElseIf j > 30 And _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 20, i) Then ' 20バイト目から社内品番文字数分
                    isOK = True '

                End If

            End If
        End If

        ' 同一データを読み取った場合はワーニングメッセージを出す
        If isWN Then
            Thread.Sleep(300)
            Dim result = MyDialogWarn.ShowDialog()
            If result = Windows.Forms.DialogResult.OK Then
                isOK = True
            ElseIf result = Windows.Forms.DialogResult.Cancel Then
                isOK = False
            End If
        End If

        If isOK Then

            ' OKダイアログ表示
            Thread.Sleep(300)
            If isWN = False Then MyDialogOK.ShowDialog()

            txtQTY.Focus()
            flgConfirm = True

        Else ' 照合ERROR

            ' 照合結果出力 SQLite Insert
            Dim rec As DBPokaRecord
            rec.MAKER = "KUBOTA"
            rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
            rec.TANCD = txtTANCD.Text
            rec.HMCD = txtHMCD.Text
            rec.TKHMCD = txtTKHMCD.Text
            rec.QTY = ""
            rec.RESULT = "NG"
            rec.DATABASE = ""
            ret = insertPokaX(tblNamePoka1, rec)
            If ret <> SQLITE_OK Then
                MessageBox.Show(sqliteErrorString & vbCrLf & _
                    "データベースの登録に失敗しました" & vbCrLf & _
                    "システム担当者に連絡してください")
                Return
            End If

            ' 照合エラー
            If isWN = False Then MyDialogError.ShowDialog()
            lblCount.Text = getRecordCount(tblNamePoka1)
            txtTKHMCD.Focus()

            flgConfirm = False

        End If
    End Sub

    Private Sub txtQTY_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQTY.GotFocus
        Dim modeSet As UInt32 = Bt.LibDef.BT_KEYINPUT_DIRECT
        Dim ret As Int32 = Bt.SysLib.Display.btSetKeyCharacter(modeSet)
        If ret <> 0 Then
            MessageBox.Show("キー入力設定に失敗しました:" & ret)
        End If

        txtQTY.SelectionStart = 0
        txtQTY.SelectionLength = txtQTY.TextLength
        txtQTY.BackColor = Color.Aqua
    End Sub

    Private Sub txtQTY_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQTY.KeyDown
        Select Case e.KeyCode
            Case System.Windows.Forms.Keys.Up
                txtTKHMCD.Focus()
            Case System.Windows.Forms.Keys.Down
                txtHMCD.Focus()
            Case System.Windows.Forms.Keys.Back
                If txtQTY.Text = "" Then
                    txtTKHMCD.Focus()
                End If
            Case System.Windows.Forms.Keys.Enter
                If txtTANCD.Text = "" Then
                    MessageBox.Show("担当者ｺｰﾄﾞを確認してください")
                    Return
                ElseIf txtHMCD.Text = "" Then
                    MessageBox.Show("社内品番を入力してください")
                    txtHMCD.Focus()
                    Return
                ElseIf txtTKHMCD.Text = "" Then
                    MessageBox.Show("メーカー品番をスキャンしてください")
                    txtTKHMCD.Focus()
                    Return
                ElseIf IsNumeric(txtQTY.Text) = False Then
                    MessageBox.Show("数値を入力してください")
                    txtQTY.Focus()
                    Return
                End If

                ' 照合せず数量入力された場合、ここで照合処理を入れる
                If flgConfirm = False Then Call Judge()

                ' 品番照合OKを出力 
                If flgConfirm Then

                    ' SQLite Insert
                    Dim rec As DBPokaRecord
                    Dim ret As Int32
                    rec.MAKER = "KUBOTA"
                    rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
                    rec.TANCD = txtTANCD.Text
                    rec.HMCD = txtHMCD.Text
                    rec.TKHMCD = txtTKHMCD.Text
                    rec.QTY = txtQTY.Text
                    rec.RESULT = "OK"
                    If gTKCD <> "" And InStr(mTargetTKCDs, gTKCD) > 0 Then
                        rec.DATABASE = "WAIT"
                    Else
                        rec.DATABASE = "-"
                    End If
                    ret = insertPokaX(tblNamePoka1, rec)
                    If ret <> SQLITE_OK Then
                        MessageBox.Show(sqliteErrorString & vbCrLf & _
                            "Pokaデータベースの登録に失敗しました" & vbCrLf & _
                            "システム担当者に連絡してください")
                        Return
                    End If
                    ' 出荷指示テーブルの更新処理はタイマーで遅延して行なわせる
                    ' （スリープ解除後の、Wi-Fi起動～Pingの応答があるまで時間が掛かるので
                    ' 　リアルタイム更新失敗の確率が高かった（画面が固まる））
                    If rec.DATABASE = "WAIT" Then
                        If gWaitRec Is Nothing Then
                            ReDim gWaitRec(0)
                            gWaitRec(0) = rec
                        Else
                            Dim idx As Integer = UBound(gWaitRec)
                            ReDim Preserve gWaitRec(idx + 1)
                            gWaitRec(idx + 1) = rec
                        End If
                        gRetry = 3
                        gInterval = getAutoPowerOFF()
                        Call setAutoPowerOFF(0)
                        TimerWiFiUpdater.Enabled = True
                    End If

                    ' 次の照合へ
                    Call txtClear()
                End If

        End Select
    End Sub

    Private Sub txtQTY_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQTY.LostFocus
        txtQTY.BackColor = Color.White
    End Sub

    Private Sub chkTe_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkTe.CheckStateChanged
        txtHMCD.Focus()
    End Sub

    Private Sub chkQR_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkQR.CheckStateChanged
        txtTKHMCD.Focus()
    End Sub

    ' Microsoft SQL Server 2008 R8 出荷指示テーブル更新
    Private Sub TimerWiFiUpdater_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerWiFiUpdater.Tick

        ' 疎通確認（リトライ２回）
        If gRetry = 0 Or gWaitRec Is Nothing Then
            TimerWiFiUpdater.Enabled = False
            Call setAutoPowerOFF(gInterval)
            Exit Sub
        Else
            Dim fm As New FormWaiting
            fm.Show()
            Refresh()
            Application.DoEvents()

            Dim ret As Boolean = checkSQLServer()
            Refresh()
            fm.Dispose()
            If ret = False Then GoTo Retry
        End If

        ' デバッグ中に再度呼び出されないようここでタイマーをオフ
        TimerWiFiUpdater.Enabled = False
        Call setAutoPowerOFF(gInterval)

        ' 溜まっている、WAITレコードを全て読み込む
        Dim idx As Integer
        For idx = 0 To UBound(gWaitRec)

            ' 出荷指示テーブルの更新 (SQL Server 2008 R2)
            Dim status As String = UpdateSHIPMENT(gWaitRec(idx).HMCD, gWaitRec(idx).QTY, txtTANCD.Text)

            ' 照合履歴ファイルの更新 (SQLite)
            Call updatePokaXDatabase(tblNamePoka1, gWaitRec(idx), status)

            gWaitRec(idx).DATABASE = status
        Next
        If gWaitRec.Where(Function(r) r.DATABASE = "WAIT").Count = 0 Then
            gWaitRec = Nothing
        Else
            ' LINQを使用して要素を削除する（配列のサイズ変更）
            ' もう、ここを通ることは無くなったがカッコいいのでとっておく
            gWaitRec = gWaitRec.Where(Function(r) r.DATABASE = "WAIT").ToArray
        End If

        Exit Sub

Retry:
        TimerWiFiUpdater.Enabled = False
        TimerWiFiUpdater.Enabled = True
        gRetry = gRetry - 1

    End Sub
End Class
