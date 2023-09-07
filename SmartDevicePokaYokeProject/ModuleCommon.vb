Imports System.Runtime.InteropServices
Imports System.Text
Imports Bt.FileLib
Imports Bt.SysLib
Imports Bt

Module ModuleCommon

    ' アプリケーション実行パス
    Public mAppPath As [String] = ""

    ' 端末名
    Public mHtName As [String] = ""

    ' iniファイル関連
    Private iniFileName As [String] = "PokaYoke.ini"

    Public Function getHTNAME() As String
        Dim ret As Int32 = 0
        Dim disp As [String] = ""
        Dim idSet As UInt32 = 0

        ' 端末名
        Dim pValueGetDef_HtName As IntPtr = Marshal.AllocCoTaskMem((LibDef.BT_SYS_HTNAME_MAXLEN + 1) * Marshal.SizeOf(GetType(Char)))
        idSet = LibDef.BT_SYS_PRM_HTNAME
        ret = Terminal.btGetHandyParameter(idSet, pValueGetDef_HtName)
        If ret <> LibDef.BT_OK Then
            disp = "btGetHandyParameter error ret[" & ret & "]"
            MessageBox.Show(disp, "エラー")
            Return "Nothing"
        End If
        Return Marshal.PtrToStringUni(pValueGetDef_HtName)
    End Function

    '*******************************************************************************
    '         * 機能 ：iniファイルで1行単位のReadを行います。
    '         * API  ：btIniReadString
    '*******************************************************************************

    Public Function getSettings() As String
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        ' セクション
        Dim sectionSet As New StringBuilder("TANTO")
        ' キー
        Dim keySet As New StringBuilder("tancd")
        ' INIファイル
        Dim filenameSet As New StringBuilder(mAppPath & "\" & iniFileName)
        ' 既定値(ReadString用)
        Dim strDefSet As New StringBuilder("000000")
        ' 取得値
        Dim bufaryGet As [Byte]() = New [Byte](LibDef.BT_INI_VALUE_MAXLEN - 1) {}
        ' サイズ
        Dim sizeSet As UInt32 = 0
        ' 取得値(ReadString用)
        Dim strGet As [String] = ""
        Try
            '-----------------------------------------------------------
            ' 読み出し
            '-----------------------------------------------------------
            sizeSet = LibDef.BT_INI_VALUE_MAXLEN
            ret = Ini.btIniReadString(sectionSet, keySet, strDefSet, bufaryGet, sizeSet, filenameSet)
            If ret = 0 Then
                disp = "btIniReadString error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
            End If
            strGet = Encoding.Unicode.GetString(bufaryGet, 0, (ret * 2))
            Return strGet
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
        Return strDefSet.ToString
    End Function

    '*******************************************************************************
    '         * 機能 ：iniファイルで1行単位のWriteを行います。
    '         * API  ：btIniWriteString
    '*******************************************************************************

    Public Sub setSettings(ByVal tancd As String)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        ' セクション
        Dim sectionSet As New StringBuilder("TANTO")
        ' キー
        Dim keySet As New StringBuilder("tancd")
        ' INIファイル
        Dim filenameSet As New StringBuilder(mAppPath & "\" & iniFileName)
        ' 書き込み用
        Dim strSet As New StringBuilder("")
        ' 既定値(ReadString用)
        Dim strDefSet As New StringBuilder("")
        ' サイズ
        Dim sizeSet As UInt32 = 0
        Try
            '-----------------------------------------------------------
            ' 書き込み
            '-----------------------------------------------------------
            strSet = New StringBuilder(tancd)
            ret = Ini.btIniWriteString(sectionSet, keySet, strSet, filenameSet)
            If ret <> LibDef.BT_OK Then
                disp = "btIniWriteString error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
            disp = "書き込み:" & strSet.ToString()
            'MessageBox.Show(disp, "Iniファイル(文字列)")
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

End Module
