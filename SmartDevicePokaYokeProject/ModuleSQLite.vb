Imports System.Text
Imports System.Data
Imports System.Runtime.InteropServices

Module ModuleSQLite

    ' SQLiteデータベース関連
    Private accPath As [String] = "\FlashDisk\BT_FILES\drv1"

    Private dbFile As [String] = "KokenMaster.DB"
    Public dbIdx As Integer = 0

    Private logFile As [String] = "PokaYoke.DB"
    Public logIdx As Integer = 0

    Private jidoFile As [String] = "shelfstock.pkdat"
    Public jidoIdx As Integer = 0

    Public tblNamePoka1 As [String] = "Poka1" ' クボタ照合履歴テーブル
    Public tblNamePoka2 As [String] = "Poka2" ' ヤンマー照合履歴テーブル
    Public tblNamePoka3 As [String] = "Poka3" ' 日立照合履歴テーブル
    Public tblNamePoka4 As [String] = "Poka4" ' オリエント照合履歴テーブル
    Public tblNamePoka5 As [String] = "Poka5" ' 棚番照合履歴テーブル
    Public itemMAKER As [String] = "メーカー"
    Public itemDATETIME As [String] = "照合日付"
    Public itemTANCD As [String] = "担当者"
    Public itemHMCD As [String] = "社内品番"    ' 社内品番(QRや手入力やOCR)
    Public itemTKHMCD As [String] = "社外品番"  ' メーカーバーコード情報
    Public itemBUCD As [String] = "伝票棚番"
    Public itemTANACD As [String] = "倉庫棚番"
    Public itemQTY As [String] = "数量"         ' 2024.05    add y.w
    Public itemRESULT As [String] = "照合結果"
    Public itemDLVRDT As [String] = "納期"      ' 2024.11.16 add y.w
    Public itemODRNO As [String] = "注番"       ' 2024.12.05 add y.w
    Public itemDB As [String] = "データベース"  ' 2024.07    add y.w

    ' エラー詳細を保持
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

    Public Structure DBPokaRecord
        Public MAKER As String
        Public DATATIME As String
        Public TANCD As String
        Public HMCD As String
        Public TKHMCD As String
        Public QTY As String        ' 2024.05    add y.w
        Public RESULT As String
        Public DLVRDT As String     ' 2024.11.16 add y.w
        Public ODRNO As String      ' 2024.12.05 add y.w
        Public DATABASE As String   ' 2024.07    add y.w
    End Structure

    Public Structure DBTanaRecord
        Public DATATIME As String
        Public TANCD As String
        Public TANACD As String
        Public HMCD As String
        Public RESULT As String
    End Structure

    '''//////////////////////////////////////////////////////////
    ''' Open
    '''//////////////////////////////////////////////////////////
    Public Function openDB() As Int32
        Dim ret As Int32 = 0
        If logIdx > 0 Or dbIdx > 0 Then
            Return SQLITE_REOPEN_ERROR
        End If
        dbIdx = Bt.FileLib.SQLite.btSQLiteOpen(New StringBuilder(accPath & "\" & dbFile))
        If dbIdx <= 0 Then
            Return SQLITE_OPEN_ERROR
        End If
        jidoIdx = Bt.FileLib.SQLite.btSQLiteOpen(New StringBuilder(accPath & "\" & jidoFile))
        If jidoIdx <= 0 Then
            Return SQLITE_OPEN_ERROR
        End If
        logIdx = Bt.FileLib.SQLite.btSQLiteOpen(New StringBuilder(accPath & "\" & logFile))
        If logIdx > 0 Then
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
            ret = createPoka5()
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
        If logIdx <= 0 Or dbIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim ret As Integer
        ret = Bt.FileLib.SQLite.btSQLiteClose(dbIdx)
        If ret <> 0 Then
            Return SQLITE_CLOSE_ERROR
        End If
        dbIdx = 0

        ret = Bt.FileLib.SQLite.btSQLiteClose(jidoIdx)
        If ret <> 0 Then
            Return SQLITE_CLOSE_ERROR
        End If
        jidoIdx = 0

        ret = Bt.FileLib.SQLite.btSQLiteClose(logIdx)
        If ret <> 0 Then
            Return SQLITE_CLOSE_ERROR
        End If
        logIdx = 0
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table1 クボタ照合履歴テーブル
    '''//////////////////////////////////////////////////////////
    Private Function createPoka1() As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemMAKER & "," & itemDATETIME & "," & itemTANCD & "," & itemHMCD & "," & itemTKHMCD & "," & itemQTY & "," & itemRESULT & "," & itemDLVRDT & "," & itemODRNO & "," & itemDB
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka1 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret <> 0 Then
            Return SQLITE_CREATE_TABLE_ERROR
        End If
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table2 ヤンマー照合履歴テーブル
    '''//////////////////////////////////////////////////////////
    Private Function createPoka2() As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemMAKER & "," & itemDATETIME & "," & itemTANCD & "," & itemHMCD & "," & itemTKHMCD & "," & itemQTY & "," & itemRESULT & "," & itemDLVRDT & "," & itemODRNO & "," & itemDB
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka2 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret <> 0 Then
            Return SQLITE_CREATE_TABLE_ERROR
        End If
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table3 日立照合履歴テーブル
    '''//////////////////////////////////////////////////////////
    Private Function createPoka3() As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemMAKER & "," & itemDATETIME & "," & itemTANCD & "," & itemHMCD & "," & itemTKHMCD & "," & itemQTY & "," & itemRESULT & "," & itemDLVRDT & "," & itemODRNO & "," & itemDB
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka3 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret <> 0 Then
            Return SQLITE_CREATE_TABLE_ERROR
        End If
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table4 オリエント照合履歴テーブル
    '''//////////////////////////////////////////////////////////
    Private Function createPoka4() As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemMAKER & "," & itemDATETIME & "," & itemTANCD & "," & itemHMCD & "," & itemTKHMCD & "," & itemQTY & "," & itemRESULT & "," & itemDLVRDT & "," & itemODRNO & "," & itemDB
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka4 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret <> 0 Then
            Return SQLITE_CREATE_TABLE_ERROR
        End If
        Return SQLITE_OK
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Create Table5 棚番照合履歴テーブル
    '''//////////////////////////////////////////////////////////
    Private Function createPoka5() As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim columns As [String] = itemDATETIME & ", " & itemTANCD & ", " & itemTANACD & "," & itemHMCD & ", " & itemRESULT
        Dim sql As New StringBuilder("CREATE TABLE IF NOT EXISTS Poka5 (" & columns & ");")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
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
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim sql As New StringBuilder("SELECT COUNT(*) FROM " & tableName & " where 社内品番 not like 'PROOFREAD%';") ' 24.09.03 mod y.w 朝一校正は件数にカウントしない
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(logIdx)
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
        If logIdx <= 0 Then
            Return bret
        End If
        Dim sql As New StringBuilder("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '" & tableName & "';")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(logIdx)
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
    Public Function insertPokaX(ByVal tableName As String, ByVal rec As DBPokaRecord) As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim val As String = rec.MAKER & "','" & rec.DATATIME & "','" & rec.TANCD & "','" & rec.HMCD & "','" & rec.TKHMCD & "','" & rec.QTY & "','" & rec.RESULT & "','" & rec.DLVRDT & "','" & rec.ODRNO & "','" & rec.DATABASE
        Dim sql As New StringBuilder("INSERT INTO " & tableName & " VALUES('" & val & "');")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret <> 0 Then
            insertPokaX = SQLITE_INSERT_ERROR
            setSQLErrorString()
        Else
            insertPokaX = SQLITE_OK
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Insert
    '''//////////////////////////////////////////////////////////
    Public Function insertTana(ByVal tableName As String, ByVal rec As DBTanaRecord) As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim val As String = rec.DATATIME & "', '" & rec.TANCD & "', '" & rec.TANACD & "', '" & rec.HMCD & "', '" & rec.RESULT
        Dim sql As New StringBuilder("INSERT INTO " & tableName & " VALUES('" & val & "');")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret <> 0 Then
            insertTana = SQLITE_INSERT_ERROR
            setSQLErrorString()
        Else
            insertTana = SQLITE_OK
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Delete
    '''//////////////////////////////////////////////////////////
    Public Function deletePokaX(ByVal tableName As String) As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim sql As New StringBuilder("DELETE FROM " & tableName & ";")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret <> 0 Then
            deletePokaX = SQLITE_DELETE_ERROR
            setSQLErrorString()
        Else
            deletePokaX = SQLITE_OK
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Delete
    '''//////////////////////////////////////////////////////////
    Public Function deleteTana(ByVal tableName As String) As Int32
        If logIdx <= 0 Then
            Return SQLITE_NOTOPEN_ERROR
        End If
        Dim sql As New StringBuilder("DELETE FROM " & tableName & ";")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret <> 0 Then
            deleteTana = SQLITE_DELETE_ERROR
            setSQLErrorString()
        Else
            deleteTana = SQLITE_OK
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' View SQL Error
    '''//////////////////////////////////////////////////////////
    Private Sub setSQLErrorString()
        Dim msg As String = ""
        If logIdx > 0 Then

            Dim lasterr As Integer = Bt.FileLib.SQLite.btSQLiteGetErrorCode(logIdx)

            Dim data As IntPtr
            data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
            Bt.FileLib.SQLite.btSQLiteGetErrorStr(logIdx, data, 8192)

            Dim data2 As [String] = Marshal.PtrToStringUni(data)
            Marshal.FreeCoTaskMem(data)

            sqliteErrorString = data2 & vbCrLf & "ErrorCode:" & lasterr.ToString()

        End If
    End Sub

    '''//////////////////////////////////////////////////////////
    ''' View SQL Error
    '''//////////////////////////////////////////////////////////
    Private Sub setDBSQLErrorString()
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
    ''' View SQL Error (JIDO)
    '''//////////////////////////////////////////////////////////
    Private Sub setJidoSQLErrorString()
        Dim msg As String = ""
        If jidoIdx > 0 Then

            Dim lasterr As Integer = Bt.FileLib.SQLite.btSQLiteGetErrorCode(jidoIdx)

            Dim data As IntPtr
            data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
            Bt.FileLib.SQLite.btSQLiteGetErrorStr(jidoIdx, data, 8192)

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
        dt.Columns.Add(New DataColumn(itemMAKER))   ' 2024.09.29 add y.w
        dt.Columns.Add(New DataColumn(itemDATETIME))
        dt.Columns.Add(New DataColumn(itemHMCD))
        dt.Columns.Add(New DataColumn(itemQTY))     ' 2024.05.30 add y.w
        dt.Columns.Add(New DataColumn(itemRESULT))
        dt.Columns.Add(New DataColumn(itemDLVRDT))  ' 2024.11.16 add y.w
        dt.Columns.Add(New DataColumn(itemODRNO))   ' 2024.12.05 add y.w
        dt.Columns.Add(New DataColumn(itemDB))      ' 2024.09.29 add y.w

        If logIdx <= 0 Then
            Return dt
        End If
        If checkTableExists(tableName) = False Then
            Return dt
        End If

        ' SQLite sreftime フォーマット %Y年 %m月 %d日 %H時 %M分 %S秒 as 照合日付 (旧：'%d日%H:%M')DataGrid列タイトルは[as ...]では変わらなかった
        ' 品番は15桁で切るとListViewに上手く表示される
        Dim hmcd As String = "CASE データベース WHEN 'OK' THEN '◎' WHEN 'NG' THEN '×' WHEN 'NONTARGET' THEN '△' WHEN 'WAIT' THEN '待' ELSE '' END||substr(社内品番,1,15) as 社内品番"
        Dim sql As New StringBuilder("SELECT ROWID, メーカー, strftime('%H%M', 照合日付)," & hmcd & ",数量,照合結果,納期,注番,データベース FROM " & tableName & " order by 照合日付 desc;")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(logIdx)
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
    ''' Update 数量変更
    '''//////////////////////////////////////////////////////////
    Public Function updatePokaXMeisai(ByVal tableName As String, ByVal _rowid As Integer, ByVal _qty As String, ByVal _newstatus As String) As Boolean
        If logIdx <= 0 Then
            Return False
        End If
        If checkTableExists(tableName) = False Then
            Return False
        End If

        Dim sql As New StringBuilder("UPDATE " & tableName & " SET " & itemQTY & "='" & _qty & "'," & itemDB & "='" & _newstatus & "' WHERE ROWID = " & _rowid & ";")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret = 0 Then
            Return True
        Else
            setSQLErrorString()
            MessageBox.Show("更新失敗:" & ret & vbCr & vbLf & _
                            sql.ToString() & vbCr & vbLf & _
                            sqliteErrorString)
            Return False
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Update
    '''//////////////////////////////////////////////////////////
    Public Function updatePokaXDatabase(ByVal tableName As String, ByVal iRec As DBPokaRecord, ByVal iNewStatus As String) As Boolean
        If logIdx <= 0 Then
            Return False
        End If
        If checkTableExists(tableName) = False Then
            Return False
        End If

        Dim wWhere As String
        wWhere = itemDATETIME & "='" & iRec.DATATIME & "' and " & _
                itemMAKER & "='" & iRec.MAKER & "' and " & _
                itemTANCD & "='" & iRec.TANCD & "' and " & _
                itemHMCD & "='" & iRec.HMCD & "' and " & _
                itemQTY & "='" & iRec.QTY & "' and " & _
                itemDLVRDT & "='" & iRec.DLVRDT & "' and " & _
                itemDB & "='WAIT';"
        Dim sql As New StringBuilder("UPDATE " & tableName & " SET " & itemDB & "='" & iNewStatus & "' WHERE " & wWhere)
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret = 0 Then
            Return True
        Else
            setSQLErrorString()
            MessageBox.Show("更新失敗:" & ret & vbCr & vbLf & _
                            sql.ToString() & vbCr & vbLf & _
                            sqliteErrorString)
            Return False
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' Delete
    '''//////////////////////////////////////////////////////////
    Public Function deletePokaXMeisai(ByVal tableName As String, ByVal _rowid As Integer) As Boolean
        If logIdx <= 0 Then
            Return False
        End If
        If checkTableExists(tableName) = False Then
            Return False
        End If

        Dim sql As New StringBuilder("DELETE FROM " & tableName & " WHERE ROWID = " & _rowid & ";")
        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteExecute(logIdx, sql)
        If ret = 0 Then
            Return True
        Else
            setSQLErrorString()
            MessageBox.Show("削除失敗:" & ret & vbCr & vbLf & _
                            sql.ToString() & vbCr & vbLf & _
                            sqliteErrorString)
            Return False
        End If
    End Function

    '''//////////////////////////////////////////////////////////
    ''' select WaitRec Waitレコードを取得し返却
    '''//////////////////////////////////////////////////////////
    Public Function selectPokaWait() As DBPokaRecord()

        Dim rec() As DBPokaRecord
        rec = Nothing
        If logIdx <= 0 Then
            Return rec
        End If
        If checkTableExists(tblNamePoka1) = False Then
            Return rec
        End If

        Dim sql As New StringBuilder("SELECT メーカー,照合日付,担当者,社内品番,社外品番,数量,照合結果,納期,注番,データベース FROM " & tblNamePoka1 & " where データベース='WAIT';")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(logIdx)
        If cIdx <= 0 Then
            MessageBox.Show("ERROR btSQLiteCmdCreate:" & cIdx)
            Return rec
        End If

        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            setSQLErrorString()
            MessageBox.Show(sqliteErrorString)
            MessageBox.Show("ERROR btSQLiteCmdSetCommandText:" & ret & vbCr & vbLf & sql.ToString())
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)

        Dim reccnt As Integer = 0
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
                ReDim Preserve rec(reccnt)
                Dim i As Integer
                For i = 0 To 9
                    Dim data As IntPtr
                    data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
                    Dim ret2 As Integer = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, i, data, 8192)
                    If ret2 <> 0 Then
                        MessageBox.Show("ERROR btSQLiteCmdGetValue:" & ret2)
                        Marshal.FreeCoTaskMem(data)
                        GoTo FUNCEND
                    End If
                    Select Case i
                        Case 0 : rec(reccnt).MAKER = Marshal.PtrToStringUni(data)
                        Case 1 : rec(reccnt).DATATIME = Marshal.PtrToStringUni(data)
                        Case 2 : rec(reccnt).TANCD = Marshal.PtrToStringUni(data)
                        Case 3 : rec(reccnt).HMCD = Marshal.PtrToStringUni(data)
                        Case 4 : rec(reccnt).TKHMCD = Marshal.PtrToStringUni(data)
                        Case 5 : rec(reccnt).QTY = Marshal.PtrToStringUni(data)
                        Case 6 : rec(reccnt).RESULT = Marshal.PtrToStringUni(data)
                        Case 7 : rec(reccnt).DLVRDT = Marshal.PtrToStringUni(data)
                        Case 8 : rec(reccnt).ODRNO = Marshal.PtrToStringUni(data)
                        Case 9 : rec(reccnt).DATABASE = Marshal.PtrToStringUni(data)
                    End Select
                    Marshal.FreeCoTaskMem(data)
                Next
                reccnt += 1
            End If
        Loop While ret = 1

