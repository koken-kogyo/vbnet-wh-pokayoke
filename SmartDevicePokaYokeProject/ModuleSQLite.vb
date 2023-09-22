Imports System.Text
Imports System.Data
Imports System.Runtime.InteropServices

Module ModuleSQLite

    ' データベース関連
    Private accPath As [String] = "\FlashDisk\BT_FILES\drv1"
    Private dbFile As [String] = "PokaYoke.DB"

    Public tblNamePoka1 As [String] = "Poka1"
    Public tblNamePoka2 As [String] = "Poka2"
    Public tblNamePoka3 As [String] = "Poka3"
    Public tblNamePoka4 As [String] = "Poka4"
    Public itemMAKER As [String] = "メーカー"
    Public itemDATETIME As [String] = "照合日付"
    Public itemTANCD As [String] = "担当者"
    Public itemHMCD As [String] = "社内品番"
    Public itemTKHMCD As [String] = "社外品番"
    Public itemRESULT As [String] = "照合結果"

    ' エラー詳細を保持
    Public dbIdx As Integer = 0
    Public sqliteErrorString As String = ""

    ' 定数
    Public Const SQLITE_OK As Int32 = 0
    Public Const SQLITE_OPEN_ERROR As Int32 = -901
    Public Const SQLITE_REOPEN_ERROR As Int32 = -902
    Public Const SQLITE_NOTOPEN_ERROR As Int32 = -903
    Public Const SQLITE_CLOSE_ERROR As Int32 = -904

    Public Const SQLITE_CREATE_TABLE_ERROR As Int32 = -911

    Public Const SQLITE_GET_RECORDE_ERROR As Int32 = -921

    Public Const SQLITE_INSERT_ERROR As Int32 = -931
    Public Const SQLITE_DELETE_ERROR As Int32 = -941

    Public Structure DBRecord
        Public MAKER As String
        Public DATATIME As String
        Public TANCD As String
        Public HMCD As String
        Public TKHMCD As String
        Public RESULT As String
    End Structure

    '''//////////////////////////////////////////////////////////
    ''' Open
    '''//////////////////////////////////////////////////////////
    Public Function openDB() As Int32
        Dim ret As Int32 = 0
        If dbIdx > 0 Then
            Return SQLITE_REOPEN_ERROR
        End If
        dbIdx = Bt.FileLib.SQLite.btSQLiteOpen(New StringBuilder(accPath & "\" & dbFile))
        If dbIdx > 0 Then
            ' Create Table
            ret = createPoka1()
            If ret <> 0 Then
                Return ret
            End If
            ret = createPoka2()
            If ret <> 0 Then
                Return ret
            End If
            ret = createPoka3()
            If ret <> 0 Then
                Return ret
            End If
            ret = createPoka4()
            If ret <> 0 Then
                Return ret
            End If
            Return SQLITE_OK
        Else
            Return SQLITE_OPEN_ERROR
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Close
    '''//////////////////////////////////////////////////////////
    Public Function closeDB() As Int32
        If dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteClose(dbIdx)
        dbIdx = 0
        If ret = 0 Then
            Return SQLITE_OK
        Else
            Return SQLITE_CLOSE_ERROR
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table1
    '''//////////////////////////////////////////////////////////
    Private Function createPoka1() As Int32
        If dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemMAKER & ", " & itemDATETIME & ", " & itemTANCD & ", " & itemHMCD & ", " & itemTKHMCD & ", " & itemRESULT
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka1 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(dbIdx, sql)
        If ret <> 0 Then
            Return SQLITE_CREATE_TABLE_ERROR
        End If
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table2
    '''//////////////////////////////////////////////////////////
    Private Function createPoka2() As Int32
        If dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemMAKER & ", " & itemDATETIME & ", " & itemTANCD & ", " & itemHMCD & ", " & itemTKHMCD & ", " & itemRESULT
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka2 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(dbIdx, sql)
        If ret <> 0 Then
            Return SQLITE_CREATE_TABLE_ERROR
        End If
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table3
    '''//////////////////////////////////////////////////////////
    Private Function createPoka3() As Int32
        If dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemMAKER & ", " & itemDATETIME & ", " & itemTANCD & ", " & itemHMCD & ", " & itemTKHMCD & ", " & itemRESULT
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka3 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(dbIdx, sql)
        If ret <> 0 Then
            Return SQLITE_CREATE_TABLE_ERROR
        End If
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table4
    '''//////////////////////////////////////////////////////////
    Private Function createPoka4() As Int32
        If dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemMAKER & ", " & itemDATETIME & ", " & itemTANCD & ", " & itemHMCD & ", " & itemTKHMCD & ", " & itemRESULT
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka4 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(dbIdx, sql)
        If ret <> 0 Then
            Return SQLITE_CREATE_TABLE_ERROR
        End If
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Get record count
    '''//////////////////////////////////////////////////////////
    Public Function getRecordCount(ByVal tableName As String) As Int32
        Dim ret As Int32
        If dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim sql As New StringBuilder("SELECT COUNT(*) FROM " & tableName & ";")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(dbIdx)
        If cIdx <= 0 Then
            Return SQLITE_GET_RECORDE_ERROR
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)

        Do
            ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
            If ret = 1 Then
                Exit Do
            ElseIf ret <= 0 Then
                GoTo FUNCEND
            End If
        Loop While ret = 1

        Dim data As IntPtr
        data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
        ret = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, 0, data, 8192)
        If ret = 0 Then
            Dim data2 As [String] = Marshal.PtrToStringUni(data)
            ret = Integer.Parse(data2)
        Else
            ret = SQLITE_GET_RECORDE_ERROR
        End If
        Marshal.FreeCoTaskMem(data)
FUNCEND:
        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        Return ret
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Check table exists
    '''//////////////////////////////////////////////////////////
    Public Function checkTableExists(ByVal tableName As String) As Boolean
        Dim bret As Boolean = False
        If dbIdx <= 0 Then
            Return bret
        End If
        Dim sql As New StringBuilder("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '" & tableName & "';")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(dbIdx)
        If cIdx <= 0 Then
            Return bret
        End If

        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)

        ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
        If ret <= 0 Then
            GoTo FUNCEND
        End If

        Dim data As IntPtr
        data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
        ret = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, 0, data, 8192)
        If ret = 0 Then
            Dim data2 As [String] = Marshal.PtrToStringUni(data)
            If Integer.Parse(data2) > 0 Then
                bret = True
            End If
        End If
        Marshal.FreeCoTaskMem(data)
