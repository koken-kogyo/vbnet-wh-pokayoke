Imports System.Data
Imports System.Data.SqlClient.SqlCommand

Module ModuleSQLServer

    ' ***************************************************************************************************************
    '
    ' 出荷指示テーブル更新
    ' 参照元：在庫検索サンプル（無線）より [在庫確認検索(無線)サンプル_HT_1100_J]
    '
    ' Copyright (c) 2012-2013 KEYENCE CORPORATION. All rights reserved.
    '
    ' ***************************************************************************************************************
    ' あらかじめ Microsoft SQL Server Compact 3.5 Service Pack 2 をインストール
    ' 参照の追加 System.Data.SqlClient.dll [3.0.5300.0] [Microsoft SQL Server Compact Edition v3.5]
    ' 実機のモジュールと同じフォルダに[dbnetlib.dll]を入れておく
    ' （C:\Program Files\Microsoft SQL Server Compact Edition\v3.5\Devices\Client\wce500\armv4i以下）
    ' ***************************************************************************************************************

    Private Const cTitle As String = "出荷指示テーブル更新"

    ' 出荷指示モード時の保存変数 ver.24.11.04 y.w
    Public mKD8330Mode As String = ""   ' 出荷指示書システム用変数（処理モード["C0101-Y","C0105-G"]等を保持）
    Public mKD8330dt As DataTable       ' 出荷指示テーブルをデータテーブルに保持して運用
    Public mDLVRDT As String            ' 受注納期を別途保持 [yyyy/MM/dd(111)形式]（タイトル文字列、DB更新用に使用）

    ' 出荷指示テーブル初期化 ver.24.11.04
    Public Sub createKD8330()
        If mKD8330dt Is Nothing Then
            mKD8330dt = New DataTable("KD8330")
            With mKD8330dt
                .Columns.Add(New DataColumn("NO"))
                .Columns.Add(New DataColumn("TKHMCD"))
                .Columns.Add(New DataColumn("HMCD"))
                .Columns.Add(New DataColumn("ODRQTY"))
                .Columns.Add(New DataColumn("INSUU"))
                .Columns.Add(New DataColumn("HTTANCD"))
                .Columns.Add(New DataColumn("HTJUDT"))
                .Columns.Add(New DataColumn("HTJUQTY"))
            End With
        End If
    End Sub

    Private Function getConnectionString(ByVal iSec As UInt16) As String
        getConnectionString = _
            "Data Source=" & mSQLServer & "\KOKEN;" & _
            "Initial Catalog=KOKEN;" & _
            "Integrated Security=False;" & _
            "User Id = KOKEN_1;" & _
            "Password = KOKEN_1;" & _
            "Connection Timeout=" & iSec
    End Function

    Public Function checkSQLServer() As Boolean
        Dim stConnectionString As String = getConnectionString(1)
        Dim cSqlConnection As New System.Data.SqlClient.SqlConnection(stConnectionString)
        Try
            cSqlConnection.Open()
            cSqlConnection.Close()
            cSqlConnection.Dispose()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ' 出荷指示テーブル更新
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Public Function UpdateKD8330(ByVal iDLVRDT As String, _
                                 ByVal iTKCD As String, _
                                 ByVal iHMCD As String, _
                                 ByVal iBADQTY As Integer, _
                                 ByVal iMODQTY As Integer, _
                                 ByVal iTANCD As String) As String

        Dim cSqlConnection As New System.Data.SqlClient.SqlConnection(getConnectionString(3))
        Dim hCommand As New System.Data.SqlClient.SqlCommand()

        Try
            ' データベースオープン
            cSqlConnection.Open()

            ' 出荷準備数が０になるまでループ
            Dim sr As System.Data.SqlClient.SqlDataReader
            Dim wSQL As String = ""

            hCommand = cSqlConnection.CreateCommand()
            hCommand.Transaction = cSqlConnection.BeginTransaction
            hCommand.CommandTimeout = 20

            ' 変更後数量が増えるパターンの場合
            If iBADQTY < iMODQTY Then
                Dim wQTY As Integer = iMODQTY - iBADQTY
                Do Until wQTY <= 0

                    Dim wAutoNo As Long = 0
                    Dim wOrder As Integer = 0
                    Dim wComp As Integer = 0

                    ' 出荷日が当日以上で指示数と出荷準備数が相違するレコードを1件だけ抽出
                    ' select 自動採番,指示数,出荷準備数 from KD8330 where 得意先品番='a' or 社内品番='a'
                    wSQL = "select top 1 AUTONO,ODRQTY,HTJUQTY from KD8330 where " & _
                            "DLVRDT='" & iDLVRDT & "' " & _
                            "and TKCD='" & iTKCD & "' " & _
                            "and (TKHMCD='" & iHMCD & "' or HMCD='" & iHMCD & "') " & _
                            "and ODRQTY<>HTJUQTY " & _
                            "order by SHIPDT asc,NO asc"
                    hCommand.CommandText = wSQL
                    sr = hCommand.ExecuteReader()
                    While sr.Read
                        wAutoNo = sr.Item("AUTONO")
                        wOrder = sr.Item("ODRQTY")
                        wComp = sr.Item("HTJUQTY")
                    End While
                    sr.Close()
                    If wAutoNo = 0 Then
                        Throw New ApplicationException("NonTargetException")
                    End If

                    ' 出荷準備数の更新とw数量を減算
                    wSQL = "update KD8330 set HTTANCD='" & iTANCD & "',HTJUDT=getdate()"
                    If (wOrder - wComp) <= wQTY Then
                        ' 指示数-出荷準備数 <= 数量 ・・・ 指示数でDB更新、
                        ' 数量から(指示数-出荷準備数)を引く
                        hCommand.CommandText = wSQL & ",HTJUQTY=" & wOrder & " where AUTONO=" & wAutoNo
                        wQTY = wQTY - (wOrder - wComp)
                    Else
                        ' 指示数-出荷準備数  > 数量 ・・・ 出荷準備数＋数量で更新
                        ' 数量を0
                        hCommand.CommandText = wSQL & ",HTJUQTY=HTJUQTY+" & wQTY & " where AUTONO=" & wAutoNo
                        wQTY = 0
                    End If
                    hCommand.ExecuteNonQuery()

                Loop

            Else ' 変更後数量が減るパターンの場合
                Dim wQTY As Integer = iBADQTY - iMODQTY
                Do Until wQTY = 0

                    Dim wAutoNo As Long = 0
                    Dim wComp As Integer = 0

                    ' 出荷日が当日以上で出荷準備数が存在するレコードを1件だけ抽出
                    ' select 自動採番,出荷準備数 from KD8330 where 得意先品番='a' or 社内品番='a'
                    wSQL = "select top 1 AUTONO,HTJUQTY from KD8330 where " & _
                            "DLVRDT='" & iDLVRDT & "' " & _
                            "and TKCD='" & iTKCD & "' " & _
                            "and (TKHMCD='" & iHMCD & "' or HMCD='" & iHMCD & "') " & _
                            "and HTJUQTY>0 " & _
                            "order by SHIPDT desc,NO desc"
                    hCommand.CommandText = wSQL
                    sr = hCommand.ExecuteReader()
                    While sr.Read
                        wAutoNo = sr.Item("AUTONO")
                        wComp = sr.Item("HTJUQTY")
                    End While
                    sr.Close()
                    If wAutoNo = 0 Then
                        Throw New ApplicationException("NonTargetException")
                    End If

                    ' 出荷準備数の更新とw数量を減算
                    wSQL = "update KD8330 set "
                    If wComp <= wQTY Then
                        ' 出荷準備数 <= 数量 ・・・ 数量0でDB更新、
                        ' 数量から(指示数-出荷準備数)を引く
                        hCommand.CommandText = wSQL & "HTTANCD=null,HTJUDT=null,HTJUQTY=0" & _
                            " where AUTONO=" & wAutoNo
                        wQTY = wQTY - wComp
                    Else
                        ' 出荷準備数  > 数量 ・・・ 出荷準備数－数量で更新
                        ' 数量を0
                        hCommand.CommandText = wSQL & "HTTANCD='" & iTANCD & "'" & _
                            ",HTJUDT=getdate(),HTJUQTY=HTJUQTY-" & wQTY & _
                            " where AUTONO=" & wAutoNo
                        wQTY = 0
                    End If
                    hCommand.ExecuteNonQuery()

                Loop

            End If

            ' トランザクションコミット
            hCommand.Transaction.Commit()
            hCommand.Dispose()

            ' コネクションを閉じて開放
            cSqlConnection.Close()
            cSqlConnection.Dispose()

            Return "OK"

        Catch ex As Exception

            If cSqlConnection.State = Data.ConnectionState.Open Then
                'トランザクションロールバック
                hCommand.Transaction.Rollback()
                hCommand.Dispose()
                ' コネクションを閉じて開放
                cSqlConnection.Close()
                cSqlConnection.Dispose()
            End If

            If ex.Message = "SqlException" Then
                'MsgBox("データベース異常が発生しました" & vbCrLf & "システム担当に連絡してください", _
                '       MsgBoxStyle.Critical, cTitle)
                Return "NG"

            ElseIf ex.Message = "NonTargetException" Then
                'MsgBox("消し込み対象品番もしくは" & vbCrLf & "消し込み対象の指示数が" & vbCrLf & "見つかりませんでした．", _
                '       MsgBoxStyle.Exclamation, cTitle)
                Return "NONTARGET"

            Else
                'MsgBox(ex.Message, MsgBoxStyle.Critical, cTitle)
                Return "NG"
            End If

        End Try

    End Function

    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ' 出荷指示テーブル取得 ver.24.11.04 y.w
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Public Function getKD8330(ByVal iTKCD As String, ByVal iMode As String) As Boolean

        Dim cSqlConnection As New System.Data.SqlClient.SqlConnection(getConnectionString(3))
        Dim hCommand As New System.Data.SqlClient.SqlCommand()

        Try
            ' データベースオープン
            cSqlConnection.Open()

            Dim sr As System.Data.SqlClient.SqlDataReader
            Dim wSQL As String = ""

            hCommand = cSqlConnection.CreateCommand()
            hCommand.CommandTimeout = 20

            ' カレンダーマスタより稼働日を取得
            wSQL = "select TOP 4 YMD from (" & _
                    "select convert(date,getdate()) 'YMD' union " & _
                    "select YMD from S0820 where CALTYP='00001' and WKKBN='1' " & _
                    "and YMD between convert(date,getdate()) and dateadd(day, 14, getdate()) " & _
                    ") S0820 order by YMD asc"
            hCommand.CommandText = wSQL
            sr = hCommand.ExecuteReader()
            Dim wTargetDate As Date
            Dim wRow As Integer = 0
            While sr.Read
                If iMode = "Y" And wRow = 0 Then wTargetDate = sr.Item("YMD")
                If iMode = "G" And wRow = 1 Then wTargetDate = sr.Item("YMD")
                If iMode = "W" And wRow = 2 Then wTargetDate = sr.Item("YMD")
                If iMode = "1W" And wRow = 3 Then wTargetDate = sr.Item("YMD")
                wRow = wRow + 1
            End While
            sr.Close()

            ' 出荷指示テーブル
            mKD8330dt.Rows.Clear()

            ' 出荷指示書テーブルを抽出
            wSQL = "select NO,TKHMCD,HMCD,ODRQTY,INSUU,HTTANCD,HTJUDT,HTJUQTY" & _
                    ",convert(nvarchar, DLVRDT, 111) 'DLVRDT' from KD8330 where " & _
                    "TKCD='" & iTKCD & "' and SHIPDT='" & wTargetDate & "' " & _
                    "order by NO asc"
            hCommand.CommandText = wSQL
            sr = hCommand.ExecuteReader()
            While sr.Read
                Dim dr As DataRow
                dr = mKD8330dt.NewRow()
                dr(0) = sr.Item("NO")
                dr(1) = sr.Item("TKHMCD")
                dr(2) = sr.Item("HMCD")
                dr(3) = sr.Item("ODRQTY")
                dr(4) = sr.Item("INSUU")
                dr(5) = sr.Item("HTTANCD")
                dr(6) = sr.Item("HTJUDT")
                dr(7) = sr.Item("HTJUQTY")
                mKD8330dt.Rows.Add(dr)
                If mKD8330dt.Rows.Count = 1 Then
                    mDLVRDT = sr.Item("DLVRDT")
                End If
            End While
            sr.Close()
            ' データベースクローズ
            cSqlConnection.Close()
            cSqlConnection.Dispose()

            getKD8330 = True

        Catch ex As Exception

            If cSqlConnection.State = Data.ConnectionState.Open Then
                hCommand.Dispose()
                ' コネクションを閉じて開放
                cSqlConnection.Close()
                cSqlConnection.Dispose()
            End If

            getKD8330 = False

        End Try

    End Function

    ' 出荷指示書システム用にSQLServerから出荷指示書データを取得 ver.24.11.04 y.w
    Public Sub refreshKD8330()
        If mKD8330Mode = "" Then Exit Sub
        Dim wTKCD As String = Split(mKD8330Mode, "-")(0)
        Dim wMode As String = Split(mKD8330Mode, "-")(1)
        Call getKD8330(wTKCD, wMode)
    End Sub

End Module