FUNCEND:

        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        selectPokaWait = rec
    End Function

    '''//////////////////////////////////////////////////////////
    ''' CSV output
    '''//////////////////////////////////////////////////////////
    Public Function SQLite2CSV(ByVal tblName As String) As Integer
        Dim ret As Integer = -1
        Dim cIdx As Integer = 0
        If logIdx <= 0 Then
            Return ret
        End If

        Dim sql = New StringBuilder((Convert.ToString("SELECT * FROM ") & tblName) & ";")
        cIdx = Bt.FileLib.SQLite.btSQLiteCmdCreate(logIdx)
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

    '''//////////////////////////////////////////////////////////
    ''' 得意先品番取得
    '''//////////////////////////////////////////////////////////
    Public Function getTKHMCD(ByVal _HMCD As String, ByVal _TKHMCD As String) As String

        Dim tkhmcd As [String] = ""

        ' SQLite sreftime フォーマット %Y年 %m月 %d日 %H時 %M分 %S秒 as 照合日付
        Dim sql As New StringBuilder("SELECT TKHMCD FROM M0600 WHERE HMCD='" & _HMCD & "';")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(dbIdx)
        If cIdx <= 0 Then
            MessageBox.Show("ERROR M0600 btSQLiteCmdCreate:" & cIdx)
            Return ""
        End If

        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            setDBSQLErrorString()
            MessageBox.Show(sqliteErrorString)
            MessageBox.Show("ERROR M0600 btSQLiteCmdSetCommandText:" & ret & vbCr & vbLf & sql.ToString())
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)
        Do
            ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
            If ret = 1 Then ' データあり

                Dim data As IntPtr
                data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
                Dim ret2 As Integer = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, 0, data, 8192)
                If ret2 <> 0 Then
                    MessageBox.Show("ERROR btSQLiteCmdGetValue:" & ret2)
                    Marshal.FreeCoTaskMem(data)
                    GoTo FUNCEND
                End If
                tkhmcd = Marshal.PtrToStringUni(data)
                Marshal.FreeCoTaskMem(data)

                ' マスタ変換した値が、入力得意先品番内に存在してたらループを抜ける
                If InStr(_TKHMCD.Replace("-", ""), tkhmcd.Replace("-", "")) > 0 Then ' 23.10.25
                    Exit Do
                End If

            ElseIf ret = 0 Then 'データが無くなったもしくは無い場合終了
                GoTo FUNCEND

            ElseIf ret < 0 Then
                MessageBox.Show("ERROR btSQLiteCmdRead:" & ret & vbCr & vbLf & sql.ToString())
                GoTo FUNCEND
            End If
        Loop While ret = 1

