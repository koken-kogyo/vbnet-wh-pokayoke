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

    ' 担当者コード
    Public mTANCD As [String] = "00000"

    ' 出荷指示書消し込み対象の得意先コード
    Public mSQLServer As String = ""
    Public mTargetTKCDs As String = ""

    ' オプション設定
    Public mBuzzer As [String] = "1" ' 照合時ブザー音
    Public mHandOK As [String] = "0" ' 社内品番手入力
    Public mQROnly As [String] = "1" ' 得意先品番QRコードしか読み取らない

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

    Public Sub getSettings()
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        ' INIファイル
        Dim filenameSet As New StringBuilder(mAppPath & "\" & iniFileName)
        ' 取得値
        Dim bufaryGet As [Byte]() = New [Byte](LibDef.BT_INI_VALUE_MAXLEN - 1) {}

        ' セクション
        Dim sectionSet As New StringBuilder("")
        ' キー
        Dim keySet As New StringBuilder("")
        ' 既定値(ReadString用)
        Dim strDefSet As New StringBuilder("")
        ' サイズ
        Dim sizeSet As UInt32 = 0
        ' 取得値(ReadString用)
        Dim strGet As [String] = ""

        ' 担当者コード取得
        sectionSet = New StringBuilder("TANTO")
        keySet = New StringBuilder("TANCD")
        strDefSet = New StringBuilder(mTANCD)
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
            mTANCD = Encoding.Unicode.GetString(bufaryGet, 0, (ret * 2))
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        ' 出荷指示書関連設定
        sectionSet = New StringBuilder("SHIPMENT")
        ' SQLServer
        keySet = New StringBuilder("SQLSERVER")
        strDefSet = New StringBuilder(mSQLServer)
        Try
            '-----------------------------------------------------------
            ' 読み出し（必須ではない）
            '-----------------------------------------------------------
            sizeSet = LibDef.BT_INI_VALUE_MAXLEN
            ret = Ini.btIniReadString(sectionSet, keySet, strDefSet, bufaryGet, sizeSet, filenameSet)
            If ret = 0 Then
                mSQLServer = ""
            Else
                mSQLServer = Encoding.Unicode.GetString(bufaryGet, 0, (ret * 2))
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
        ' 対象得意先コード取得
        keySet = New StringBuilder("TARGETTKCDS")
        strDefSet = New StringBuilder(mTargetTKCDs)
        Try
            '-----------------------------------------------------------
            ' 読み出し（必須ではない）
            '-----------------------------------------------------------
            sizeSet = LibDef.BT_INI_VALUE_MAXLEN
            ret = Ini.btIniReadString(sectionSet, keySet, strDefSet, bufaryGet, sizeSet, filenameSet)
            If ret = 0 Then
                mTargetTKCDs = ""
            Else
                mTargetTKCDs = Encoding.Unicode.GetString(bufaryGet, 0, (ret * 2))
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

        ' オプション設定
        sectionSet = New StringBuilder("OPTION")
        ' Buzzer ON
        keySet = New StringBuilder("BUZZER")
        strDefSet = New StringBuilder(mBuzzer)
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
            mBuzzer = Encoding.Unicode.GetString(bufaryGet, 0, (ret * 2))
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
        ' Hand OK
        keySet = New StringBuilder("HANDOK")
        strDefSet = New StringBuilder(mHandOK)
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
            mHandOK = Encoding.Unicode.GetString(bufaryGet, 0, (ret * 2))
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
        ' QR Only
        keySet = New StringBuilder("QRONLY")
        strDefSet = New StringBuilder(mQROnly)
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
            mQROnly = Encoding.Unicode.GetString(bufaryGet, 0, (ret * 2))
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try

    End Sub

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
        Dim keySet As New StringBuilder("TANCD")
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

    Public Sub saveSettingTANCD(ByVal iTancd As String)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim sectionSet As New StringBuilder("TANTO")
        Dim keySet As New StringBuilder("TANCD")
        Dim filenameSet As New StringBuilder(mAppPath & "\" & iniFileName)
        Dim strSet As New StringBuilder(iTancd)
        Try
            ret = Ini.btIniWriteString(sectionSet, keySet, strSet, filenameSet)
            If ret <> LibDef.BT_OK Then
                disp = "btIniWriteString error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Public Sub saveSettingBuzzer(ByVal checked As Boolean)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim sectionSet As New StringBuilder("OPTION")
        Dim keySet As New StringBuilder("BUZZER")
        Dim filenameSet As New StringBuilder(mAppPath & "\" & iniFileName)
        Dim strSet As New StringBuilder(If(checked, "1", "0"))
        Try
            ret = Ini.btIniWriteString(sectionSet, keySet, strSet, filenameSet)
            If ret <> LibDef.BT_OK Then
                disp = "btIniWriteString error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Public Sub saveSettingHandOK(ByVal checked As Boolean)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim sectionSet As New StringBuilder("OPTION")
        Dim keySet As New StringBuilder("HANDOK")
        Dim filenameSet As New StringBuilder(mAppPath & "\" & iniFileName)
        Dim strSet As New StringBuilder(If(checked, "1", "0"))
        Try
            ret = Ini.btIniWriteString(sectionSet, keySet, strSet, filenameSet)
            If ret <> LibDef.BT_OK Then
                disp = "btIniWriteString error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Public Sub saveSettingQROnly(ByVal checked As Boolean)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim sectionSet As New StringBuilder("OPTION")
        Dim keySet As New StringBuilder("QRONLY")
        Dim filenameSet As New StringBuilder(mAppPath & "\" & iniFileName)
        Dim strSet As New StringBuilder(If(checked, "1", "0"))
        Try
            ret = Ini.btIniWriteString(sectionSet, keySet, strSet, filenameSet)
            If ret <> LibDef.BT_OK Then
                disp = "btIniWriteString error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    '*******************************************************************************
    '         * 機能 ：オートパワーオフ動作の有効／無効を取得します。
    '         * API  ：btGetAutoPowerOFF
    '*******************************************************************************
    Public Function getAutoPowerOFF() As UInteger
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim intervalDef As UInt32 = 0

        Try
            '-----------------------------------------------------------
            ' 設定前取得
            '-----------------------------------------------------------
            ret = Power.btGetAutoPowerOFF(intervalDef)
            If ret <> LibDef.BT_OK Then
                disp = "btGetAutoPowerOFF error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return 0
            End If
            'disp = "パワーオフ時間[s]  :" & intervalDef & vbCr & vbLf
            'MessageBox.Show(disp, "パワーOFF(オート)(設定前)")
            Return intervalDef
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            Return 0
        End Try
    End Function

    '*******************************************************************************
    '         * 機能 ：オートパワーオフ動作の有効／無効を設定します。
    '         * API  ：btSetAutoPowerOFF, btGetAutoPowerOFF
    '*******************************************************************************
    Public Sub setAutoPowerOFF(ByVal iSec As UInteger)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Try
            '-----------------------------------------------------------
            ' 設定
            '-----------------------------------------------------------
            ret = Power.btSetAutoPowerOFF(iSec)
            If ret <> LibDef.BT_OK Then
                disp = "btSetAutoPowerOFF error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    '*******************************************************************************
    '         * 機能 ：LCDバックライト動作を取得します。
    '         * API  ：btGetLCDBacklight
    '*******************************************************************************
    Public Function getLCDBacklight(ByRef stPropGet As LibDef.BT_LCD_BACKLIGHT) As Boolean
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim modeGet As UInt32 = 0

        Try
            '-----------------------------------------------------------
            ' 設定前取得
            '-----------------------------------------------------------
            ret = Device.btGetLCDBacklight(modeGet, stPropGet)
            If ret <> LibDef.BT_OK Then
                disp = "btGetLCDBacklight error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return False
            End If
            disp = "動作モード     :" & modeGet & vbCr & vbLf & _
                   "通常時         :" & stPropGet.dwNormal & vbCr & vbLf & _
                   "スタンバイ時   :" & stPropGet.dwStandby & vbCr & vbLf & _
                   "スタンバイ時移行時間[s]   :" & stPropGet.dwTimeout
            'MessageBox.Show(disp, "LCDバックライト(設定後)")
            Return True
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            Return False
        End Try
    End Function

    '*******************************************************************************
    '         * 機能 ：LCDバックライト動作を設定します。
    '         * API  ：btSetLCDBacklight
    '*******************************************************************************
    Public Sub setLCDBacklight(ByRef stPropSet As LibDef.BT_LCD_BACKLIGHT)
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Dim modeSet As UInt32 = 0

        Try
            '-----------------------------------------------------------
            ' 設定
            '-----------------------------------------------------------
            modeSet = LibDef.BT_LCD_BL_MANUAL
            ret = Device.btSetLCDBacklight(modeSet, stPropSet)
            If ret <> LibDef.BT_OK Then
                disp = "btSetLCDBacklight error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return
            End If
            disp = "動作モード     :" & modeSet & vbCr & vbLf & _
                   "通常時         :" & stPropSet.dwNormal & vbCr & vbLf & _
                   "スタンバイ時   :" & stPropSet.dwStandby & vbCr & vbLf & _
                   "スタンバイ時移行時間[s]   :" & stPropSet.dwTimeout
            'MessageBox.Show(disp, "LCDバックライト(設定後)")
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

End Module
