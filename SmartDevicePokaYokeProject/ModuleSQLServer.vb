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
    Public mKD8330Mode As String = ""   ' 出荷指示書システム用フラグ(Form1を抜けても保持されるようグローバル変数にする)
    '   ""          : ローカル品番照合モード
    '   CHECKING    : SQLServerデータベースの確認中
    '   TROUBLE     : WiFiもしくはSQLServerデータベースのエラーで中断中
    '   上記以外    : 出荷指示モード
    Public mKD8330dt As DataTable       ' 出荷指示テーブルをデータテーブルに保持して運用

    ' 出荷指示テーブル初期化 ver.24.11.04
    Public Sub createKD8330()
        If mKD8330dt Is Nothing Then
            mKD8330dt = New DataTable("KD8330")
            With mKD8330dt
                .Columns.Add(New DataColumn("TKCD"))
                .Columns.Add(New DataColumn("DLVRDT"))
                .Columns.Add(New DataColumn("NO"))
                .Columns.Add(New DataColumn("TKHMCD"))
                .Columns.Add(New DataColumn("HMCD"))
                .Columns.Add(New DataColumn("ODRQTY"))
                .Columns.Add(New DataColumn("INSUU"))
                .Columns.Add(New DataColumn("ODRNO"))
                .Columns.Add(New DataColumn("HTTANCD"))
                .Columns.Add(New DataColumn("HTJUDT"))
                .Columns.Add(New DataColumn("HTJUQTY"))
            End With
        End If
    End Sub

    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ' 出荷指示テーブル取得 ver.24.11.04 y.w
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Public Function getKD8330() As Boolean

        Dim cSqlConnection As New System.Data.SqlClient.SqlConnection(getConnectionString(3))
        Dim hCommand As New System.Data.SqlClient.SqlCommand()

        Try
            ' データベースオープン
            cSqlConnection.Open()

            Dim sr As System.Data.SqlClient.SqlDataReader
            Dim wSQL As String = ""

            hCommand = cSqlConnection.CreateCommand()
            hCommand.CommandTimeout = 20

            ' 出荷指示テーブル
            mKD8330dt.Rows.Clear()

            ' 出荷指示書テーブルを抽出
            '   Debug用 ⇒ convert(date,getdate()-7)
            wSQL = "select TKCD,convert(nvarchar, DLVRDT, 111) 'DLVRDT',NO,TKHMCD,HMCD,ODRQTY,INSUU" & _
                    ",ODRNO,HTTANCD,HTJUDT,HTJUQTY " & _
                    "from KD8330 where SHIPDT>=convert(date,getdate())" & _
                    "order by TKCD,SHIPDT,NO asc"
            hCommand.CommandText = wSQL
            sr = hCommand.ExecuteReader()
            While sr.Read
                Dim dr As DataRow
                dr = mKD8330dt.NewRow()
                dr(0) = sr.Item("TKCD")
                dr(1) = sr.Item("DLVRDT")
                dr(2) = sr.Item("NO")
                dr(3) = sr.Item("TKHMCD")
                dr(4) = sr.Item("HMCD")
                dr(5) = sr.Item("ODRQTY")
                dr(6) = sr.Item("INSUU")
                dr(7) = sr.Item("ODRNO")
                dr(8) = sr.Item("HTTANCD")
                dr(9) = sr.Item("HTJUDT")
                dr(10) = sr.Item("HTJUQTY")
                mKD8330dt.Rows.Add(dr)
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
        Dim stConnectionString As String = getConnectionString(3)
        Dim cSqlConnection As New System.Data.SqlClient.SqlConnection(stConnectionString)
        Dim hCommand As New System.Data.SqlClient.SqlCommand()
        Try
            cSqlConnection.Open()
            cSqlConnection.Close()
            cSqlConnection.Dispose()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ' 別スレッドにてチェックを行う
    Public Sub checkSQLServer2()
        Dim stConnectionString As String = getConnectionString(3)
        Dim cSqlConnection As New System.Data.SqlClient.SqlConnection(stConnectionString)
        Dim hCommand As New System.Data.SqlClient.SqlCommand()
        Try
            cSqlConnection.Open()
            cSqlConnection.Close()
            cSqlConnection.Dispose()
            mKD8330Mode = "SQLSERVER"
        Catch ex As Exception
            ' 親スレッドが、ここの処理前に閉じられてしまっていた場合は
            ' 出荷指示モードの変更を行わない
            If mKD8330Mode = "CHECKING" Then mKD8330Mode = "TROUBLE"
        End Try
    End Sub

    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ' 出荷指示テーブル更新
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Public Function UpdateKD8330(ByVal iTKCD As String, _
                                 ByVal iDLVRDT As String, _
                                 ByVal iODRNO As String, _
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
                    If iODRNO <> "" Then
                        wSQL = "select top 1 AUTONO,ODRQTY,HTJUQTY from KD8330 where " & _
                                "ODRNO like '" & iODRNO & "%' " & _
                                "order by SHIPDT asc,NO asc"
                    Else
                        wSQL = "select top 1 AUTONO,ODRQTY,HTJUQTY from KD8330 where " & _
                                "TKCD='" & iTKCD & "' and " & _
                                "DLVRDT='" & iDLVRDT & "' and " & _
                                "(TKHMCD='" & iHMCD & "' or HMCD='" & iHMCD & "') and " & _
                                "ODRQTY<>HTJUQTY " & _
                                "order by SHIPDT asc,NO asc"
                    End If
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

                    wSQL = "update KD8330 set HTTANCD='" & iTANCD & "',HTJUDT=getdate()"
                    If wOrder - wComp = 0 Then
                        ' 更新対象がなくなってしまった場合は強制出荷準備完了にしてループを抜ける（無限ループになってしまった）
                        hCommand.CommandText = wSQL & ",SHIPSTS='5' where AUTONO=" & wAutoNo
                        wQTY = 0
                    ElseIf (wOrder - wComp) <= wQTY Then
                        ' 指示数-出荷準備数 <= 数量 ・・・ 指示数でDB更新、
                        ' 数量から(指示数-出荷準備数)を引く
                        hCommand.CommandText = wSQL & ",HTJUQTY=" & wOrder & ",SHIPSTS='4' where AUTONO=" & wAutoNo
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
                Dim wCancelQty As Integer = iBADQTY - iMODQTY
                Do Until wCancelQty = 0

                    Dim wAutoNo As Long = 0
                    Dim wHTQty As Integer = 0

                    ' 出荷日が当日以上で出荷準備数が存在するレコードを1件だけ抽出
                    ' select 自動採番,出荷準備数 from KD8330 where 得意先品番='a' or 社内品番='a'
                    If iODRNO <> "" Then
                        wSQL = "select top 1 AUTONO,HTJUQTY from KD8330 where " & _
                                "ODRNO like '" & iODRNO & "%'" & _
                                "order by SHIPDT desc,NO desc"
                    Else
                        wSQL = "select top 1 AUTONO,HTJUQTY from KD8330 where " & _
                                "TKCD='" & iTKCD & "' and " & _
                                "DLVRDT='" & iDLVRDT & "' and " & _
                                "(TKHMCD='" & iHMCD & "' or HMCD='" & iHMCD & "') and " & _
                                "HTJUQTY>0 " & _
                                "order by SHIPDT desc,NO desc"
                    End If
                    hCommand.CommandText = wSQL
                    sr = hCommand.ExecuteReader()
                    While sr.Read
                        wAutoNo = sr.Item("AUTONO")
                        wHTQty = sr.Item("HTJUQTY")
                    End While
                    sr.Close()
                    If wAutoNo = 0 Then
                        Throw New ApplicationException("NonTargetException")
                    End If

                    wSQL = "update KD8330 set "
                    If wHTQty = 0 Then
                        Exit Do ' これ以上、出荷準備数量を引けない状態
                    ElseIf wHTQty <= wCancelQty Then
                        ' 出荷準備数 <= 取消数量 ・・・ 数量0でDB更新、
                        ' 数量から(指示数-出荷準備数)を引く
                        hCommand.CommandText = wSQL & "HTTANCD=null,HTJUDT=null,HTJUQTY=0,SHIPSTS='1'" & _
                            " where AUTONO=" & wAutoNo
                        wCancelQty = wCancelQty - wHTQty
                    Else
                        ' 出荷準備数  > 取消数量 ・・・ 出荷準備数－数量で更新
                        ' 数量を0
                        hCommand.CommandText = wSQL & "HTTANCD='" & iTANCD & "'" & _
                            ",HTJUDT=getdate(),HTJUQTY=HTJUQTY-" & wCancelQty & _
                            ",SHIPSTS='1' where AUTONO=" & wAutoNo
                        wCancelQty = 0
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

    ' 出荷指示書(dt)上の「7:注文番号」もしくは「社内品番」を検索
    ' 得意先コードが１件に特定された場合のみ得意先コードを文字列で返却
    Public Function getKD8330dtTKCDfromODRNO(ByVal iODRNO As String) As String
        Dim tkcd() As String
        tkcd = mKD8330dt.AsEnumerable _
                .Where(Function(r) r("ODRNO").ToString().StartsWith(iODRNO) And r("TKCD").ToString() <> "XXXXX") _
                .Select(Function(c) c("TKCD").ToString()) _
                .ToArray()
        If tkcd.Length = 1 Then
            Return tkcd(0)
        Else
            Return "" ' 0件もしくは複数件Hitした場合、空白を返却
        End If
    End Function

    ' 出荷指示書(dt)上の「得意先品番」「社内品番」を検索
    ' (未使用) Linq GroupBy の検証用
    Public Function getKD8330dtTKCDfromHMCD(ByVal iHMCD As String) As String()
        Dim tkcd() As String
        tkcd = mKD8330dt.AsEnumerable _
                .Where(Function(r) _
                    Replace(r("TKHMCD").ToString(), "-", "") = Replace(iHMCD, "-", "") Or _
                    Replace(r("HMCD").ToString(), "-", "") = Replace(iHMCD, "-", "")) _
                .GroupBy(Function(g) g("TKCD").ToString()) _
                .Select(Function(c) c.Key) _
                .ToArray()
        Return tkcd
    End Function

    ' 出荷指示書(dt)上の「得意先ｺｰﾄﾞ＋得意先品番＋未完」を検索した納期を/付き10文字に変換し(た後ソートして)返却
    Public Function getKD8330dt(ByVal iTKHMCD As String) As DataTable
        Dim dt As DataTable = mKD8330dt.AsEnumerable.Where(Function(r) ( _
            Replace(r("TKHMCD").ToString(), "-", "") = iTKHMCD And _
            r("ODRQTY").ToString() <> r("HTJUQTY").ToString() _
            )).CopyToDataTable()
        ').Select(Function(r) r("DLVRDT").ToString()). '.OrderBy(Function(s) s)
        Return dt
    End Function

    ' 出荷指示書(dt)上の「7:注文番号」を検索し残りの指示数と準備済数を返却
    Public Function getKD8330dtZanQTY(ByVal iODRNO As String, ByRef oODRQTY As Integer, ByRef oHTQTY As Integer, ByRef oINSUU As Integer) As Boolean
        Dim dr() As DataRow
        dr = mKD8330dt.AsEnumerable _
            .Where(Function(r) r("ODRNO").ToString().StartsWith(iODRNO) And r("TKCD").ToString() <> "XXXXX") _
            .ToArray()
        If dr.Length = 1 Then
            oODRQTY = IIf(IsNumeric(dr(0)("ODRQTY")), Integer.Parse(dr(0)("ODRQTY")), 0)
            oHTQTY = IIf(IsNumeric(dr(0)("HTJUQTY")), Integer.Parse(dr(0)("HTJUQTY")), 0)
            oINSUU = IIf(IsNumeric(dr(0)("INSUU")), Integer.Parse(dr(0)("INSUU")), 0)
            Return True
        Else
            Return False
        End If
    End Function

    ' 出荷指示書(dt)上の「得意先品番」または「社内品番」で
    '指定出荷日を超える出荷準備済みのレコードを検索
    Public Function getKD8330dtODRNObyHTComp(ByVal iHMCD As String, ByVal iQTY As Integer, ByVal iDLVRDT As String) As DataRow()
        Dim dr() As DataRow
        dr = mKD8330dt.AsEnumerable _
            .Where(Function(r) _
                ( _
                    Replace(r("TKHMCD").ToString(), "-", "") = Replace(iHMCD, "-", "") Or _
                    Replace(r("HMCD").ToString(), "-", "") = Replace(iHMCD, "-", "") _
                ) _
                And CInt(r("HTJUQTY")) >= iQTY _
                And CDate(r("DLVRDT").ToString()) > CDate(iDLVRDT) _
            ) _
            .OrderBy(Function(c) c("DLVRDT")) _
            .ToArray()
        Return dr
    End Function

End Module
