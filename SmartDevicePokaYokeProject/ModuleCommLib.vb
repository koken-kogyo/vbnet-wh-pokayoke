Imports System.Text
Imports Bt.CommLib
Imports Bt

' 通信制御系モジュール
Module ModuleCommLib

    Private accPath As [String] = "\FlashDisk\BT_FILES\drv1"

    ' USB Open
    Public Function USBOpen() As Boolean
        Dim ret As Int32 = 0
        Dim disp As [String] = ""
        Dim strEnable As [String] = ""

        Dim enableDef As Int32 = 0
        Dim funcTypeDef As UInt32 = 0
        Dim storageTypeDef As UInt32 = 0
        Dim funcTypeSet As UInt32 = 0
        Dim storageTypeSet As UInt32 = 0
        Dim enableGet As Int32 = 0
        Dim funcTypeGet As UInt32 = 0
        Dim storageTypeGet As UInt32 = 0

        Try
            '-----------------------------------------------------------
            ' 設定
            '-----------------------------------------------------------
            funcTypeSet = LibDef.BT_USB_CLIENT_COM
            storageTypeSet = LibDef.BT_USB_STORAGE_SD
            ret = Usb.btUSBOpen(funcTypeSet, storageTypeSet)
            If ret <> LibDef.BT_OK Then
                disp = "btUSBOpen error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return False
            End If

            ret = Usb.btUSBGetSetting(enableGet, funcTypeGet, storageTypeGet)
            If ret <> LibDef.BT_OK Then
                disp = "btUSBGetSetting error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return False
            End If
            If enableGet = 1 Then
                strEnable = "有効"
            ElseIf enableGet = 0 Then
                strEnable = "無効"
                disp = "有効/無効        :" & strEnable & vbCr & vbLf & "機能               :" & funcTypeGet & vbCr & vbLf & "公開ストレージ   :" & storageTypeGet & vbCr & vbLf
                MessageBox.Show(disp, "通信ユニット設定(設定後)")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            Return False
        End Try

        Return True

    End Function

    ' USB Close
    Public Sub USBClose()
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        '-----------------------------------------------------------
        ' 退避を戻す
        '-----------------------------------------------------------
        ret = Usb.btUSBClose()
        If ret <> LibDef.BT_OK Then
            disp = "btUSBClose error ret[" & ret & "]"
            MessageBox.Show(disp, "エラー")
        End If

    End Sub

    Public Function PCPutFile(ByVal tblName As String, ByRef dialog As FormTransmitting) As Boolean
        Dim ret As Int32 = 0
        Dim disp As [String] = ""

        Try
            ' 通信経路設定
            Dim route As UInt32 = LibDef.BT_COMM_DEVICE_USBCOM
            ret = KProtocol2.btComm2SetDev(route)
            If ret <> LibDef.BT_OK Then
                disp = "btComm2SetDev error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return False
            End If

            ' PCへコネクト (タイムアウト10秒、キャンセル＝Cキー)
            ret = KProtocol2.btComm2Connect(10)
            If ret <> LibDef.BT_OK Then
                disp = "btComm2Connect error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return False
            End If


            ' PCへファイルを送信
            Dim week As Int32 = DateTime.Now.DayOfWeek
            Dim jpName As String = ""
            If tblName = "Poka1" Then jpName = "品番照合クボタ"
            If tblName = "Poka2" Then jpName = "品番照合ヤンマー"
            If tblName = "Poka3" Then jpName = "品番照合ティエラ"
            If tblName = "Poka4" Then jpName = "品番照合オリエント"

            Dim localFileName As String = accPath & "\" & tblName & "_" & week & ".csv"
            Dim remoteFileName As String = Format(Now, "yyyyMMdd") & "_" & _
                mHtName & "_" & _
                jpName & ".csv"



            Dim localFile = New StringBuilder(localFileName)
            Dim remoteFile = New StringBuilder(remoteFileName)
            ret = KProtocol2.btComm2PutFile(localFile, remoteFile, CType(10, UInteger))
            If ret <> LibDef.BT_OK Then
                disp = "btComm2PutFile error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return False
            End If



            ' 切断
            ret = KProtocol2.btComm2Disconnect(10)
            If ret <> LibDef.BT_OK Then
                disp = "btComm2Disconnect error ret[" & ret & "]"
                MessageBox.Show(disp, "エラー")
                Return False
            End If


        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            Return False
        End Try

        Return True

    End Function

End Module