FUNCEND:
        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        Return tkhmcd
    End Function

    '''//////////////////////////////////////////////////////////
    ''' 得意先コード取得
    '''//////////////////////////////////////////////////////////
    Public Function getTKCD(ByVal _HMCD As String) As String

        Dim tkcd As [String] = ""

        ' SQLite sreftime フォーマット %Y年 %m月 %d日 %H時 %M分 %S秒 as 照合日付
        Dim sql As New StringBuilder("SELECT TKCD FROM M0500 WHERE HMCD='" & _HMCD & "';")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(dbIdx)
        If cIdx <= 0 Then
            MessageBox.Show("ERROR M0500 btSQLiteCmdCreate:" & cIdx)
            Return ""
        End If

        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            setDBSQLErrorString()
            MessageBox.Show(sqliteErrorString)
            MessageBox.Show("ERROR M0500 btSQLiteCmdSetCommandText:" & ret & vbCr & vbLf & sql.ToString())
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)
        Do
            ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
            If ret = 1 Then ' データあり

                Dim data As IntPtr
                data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
                Dim ret2 As Integer = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, 0, data, 8192)
                If ret2 <> 0 Then
                    MessageBox.Show("ERROR btSQLiteCmdGetValue:" & ret2)
                    Marshal.FreeCoTaskMem(data)
                    GoTo FUNCEND
                End If
                tkcd = Marshal.PtrToStringUni(data)
                Marshal.FreeCoTaskMem(data)

                Exit Do

            ElseIf ret = 0 Then 'データが無くなったもしくは無い場合終了
                GoTo FUNCEND

            ElseIf ret < 0 Then
                MessageBox.Show("ERROR btSQLiteCmdRead:" & ret & vbCr & vbLf & sql.ToString())
                GoTo FUNCEND

            End If
        Loop While ret = 1

FUNCEND:
        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        Return tkcd
    End Function

    '''//////////////////////////////////////////////////////////
    ''' 自動倉庫棚番チェック
    '''//////////////////////////////////////////////////////////
    Public Function checkBUCD(ByVal _BUCD As String) As Boolean
        Dim check As Boolean = False

        ' SQLite
        Dim sql As New StringBuilder("SELECT * FROM pkdat WHERE field1='" & _BUCD & "';")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(jidoIdx)
        If cIdx <= 0 Then
            MessageBox.Show("ERROR JIDO btSQLiteCmdCreate:" & cIdx)
            Return False
        End If

        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            setJidoSQLErrorString()
            MessageBox.Show(sqliteErrorString)
            MessageBox.Show("ERROR JIDO btSQLiteCmdSetCommandText:" & ret & vbCr & vbLf & sql.ToString())
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)
        Do
            ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
            If ret = 1 Then ' データあり
                check = True
                Exit Do

            ElseIf ret = 0 Then 'データが無くなったもしくは無い場合終了
                GoTo FUNCEND

            ElseIf ret < 0 Then
                MessageBox.Show("ERROR btSQLiteCmdRead:" & ret & vbCr & vbLf & sql.ToString())
                GoTo FUNCEND

            End If
        Loop While ret = 1

