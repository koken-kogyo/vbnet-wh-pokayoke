Public Class FormPokaModify

    ' SQLite
    Private rowId As Integer = 0
    Private tableName As [String] = ""
    Private tkcd As [String] = ""
    Private tancd As [String] = ""
    Private dbstatus As [String] = ""

    Public Sub New(ByVal _tablename As String, ByVal _rowid As Integer, _
                   ByVal _tkcd As String, _
                   ByVal _hmcd As String, _
                   ByVal _qty As String, _
                   ByVal _tancd As String, _
                   ByVal _db As String)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        tableName = _tablename
        rowId = _rowid
        tkcd = _tkcd
        txtHMCD.Text = Replace(Replace(Replace(_hmcd, "待", ""), "×", ""), "△", "")
        txtQTYbefore.Text = _qty
        txtQTY.Text = _qty
        tancd = _tancd
        dbstatus = _db

    End Sub

    Private Sub FormPokaModify_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' カーソル初期位置
        txtQTY.Focus()

    End Sub

    ' F1キー (戻る)
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    ' F4キー (更新)
    Private Sub btnF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF4.Click
        If IsNumeric(txtQTY.Text) = False Then
            MessageBox.Show("数値を入力してください．", "入力チェック", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            txtQTY.Focus()
            Exit Sub
        End If
        If dbstatus = "OK" And txtQTYbefore.Text = txtQTY.Text Then
            MessageBox.Show("変更がありません．", "入力チェック", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            txtQTY.Focus()
            Exit Sub
        End If
        Dim newstatus As String = "-"
        ' SQLServer側が既に更新されていたら差分で更新し直す
        If dbstatus <> "-" Then
            Dim badqty As String = "0"  ' エラーだった場合は新規更新
            If dbstatus = "OK" Then badqty = txtQTYbefore.Text '更新済みの場合は差分で更新
            newstatus = UpdateKD8330(tkcd, txtHMCD.Text, badqty, txtQTY.Text, tancd)
        End If
        ' ローカルSQLiteデータベースを更新
        If updatePokaXMeisai(tableName, rowId, txtQTY.Text, newstatus) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close() ' 更新OK時このダイアログは閉じる
        End If
    End Sub

    Private Sub FormPokaModify_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyValue
            Case System.Windows.Forms.Keys.Enter
                Call btnF4_Click(sender, e)
            Case Bt.LibDef.BT_VK_F1
                Call btnF1_Click(sender, e)
            Case Bt.LibDef.BT_VK_F4
                Call btnF4_Click(sender, e)
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

    Private Sub txtQTY_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQTY.LostFocus
        ' 入力待機色を解除
        txtHMCD.BackColor = Color.White
    End Sub
End Class