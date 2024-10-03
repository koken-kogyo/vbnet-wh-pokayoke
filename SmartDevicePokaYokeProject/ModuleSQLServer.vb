Imports System.Runtime.InteropServices
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

    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ' 出荷指示テーブル更新
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Public Function UpdateKD8330(ByVal iTKCD As String, ByVal iHMCD As String, _
                                 ByVal iBADQTY As Integer, ByVal iMODQTY As Integer, _
                                 ByVal iTANCD As String) As String

        Dim stConnectionString As String = _
            "Data Source=" & mSQLServer & "\KOKEN;" & _
            "Initial Catalog=KOKEN;" & _
            "Integrated Security=False;" & _
            "User Id = KOKEN_1;" & _
            "Password = KOKEN_1;" & _
            "Connection Timeout=5"

        Dim cSqlConnection As New System.Data.SqlClient.SqlConnection(stConnectionString)
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
                    wSQL = "select top 1 AUTONO,ODRQTY,HTJUQTY from KD8330 where (TKHMCD='" & iHMCD & "' or HMCD='" & iHMCD & "') " & _
                            "and ODRQTY<>HTJUQTY and SHIPDT>=convert(date,getdate())" & _
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
                    wSQL = "select top 1 AUTONO,HTJUQTY from KD8330 where (TKHMCD='" & iHMCD & "' or HMCD='" & iHMCD & "') " & _
                            "and HTJUQTY>0 and SHIPDT>=convert(date,getdate()) " & _
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

    Public Function checkSQLServer() As Boolean
        Dim stConnectionString As String = _
            "Data Source=" & mSQLServer & "\KOKEN;" & _
            "Initial Catalog=KOKEN;" & _
            "Integrated Security=False;" & _
            "User Id = KOKEN_1;" & _
            "Password = KOKEN_1;" & _
            "Connection Timeout=5"
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

End Module