FUNCEND:
        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        Return bret
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Insert
    '''//////////////////////////////////////////////////////////
    Public Function insertPokaX(ByVal tableName As String, ByVal rec As DBRecord) As Int32
        If dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim val As String = rec.MAKER & "', '" & rec.DATATIME & "', '" & rec.TANCD & "', '" & rec.HMCD & "', '" & rec.TKHMCD & "', '" & rec.RESULT
        Dim sql As New StringBuilder("INSERT INTO " & tableName & " VALUES('" & val & "');")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(dbIdx, sql)
        If ret <> 0 Then
            insertPokaX = SQLITE_INSERT_ERROR
            setSQLErrorString()
        Else
            insertPokaX = SQLITE_OK
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Delete
    '''//////////////////////////////////////////////////////////
    Public Function deletePokaX(ByVal tableName As String) As Int32
        If dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim sql As New StringBuilder("DELETE FROM " & tableName & ";")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(dbIdx, sql)
        If ret <> 0 Then
            deletePokaX = SQLITE_DELETE_ERROR
            setSQLErrorString()
        Else
            deletePokaX = SQLITE_OK
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' View SQL Error
    '''//////////////////////////////////////////////////////////
    Private Sub setSQLErrorString()
        Dim msg As String = ""
        If dbIdx > 0 Then

            Dim lasterr As Integer = Bt.FileLib.SQLite.btSQLiteGetErrorCode(dbIdx)

            Dim data As IntPtr
            data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
            Bt.FileLib.SQLite.btSQLiteGetErrorStr(dbIdx, data, 8192)

            Dim data2 As [String] = Marshal.PtrToStringUni(data)
            Marshal.FreeCoTaskMem(data)

            sqliteErrorString = data2 & vbCrLf & "ErrorCode:" & lasterr.ToString()

        End If
    End Sub

    '''//////////////////////////////////////////////////////////
    ''' View data
    '''//////////////////////////////////////////////////////////
    Public Function selectPokaX(ByVal tableName As String) As DataTable
        Dim dt As New DataTable(tableName)
        dt.Columns.Add(New DataColumn("ID"))
        dt.Columns.Add(New DataColumn(itemDATETIME))
        dt.Columns.Add(New DataColumn(itemHMCD))
        dt.Columns.Add(New DataColumn(itemRESULT))

        If dbIdx <= 0 Then
            Return dt
        End If
        If checkTableExists(tableName) = False Then
            Return dt
        End If

        ' SQLite sreftime フォーマット %Y年 %m月 %d日 %H時 %M分 %S秒 as 照合日付
        Dim sql As New StringBuilder("SELECT ROWID, strftime('%d日%H:%M', 照合日付), 社内品番, 照合結果 as 結 FROM " & tableName & " order by 照合日付 desc;")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(dbIdx)
        If cIdx <= 0 Then
            MessageBox.Show("ERROR btSQLiteCmdCreate:" & cIdx)
            Return dt
        End If

        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            setSQLErrorString()
            MessageBox.Show(sqliteErrorString)
            MessageBox.Show("ERROR btSQLiteCmdSetCommandText:" & ret & vbCr & vbLf & sql.ToString())
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)

        Dim reccnt As Integer = 1
        Do
            ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
            If ret < 0 Then
                MessageBox.Show("ERROR btSQLiteCmdRead:" & ret & vbCr & vbLf & sql.ToString())
                GoTo FUNCEND
            ElseIf ret = 1 Then
                Dim cnt As Integer = Bt.FileLib.SQLite.btSQLiteCmdGetValueCount(cIdx)
                If cnt < 0 Then
                    MessageBox.Show("ERROR btSQLiteCmdGetValueCount:" & ret)
                    GoTo FUNCEND
                End If
                Dim i As Integer
                Dim dr As DataRow
                dr = dt.NewRow()
                For i = 0 To cnt - 1
                    Dim data As IntPtr
                    data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
                    Dim ret2 As Integer = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, i, data, 8192)
                    If ret2 <> 0 Then
                        MessageBox.Show("ERROR btSQLiteCmdGetValue:" & ret2)
                        Marshal.FreeCoTaskMem(data)
                        GoTo FUNCEND
                    End If
                    dr(i) = Marshal.PtrToStringUni(data)
                    Marshal.FreeCoTaskMem(data)
                Next
                dt.Rows.Add(dr)
                reccnt += 1
            End If
        Loop While ret = 1

