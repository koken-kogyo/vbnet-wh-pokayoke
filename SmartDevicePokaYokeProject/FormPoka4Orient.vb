Imports System.Threading
Imports System.Runtime.InteropServices

Public Class FormPoka4Orient

    ' このWindowのインスタンス
    Public Shared FormPoka4Instance As FormPoka4Orient


    Dim flgConfirm As Boolean = False

    Private Sub FormPoka4Orient_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtTANCD.Text = FormMain.txtTANCD.Text
        txtClear()

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormPoka4Instance = Me

    End Sub

    ' F1キー
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F2キー
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click

        If getRecordCount(tblNamePoka4) = 0 Then
            MessageBox.Show("送信するデータが存在しません", "警告")
            Exit Sub
        End If

        ' CSVファイル作成
        If SQLite2CSV(tblNamePoka4) <> 0 Then
            MessageBox.Show("CSVファイルの作成に失敗しました")
            Exit Sub
        End If

        ' ファイル転送
        Dim dialog As New FormTransmitting(tblNamePoka4)
        If (System.Windows.Forms.DialogResult.OK = dialog.ShowDialog()) Then

            'DB Delete
            If deletePokaX(tblNamePoka4) = 0 Then
                txtClear()
            Else
                MessageBox.Show("CSVファイルの作成に失敗しました")
                Exit Sub
            End If
        End If

    End Sub

    ' F3キー
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        Dim frm As Form = New FormPokaHistory(tblNamePoka4)
        frm.Show()
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
        lblCount.Text = getRecordCount(tblNamePoka4)
        txtHMCD.Focus()
        flgConfirm = False
    End Sub

    Private Sub FormPoka4Orient_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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

    Private Sub txtHMCD_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtHMCD.LostFocus
        ' エラーチェック
        If txtHMCD.TextLength > 24 Or Strings.Left(txtHMCD.Text, 2) = "**" Then
            MessageBox.Show("社内品番を読み取ってください")
            txtHMCD.Focus()
            Return
        End If

        ' 入力待機食を解除
        txtHMCD.BackColor = Color.White

        ' オリエント社内品番はなにもしない
        ' (SATOのラベルプリンターは品番24桁空白パディングでバーコードが作成されている)
        txtHMCD.Text = Trim(txtHMCD.Text)
        Dim tmpHMCD As String = ""
        tmpHMCD = txtHMCD.Text
        lblHMCD.Text = tmpHMCD.Replace("-", "")

    End Sub

    Private Sub txtHMCD_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtHMCD.KeyDown
        Select Case e.KeyCode
            Case System.Windows.Forms.Keys.Up
                txtQTY.Focus() ' 24.05.10 mod y.w
            Case System.Windows.Forms.Keys.Down
                txtTKHMCD.Focus()
            Case System.Windows.Forms.Keys.Enter
                If txtHMCD.Text <> "" Then
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

    Private Sub txtTKHMCD_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTKHMCD.LostFocus
        txtTKHMCD.BackColor = Color.White
        lblTKHMCD.Text = txtTKHMCD.Text
    End Sub

    Private Sub txtTKHMCD_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTKHMCD.KeyDown
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
                    rec.MAKER = "ORIENT"
                    rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
                    rec.TANCD = txtTANCD.Text
                    rec.HMCD = txtHMCD.Text
                    rec.TKHMCD = txtTKHMCD.Text
                    rec.QTY = txtQTY.Text
                    rec.RESULT = "OK"
                    ret = insertPokaX(tblNamePoka4, rec)
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
        Dim ret As Int32
        Dim i As Int32 = lblHMCD.Text.Length
        Dim j As Int32 = txtTKHMCD.TextLength
        Dim isOK As Boolean = False

        ' 照合処理
        If lblHMCD.Text = Strings.Left(txtTKHMCD.Text.Replace("-", ""), i) Or _
           lblHMCD.Text = Strings.Left(txtTKHMCD.Text, i) Then

            isOK = True

        End If

        If isOK Then

            ' OKダイアログ表示
            Thread.Sleep(300)
            MyDialogOK.ShowDialog()

            txtQTY.Focus()
            flgConfirm = True

        Else ' 照合ERROR

            ' 照合結果出力 SQLite Insert
            Dim rec As DBPokaRecord
            rec.MAKER = "ORIENT"
            rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
            rec.TANCD = txtTANCD.Text
            rec.HMCD = txtHMCD.Text
            rec.TKHMCD = txtTKHMCD.Text
            rec.QTY = ""
            rec.RESULT = "NG"
            ret = insertPokaX(tblNamePoka4, rec)
            If ret <> SQLITE_OK Then
                MessageBox.Show(sqliteErrorString & vbCrLf & _
                    "データベースの登録に失敗しました" & vbCrLf & _
                    "システム担当者に連絡してください")
                Return
            End If

            ' 照合エラー
            MyDialogError.ShowDialog()
            lblCount.Text = getRecordCount(tblNamePoka4)
            txtTKHMCD.Focus()

            flgConfirm = False

        End If
    End Sub

End Class