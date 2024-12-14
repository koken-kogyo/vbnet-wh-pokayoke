Attribute VB_Name = "Module1"
Option Explicit

' ��M�t�H���_����i�ԏƍ����O�t�@�C�������ʃt�H���_�Ɉړ�����
'    wA:�l�b�g���[�N���s = "\\192.168.3.3(��������pc090n)\bt-w250\��M�t�H���_�[\�i�ԏƍ�"
'    wB:���[�J�����s     = "D:\Libraries\Desktop\BT-W250\��M�t�H���_�[\�i�ԏƍ�"
Public Sub moveReceiveCSV()
    Dim wMyPath As String
    Dim wReceive As String
    
    wMyPath = ThisWorkbook.Path
    wReceive = Left(wMyPath, InStr(wMyPath, "��M�t�H���_�[") + 7)
    
    Dim wFN() As String
    Dim wStr As String
    Dim wTarget As String
    Dim wI As Integer
    
    ' �Ώۂ�z��Ɋi�[�i�r����Dir�֐����g���Ȃ��ׁj
    wStr = Dir(wReceive & "*.csv")
    Do Until wStr = ""
        If InStr(wStr, "�i�ԏƍ�") > 0 Then
            ReDim Preserve wFN(wI)
            wFN(wI) = wStr
            wI = wI + 1
        End If
        wStr = Dir
    Loop
    
    ' �t�@�C���̈ړ�
    If wI > 0 Then
        For wI = 0 To UBound(wFN)
            wTarget = PathJoin(wMyPath, Mid(wFN(wI), 5, 2) & "��")
            ' CheckDirectory (wTarget)' ���ʃt�H���_�̃`�F�b�N�ƍ쐬�i����̓t�H���_�`�F�b�N���ɂ���j
            If Dir(wTarget & "\" & wFN(wI)) <> "" Then
                If MsgBox("����t�@�C�������݂��܂��B�㏑�����Ă���낵���ł����H", vbYesNo + vbQuestion, "�m�F") = vbYes Then
                    Kill wTarget & "\" & wFN(wI)
                    Name PathJoin(wReceive, wFN(wI)) As wTarget & "\" & wFN(wI)
                End If
            Else
                Name PathJoin(wReceive, wFN(wI)) As wTarget & "\" & wFN(wI)
            End If
        Next
    End If
    
End Sub

' �e�V�[�g���ɑΉ������t�H���_���̃t�@�C���̓��A���捞�̂��̂���荞��
Public Sub getCSV(sheetName As String)
    Dim wWsMst As Worksheet
    Dim wWs As Worksheet
    Dim wMyPath As String
    Dim wFileName As String
    
    wMyPath = ThisWorkbook.Path
    
    ' �V�[�g�����d�l�ʂ肩�`�F�b�N
    If IsNumeric(Left(sheetName, 2)) = False Then
        MsgBox "�V�[�g���̌��͐���2���ł��肢���܂�.", vbExclamation
        Exit Sub
    End If
    
    ' �V�[�g�����d�l�ʂ肩�`�F�b�N
    If Left(sheetName, 2) <> StrConv(Left(sheetName, 2), vbNarrow) Then
        MsgBox "�V�[�g���̌��͔��p2���ł��肢���܂�.", vbExclamation
        Exit Sub
    End If
    
    ' �捞�Ώۂ̃t�H���_�����݂��邩�`�F�b�N
    If Dir(wMyPath & "\" & sheetName, vbDirectory) = "" Then
        MsgBox "�t�H���_���쐬���Ă�����s���Ă�������." & vbCrLf & _
            wMyPath & "\" & sheetName & vbCrLf & _
            "�� " & sheetName, vbExclamation
        Exit Sub
    End If
    
    ' ��M�t�H���_����i�ԏƍ�CSV�t�@�C�����ړ�
    moveReceiveCSV
    
    ' �I�u�W�F�N�g�Z�b�g
    Set wWsMst = ThisWorkbook.Worksheets("�捞��")
    Set wWs = ThisWorkbook.Worksheets(sheetName)
    
    ' ���������b�Z�[�W�̕\��
    UserFormWaitMessage.Show vbModeless
    Application.Wait Now() + TimeValue("00:00:01")
    Application.StatusBar = "�捞�����J�n"
    Application.ScreenUpdating = False
    
    Dim queryTb As QueryTable
    Dim strFilePath As String
    Dim wEndRow As Long
    Dim wEndCol As Long
    Dim wRng As Range
    
    ' �捞�Ώۃt�H���_���̃t�@�C���𑍃`�F�b�N
    wFileName = Dir(wMyPath & "\" & sheetName & "\*.*")
    Do Until wFileName = ""
        Set wRng = wWsMst.Range("A:A").Find(wFileName, LookAt:=xlWhole)
        
        ' �捞�ς݃V�[�g�ɑ��݂��Ȃ��̂ŏ���
        If wRng Is Nothing Then
            Application.StatusBar = "[" & wFileName & "] ������"
            strFilePath = wMyPath & "\" & sheetName & "\" & wFileName
            
            wEndRow = getEndRow(wWs, 4, 2)
            
            Set queryTb = wWs.QueryTables.Add(Connection:="TEXT;" & strFilePath, _
                                         Destination:=wWs.Cells(wEndRow, 1)) ' CSV ���J��
            With queryTb
                .TextFilePlatform = 932          ' �����R�[�h���w��
                .TextFileParseType = xlDelimited ' ��؂蕶���̌`��
                .TextFileCommaDelimiter = True   ' �J���}��؂�
                .TextFileColumnDataTypes = Array(2, 2, 2, 2, 2, 2, 2, 2, 2) ' CSV�����ڂ̌^���w�肵�Ď�荞�� 2:xlTextFormat
                .RefreshStyle = xlOverwriteCells ' �Z���ɏ������ޕ���
                .Refresh                         ' �f�[�^��\��
                .Delete                          ' CSV�t�@�C���Ƃ̐ڑ�������
            End With
            
            ' �t�@�C���������ŏI��+1�ɒǋL�i���ς͂��܂������Ȃ��̂ŗ�ԍ��W�Œ�ɕύX24.07.19 y.w�j
            wEndCol = 10 ' wWs.Cells(wEndRow, 99).End(xlToLeft).Column
            Dim wHTName As String
            Dim wStrRow As Long
            wHTName = Mid(wFileName, 10, 6)
            wStrRow = wEndRow
            wEndRow = getEndRow(wWs, 4, 2) - 1
            wWs.Range(wWs.Cells(wStrRow, wEndCol + 1), wWs.Cells(wEndRow, wEndCol + 1)).Value = wHTName
            Dim wRow As Long
            For wRow = wStrRow To wEndRow
                wWs.Cells(wRow, wEndCol + 2).NumberFormatLocal = "G/�W��"
                wWs.Cells(wRow, wEndCol + 2).Formula = "=VLOOKUP(TEXT(C" & wRow & ",""@""),�Ј��}�X�^,3,FALSE)"
            Next
            
            wWs.Cells(wEndRow, 1).Select
            ActiveWindow.ScrollRow = IIf(wEndRow > 20, wEndRow - 20, 1)
            
            ' ��̕��͎蓮�Őݒ�
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
            
            ' �捞�σV�[�g�֓]�L
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
    
    If Application.StatusBar = "�捞�����J�n" Then
        MsgBox "�ύX�͂���܂���ł����D", vbInformation
        Application.StatusBar = ""
    Else
        If MsgBox("���̃u�b�N��ۑ�����" & vbCrLf & "�o�b�N�A�b�v�������J�n���Ă���낵���ł����H", vbInformation + vbYesNo) = vbYes Then
            ThisWorkbook.Save
            Shell wMyPath & "\" & "�ƍ��f�[�^�o�b�N�A�b�v.bat"
        End If
        Application.StatusBar = "�捞��������"
    End If
    
End Sub

' �ŏI�s�̎��̍s���擾�i�f�[�^��������Ԃ̏ꍇ�͏����l��ԋp�j
' iWs:�Ώۂ̃��[�N�V�[�g iCol:��������Ώۂ̗�ԍ� iDef:�����J�n�s�ԍ���������Ȃ������ꍇ�̏����l
Public Function getEndRow(iWs As Worksheet, iCol As Integer, iDef As Integer) As Integer
    If iWs.Cells(iDef, iCol).Value = "" Then
        getEndRow = iDef
    ElseIf iWs.Cells(iDef + 1, iCol).Value = "" Then
        getEndRow = iDef + 1
    Else
        getEndRow = iWs.Cells(iDef, iCol).End(xlDown).Row + 1
    End If
End Function

' �t�@�C���̃p�X����
Private Function PathJoin(ParamArray name()) As String
    Dim wStr As String
    Dim wI As Integer
    For wI = LBound(name) To UBound(name)
        wStr = wStr & name(wI) & "\"
    Next
    PathJoin = Left(wStr, Len(wStr) - 1)
End Function

' �t�H���_�̑��݃`�F�b�N�`�쐬
Private Sub CheckDirectory(wPath As String)
    Dim wArr() As String
    Dim wI As Integer
    Dim wTmpPath As String
    
    If Left(wPath, 2) = "\\" Then
        wArr = Split(Mid(wPath, 3), "\")
        wTmpPath = "\\" & wArr(0)  ' PC���̑��
    Else
        wArr = Split(wPath, "\")
        wTmpPath = wArr(0)  ' �h���C�u���̑��
    End If
    
    For wI = 1 To UBound(wArr)
        wTmpPath = wTmpPath & "\" & wArr(wI)
        If wI > 2 And Dir(wTmpPath, vbDirectory + vbHidden) = "" Then
            MkDir wTmpPath
        End If
    Next
    
End Sub