FUNCEND:

        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        selectPokaX = dt
    End Function

    '''//////////////////////////////////////////////////////////
    ''' CSV output
    '''//////////////////////////////////////////////////////////
    Public Function SQLite2CSV(ByVal tblName As String) As Integer
        Dim ret As Integer = -1
        Dim cIdx As Integer = 0
        If dbIdx <= 0 Then
            Return ret
        End If

        Dim sql = New StringBuilder((Convert.ToString("SELECT * FROM ") & tblName) & ";")
        cIdx = Bt.FileLib.SQLite.btSQLiteCmdCreate(dbIdx)
        If cIdx <= 0 Then
            GoTo FUNCEND
        End If
        ret = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret < 0 Then
            GoTo FUNCEND
        End If
        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)
        If ret < 0 Then
            GoTo FUNCEND
        End If

        Dim writer As System.IO.StreamWriter = Nothing
        Dim sep As String = ","
        Dim data As IntPtr
        Dim week As Int32 = DateTime.Now.DayOfWeek
        Dim filePath As String = accPath & "\" & tblName & "_" & week & ".csv"
        data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
        Try
            writer = New System.IO.StreamWriter(filePath, False, System.Text.Encoding.GetEncoding("shift_jis"))
            Do
                ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
                If ret < 0 Then
                    GoTo FUNCEND
                ElseIf ret = 1 Then
                    Dim line As New StringBuilder("")
                    Dim cnt As Integer = Bt.FileLib.SQLite.btSQLiteCmdGetValueCount(cIdx)
                    For i As Integer = 0 To cnt - 1
                        If line.ToString() <> "" Then
                            line.Append(sep)
                        End If
                        Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, i, data, 8192)
                        line.Append("""" & Marshal.PtrToStringUni(data) & """")
                    Next
                    writer.WriteLine(line)
                End If
            Loop While ret = 1
        Finally
            If writer IsNot Nothing Then
                writer.Close()
            End If
        End Try

        ret = 0
FUNCEND:

        If cIdx > 0 Then
            Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        End If
        Marshal.FreeCoTaskMem(data)
        Return ret
    End Function

End Module
