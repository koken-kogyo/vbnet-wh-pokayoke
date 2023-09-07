Public Class FormMain
    Public Shared FormMainInstance As FormMain

    Private Sub txtTANCD_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTANCD.GotFocus
        '// 表示処理クラス.キー入力モード設定:IMEオフ
        Dim modeSet As UInt32 = Bt.LibDef.BT_KEYINPUT_DIRECT
        Dim ret As Int32 = Bt.SysLib.Display.btSetKeyCharacter(modeSet)
        If ret <> 0 Then
            MessageBox.Show("キー入力設定に失敗しました:" & ret)
        End If

        txtTANCD.SelectionStart = 0
        txtTANCD.SelectionLength = txtTANCD.TextLength
    End Sub

    Private Sub txtTANCD_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTANCD.KeyDown
        Select e.KeyCode
            Case Keys.Up
                btnClose.Focus()
            Case Keys.Down
                btnKUBOTA.Focus()
            Case Keys.Enter
                btnKUBOTA.Focus()
        End Select
    End Sub

    Private Sub FormMain_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed

        ' 担当者コード保存
        setSettings(txtTANCD.Text)

        ' スキャナ設定初期値に戻す
        restoreScanProperty()

        ' データベースクローズ
        closeDB()

        ' インスタンス破棄
        If FormMain.FormMainInstance IsNot Nothing Then
            FormMain.FormMainInstance.Dispose()
        End If
        If FormPoka1Kubota.FormPoka1Instance IsNot Nothing Then
            FormPoka1Kubota.FormPoka1Instance.Dispose()
        End If

    End Sub

    Private Sub FormMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' スキャナ設定初期値を保存
        backupScanProperty()

        ' 実行パスを取得
        Dim path As [String] = Me.[GetType]().Assembly.GetModules()(0).FullyQualifiedName
        Dim en As Int32 = path.LastIndexOf("\")
        mAppPath = path.Substring(0, en)

        ' 端末名をモジュール変数に設定
        mHtName = getHTNAME()

        ' データベースを開く
        Dim ret As Int32 = openDB()
        If ret <> 0 Then
            MessageBox.Show("SQLiteデータベースの" & vbCrLf & "オープンに失敗しました:" & ret & vbCrLf & "処理を終了します")
            Close()
        End If

        ' 初期画面設定
        'txtTANCD.Text = selectTanto()
        txtTANCD.Text = getSettings()

        ' インスタンス保持
        FormMainInstance = Me

        ' 初期フォーカス ボタンの先頭
        btnKUBOTA.Focus()

    End Sub

    Private Sub btnKUBOTA_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnKUBOTA.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                txtTANCD.Focus()
            Case Keys.Down
                btnYANMAR.Focus()
            Case Keys.NumPad1
                btnKUBOTA_Click(sender, e)
            Case Keys.NumPad2
                btnYANMAR_Click(sender, e)
            Case Keys.NumPad3
                btnHITATI_Click(sender, e)
            Case Keys.NumPad9
                Me.Close()
        End Select
    End Sub

    Private Sub btnYANMAR_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnYANMAR.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                btnKUBOTA.Focus()
            Case Keys.Down
                btnHITATI.Focus()
            Case Keys.NumPad1
                btnKUBOTA_Click(sender, e)
            Case Keys.NumPad2
                btnYANMAR_Click(sender, e)
            Case Keys.NumPad3
                btnHITATI_Click(sender, e)
            Case Keys.NumPad9
                Me.Close()
        End Select
    End Sub

    Private Sub btnHITATI_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnHITATI.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                btnYANMAR.Focus()
            Case Keys.Down
                btnClose.Focus()
            Case Keys.NumPad1
                btnKUBOTA_Click(sender, e)
            Case Keys.NumPad2
                btnYANMAR_Click(sender, e)
            Case Keys.NumPad3
                btnHITATI_Click(sender, e)
            Case Keys.NumPad9
                Me.Close()
        End Select
    End Sub

    Private Sub btnKUBOTA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKUBOTA.Click
        Dim frm As Form = New FormPoka1Kubota()
        frm.Show()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnYANMAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYANMAR.Click
        Dim frm As Form = New FormPoka2yanmar()
        frm.Show()
    End Sub

    Private Sub btnHITATI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHITATI.Click
        Dim frm As Form = New FormPoka3Hitati()
        frm.Show()
    End Sub

    Private Sub chkBuzzer_CheckStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBuzzer.CheckStateChanged
        btnKUBOTA.Focus()
    End Sub
End Class
