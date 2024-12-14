VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Sheet4"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = True
Option Explicit

' Grep検索
Private Sub CommandButton1_Click()
    Dim wArg As String
    Dim wFnd As String
    Dim wPath As String
    Dim wGFile As String
    
    wFnd = TextBox1.Text
    wPath = ThisWorkbook.Path
    
    If wFnd = "" Then
        MsgBox "検索する品番を入力してください．", vbExclamation
        Exit Sub
    End If
    
    ' 検索履歴を保存
    Dim wRow As Long
    If Cells(Range("検索履歴").Row + 2, 2) = "" Then
        wRow = Range("検索履歴").Row + 2
    Else
        wRow = Cells(Range("検索履歴").Row + 1, 2).End(xlDown).Row + 1
    End If
    Cells(wRow, 2).Value = wFnd
    Cells(wRow, 3).Value = Now
    ThisWorkbook.Save
    
    ' 検索対象ファイル名の設定
    If CheckBox過去分.Value Then
        wGFile = "*.csv"
    Else
        wGFile = "#*過去分*,*.csv"
    End If
    
    ' サクラエディタの起動時パラメータを設定
    wArg = " -GREPMODE -GKEY=" & wFnd & " -GFILE=" & wGFile & " -GFOLDER=" & wPath & " -GCODE=99 -GOPT:SP1"
    
    ' サクラエディタを直接起動
    Shell "C:\Program Files (x86)\sakura\sakura.exe" & wArg, vbNormalFocus

End Sub
Private Sub TextBox1_Change()
    TextBox1.Text = StrConv(TextBox1.Text, vbUpperCase)
End Sub

Private Sub TextBox1_GotFocus()
    TextBox1.SelStart = 0
    TextBox1.SelLength = Len(TextBox1.Text)
End Sub