FUNCEND:
        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        Return check
    End Function

    '''//////////////////////////////////////////////////////////
    ''' 自動倉庫マスタチェック
    '''//////////////////////////////////////////////////////////
    Public Function checkJIDO(ByVal _BUCD As String, ByVal _HMCD As String) As Boolean
        Dim check As Boolean = False

        ' SQLite
        Dim sql As New StringBuilder("SELECT * FROM pkdat WHERE " & _
            "field1='" & _BUCD & "' and field2='" & _HMCD & "';")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(jidoIdx)
        If cIdx <= 0 Then
            MessageBox.Show("CmdCreate ERROR [shelfstock.pkdat]")
            Return False
        End If

        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            setJidoSQLErrorString()
            MessageBox.Show("CmdSetCommandText ERROR [shelfstock.pkdat]")
            MessageBox.Show(sqliteErrorString)
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)
        Do
            ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
            If ret = 1 Then ' データあり
                check = True
                Exit Do

            ElseIf ret = 0 Then 'データが無くなったもしくは無い場合終了
                checkJIDO = False
                GoTo FUNCEND

            ElseIf ret < 0 Then
                MessageBox.Show("ERROR btSQLiteCmdRead:" & ret & vbCr & vbLf & sql.ToString())
                GoTo FUNCEND

            End If
        Loop While ret = 1

FUNCEND:
        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        Return check
    End Function

    '''//////////////////////////////////////////////////////////
    ''' 紐色＆数取得
    '''//////////////////////////////////////////////////////////
    Public Function getM0500(ByVal iHMCD As String, _
        ByRef oSKHIASU As String, ByRef oCOLOR As String, ByRef oSU As String) As Boolean

        Dim wFlg As Boolean

        ' SQLite sreftime フォーマット %Y年 %m月 %d日 %H時 %M分 %S秒 as 照合日付
        Dim sql As New StringBuilder("SELECT SKHIASU, IFNULL(COLOR,'-') AS COLOR, IFNULL(SU,0) AS SU FROM M0500 WHERE HMCD='" & iHMCD & "';")
        Dim cIdx As Integer = Bt.FileLib.SQLite.btSQLiteCmdCreate(dbIdx)
        If cIdx <= 0 Then
            MessageBox.Show("ERROR M0500 btSQLiteCmdCreate:" & cIdx)
            Return False
        End If

        Dim ret As Integer = Bt.FileLib.SQLite.btSQLiteCmdSetCommandText(cIdx, sql)
        If ret <> 0 Then
            setDBSQLErrorString()
            MessageBox.Show(sqliteErrorString)
            MessageBox.Show("ERROR M0500 btSQLiteCmdSetCommandText:" & ret & vbCr & vbLf & sql.ToString())
            GoTo FUNCEND
        End If

        ret = Bt.FileLib.SQLite.btSQLiteCmdExecuteReader(cIdx)
        Do
            ret = Bt.FileLib.SQLite.btSQLiteCmdRead(cIdx)
            If ret = 1 Then ' データあり

                Dim data As IntPtr
                Dim ret2 As Integer

                ' HIASU取得
                data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
                ret2 = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, 0, data, 8192)
                If ret2 <> 0 Then
                    MessageBox.Show("ERROR btSQLiteCmdGetValue(0):" & ret2)
                    Marshal.FreeCoTaskMem(data)
                    GoTo FUNCEND
                End If
                oSKHIASU = Marshal.PtrToStringUni(data)
                Marshal.FreeCoTaskMem(data)

                ' COLOR取得
                data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
                ret2 = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, 1, data, 8192)
                If ret2 <> 0 Then
                    MessageBox.Show("ERROR btSQLiteCmdGetValue(1):" & ret2)
                    Marshal.FreeCoTaskMem(data)
                    GoTo FUNCEND
                End If
                oCOLOR = Marshal.PtrToStringUni(data)
                Marshal.FreeCoTaskMem(data)

                ' SU取得
                data = Marshal.AllocCoTaskMem(Marshal.SizeOf(GetType([Char])) * (8192 + 1))
                ret2 = Bt.FileLib.SQLite.btSQLiteCmdGetValue(cIdx, 2, data, 8192)
                If ret2 <> 0 Then
                    MessageBox.Show("ERROR btSQLiteCmdGetValue(2):" & ret2)
                    Marshal.FreeCoTaskMem(data)
                    GoTo FUNCEND
                End If
                oSU = Marshal.PtrToStringUni(data)
                Marshal.FreeCoTaskMem(data)

                wFlg = True
                Exit Do

            ElseIf ret = 0 Then 'データが無くなったもしくは無い場合終了
                GoTo FUNCEND

            ElseIf ret < 0 Then
                MessageBox.Show("ERROR btSQLiteCmdRead:" & ret & vbCr & vbLf & sql.ToString())
                GoTo FUNCEND

            End If
        Loop While ret = 1

FUNCEND:
        Bt.FileLib.SQLite.btSQLiteCmdDelete(cIdx)
        Return wFlg
    End Function

End Module

