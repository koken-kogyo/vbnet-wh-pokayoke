Imports System.Data

Public Class MyDialogSelector

    Public SelectedDataRow As DataRow

    Public Sub New(ByVal _HMCD As String)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        Dim dv As New DataView(mKD8330dt)
        dv.RowFilter = "TKHMCD = '" & _HMCD & "' And ODRQTY <> HTJUQTY"

        Dim dt As New DataTable("DataTable1")
        ' ヘッダー作成
        dt.Columns.Add(New DataColumn("Column1"))
        dt.Columns.Add(New DataColumn("Column2"))
        dt.Columns.Add(New DataColumn("Column3"))
        dt.Columns.Add(New DataColumn("Column4"))
        dt.Columns.Add(New DataColumn("Column5"))
        ' データ作成
        For Each r As DataRowView In dv
            Dim dr As DataRow
            dr = dt.NewRow()
            dr(0) = r("TKCD").ToString()
            dr(1) = Format(CDate(r("DLVRDT").ToString()), "M/d")
            dr(2) = r("ODRQTY").ToString()
            dr(3) = r("ODRNO").ToString()
            If r("HMCD").ToString() <> "" Then
                dr(4) = r("HMCD").ToString()
            Else
                dr(4) = r("TKHMCD").ToString()
            End If
            dt.Rows.Add(dr)
        Next

        ' DataGridTableStyleの設定
        Dim ts As New DataGridTableStyle
        ts.MappingName = "DataTable1"

        ' テーブルスタイルの設定
        Dim cs1 As DataGridTextBoxColumn = New DataGridTextBoxColumn()
        Dim cs2 As DataGridTextBoxColumn = New DataGridTextBoxColumn()
        Dim cs3 As DataGridTextBoxColumn = New DataGridTextBoxColumn()
        Dim cs4 As DataGridTextBoxColumn = New DataGridTextBoxColumn()
        Dim cs5 As DataGridTextBoxColumn = New DataGridTextBoxColumn()
        cs1.MappingName = "Column1"
        cs2.MappingName = "Column2"
        cs3.MappingName = "Column3"
        cs4.MappingName = "Column4"
        cs5.MappingName = "Column5"
        cs1.HeaderText = "得意"
        cs2.HeaderText = "納期"
        cs3.HeaderText = "指示数"
        cs4.HeaderText = "注番"
        cs5.HeaderText = "品番"
        cs1.Width = 50
        cs2.Width = 50
        cs3.Width = 50
        cs4.Width = 90
        cs5.Width = 120
        ts.GridColumnStyles.Add(cs1)
        ts.GridColumnStyles.Add(cs2)
        ts.GridColumnStyles.Add(cs3)
        ts.GridColumnStyles.Add(cs4)
        ts.GridColumnStyles.Add(cs5)
        DataGrid1.TableStyles.Add(ts)

        ' コントロール設定
        DataGrid1.DataSource = dt
        DataGrid1.RowHeadersVisible = False

    End Sub

    Private Sub DataGrid1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGrid1.KeyDown
        Select Case e.KeyCode
            Case System.Windows.Forms.Keys.Back
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            Case System.Windows.Forms.Keys.Enter
                Dim row As Integer = DataGrid1.CurrentCell.RowNumber
                Dim wODRNO As String = DataGrid1(row, 3).ToString()
                Dim wTKHMCD As String = DataGrid1(row, 4).ToString()
                ' 注文番号のDataRowを取得し返却用変数にセット
                SelectedDataRow = getKD8330dtODRNO(wODRNO, wTKHMCD)(0)
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
        End Select
    End Sub

    ' 擬似的に行選択モードを再現させる
    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        DataGrid1.Select(DataGrid1.CurrentCell.RowNumber)
    End Sub
    Private Sub DataGrid1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles DataGrid1.Paint
        DataGrid1.Select(DataGrid1.CurrentCell.RowNumber)
    End Sub

End Class