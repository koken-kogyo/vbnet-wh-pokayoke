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

' Grep����
Private Sub CommandButton1_Click()
    Dim wArg As String
    Dim wFnd As String
    Dim wPath As String
    Dim wGFile As String
    
    wFnd = TextBox1.Text
    wPath = ThisWorkbook.Path
    
    If wFnd = "" Then
        MsgBox "��������i�Ԃ���͂��Ă��������D", vbExclamation
        Exit Sub
    End If
    
    ' ����������ۑ�
    Dim wRow As Long
    If Cells(Range("��������").Row + 2, 2) = "" Then
        wRow = Range("��������").Row + 2
    Else
        wRow = Cells(Range("��������").Row + 1, 2).End(xlDown).Row + 1
    End If
    Cells(wRow, 2).Value = wFnd
    Cells(wRow, 3).Value = Now
    
    ' �����Ώۃt�@�C�����̐ݒ�
    If CheckBox�ߋ���.Value Then
        wGFile = "*.csv"
    Else
        wGFile = "#*�ߋ���*,*.csv"
    End If
    
    ' �T�N���G�f�B�^�̋N�����p�����[�^��ݒ�
    wArg = " -GREPMODE -GKEY=" & wFnd & " -GFILE=" & wGFile & " -GFOLDER=" & wPath & " -GCODE=99 -GOPT:SP1"
    
    ' �T�N���G�f�B�^�𒼐ڋN��
    Shell "C:\Program Files (x86)\sakura\sakura.exe" & wArg, vbNormalFocus

End Sub
Private Sub TextBox1_Change()
    TextBox1.Text = StrConv(TextBox1.Text, vbUpperCase)
End Sub

Private Sub TextBox1_GotFocus()
    TextBox1.SelStart = 0
    TextBox1.SelLength = Len(TextBox1.Text)
End Sub