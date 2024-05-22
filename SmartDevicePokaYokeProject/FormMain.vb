Imports Bt.SysLib.Power
Imports Bt.LibDef

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
        Select Case e.KeyCode
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

        ' レジューム動作モードを1:レジューム有効（パワーオフ直前の処理から実行） ※初期値
        Call setResume(1)

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
                'Case Keys.NumPad4
                '    btnORIENT_Click(sender, e)
            Case Keys.NumPad5
                btnTANA_Click(sender, e)
            Case Keys.NumPad8
                btnRestart_Click(sender, e)
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
                'Case Keys.NumPad4
                '    btnORIENT_Click(sender, e)
            Case Keys.NumPad5
                btnTANA_Click(sender, e)
            Case Keys.NumPad8
                btnRestart_Click(sender, e)
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
                'Case Keys.NumPad4
                '    btnORIENT_Click(sender, e)
            Case Keys.NumPad5
                btnTANA_Click(sender, e)
            Case Keys.NumPad8
                btnRestart_Click(sender, e)
            Case Keys.NumPad9
                Me.Close()
        End Select
    End Sub

    Private Sub btnORIENT_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnORIENT.KeyDown
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
                'Case Keys.NumPad4
                '    btnORIENT_Click(sender, e)
            Case Keys.NumPad5
                btnTANA_Click(sender, e)
            Case Keys.NumPad8
                btnRestart_Click(sender, e)
            Case Keys.NumPad9
                Me.Close()
        End Select
    End Sub

    Private Sub btnTANA_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles btnTANA.KeyDown
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
                'Case Keys.NumPad4
                '    btnORIENT_Click(sender, e)
            Case Keys.NumPad5
                btnTANA_Click(sender, e)
            Case Keys.NumPad8
                btnRestart_Click(sender, e)
            Case Keys.NumPad9
                Me.Close()
        End Select
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnKUBOTA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKUBOTA.Click
        btnKUBOTA.Focus()
        Dim frm As Form = New FormPoka1Kubota()
        frm.Show()
    End Sub

    Private Sub btnYANMAR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYANMAR.Click
        btnYANMAR.Focus()
        Dim frm As Form = New FormPoka2Yanmar()
        frm.Show()
    End Sub

    Private Sub btnHITATI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHITATI.Click
        btnHITATI.Focus()
        Dim frm As Form = New FormPoka3Hitati()
        frm.Show()
    End Sub

    Private Sub btnORIENT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnORIENT.Click
        btnORIENT.Focus()
        Dim frm As Form = New FormPoka4Orient()
        frm.Show()
    End Sub

    Private Sub btnTANA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTANA.Click
        btnTANA.Focus()
        Dim frm As Form = New FormPoka5Tana()
        frm.Show()
    End Sub

    Private Sub chkBuzzer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkBuzzer.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                chkBuzzer.Checked = Not chkBuzzer.Checked
            Case Keys.NumPad1
                btnKUBOTA_Click(sender, e)
            Case Keys.NumPad2
                btnYANMAR_Click(sender, e)
            Case Keys.NumPad3
                btnHITATI_Click(sender, e)
                'Case Keys.NumPad4
                '    btnORIENT_Click(sender, e)
            Case Keys.NumPad5
                btnTANA_Click(sender, e)
            Case Keys.NumPad8
                btnRestart_Click(sender, e)
            Case Keys.NumPad9
                Me.Close()
        End Select
    End Sub

    ' 端末リセット（レジームなしでマニュアルパワーオフ）
    Private Sub btnRestart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestart.Click
        Call setResume(0)
        Call btn_ManualPowerOFF_Click()
    End Sub

    '*******************************************************************************
    '         * 機能 ：レジューム動作を設定／取得します。
    '         * API  ：btSetResume,         ' enableSet As Int32 = 0:無効 1:有効
    '*******************************************************************************
    Private Sub setResume(ByVal enableSet As Int32)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""
        Try
            ret = btSetResume(enableSet)
            If ret <> BT_OK Then
                disp = "btSetResume error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    '*******************************************************************************
    '         * 機能 ：API実行によるパワーオフ動作を実行します。
    '         * API  ：btManualPowerOFF
    '*******************************************************************************
    Private Sub btn_ManualPowerOFF_Click()
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        ' Dim modeSet As UInt32 = LibDef.BT_PW_SUSPEND ' サスペンド 
        Dim modeSet As UInt32 = BT_PW_RESET ' リセット（Warm ブート） 

        Try
            ' If MessageBox.Show("サスペンドを実行してよろしいですか？", "パワーOFF(マニュアル)", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            If MessageBox.Show("端末リセットを実行してよろしいですか？", "パワーOFF(マニュアル)", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) = DialogResult.OK Then

                '-----------------------------------------------------------
                ' 設定
                '-----------------------------------------------------------
                ret = btManualPowerOFF(modeSet)
                If ret <> BT_OK Then
                    disp = "btManualPowerOFF error ret[" & ret & "]"
                    MessageBox.Show(disp, "エラー")
                    Return
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

End Class
