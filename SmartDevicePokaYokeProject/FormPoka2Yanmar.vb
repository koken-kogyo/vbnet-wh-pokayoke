Imports System.Threading
Imports System.Runtime.InteropServices

Public Class FormPoka2Yanmar

    ' このWindowのインスタンス
    Public Shared FormPoka2Instance As FormPoka2yanmar

    Private Const PROCESS_EXIT_WAIT_TIME As Integer = 200
    '--------------------------------------------------------------
    ' DLLImport
    '--------------------------------------------------------------
    <DllImport("coredll.dll", EntryPoint:="DeleteObject")> _
    Public Shared Function DeleteObject(ByVal hObject As IntPtr) As Boolean
    End Function

    ' F1キー
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F2キー
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click

        If getRecordCount(tblNamePoka2) = 0 Then
            MessageBox.Show("送信するデータが存在しません", "警告")
            Exit Sub
        End If

        ' CSVファイル作成
        If SQLite2CSV(tblNamePoka2) <> 0 Then
            MessageBox.Show("CSVファイルの作成に失敗しました")
            Exit Sub
        End If

        ' ファイル転送
        Dim dialog As New FormTransmitting(tblNamePoka2)
        If (System.Windows.Forms.DialogResult.OK = dialog.ShowDialog()) Then

            'DB Delete
            If deletePokaX(tblNamePoka2) = 0 Then
                txtClear()
            Else
                MessageBox.Show("CSVファイルの作成に失敗しました")
                Exit Sub
            End If
        End If

    End Sub

    ' F3キー
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        Dim frm As Form = New FormPokaHistory(tblNamePoka2)
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
        lblCount.Text = getRecordCount(tblNamePoka2)
        txtHMCD.Focus()
    End Sub

    Private Sub FormPoka2Yanmar_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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

    Private Sub FormPoka2Yanmar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtTANCD.Text = FormMain.txtTANCD.Text
        txtClear()

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormPoka2Instance = Me

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

        ' 社内品番は [ハイフン, 空白] を除去
        ' (SATOのラベルプリンターは品番24桁空白パディングでバーコードが作成されている)
        txtHMCD.Text = Trim(txtHMCD.Text)
        lblHMCD.Text = txtHMCD.Text.Replace("-", "")

    End Sub

    Private Sub txtHMCD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtHMCD.KeyDown
        Select Case e.KeyCode
            Case System.Windows.Forms.Keys.Up
                txtTKHMCD.Focus()
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

    Private Sub txtTKHMCD_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTKHMCD.LostFocus
        txtTKHMCD.BackColor = Color.White
        lblTKHMCD.Text = txtTKHMCD.Text
    End Sub

    Private Sub txtTKHMCD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTKHMCD.KeyDown
        Select Case e.KeyCode
            Case System.Windows.Forms.Keys.Up
                txtHMCD.Focus()
            Case System.Windows.Forms.Keys.Down
                txtHMCD.Focus()
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

        Dim _HMCD As String = txtHMCD.Text.Replace("-", "")         ' ハイフンは除去
        Dim _TKHMCD As String = txtTKHMCD.Text.Replace("-", "")     ' ハイフンは除去

        If Strings.Right(txtHMCD.Text, 3) = "-YR" Then ' 23.10.04 YR品番対応
            _HMCD = Strings.Left(txtHMCD.Text, txtHMCD.Text.Length - 3).Replace("-", "")
        End If

        Dim i As Int32 = _HMCD.Length
        Dim j As Int32 = _TKHMCD.Length

        Dim isOK As Boolean = False

        ' 照合処理
        If _HMCD = Strings.Left(_TKHMCD, i) Then ' 先頭から社内品番文字数分
            isOK = True

        ElseIf _HMCD = Strings.Mid(_TKHMCD, 2, i) Then ' OCR対応 先頭"*"が入る 23.09.27
            isOK = True

        Else

            ' 得意先マスタ[M0600]の情報で照合 23.09.27
            _HMCD = getTKHMCD(txtHMCD.Text).Replace("-", "") ' SQLiteのマスタサーチ
            i = _HMCD.Length

            If _HMCD <> "" And _HMCD = Strings.Left(_TKHMCD, i) Then ' 先頭から得意先品番文字数分
                isOK = True

            ElseIf _HMCD <> "" And _HMCD = Strings.Mid(_TKHMCD, 2, i) Then ' OCR対応 先頭"*"が入る 23.09.27
                isOK = True

            End If

        End If

        ' 照合結果出力
        Dim rec As DBRecord
        rec.MAKER = "YANMAR"
        rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
        rec.TANCD = txtTANCD.Text
        rec.HMCD = txtHMCD.Text
        rec.TKHMCD = txtTKHMCD.Text
        If isOK Then

            ' SQLite Insert
            rec.RESULT = "OK"
            ret = insertPokaX(tblNamePoka2, rec)
            If ret <> SQLITE_OK Then
                MessageBox.Show(sqliteErrorString & vbCrLf & _
                    "データベースの登録に失敗しました" & vbCrLf & _
                    "システム担当者に連絡してください")
                Return
            End If

            ' OKダイアログ表示
            Thread.Sleep(300)
            MyDialogOK.ShowDialog()

            ' 次の照合へ
            Call txtClear()

        Else ' 照合ERROR

            ' SQLite Insert
            rec.RESULT = "NG"
            ret = insertPokaX(tblNamePoka2, rec)
            If ret <> SQLITE_OK Then
                MessageBox.Show(sqliteErrorString & vbCrLf & _
                    "データベースの登録に失敗しました" & vbCrLf & _
                    "システム担当者に連絡してください")
                Return
            End If

            ' 照合エラー
            MyDialogError.ShowDialog()
            lblCount.Text = getRecordCount(tblNamePoka2)
            txtTKHMCD.Focus()

        End If
    End Sub

End Class