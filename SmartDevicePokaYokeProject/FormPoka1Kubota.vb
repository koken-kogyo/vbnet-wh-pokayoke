Imports System.Data
Imports System.Threading

Public Class FormPoka1Kubota

    ' このWindowのインスタンス
    Public Shared FormPoka1Instance As FormPoka1Kubota

    Dim flgSCAN As Boolean = False      ' 社内品番のキー入力禁止フラグ
    Dim flgConfirm As Boolean = False   ' 照合済みフラグ
    Dim flgPowerOff As Boolean = False
    ' 出荷指示書システム更新用
    Dim gODRNO As String                ' メーカー現品票の注文番号(QR:10桁 or 1D:8桁)(出荷指示書の注文番号には変な日付が入っているので注意！(10桁+5桁)4504376636-1202)
    Dim gTKCD As String                 ' メーカー現品票の得意先コード
    Dim gDLVRDT As String               ' メーカー現品票の受注納期10桁[yyyy/mm/dd(111)]
    Dim gTKHMCD As String               ' メーカー現品票の得意先品番（ハイフンなし）

    ' SQLServer遅延更新関連
    Dim gWaitRec() As DBPokaRecord      ' 更新すべきレコードを配列に保持
    Dim gDisableMin As Integer = 30     ' 次回Timerイベント発生OKまでの時間を分で指定
    Dim gRetryDt As DateTime = New DateTime(1999, 12, 31, 23, 59, 59) ' 最終疎通時間を保持
    Dim gInterval As UInt32             ' 端末のオートパワーオフ設定値を保持

    Private Sub FormPoka1Kubota_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        txtTANCD.Text = FormMain.txtTANCD.Text
        chkTe.Checked = If(mHandOK = "1", True, False)
        chkQR.Checked = If(mQROnly = "1", True, False)
        Dim bkKD8330Mode As String = mKD8330Mode
        mKD8330Mode = ""
        txtClear()

        ' SQLServer遅延更新関連の初期設定
        gWaitRec = selectPokaWait()
        gInterval = getAutoPowerOFF()

        ' 出荷指示書システム用変数初期化 ver.24.11.04 y.w
        Call createKD8330()
        If bkKD8330Mode <> "" Then
            If MessageBox.Show("出荷指示モードを復元しますか？", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                Call setupKD8330Mode(bkKD8330Mode) ' 出荷指示モードの復元
            End If
        End If

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormPoka1Instance = Me

    End Sub

    ' オートパワーオフ設定値を保存
    Private Sub FormPoka1Kubota_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Closed
        Call saveSettingHandOK(chkTe.Checked)
        Call saveSettingQROnly(chkQR.Checked)
    End Sub

    ' F1キー(戻る)
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F2キー(送信)
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click

        If getRecordCount(tblNamePoka1) = 0 Then
            MessageBox.Show("送信するデータが存在しません", "警告")
            Exit Sub
        End If

        ' 出荷指示モードの更新忘れをここで処理させたい
        If getWaitRecCnt() <> 0 Then
            Dim msg As String = _
                "「待ち」レコードがあります" & vbCrLf & _
                "出荷指示書を消し込みに" & vbCrLf & "行ってもよいですか？"
            If MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                Call UpdateWaitRec()    ' Yes
            Else
                msg = "「待ち」レコードのまま" & vbCrLf & "今日は終了しますか？"
                If MessageBox.Show(msg, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
                    Exit Sub            ' No
                End If
            End If
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
                Me.LabelMenu.BackColor = mcDarkBlack
                Me.LabelMenu.Text = "クボタ照合"
                mKD8330Mode = ""
                gRetryDt = New DateTime(1999, 12, 31, 23, 59, 59)
                txtClear()
            Else
                MessageBox.Show("CSVファイルの作成に失敗しました")
                Exit Sub
            End If
        End If

    End Sub

    ' F3キー (履歴表示)
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        Dim frm As Form = New FormPokaHistory(tblNamePoka1, txtTANCD.Text)
        frm.ShowDialog()
        lblCount.Text = getRecordCount(tblNamePoka1) ' 24.05.30 add y.w
        Select Case mKD8330Mode ' 履歴画面やデータ編集画面でステータスが変わる事があるので帰ってきたら再設定
            Case "TROUBLE" : Me.LabelMenu.BackColor = Color.Red
            Case "SQLSERVER" : Me.LabelMenu.BackColor = Color.Blue
        End Select
    End Sub

    ' F4キー(クリア)
    Private Sub btnF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF4.Click
        Call txtClear()
    End Sub

    Private Sub txtClear()
        If txtHMCD.Text = "" And mKD8330Mode <> "" Then
            If MessageBox.Show("出荷指示モードを解除しますか？", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MsgBoxStyle.DefaultButton1) = Windows.Forms.DialogResult.Yes Then
                Me.LabelMenu.BackColor = mcDarkBlack
                Me.LabelMenu.Text = "クボタ照合"
                mKD8330Mode = ""
                gRetryDt = New DateTime(1999, 12, 31, 23, 59, 59)
                gODRNO = ""
                gTKCD = ""
                gDLVRDT = ""
                gTKHMCD = ""
            End If
        End If
        txtHMCD.Text = ""
        lblHMCD.Text = ""
        txtTKHMCD.Text = ""
        lblTKHMCD.Text = ""
        txtTotalQty.Text = "" ' 25.02.13 add y.w
        txtQTY.Text = ""    ' 24.05.10 add y.w
        lblHIASU.Text = ""  ' 24.05.20 add y.w
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
                If txtHMCD.Text = "" Then
                    txtTKHMCD.Focus()
                    Exit Sub
                Else
                    ' 出荷指示モードを判定 ver.24.11.04 y.w
                    If InStr(mTargetTKCDs, Split(txtHMCD.Text, "-")(0) & "|") > 0 Then

                        ' Wi-Fi通信経路確認ダイアログボックス表示
                        Dim dialog As New FormWiFiCheck
                        If dialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                            Call setupKD8330Mode(txtHMCD.Text)
                        Else
                            ' 通信エラーの場合でも、前回読み込みがあれば「保留モード」で続行するか問い合わせる
                            If mKD8330dt.Rows.Count > 100 Then
                                If MessageBox.Show("保留モードで継続しますか？", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                                    mKD8330Mode = "TROUBLE"
                                    Me.LabelMenu.BackColor = Color.Red
                                    txtHMCD.Text = ""
                                    Exit Sub
                                End If
                            End If
                            MsgBox("通常モードに戻して" & vbCrLf & _
                                   "処理を続行させます．", MsgBoxStyle.Information)
                            mKD8330Mode = ""
                            Me.LabelMenu.BackColor = mcDarkBlack
                            Me.LabelMenu.Text = "クボタ照合"
                        End If
                        txtHMCD.Text = ""
                        Exit Sub
                    End If
                End If

                Dim wSKHIASU As String = ""
                Dim wCOLOR As String = ""
                Dim wSU As String = ""
                lblHIASU.Text = ""
                ' 品目マスターチェック
                Dim ret As Boolean = getM0500(Trim(txtHMCD.Text), wSKHIASU, wCOLOR, wSU)
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
        Dim rec As DBPokaRecord
        Dim ret As Int32

        Dim _HMCD As String = txtHMCD.Text.Replace("-", "") ' ハイフンは除去
        Dim _TKHMCD As String = txtTKHMCD.Text              ' 23.09.28 得意先品番のハイフンは除去しない

        'If txtHMCD.Text = "V0531-68961-S" Then _HMCD = "V053168961" ' スリーブ品番特化対策 23.09.22
        'If txtHMCD.Text = "V0531-68971-S" Then _HMCD = "V053168971" ' スリーブ品番特化対策 23.09.28
        If Strings.Right(txtHMCD.Text, 2) = "-S" Then ' 23.10.04 スリーブ品番対応
            _HMCD = Strings.Left(txtHMCD.Text, txtHMCD.Text.Length - 2).Replace("-", "")
        End If

        ' KIC-QRコードの"\t"[0x09]->"|"[0x7C(124)]対応
        If Strings.Left(_TKHMCD, 7) = "KQR_Tag" Then
            _TKHMCD = Split(_TKHMCD, "|")(7)
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

            ' ********************************************************************
            ' 出荷指示モードを確認し、対象であれば得意先コードと納期を設定
            Dim s As String = txtTKHMCD.Text
            gODRNO = ""
            gTKCD = ""
            gDLVRDT = ""
            gTKHMCD = ""
            If mKD8330Mode <> "" And _
            ( _
                (Strings.Left(s, 2) = "23" And txtTKHMCD.TextLength = 100) Or _
                (Strings.Left(s, 2) = "28" And s.Length > 90) Or _
                (Strings.Left(s, 7) = "KQR_Tag" And UBound(Split(s, "|")) >= 13) Or _
                (Strings.Left(s, 2) = "21" And txtTKHMCD.TextLength = 17) _
            ) Then
                lblTotalQty.Text = "残数"

                If Strings.Left(s, 2) = "23" Or Strings.Left(s, 2) = "28" Then
                    ' KSCM バーコード仕様
                    '   (0) :工場           :1  :2桁
                    '   (1) :品番           :3  :10桁
                    '   (2) :注番           :13 :8桁
                    '   (3) :枝番           :21 :3桁
                    '   (4) :ﾗｲﾝﾛｹｰｼｮﾝ      :24 :8桁
                    '   (5) :材管ﾛｹｰｼｮﾝ     :32 :8桁
                    '   (6) :納入指示数     :40 :7桁
                    '   (7) :箱種           :47 :5桁
                    '   (8) :収容数         :52 :7桁
                    '   (9) :生地品番       :59 :12桁
                    '   (10):塗装色         :71 :10桁
                    '   (11):納入指示日     :81 :8桁
                    '   (12):予備           :89 :12桁 = 固定 100 Byte
                    gODRNO = Strings.Mid(s, 13, 8)
                    gTKCD = getKD8330dtTKCDfromODRNO(gODRNO)
                    gDLVRDT = Strings.Mid(s, 81, 8).Insert(6, "/").Insert(4, "/") ' 88文字超必要
                    gTKHMCD = Strings.Mid(s, 3, 10)
                    Dim oOdrQTY As Integer
                    Dim oHTQTY As Integer
                    Dim oINSUU As Integer
                    If getKD8330dtZanQTY(gODRNO, oOdrQTY, oHTQTY, oINSUU) Then
                        ' 保留中の件数が存在するかを調査
                        Dim oWaitQTY As Integer = getWaitOdrRecQTY(gODRNO)
                        If oOdrQTY = (oHTQTY + oWaitQTY) Then
                            txtTotalQty.Text = "済"
                            MsgBox("既に出荷準備されています．" & vbCrLf & vbCrLf & _
                                   "確認してください！", MsgBoxStyle.Exclamation)
                            Exit Sub
                        Else
                            txtTotalQty.Text = oOdrQTY - oHTQTY - oWaitQTY      ' 指示書残数
                            If (oOdrQTY - oHTQTY - oWaitQTY) < oINSUU Then
                                txtQTY.Text = oOdrQTY - oHTQTY - oWaitQTY       ' 収容数より少ない場合は指示書残数
                            Else
                                txtQTY.Text = oINSUU                            ' 収容数
                            End If
                        End If
                    Else
                        txtTotalQty.Text = "---"                                ' 指示書なし Integer.Parse(Strings.Mid(s, 40, 7)) ' 納入数量をセットしてみる（いらないかも）
                        txtQTY.Text = Integer.Parse(Strings.Mid(s, 52, 7))      ' 収容数
                    End If
                ElseIf Strings.Left(s, 7) = "KQR_Tag" Then
                    ' KIC QRバーコード仕様（可変長）
                    '   QRコードの"\t"[0x09]はHT側にて"|"[0x7C(124)]に変換してある（しないとセパレーターが入ってこない）
                    '   (0) :識別子         :最大7桁(KQR_Tag)
                    '   (1) :現品票番号     :最大10桁
                    '   (2) :品目           :最大13桁(ハイフンなし品番で変なコード[R]も着いてくる)
                    '   (3) :納入数量       :最大11桁
                    '   (4) :材管保管場所   :最大6桁
                    '   (5) :注番           :最大10桁（注文番号）
                    '   (6) :発注明細番号   :最大5桁
                    '   (7) :品番           :最大10桁（ハイフンなし品番）
                    '   (10):収容数         :最大11桁
                    '   (13):納入期日       :最大8桁
                    gODRNO = Split(s, "|")(5)
                    gTKCD = getKD8330dtTKCDfromODRNO(gODRNO)
                    gDLVRDT = Split(s, "|")(13).Insert(6, "/").Insert(4, "/")
                    gTKHMCD = Split(s, "|")(7)
                    Dim oOdrQTY As Integer
                    Dim oHTQTY As Integer
                    Dim oINSUU As Integer
                    If getKD8330dtZanQTY(gODRNO, oOdrQTY, oHTQTY, oINSUU) Then
                        ' 保留中の件数が存在するかを調査
                        Dim oWaitQTY As Integer = getWaitOdrRecQTY(gODRNO)
                        If oOdrQTY = (oHTQTY + oWaitQTY) Then
                            txtTotalQty.Text = "済"
                            MsgBox("既に出荷準備されています．" & vbCrLf & vbCrLf & _
                                   "確認してください！", MsgBoxStyle.Exclamation)
                            Exit Sub
                        Else
                            txtTotalQty.Text = oOdrQTY - oHTQTY - oWaitQTY      ' 指示書残数
                            If (oOdrQTY - oHTQTY - oWaitQTY) < oINSUU Then
                                txtQTY.Text = oOdrQTY - oHTQTY - oWaitQTY       ' 収容数より少ない場合は指示書残数
                            Else
                                txtQTY.Text = oINSUU                            ' 収容数
                            End If
                        End If
                    Else
                        txtTotalQty.Text = "---"                                ' 指示書なし Split(s, "|")(3) ' 納入数量をセットしてみる（いらないかも）
                        txtQTY.Text = Split(s, "|")(10)                         ' 収容数
                    End If

                ElseIf Strings.Left(s, 2) = "21" Then
                    lblTotalQty.Text = "指示数"
                    txtTotalQty.Text = "---"                                ' 指示書なし初期値
                    txtQTY.Text = ""                                        ' 初期表示なし
                    ' KIC 1次元バーコード仕様（固定長）
                    '   (0) :生産拠点コード :1  :2桁
                    '   (1) :品番           :3  :10桁
                    '   (2) :月度           :13 :1桁（1,2,3～A,B,C(12月)）
                    '   (3) :納入数         :14 :4桁 = 固定 17 Byte

                    ' 出荷指示書に存在する品番かをまずはチェック
                    Dim dr() As DataRow = getKD8330dtTKCD("C0101", Strings.Mid(s, 3, 10).Trim())

                    Dim rslt As DialogResult
                    Dim msg As String
                    Dim cnt As Integer = 1
                    If dr.Length > 0 Then
                        For Each r As DataRow In dr
                            If dr.Length > 1 Then
                                msg = "消込対象が複数あります" & vbCrLf & vbCrLf & _
                                      "その現品票は" & vbCrLf & _
                                      "納期:[" & r("DLVRDT").ToString() & "]ですか？" & vbCrLf & _
                                      "(" & cnt & " / " & dr.Length & ")"
                                rslt = MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MsgBoxStyle.DefaultButton1)
                            End If
                            If dr.Length = 1 Or rslt = Windows.Forms.DialogResult.Yes Then
                                gTKCD = "C0101"
                                gDLVRDT = r("DLVRDT").ToString()
                                gTKHMCD = Strings.Mid(s, 3, 10)
                                gODRNO = r("ODRNO").ToString()
                                Dim oOdrQTY As Integer = IIf(IsNumeric(r("ODRQTY")), Integer.Parse(r("ODRQTY")), 0)
                                Dim oHTQTY As Integer = IIf(IsNumeric(r("HTJUQTY")), Integer.Parse(r("HTJUQTY")), 0)
                                Dim oINSUU As Integer = IIf(IsNumeric(r("INSUU")), Integer.Parse(r("INSUU")), 0)
                                Dim oWaitQTY As Integer = getWaitOdrRecQTY(gODRNO)
                                If oOdrQTY = (oHTQTY + oWaitQTY) Then
                                    txtTotalQty.Text = "済"
                                    MsgBox("既に出荷準備されています．" & vbCrLf & vbCrLf & _
                                           "確認してください！", MsgBoxStyle.Exclamation)
                                    Exit Sub
                                Else
                                    lblTotalQty.Text = "残数"
                                    txtTotalQty.Text = oOdrQTY - oHTQTY - oWaitQTY      ' 指示書残数
                                    If (oOdrQTY - oHTQTY - oWaitQTY) < oINSUU Then
                                        txtQTY.Text = oOdrQTY - oHTQTY - oWaitQTY       ' 収容数より少ない場合は指示書残数
                                    Else
                                        txtQTY.Text = oINSUU                            ' 収容数
                                    End If
                                End If
                                Exit For
                            End If
                            cnt = cnt + 1
                        Next
                    End If

                End If

            ElseIf mKD8330Mode <> "" Then
                lblTotalQty.Text = "指示数"
                txtTotalQty.Text = "---"                                ' 指示書なし初期値
                txtQTY.Text = ""                                        ' 初期表示なし

                ' 出荷指示書に存在する品番かをまずはチェック
                Dim dr() As DataRow = getKD8330dt(_HMCD)

                ' 拠点コードがわかっている場合は更新しない
                '   84: C0282 : KCW
                '   98: C0283 : KAMS中国向け農機
                If dr.Length = 0 Or s.StartsWith("84") Or s.StartsWith("98") Then
                    txtTotalQty.Text = "---"                                ' 指示書なし初期値
                    txtQTY.Text = ""                                        ' 初期表示なし
                Else
                    Dim msgbase As String = IIf(dr.Length > 1, "消込対象が複数あります" & vbCrLf & vbCrLf, "")
                    Dim msg As String
                    Dim cnt As Integer = 1
                    For Each r As DataRow In dr
                        msg = msgbase & _
                            "その現品票は" & vbCrLf & _
                            "得意先コード:[" & r("TKCD").ToString() & "]" & vbCrLf & _
                            "納期:[" & r("DLVRDT").ToString() & "]" & vbCrLf & _
                            "品番:[" & txtHMCD.Text & "]" & vbCrLf & vbCrLf & _
                            "であっていますか？" & vbCrLf & _
                            "(" & cnt & " / " & dr.Length & ")"
                        If MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MsgBoxStyle.DefaultButton2) = Windows.Forms.DialogResult.Yes Then
                            gTKCD = r("TKCD").ToString()
                            gDLVRDT = r("DLVRDT").ToString()
                            gTKHMCD = _HMCD
                            gODRNO = r("ODRNO").ToString()

                            Dim oOdrQTY As Integer = IIf(IsNumeric(r("ODRQTY")), Integer.Parse(r("ODRQTY")), 0)
                            Dim oHTQTY As Integer = IIf(IsNumeric(r("HTJUQTY")), Integer.Parse(r("HTJUQTY")), 0)
                            Dim oINSUU As Integer = IIf(IsNumeric(r("INSUU")), Integer.Parse(r("INSUU")), 0)
                            Dim oWaitQTY As Integer = getWaitOdrRecQTY(gODRNO)
                            If oOdrQTY = (oHTQTY + oWaitQTY) Then
                                txtTotalQty.Text = "済"
                                MsgBox("既に出荷準備されています．" & vbCrLf & vbCrLf & _
                                       "確認してください！", MsgBoxStyle.Exclamation)
                                Exit Sub
                            Else
                                lblTotalQty.Text = "残数"
                                txtTotalQty.Text = oOdrQTY - oHTQTY - oWaitQTY  ' 指示書残数
                                txtQTY.Text = oINSUU                            ' 収容数
                            End If
                            Exit For
                        End If
                        cnt = cnt + 1
                    Next
                End If

            ElseIf mKD8330Mode = "" Then ' ローカル照合モード（クボタ照合）のときでも数量と収容数をセット

                lblTotalQty.Text = "指示数"

                If mKD8330Mode = "" And _
                ( _
                    (Strings.Left(s, 2) = "23" And txtTKHMCD.TextLength = 100) Or _
                    (Strings.Left(s, 2) = "28" And s.Length > 90) Or _
                    (Strings.Left(s, 7) = "KQR_Tag" And UBound(Split(s, "|")) >= 13) Or _
                    (Strings.Left(s, 2) = "21" And txtTKHMCD.TextLength = 17) _
                ) Then
                    If Strings.Left(s, 2) = "23" Or Strings.Left(s, 2) = "28" Then
                        ' KSCM バーコード仕様
                        '   (0) :工場           :1  :2桁
                        '   (1) :品番           :3  :10桁
                        '   (2) :注番           :13 :8桁
                        '   (3) :枝番           :21 :3桁
                        '   (4) :ﾗｲﾝﾛｹｰｼｮﾝ      :24 :8桁
                        '   (5) :材管ﾛｹｰｼｮﾝ     :32 :8桁
                        '   (6) :納入指示数     :40 :7桁
                        '   (7) :箱種           :47 :5桁
                        '   (8) :収容数         :52 :7桁
                        '   (9) :生地品番       :59 :12桁
                        '   (10):塗装色         :71 :10桁
                        '   (11):納入指示日     :81 :8桁
                        '   (12):予備           :89 :12桁 = 固定 100 Byte
                        txtTotalQty.Text = Integer.Parse(Strings.Mid(s, 40, 7)) ' 納入数量をセットしてみる（いらないかも）
                        txtQTY.Text = "" ' Integer.Parse(Strings.Mid(s, 52, 7))
                    ElseIf Strings.Left(s, 7) = "KQR_Tag" Then
                        ' KIC QRバーコード仕様（可変長）
                        '   QRコードの"\t"[0x09]はHT側にて"|"[0x7C(124)]に変換してある（しないとセパレーターが入ってこない）
                        '   (0) :識別子         :最大7桁(KQR_Tag)
                        '   (1) :現品票番号     :最大10桁
                        '   (2) :品目           :最大13桁(ハイフンなし品番で変なコード[R]も着いてくる)
                        '   (3) :納入数量       :最大11桁
                        '   (4) :材管保管場所   :最大6桁
                        '   (5) :注番           :最大10桁（注文番号）
                        '   (6) :発注明細番号   :最大5桁
                        '   (7) :品番           :最大10桁（ハイフンなし品番）
                        '   (10):収容数         :最大11桁
                        '   (13):納入期日       :最大8桁
                        txtTotalQty.Text = Split(s, "|")(3) ' 納入数量をセットしてみる（いらないかも）
                        txtQTY.Text = "" ' Split(s, "|")(10)
                    ElseIf Strings.Left(s, 2) = "21" Then
                        ' KIC 1次元バーコード仕様（固定長）
                        '   (0) :生産拠点コード :1  :2桁
                        '   (1) :品番           :3  :10桁
                        '   (2) :月度           :13 :1桁（1,2,3～A,B,C(12月)）
                        '   (3) :納入数         :14 :4桁 = 固定 17 Byte
                        txtTotalQty.Text = Integer.Parse(Strings.Mid(s, 14, 4))
                        txtQTY.Text = "" ' Integer.Parse(Strings.Mid(s, 14, 4))
                    End If
                End If
            End If
            ' ********************************************************************

            If txtHMCD.Text = "PROOFREAD-OK" Then
                ' 朝一校正処理 24.09.03 add y.w
                rec.MAKER = "KUBOTA"
                rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
                rec.TANCD = txtTANCD.Text
                rec.HMCD = txtHMCD.Text
                rec.TKHMCD = txtTKHMCD.Text
                rec.QTY = ""
                rec.RESULT = "OK"
                rec.DLVRDT = "-"
                rec.ODRNO = "-"
                rec.DATABASE = "-"
                ret = insertPokaX(tblNamePoka1, rec)
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
            If isWN = False Then MyDialogError.ShowDialog()

            ' 照合結果出力 SQLite Insert
            rec.MAKER = "KUBOTA"
            rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
            rec.TANCD = txtTANCD.Text
            rec.HMCD = txtHMCD.Text
            rec.TKHMCD = txtTKHMCD.Text
            rec.QTY = ""
            rec.RESULT = "NG"
            rec.DLVRDT = "-"
            rec.ODRNO = "-"
            rec.DATABASE = "-"
            ret = insertPokaX(tblNamePoka1, rec)
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
                lblCount.Text = getRecordCount(tblNamePoka1)
                txtTKHMCD.Focus()
                flgConfirm = False
            End If

        End If
    End Sub

    ' gWaitRecからODRNOを検索し保留中の件数をサマリーする
    Private Function getWaitOdrRecQTY(ByVal gODRNO As String) As Integer
        If gWaitRec Is Nothing Then getWaitOdrRecQTY = 0 : Exit Function
        Dim qty As Integer
        qty = gWaitRec _
            .Where(Function(r) r.DATABASE = "WAIT" And r.ODRNO.StartsWith(gODRNO)) _
            .Select(Function(r) If(IsNumeric(r.QTY), CInt(r.QTY), 0)) _
            .Sum()
        getWaitOdrRecQTY = qty
    End Function

    ' gWaitRecから保留中の件数をサマリーする
    Private Function getWaitRecCnt() As Integer
        If gWaitRec Is Nothing Then Return 0
        getWaitRecCnt = gWaitRec.Where(Function(r) r.DATABASE = "WAIT").Count
    End Function

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

                ' 出荷指示書システムへの数量チェックを行なう ver.24.11.04 y.w
                If mKD8330Mode <> "" And gTKCD <> "" And InStr(mTargetTKCDs, gTKCD & "|") > 0 Then
                    Dim dr() As DataRow
                    If gODRNO <> "" Then
                        dr = mKD8330dt.AsEnumerable.Where(Function(r) r("ODRNO").ToString().StartsWith(gODRNO)).ToArray()
                    Else
                        dr = mKD8330dt.AsEnumerable.Where(Function(r) ( _
                                r("TKCD").ToString() = gTKCD And _
                                r("DLVRDT").ToString() = gDLVRDT And _
                                Replace(r("TKHMCD").ToString(), "-", "") = gTKHMCD _
                            )).ToArray()
                    End If
                    Dim qty As Integer = Integer.Parse(txtQTY.Text)
                    Dim odrttl As Integer = 0   ' 同一品番を集計（出荷指示書の指示数）
                    Dim htttl As Integer = 0    ' 同一品番を集計（HTで更新した数量）
                    For wI = 0 To dr.Length - 1
                        If IsNumeric(dr(wI)("ODRQTY")) And IsNumeric(dr(wI)("INSUU")) And IsNumeric(dr(wI)("HTJUQTY")) Then
                            Dim odrqty As Integer = Integer.Parse(dr(wI)("ODRQTY"))
                            Dim insuu As Integer = Integer.Parse(dr(wI)("INSUU"))
                            Dim htqty As Integer = Integer.Parse(dr(wI)("HTJUQTY"))
                            If insuu = 0 Then insuu = odrqty
                            '' 数量チェックの廃止 2024.11.07 y.w => 梱包数以外の入力が結構あるらしい（亮談）
                            ''If odrqty < qty Or insuu > qty Or qty Mod insuu <> 0 Then
                            ''    MsgBox("梱包数[" & insuu & "]に対して" & vbCrLf & _
                            ''           "入力数量が不整合です．" & vbCrLf & vbCrLf & _
                            ''           "確認してください！", MsgBoxStyle.Exclamation)
                            ''    Exit Sub
                            ''End If
                            odrttl += odrqty
                            htttl += htqty
                        End If
                    Next
                    If odrttl > 0 And odrttl < htttl + qty Then
                        Dim msg As String = _
                            "出荷指示数:[" & odrttl & "]" & vbCrLf & _
                            "読取済の数:[" & htttl & "]" & vbCrLf & _
                            "残り必要数:[" & odrttl - htttl & "]" & vbCrLf & _
                            "入力された数:[" & qty & "]" & vbCrLf & vbCrLf & _
                            "必要数を超えた入力です！" & vbCrLf & vbCrLf & _
                            "1:指示書は準備完了に" & vbCrLf & _
                            "2:照合ログは入力数を出力" & vbCrLf & vbCrLf & _
                            "強制完了で続行しますか？"
                        If MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MsgBoxStyle.DefaultButton1) = Windows.Forms.DialogResult.Yes Then
                            flgConfirm = True
                        Else
                            Exit Sub
                        End If
                    ElseIf odrttl > 0 And odrttl = htttl Then
                        MsgBox("既に出荷準備されています．" & vbCrLf & vbCrLf & _
                               "確認してください！", MsgBoxStyle.Exclamation)
                        Exit Sub
                    End If

                End If

                ' 品番照合OKを出力 
                If flgConfirm Then

                    ' SQLite Insert
                    Dim rec As DBPokaRecord
                    Dim ret As Int32
                    rec.DATATIME = Format(Now, "yyyy-MM-dd HH:mm:ss")
                    rec.TANCD = txtTANCD.Text
                    rec.HMCD = txtHMCD.Text
                    rec.TKHMCD = txtTKHMCD.Text
                    rec.QTY = txtQTY.Text
                    rec.RESULT = "OK"
                    If mKD8330Mode <> "" And gTKCD <> "" And InStr(mTargetTKCDs, gTKCD & "|") > 0 Then
                        rec.MAKER = gTKCD
                        rec.DLVRDT = gDLVRDT
                        rec.ODRNO = gODRNO
                        rec.DATABASE = "WAIT"
                    Else
                        rec.MAKER = "KUBOTA"
                        rec.DLVRDT = "-"
                        rec.ODRNO = "-"
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
                        ' オートパワーオフ設定を解除
                        Call setAutoPowerOFF(0)
                        ' SQLServer遅延更新
                        TimerWiFiUpdater.Interval = CInt(mWaitTime) * 1000
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

    ' 出荷指示書システム用にSQLServerから出荷指示書データを取得し、画面モードも変える ver.24.11.04 y.w
    Private Sub setupKD8330Mode(ByVal iStr As String)
        gRetryDt = New DateTime(1999, 12, 31, 23, 59, 59) ' 初期値にクリア
        Dim ret = getKD8330()
        If ret = False Then
            MsgBox("出荷指示書読み込みで" & vbCrLf & _
                   "異常が発生しました．", MsgBoxStyle.Critical)
            MsgBox("通常モードに戻して" & vbCrLf & _
                   "処理を続行させます．", MsgBoxStyle.Information)
            mKD8330Mode = ""
            Me.LabelMenu.BackColor = mcDarkBlack
            Me.LabelMenu.Text = "クボタ照合"

        ElseIf mKD8330dt.Rows.Count = 0 Then
            MsgBox("消込対象の出荷指示書が" & vbCrLf & _
                   "見つかりませんでした．" & vbCrLf & vbCrLf & _
                   "通常モードに戻して" & vbCrLf & _
                   "処理を続行させます．", MsgBoxStyle.Information)
            mKD8330Mode = ""
            Me.LabelMenu.BackColor = mcDarkBlack
            Me.LabelMenu.Text = "クボタ照合"

        Else
            ' 出荷指示モードを変更
            mKD8330Mode = "SQLSERVER"
            Me.LabelMenu.BackColor = Color.Blue
            Me.LabelMenu.Text = "出荷指示書消込"
        End If

    End Sub

    ' Microsoft SQL Server 2008 R8 出荷指示テーブル遅延更新処理
    Private Sub TimerWiFiUpdater_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerWiFiUpdater.Tick

        ' 最初にタイマーをオフ
        TimerWiFiUpdater.Enabled = False

        ' オートパワーオフ設定を元に戻す
        Call setAutoPowerOFF(gInterval)

        ' 保留明細なしは処理終了
        If gWaitRec Is Nothing Then Exit Sub

        ' 疎通失敗時の制御（前回疎通失敗時刻＋指定時間(30分)の間は遅延更新処理を行なわない）
        If mKD8330Mode = "TROUBLE" And DateTime.Now < gRetryDt.AddMinutes(gDisableMin) Then Exit Sub

        ' 別スレッドにて疎通確認
        ' 確認中はWait画面を表示
        Dim fm As New FormWaiting
        fm.Show()
        mKD8330Mode = "CHECKING"
        Dim thread1 As New Thread(AddressOf checkSQLServer2)
        thread1.Start()  ' 別スレッドでの処理開始

        ' 3秒後に疎通状態を確認してみる
        Refresh()
        Thread.Sleep(3000)
        fm.Close()
        fm.Dispose()
        If mKD8330Mode = "CHECKING" Then
            TimerServerChecker.Enabled = True ' 疎通結果を確認するためのポーリング開始
            Exit Sub

        ElseIf mKD8330Mode = "TROUBLE" Then
            Me.LabelMenu.BackColor = Color.Red
            Dim msg As String = _
                "データベースの更新失敗．" + vbCrLf + _
                "システム担当者に連絡を．" + vbCrLf + _
                "以降" + gDisableMin.ToString + "分間更新しません．"
            Dim result As Integer = MessageBox.Show(msg, "システム更新", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
            gRetryDt = DateTime.Now
            Exit Sub

        ElseIf mKD8330Mode = "SQLSERVER" Then
            Me.LabelMenu.BackColor = Color.Blue
            gRetryDt = New DateTime(1999, 12, 31, 23, 59, 59) ' 最終リトライ失敗時刻を初期値にクリア

        End If

        ' 溜まっている、WAITレコードを全て読み込む
        Dim idx As Integer
        For idx = 0 To UBound(gWaitRec)

            ' 出荷指示テーブルの更新 (SQL Server 2008 R2)
            Dim dbstatus As String = UpdateKD8330(gWaitRec(idx).MAKER, gWaitRec(idx).DLVRDT, gWaitRec(idx).ODRNO, gWaitRec(idx).HMCD, 0, gWaitRec(idx).QTY, txtTANCD.Text)

            ' 出荷指示モードテーブルを再取得(最新のHTQTY情報がほしい為) ver.24.11.04 y.w
            If dbstatus = "OK" Then
                Call getKD8330()
            End If

            ' 照合履歴ファイルの更新 (SQLite)
            Call updatePokaXDatabase(tblNamePoka1, gWaitRec(idx), dbstatus)

            gWaitRec(idx).DATABASE = dbstatus
        Next
        If gWaitRec.Where(Function(r) r.DATABASE = "WAIT").Count = 0 Then
            gWaitRec = Nothing
        Else
            ' LINQを使用してgWaitRec要素を再作成
            gWaitRec = gWaitRec.Where(Function(r) r.DATABASE = "WAIT").ToArray
        End If

    End Sub

    Private Sub UpdateWaitRec()
        ' 溜まっている、WAITレコードを全て読み込む
        Dim idx As Integer
        For idx = 0 To UBound(gWaitRec)

            ' 出荷指示テーブルの更新 (SQL Server 2008 R2)
            Dim dbstatus As String = UpdateKD8330(gWaitRec(idx).MAKER, gWaitRec(idx).DLVRDT, gWaitRec(idx).ODRNO, gWaitRec(idx).HMCD, 0, gWaitRec(idx).QTY, txtTANCD.Text)

            ' 照合履歴ファイルの更新 (SQLite)
            Call updatePokaXDatabase(tblNamePoka1, gWaitRec(idx), dbstatus)

            gWaitRec(idx).DATABASE = dbstatus
        Next
    End Sub

    Private Sub TimerServerChecker_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerServerChecker.Tick
        If mKD8330Mode = "CHECKING" Then Exit Sub
        TimerServerChecker.Enabled = False
        If mKD8330Mode = "TROUBLE" Then
            Me.LabelMenu.BackColor = Color.Red
            Dim msg As String = _
                "データベースの更新失敗．" + vbCrLf + _
                "システム担当者に連絡を．" + vbCrLf + _
                "以降" + gDisableMin.ToString + "分間更新しません．"
            Dim result As Integer = MessageBox.Show(msg, "システム更新", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
            gRetryDt = DateTime.Now

        ElseIf mKD8330Mode = "SQLSERVER" Then
            Me.LabelMenu.BackColor = Color.Blue
            gRetryDt = New DateTime(1999, 12, 31, 23, 59, 59) ' 最終リトライ失敗時刻を初期値にクリア
            ' DB更新は次回照合時にまかせる

        End If
    End Sub
End Class
