Imports System.Text
Imports System.Data
Imports System
Imports System.Windows.Forms
Imports Microsoft.WindowsCE.Forms

Public Class FormPokaHistory

    ' このWindowのインスタンス
    Public Shared FormPokaHistInstance As FormPokaHistory

    ' SQLite
    Private tableName As [String] = ""

    Private tancd As [String] = ""

    ' DataGrid1を操作
    Public totalRow As Int32

    Public Sub New(ByVal _tablename As String, ByVal _tancd As String)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        tableName = _tablename
        tancd = _tancd

    End Sub

    Private Sub FormPokaHistory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormPokaHistInstance = Me

        viewData()

        If totalRow > 0 Then
            DataGrid1.CurrentRowIndex = 0
            DataGrid1.Select(0)
        End If

    End Sub

    '''//////////////////////////////////////////////////////////
    ''' View data
    '''//////////////////////////////////////////////////////////
    Private Sub viewData()
        Dim dt As New DataTable(tableName)
        dt = selectPokaX(tableName)

        DataGrid1.TableStyles.Clear()
        DataGrid1.DataSource = New DataView(dt)
        DataGrid1.RowHeadersVisible = False
        Dim ts As New DataGridTableStyle()
        ts.MappingName = tableName
        DataGrid1.TableStyles.Add(ts)
        DataGrid1.TableStyles(tableName).GridColumnStyles("ID").Width = -1
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemMAKER).Width = -1
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemDATETIME).HeaderText = "時刻"
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemDATETIME).Width = 35
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemHMCD).HeaderText = "社内品番"
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemHMCD).Width = 135
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemQTY).HeaderText = "数"
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemQTY).Width = 27
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemRESULT).HeaderText = "結"
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemRESULT).Width = 18
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemDLVRDT).Width = -1
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemODRNO).Width = -1
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemDB).Width = -1

        totalRow = getRecordCount(tableName)
        lblCount.Text = totalRow.ToString() & "件"
        'If totalRow > 0 Then
        '    DataGrid1.CurrentCell = New DataGridCell(0, 0)
        '    DataGrid1.CurrentRowIndex = 0
        'End If

    End Sub

    Private Sub viewData2()
        ''テーブルを作成()
        Dim dataSet1 As New DataSet("商品マスター")
        Dim dataTable1 As DataTable = dataSet1.Tables.Add("商品テーブル")
        Dim dataClumn1 As DataColumn = dataTable1.Columns.Add("ID")
        Dim dataClumn2 As DataColumn = dataTable1.Columns.Add("商品")
        Dim dataClumn3 As DataColumn = dataTable1.Columns.Add("個数")

        DataGrid1.TableStyles.Clear()
        DataGrid1.RowHeadersVisible = False

        '' テーブルにデータを追加
        dataTable1.Rows.Add(New Object() {1, "みかん", 100})
        dataTable1.Rows.Add(New Object() {2, "りんご", 300})
        dataTable1.Rows.Add(New Object() {3, "バナナ", 120})
        dataTable1.Rows.Add(New Object() {4, "すいか", 280})
        dataTable1.Rows.Add(New Object() {5, "いちご", 200})
        dataTable1.Rows.Add(New Object() {6, "メロン", 150})

        '' データグリッドにテーブルを表示する
        DataGrid1.DataSource = dataSet1.Tables("商品テーブル")

        Dim ts As New DataGridTableStyle()
        ts.MappingName = "商品テーブル"
        DataGrid1.TableStyles.Add(ts)
        DataGrid1.TableStyles("商品テーブル").GridColumnStyles("ID").Width = -1
        DataGrid1.TableStyles("商品テーブル").GridColumnStyles("商品").Width = 160
        DataGrid1.TableStyles("商品テーブル").GridColumnStyles("個数").Width = 70

    End Sub

    ' F1キー (戻る)
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F2キー (先頭行へ移動)
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click
        If totalRow = 0 Then Exit Sub

        'C:\Program Files(x86)\Windows Mobile 5.0 R2\PocketPC\Incude\Armv4i\winuser.hより
        '#define WM_KEYDOWN             0x0100
        '#define WM_KEYUP               0x0101
        '#define VK_UP                  0x26
        '#define VK_DOWN                0x28
        '#define VK_F1                  0x70
        '#define VK_F2                  0x71
        Dim VK_UP As Integer = Bt.LibDef.BT_VK_UP '&H26(38)
        Dim VK_DOWN As Integer = Bt.LibDef.BT_VK_DOWN '&H28(40)
        Dim VK_F1 As Integer = Bt.LibDef.BT_VK_F1 '&H70(112)

        If totalRow > 0 Then
            DataGrid1.CurrentRowIndex = totalRow - 1 ' 一度最下行にしてから最上部にもってくる 24.05.30 add y.w
            DataGrid1.CurrentRowIndex = 0
            DataGrid1.Focus()
        End If

    End Sub

    ' F3キー (最終行へ移動)
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        If totalRow = 0 Then Exit Sub
        DataGrid1.CurrentRowIndex = totalRow - 1
        DataGrid1.Focus()
    End Sub

    ' F4キー (削除)
    Private Sub btnF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF4.Click
        If totalRow = 0 Then Exit Sub
        Dim row As Long = DataGrid1.CurrentRowIndex
        If DataGrid1.IsSelected(row) = False Then
            MessageBox.Show("削除する明細を選択してください．")
            Exit Sub
        End If
        If MessageBox.Show("削除してもよろしいですか？", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MsgBoxStyle.DefaultButton1) = Windows.Forms.DialogResult.Yes Then
            Dim rowid As Integer = Integer.Parse(DataGrid1(row, 0).ToString())
            Dim tkcd As String = DataGrid1(row, 1).ToString()
            Dim hmcd As String = DataGrid1(row, 3).ToString()
            Dim qty As String = DataGrid1(row, 4).ToString()
            Dim dlvrdt As String = DataGrid1(row, 6).ToString()
            Dim odrno As String = DataGrid1(row, 7).ToString()
            Dim db As String = DataGrid1(row, 8).ToString()
            hmcd = Replace(Replace(Replace(Replace(hmcd, "待", ""), "◎", ""), "△", ""), "×", "")
            ' SQLServer側が既に更新されていたら0で更新し直す
            If db = "OK" Then
                UpdateKD8330(tkcd, dlvrdt, odrno, hmcd, qty, 0, tancd)
                Call getKD8330() ' 出荷指示テーブル再取得 ver.24.11.04 y.w
            End If

            If deletePokaXMeisai(tableName, DataGrid1(row, 0).ToString()) Then
                viewData() ' OK時、データを取得し直してDataGrid1を再表示
            End If
            If totalRow = 0 Then
                Exit Sub
            ElseIf row >= totalRow Then '最終行を消した場合
                DataGrid1.CurrentRowIndex = totalRow - 1
                DataGrid1.Select(totalRow - 1)
            Else
                DataGrid1.CurrentRowIndex = row
                DataGrid1.Select(row)
            End If
        End If
    End Sub

    Private Sub FormPokaHistory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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

    ' CrrentCell変更時はSelectをコードで一旦消してから変更しないといけないみたい
    ' 逆にSelectだけ変更してもCurrentCellが自動で追いかけてくれない
    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        If totalRow > 0 Then
            For wRow As Integer = 0 To totalRow - 1
                DataGrid1.UnSelect(wRow)
            Next
            DataGrid1.Select(DataGrid1.CurrentRowIndex)
        End If
    End Sub

    ' 数量変更
    Private Sub DataGrid1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGrid1.KeyDown
        Select e.KeyCode
            Case System.Windows.Forms.Keys.Enter
                Dim row As Long = DataGrid1.CurrentRowIndex
                If DataGrid1.IsSelected(row) Then
                    Dim rowid As Integer = Integer.Parse(DataGrid1(row, 0).ToString())
                    Dim tkcd As String = DataGrid1(row, 1).ToString()
                    Dim hmcd As String = DataGrid1(row, 3).ToString()
                    Dim qty As String = DataGrid1(row, 4).ToString()
                    Dim dlvrdt As String = DataGrid1(row, 6).ToString()
                    Dim odrno As String = DataGrid1(row, 7).ToString()
                    Dim db As String = DataGrid1(row, 8).ToString()
                    hmcd = Replace(Replace(Replace(Replace(hmcd, "待", ""), "◎", ""), "△", ""), "×", "")
                    Dim form As FormPokaModify = New FormPokaModify(tableName, rowid, tkcd, dlvrdt, odrno, hmcd, qty, tancd, db)
                    Dim result = form.ShowDialog()
                    If result = Windows.Forms.DialogResult.OK Then
                        viewData()
                        DataGrid1.CurrentRowIndex = row
                        DataGrid1.Select(row)
                        DataGrid1.Focus()
                    End If
                End If
        End Select

    End Sub
End Class
