Attribute VB_Name = "Module1"
Option Explicit

' 受信フォルダから品番照合ログファイルを月別フォルダに移動する
'    wA:ネットワーク実行 = "\\192.168.3.3(もしくはpc090n)\bt-w250\受信フォルダー\品番照合"
'    wB:ローカル実行     = "D:\Libraries\Desktop\BT-W250\受信フォルダー\品番照合"
Public Sub moveReceiveCSV()
    Dim wMyPath As String
    Dim wReceive As String
    
    wMyPath = ThisWorkbook.Path
    wReceive = Left(wMyPath, InStr(wMyPath, "受信フォルダー") + 7)
    
    Dim wFN() As String
    Dim wStr As String
    Dim wTarget As String
    Dim wI As Integer
    
    ' 対象を配列に格納（途中でDir関数が使えない為）
    wStr = Dir(wReceive & "*.csv")
    Do Until wStr = ""
        If InStr(wStr, "品番照合") > 0 Then
            ReDim Preserve wFN(wI)
            wFN(wI) = wStr
            wI = wI + 1
        End If
        wStr = Dir
    Loop
    
    ' ファイルの移動
    If wI > 0 Then
        For wI = 0 To UBound(wFN)
            wTarget = PathJoin(wMyPath, Mid(wFN(wI), 5, 2) & "月")
            ' CheckDirectory (wTarget)' 月別フォルダのチェックと作成（今回はフォルダチェックを先にする）
            If Dir(wTarget & "\" & wFN(wI)) <> "" Then
                If MsgBox("同一ファイルが存在します。上書きしてもよろしいですか？", vbYesNo + vbQuestion, "確認") = vbYes Then
                    Kill wTarget & "\" & wFN(wI)
                    Name PathJoin(wReceive, wFN(wI)) As wTarget & "\" & wFN(wI)
                End If
            Else
                Name PathJoin(wReceive, wFN(wI)) As wTarget & "\" & wFN(wI)
            End If
        Next
    End If
    
End Sub

' 各シート名に対応したフォルダ内のファイルの内、未取込のものを取り込む
Public Sub getCSV(sheetName As String)
    Dim wWsMst As Worksheet
    Dim wWs As Worksheet
    Dim wMyPath As String
    Dim wFileName As String
    
    wMyPath = ThisWorkbook.Path
    
    ' シート名が仕様通りかチェック
    If IsNumeric(Left(sheetName, 2)) = False Then
        MsgBox "シート名の月は数字2桁でお願いします.", vbExclamation
        Exit Sub
    End If
    
    ' シート名が仕様通りかチェック
    If Left(sheetName, 2) <> StrConv(Left(sheetName, 2), vbNarrow) Then
        MsgBox "シート名の月は半角2桁でお願いします.", vbExclamation
        Exit Sub
    End If
    
    ' 取込対象のフォルダが存在するかチェック
    If Dir(wMyPath & "\" & sheetName, vbDirectory) = "" Then
        MsgBox "フォルダを作成してから実行してください." & vbCrLf & _
            wMyPath & "\" & sheetName & vbCrLf & _
            "→ " & sheetName, vbExclamation
        Exit Sub
    End If
    
    ' 受信フォルダから品番照合CSVファイルを移動
    moveReceiveCSV
    
    ' オブジェクトセット
    Set wWsMst = ThisWorkbook.Worksheets("取込済")
    Set wWs = ThisWorkbook.Worksheets(sheetName)
    
    ' 処理中メッセージの表示
    UserFormWaitMessage.Show vbModeless
    Application.Wait Now() + TimeValue("00:00:01")
    Application.StatusBar = "取込処理開始"
    Application.ScreenUpdating = False
    
    Dim queryTb As QueryTable
    Dim strFilePath As String
    Dim wEndRow As Long
    Dim wEndCol As Long
    Dim wRng As Range
    
    ' 取込対象フォルダ内のファイルを総チェック
    wFileName = Dir(wMyPath & "\" & sheetName & "\*.*")
    Do Until wFileName = ""
        Set wRng = wWsMst.Range("A:A").Find(wFileName, LookAt:=xlWhole)
        
        ' 取込済みシートに存在しないので処理
        If wRng Is Nothing Then
            Application.StatusBar = "[" & wFileName & "] 処理中"
            strFilePath = wMyPath & "\" & sheetName & "\" & wFileName
            
            wEndRow = getEndRow(wWs, 4, 2)
            
            Set queryTb = wWs.QueryTables.Add(Connection:="TEXT;" & strFilePath, _
                                         Destination:=wWs.Cells(wEndRow, 1)) ' CSV を開く
            With queryTb
                .TextFilePlatform = 932          ' 文字コードを指定
                .TextFileParseType = xlDelimited ' 区切り文字の形式
                .TextFileCommaDelimiter = True   ' カンマ区切り
                .TextFileColumnDataTypes = Array(2, 2, 2, 2, 2, 2, 2, 2, 2) ' CSVを項目の型を指定して取り込む 2:xlTextFormat
                .RefreshStyle = xlOverwriteCells ' セルに書き込む方式
                .Refresh                         ' データを表示
                .Delete                          ' CSVファイルとの接続を解除
            End With
            
            ' ファイル名情報を最終列+1に追記（※可変はうまくいかないので列番号８固定に変更24.07.19 y.w）
            wEndCol = 10 ' wWs.Cells(wEndRow, 99).End(xlToLeft).Column
            Dim wHTName As String
            Dim wStrRow As Long
            wHTName = Mid(wFileName, 10, 6)
            wStrRow = wEndRow
            wEndRow = getEndRow(wWs, 4, 2) - 1
            wWs.Range(wWs.Cells(wStrRow, wEndCol + 1), wWs.Cells(wEndRow, wEndCol + 1)).Value = wHTName
            Dim wRow As Long
            For wRow = wStrRow To wEndRow
                wWs.Cells(wRow, wEndCol + 2).NumberFormatLocal = "G/標準"
                wWs.Cells(wRow, wEndCol + 2).Formula = "=VLOOKUP(TEXT(C" & wRow & ",""@""),社員マスタ,3,FALSE)"
            Next
            
            wWs.Cells(wEndRow, 1).Select
            ActiveWindow.ScrollRow = IIf(wEndRow > 20, wEndRow - 20, 1)
            
            ' 列の幅は手動で設定
            wWs.Columns("A").ColumnWidth = 8
            wWs.Columns("B").ColumnWidth = 18
            wWs.Columns("C").ColumnWidth = 6
            wWs.Columns("D").ColumnWidth = 30
            wWs.Columns("E").ColumnWidth = 40
            wWs.Columns("F").ColumnWidth = 7
            wWs.Columns("G").ColumnWidth = 7
            wWs.Columns("H").ColumnWidth = 11
            wWs.Columns("I").ColumnWidth = 7
            wWs.Columns("J").ColumnWidth = 7
            wWs.Columns("K").ColumnWidth = 7
            wWs.Columns("L").ColumnWidth = 7
            
            ' 取込済シートへ転記
            wEndRow = getEndRow(wWsMst, 1, 2)
            wWsMst.Cells(wEndRow, 1).Value = wFileName
            wWsMst.Activate
            wWsMst.Cells(wEndRow, 1).Select
            ActiveWindow.ScrollRow = IIf(wEndRow > 20, wEndRow - 20, 1)
            wWs.Activate
        
        End If
        wFileName = Dir
    Loop
    
    UserFormWaitMessage.Hide
    Application.ScreenUpdating = True
    
    If Application.StatusBar = "取込処理開始" Then
        MsgBox "変更はありませんでした．", vbInformation
        Application.StatusBar = ""
    Else
        If MsgBox("このブックを保存して" & vbCrLf & "バックアップ処理を開始してもよろしいですか？", vbInformation + vbYesNo) = vbYes Then
            ThisWorkbook.Save
            Shell wMyPath & "\" & "照合データバックアップ.bat"
        End If
        Application.StatusBar = "取込処理完了"
    End If
    
End Sub

' 最終行の次の行を取得（データが無い状態の場合は初期値を返却）
' iWs:対象のワークシート iCol:検査する対象の列番号 iDef:検査開始行番号＆見つからなかった場合の初期値
Public Function getEndRow(iWs As Worksheet, iCol As Integer, iDef As Integer) As Integer
    If iWs.Cells(iDef, iCol).Value = "" Then
        getEndRow = iDef
    ElseIf iWs.Cells(iDef + 1, iCol).Value = "" Then
        getEndRow = iDef + 1
    Else
        getEndRow = iWs.Cells(iDef, iCol).End(xlDown).Row + 1
    End If
End Function

' ファイルのパス結合
Private Function PathJoin(ParamArray name()) As String
    Dim wStr As String
    Dim wI As Integer
    For wI = LBound(name) To UBound(name)
        wStr = wStr & name(wI) & "\"
    Next
    PathJoin = Left(wStr, Len(wStr) - 1)
End Function

' フォルダの存在チェック～作成
Private Sub CheckDirectory(wPath As String)
    Dim wArr() As String
    Dim wI As Integer
    Dim wTmpPath As String
    
    If Left(wPath, 2) = "\\" Then
        wArr = Split(Mid(wPath, 3), "\")
        wTmpPath = "\\" & wArr(0)  ' PC名の代入
    Else
        wArr = Split(wPath, "\")
        wTmpPath = wArr(0)  ' ドライブ名の代入
    End If
    
    For wI = 1 To UBound(wArr)
        wTmpPath = wTmpPath & "\" & wArr(wI)
        If wI > 2 And Dir(wTmpPath, vbDirectory + vbHidden) = "" Then
            MkDir wTmpPath
        End If
    Next
    
End Sub


