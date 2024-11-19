Imports System.Threading

Public Class FormPoka3Hitati

    ' このWindowのインスタンス
    Public Shared FormPoka3Instance As FormPoka3Hitati


    Dim flgConfirm As Boolean = False

    Private Sub FormPoka3Hitati_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtTANCD.Text = FormMain.txtTANCD.Text
        txtClear()

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormPoka3Instance = Me

    End Sub

    ' F1キー
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F2キー
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click

        If getRecordCount(tblNamePoka3) = 0 Then
            MessageBox.Show("送信するデータが存在しません", "警告")
            Exit Sub
        End If

        ' CSVファイル作成
        If SQLite2CSV(tblNamePoka3) <> 0 Then
            MessageBox.Show("CSVファイルの作成に失敗しました")
            Exit Sub
        End If

        ' ファイル転送
        Dim dialog As New FormTransmitting(tblNamePoka3)
        If (System.Windows.Forms.DialogResult.OK = dialog.ShowDialog()) Then

            'DB Delete
            If deletePokaX(tblNamePoka3) = 0 Then
                txtClear()
            Else
                MessageBox.Show("CSVファイルの作成に失敗しました")
                Exit Sub
            End If
        End If

    End Sub

    ' F3キー (履歴表示)
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        Dim frm As Form = New FormPokaHistory(tblNamePoka3, txtTANCD.Text)
        frm.ShowDialog()
        lblCount.Text = getRecordCount(tblNamePoka3) ' 24.05.30 add y.w
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
        lblCount.Text = getRecordCount(tblNamePoka3)
        txtHMCD.Focus()
        flgConfirm = False
    End Sub

    Private Sub FormPoka3Hitati_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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
        'Call setScanProperty(1)
        txtHMCD.SelectionStart = 0
        txtHMCD.SelectionLength = txtHMCD.TextLength
        txtHMCD.BackColor = Color.Aqua
    End Sub

    Private Sub txtHMCD_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtHMCD.LostFocus
        ' エラーチェック
        If txtHMCD.TextLength > 24 Or Strings.Left(txtHMCD.Text, 2) = "**" Then
            MessageBox.Show("社内品番を読み取ってください")
            txtHMCD.Focus()
            Return
        End If

        ' 入力待機食を解除
        txtHMCD.BackColor = Color.White

        ' 社内品番は [空白, 990, 993, A001, _0010-0010, ハイフン] を除去
        ' (SATOのラベルプリンターは品番24桁空白パディングでバーコードが作成されている)
        txtHMCD.Text = Trim(txtHMCD.Text)
        lblHMCD.Text = txtHMCD.Text.Replace("-", "")

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
                    ' 品目マスターチェック
                    ret = getM0500(txtHMCD.Text, wSKHIASU, wCOLOR, wSU)
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
        End Select
    End Sub

    Private Sub txtTKHMCD_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTKHMCD.GotFocus
        Dim modeSet As UInt32 = Bt.LibDef.BT_KEYINPUT_DIRECT
        Dim ret As Int32 = Bt.SysLib.Display.btSetKeyCharacter(modeSet)
        If ret <> 0 Then
            MessageBox.Show("キー入力設定に失敗しました:" & ret)
        End If
        'Call setScanProperty(2) ' Code系, JAN系 のみ読み取り可能に設定
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

                ' 照合せず数量入力された場合に備える
                If flgConfirm = False Then Call Judge()

                ' 品番照合OKを出力 SQLite Insert
                If flgConfirm Then
                    Dim rec As DBPokaRecord
                    Dim ret As Int32
                    rec.MAKER = "HITATI"
                    rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
                    rec.TANCD = txtTANCD.Text
                    rec.HMCD = txtHMCD.Text
                    rec.TKHMCD = txtTKHMCD.Text
                    rec.QTY = txtQTY.Text
                    rec.RESULT = "OK"
                    rec.DLVRDT = "-"
                    rec.DATABASE = "-"
                    ret = insertPokaX(tblNamePoka3, rec)
                    If ret <> SQLITE_OK Then
                        MessageBox.Show(sqliteErrorString & vbCrLf & _
                            "データベースの登録に失敗しました" & vbCrLf & _
                            "システム担当者に連絡してください")
                        Return
                    End If

                    ' 次の照合へ
                    Call txtClear()
                End If

        End Select
    End Sub

    Private Sub txtQTY_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQTY.LostFocus
        txtQTY.BackColor = Color.White
    End Sub

    ' 品番照合
    ' lblHMCD(ハイフンが除去されたもの)とtxtTKHMCDを照合
    Private Sub Judge()
        Dim rec As DBPokaRecord
        Dim ret As Int32

        Dim _HMCD As String = txtHMCD.Text.Replace("-", "") ' ハイフンは除去
        Dim _TKHMCD As String = txtTKHMCD.Text              ' 23.09.28 得意先品番のハイフンは除去しない

        ' 社内品番変換処理
        If Strings.Right(_HMCD, 3) = "990" Or _
           Strings.Right(_HMCD, 3) = "993" Then
            _HMCD = Strings.Left(_HMCD, _HMCD.Length - 3)
        ElseIf Strings.Right(_HMCD, 4) = "A001" Then
            _HMCD = Strings.Left(_HMCD, _HMCD.Length - 4)
        ElseIf _HMCD.Length > 10 And Strings.Right(txtHMCD.Text, 10) = "_0010-0010" Then
            _HMCD = Strings.Left(txtHMCD.Text, txtHMCD.Text.Length - 10)
        End If

        ' 照合桁数決定
        Dim i As Int32 = _HMCD.Length
        Dim j As Int32 = _TKHMCD.Length
        Dim isOK As Boolean = False

        ' 照合処理
        If _HMCD = Strings.Left(_TKHMCD.Replace("-", ""), i) Then
            isOK = True

        ElseIf _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 6, i) Then
            isOK = True

        Else

            ' 得意先マスタ[M0600]の情報で照合 23.10.20
            _HMCD = getTKHMCD(txtHMCD.Text, _TKHMCD).Replace("-", "") ' SQLiteのマスタサーチ
            i = _HMCD.Length

            If _HMCD <> "" And _HMCD = Strings.Left(_TKHMCD.Replace("-", ""), i) Then ' 先頭から得意先品番文字数分
                isOK = True

            ElseIf _HMCD <> "" And _HMCD = Strings.Mid(_TKHMCD.Replace("-", ""), 6, i) Then ' 23.10.23
                isOK = True

            End If

        End If

        If isOK Then

            ' OKダイアログ表示
            Thread.Sleep(300)
            MyDialogOK.ShowDialog()

            If txtHMCD.Text = "PROOFREAD-OK" Then
                ' 朝一校正処理 24.09.03 add y.w
                rec.MAKER = "HITATI"
                rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
                rec.TANCD = txtTANCD.Text
                rec.HMCD = txtHMCD.Text
                rec.TKHMCD = txtTKHMCD.Text
                rec.QTY = ""
                rec.RESULT = "OK"
                rec.DLVRDT = "-"
                rec.DATABASE = "-"
                ret = insertPokaX(tblNamePoka3, rec)
                If ret <> SQLITE_OK Then
                    MessageBox.Show(sqliteErrorString & vbCrLf & _
                        "Pokaデータベースの登録に失敗しました" & vbCrLf & _
                        "システム担当者に連絡してください")
                    Return
                End If
                Call txtClear()
            Else
                ' 数量入力へ
                txtQTY.Focus()
                flgConfirm = True
            End If

        Else ' 照合ERROR

            ' 照合エラー
            MyDialogError.ShowDialog()

            ' 照合結果出力 SQLite Insert
            rec.MAKER = "HITATI"
            rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
            rec.TANCD = txtTANCD.Text
            rec.HMCD = txtHMCD.Text
            rec.TKHMCD = txtTKHMCD.Text
            rec.QTY = ""
            rec.RESULT = "NG"
            rec.DLVRDT = "-"
            rec.DATABASE = "-"
            ret = insertPokaX(tblNamePoka3, rec)
            If ret <> SQLITE_OK Then
                MessageBox.Show(sqliteErrorString & vbCrLf & _
                    "データベースの登録に失敗しました" & vbCrLf & _
                    "システム担当者に連絡してください")
                Return
            End If

            If txtHMCD.Text = "PROOFREAD-NG" Then
                ' 朝一校正処理 24.09.03 add y.w
                Call txtClear()
            Else
                ' メーカー品番へ
                lblCount.Text = getRecordCount(tblNamePoka3)
                txtTKHMCD.Focus()
                flgConfirm = False
            End If

        End If
    End Sub

End Class