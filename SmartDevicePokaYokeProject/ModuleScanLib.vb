Imports Bt
Imports Bt.ScanLib
Imports System.Runtime.InteropServices

Module ModuleScanLib

    ' スキャンモード保存
    Private ulDataDef As UInt32 = 0

    '*******************************************************************************
    '         * 機能 ：コード読み取りプロパティ現在の値を保持しておく。
    '         * API  ：btScanGetProperty
    '*******************************************************************************

    Public Sub backupScanProperty()
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim idSet As UInt32 = 0
        Dim pValueGet As New IntPtr()

        Try
            '-----------------------------------------------------------
            ' 設定前取得
            '-----------------------------------------------------------
            idSet = LibDef.BT_SCAN_PROP_ENABLE_SYMBOLS
            pValueGet = Marshal.AllocCoTaskMem(Marshal.SizeOf(ulDataDef))
            ret = Setting.btScanGetProperty(idSet, pValueGet)
            ulDataDef = CType(Marshal.ReadInt32(pValueGet), UInt32)
            Marshal.FreeCoTaskMem(pValueGet)
            If ret <> LibDef.BT_OK Then
                disp = "btScanGetProperty error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

    End Sub

    '*******************************************************************************
    '         * 機能 ：コード読み取りプロパティ現在の値を保持しておく。
    '         * API  ：btScanSetProperty
    '*******************************************************************************

    Public Sub restoreScanProperty()
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim idSet As UInt32 = 0
        Dim ulDataSet As UInt32 = 0
        Dim pValueSet As New IntPtr()

        Try
            '-----------------------------------------------------------
            ' 退避を戻す
            '-----------------------------------------------------------
            idSet = LibDef.BT_SCAN_PROP_ENABLE_SYMBOLS
            ulDataSet = ulDataDef
            pValueSet = Marshal.AllocCoTaskMem(Marshal.SizeOf(ulDataSet))
            Marshal.WriteInt32(pValueSet, CType(ulDataSet, Int32))
            ret = Setting.btScanSetProperty(idSet, pValueSet)
            Marshal.FreeCoTaskMem(pValueSet)
            If ret <> LibDef.BT_OK Then
                disp = "btScanSetProperty error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

    End Sub

    '*******************************************************************************
    '         * 機能 ：コード読み取りプロパティを設定／取得します。
    '         * API  ：btScanSetProperty
    '*******************************************************************************

    Public Sub setScanProperty(ByVal iMode As Integer)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim idSet As UInt32 = 0
        Dim ulDataSet As UInt32 = 0
        Dim pValueSet As New IntPtr()

        Try
            '-----------------------------------------------------------
            ' 設定①コード読み取り許可
            '-----------------------------------------------------------
            idSet = LibDef.BT_SCAN_PROP_ENABLE_SYMBOLS
            If iMode = 1 Then
                ulDataSet = LibDef.BT_SCAN_ENABLE_ALL
            Else
                ulDataSet = LibDef.BT_SCAN_ENABLE_QR
            End If
            pValueSet = Marshal.AllocCoTaskMem(Marshal.SizeOf(ulDataSet))
            Marshal.WriteInt32(pValueSet, CType(ulDataSet, Int32))
            ret = Setting.btScanSetProperty(idSet, pValueSet)
            Marshal.FreeCoTaskMem(pValueSet)
            If ret <> LibDef.BT_OK Then
                disp = "btScanSetProperty ENABLE_SYMBOLS error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If

            '----------------------------------------------------------
            ' 設定②[OCR] 読取組合せ
            '----------------------------------------------------------
            idSet = LibDef.BT_SCAN_PROP_OCR_ENABLE_COMBI
            If iMode = 1 Then
                ulDataSet = LibDef.BT_SCAN_OCR_ENABLE_PLUS
            Else
                ulDataSet = LibDef.BT_SCAN_OCR_DISABLE
            End If
            pValueSet = Marshal.AllocCoTaskMem(Marshal.SizeOf(ulDataSet))
            Marshal.WriteInt32(pValueSet, CType(ulDataSet, Int32))
            ret = Setting.btScanSetProperty(idSet, pValueSet)
            Marshal.FreeCoTaskMem(pValueSet)
            If ret <> LibDef.BT_OK Then
                disp = "btScanSetProperty error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

End Module
