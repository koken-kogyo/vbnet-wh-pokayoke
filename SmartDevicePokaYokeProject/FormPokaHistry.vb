Imports System.Text
Imports System.Data
Imports System.Runtime.InteropServices
Imports System
Imports System.Windows.Forms
Imports Microsoft.WindowsCE.Forms

Public Class FormPokaHistory

    ' このWindowのインスタンス
    Public Shared FormPokaHistInstance As FormPokaHistory

    ' SQLite
    Private tableName As [String] = ""

    ' DataGrid1を操作
    Public totalRow As Int32

    Public Sub New(ByVal val As String)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        tableName = val

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
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemDATETIME).Width = 75
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemHMCD).Width = 120
        DataGrid1.TableStyles(tableName).GridColumnStyles(itemRESULT).Width = 20

        totalRow = getRecordCount(tableName)
        lblCount.Text = totalRow.ToString() & "件"
        If totalRow > 0 Then
            DataGrid1.CurrentCell = New DataGridCell(0, 0)
        End If

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

    ' F1キー
    Private Sub btnF1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF1.Click
        Close()
    End Sub

    ' F2キー (先頭行へ移動)
    Private Sub btnF2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF2.Click

        DataGrid1.CurrentCell = New DataGridCell(0, 0)

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


        Dim hWnd As IntPtr = Me.DataGrid1.Handle
        Dim msg As Message
        msg = Message.Create(hWnd, VK_DOWN, IntPtr.Zero, IntPtr.Zero)
        MessageWindow.SendMessage(msg)
        msg = Message.Create(hWnd, VK_UP, IntPtr.Zero, IntPtr.Zero)
        MessageWindow.SendMessage(msg)

    End Sub

    ' F3キー
    Private Sub btnF3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF3.Click
        If totalRow > 0 Then
            DataGrid1.CurrentCell = New DataGridCell(totalRow - 1, 0)
        End If
    End Sub

    ' F4キー
    Private Sub btnF4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnF4.Click
        MessageBox.Show("まだ作成が追いついていません")
    End Sub

    Private Sub FormPokaHistory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyValue
            'Case Bt.LibDef.BT_VK_UP
            '    If DataGrid1.CurrentRowIndex > 0 Then
            '        DataGrid1.CurrentCell = New DataGridCell(DataGrid1.CurrentRowIndex - 1, 1)
            '        e.Handled = True
            '    End If
            'Case Bt.LibDef.BT_VK_DOWN
            '    If DataGrid1.CurrentRowIndex < totalRow - 1 Then
            '        DataGrid1.CurrentCell = New DataGridCell(DataGrid1.CurrentRowIndex + 1, 1)
            '        e.Handled = True
            '    End If
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

    Private Sub FormPokaHistory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' フォーム上でキーダウンイベントを取得
        Me.KeyPreview = True

        ' インスタンス保持
        FormPokaHistInstance = Me

        viewData()
    End Sub

    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        DataGrid1.Select(DataGrid1.CurrentRowIndex)
    End Sub

    Private Sub DataGrid1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGrid1.KeyDown
        'Debug.WriteLine(e.KeyCode)
    End Sub
End Class
