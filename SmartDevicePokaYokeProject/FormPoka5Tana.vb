Imports System.Threading
Imports System.Runtime.InteropServices

Public Class FormPoka5Tana

    ' このWindowのインスタンス
    Public Shared FormPoka5Instance As FormPoka5Tana

    Private Const PROCESS_EXIT_WAIT_TIME As Integer = 200

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
        txtBUCD.Text = ""
        lblCount.Text = getRecordCount(tblNamePoka5)
        txtTANACD.Focus()
    End Sub

    Private Sub txtClear()
        txtHMCD.Text = ""
        txtBUCD.Text = ""
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

    Private Sub txtTANACD_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTANACD.LostFocus
        txtTANACD.BackColor = Color.White
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
                Else
                    txtBUCD.Text = ""
                End If

            Case Keys.Enter

                ' 前データクリア
                txtBUCD.Text = ""

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

                ' 商品マスターを検索しローケーション番号を取得
                Dim bucd As String = getBUCD(txtHMCD.Text)

                ' 結果チェック
                If bucd = "NotFound" Then
                    MessageBox.Show("社内品番がマスターに存在しません")
                    txtHMCD.Focus()
                    Return
                ElseIf bucd = "" Then
                    MessageBox.Show("ロケーション番号が設定されていません")
                    txtHMCD.Focus()
                    Return
                Else
                    txtBUCD.Text = bucd
                End If

                ' ロケーション照合
                Call Judge()

        End Select
    End Sub

    ' ロケーション照合
    Private Sub Judge()
        Dim ret As Int32
        Dim i As Int32 = txtTANACD.TextLength
        Dim j As Int32 = txtBUCD.Text.Length
        Dim isOK As Boolean = False

        ' 照合処理
        If txtTANACD.Text = txtBUCD.Text Then

            isOK = True

        End If

        ' 照合結果出力
        Dim rec As DBTanaRecord
        rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
        rec.TANCD = txtTANCD.Text
        rec.HMCD = txtHMCD.Text
        rec.BUCD = txtBUCD.Text
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

            ' OKダイアログ表示
            Thread.Sleep(300)
            MyDialogOK.ShowDialog()

            ' 次の照合へ
            Call txtClear()

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
            MyDialogError.ShowDialog()
            lblCount.Text = getRecordCount(tblNamePoka5)
            txtHMCD.Focus()

        End If
    End Sub

End Class