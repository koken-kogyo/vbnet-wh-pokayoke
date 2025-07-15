Imports System.Net

Public Class FormPing

    Public Sub New()

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        Call Ping()

    End Sub

    Private Sub btnPing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPing.Click
        Call Ping()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Dispose()
    End Sub

    Private Sub Ping()

        txtOutput.Text = "ネットワーク探索開始..."

        Dim fn As String = mAppPath & "\" & "ping.log"

        Dim proc As New Process()

        proc.StartInfo.FileName = "cmd.exe"
        proc.StartInfo.Arguments = "/c echo ルーター探索開始... " & " > " & fn
        proc.StartInfo.UseShellExecute = False
        proc.Start()
        proc.WaitForExit()

        proc.StartInfo.FileName = "cmd.exe"
        proc.StartInfo.Arguments = "/c ping -n 2 -4 WHap01 >> " & fn
        proc.StartInfo.UseShellExecute = False
        proc.Start()
        proc.WaitForExit()

        proc.StartInfo.FileName = "cmd.exe"
        proc.StartInfo.Arguments = "/c echo 物流事務所探索開始... " & " >> " & fn
        proc.StartInfo.UseShellExecute = False
        proc.Start()
        proc.WaitForExit()

        proc.StartInfo.FileName = "cmd.exe"
        proc.StartInfo.Arguments = "/c ping -n 2 -4 " & mSQLServer & " >> " & fn
        proc.StartInfo.UseShellExecute = False
        proc.Start()
        proc.WaitForExit()

        proc.StartInfo.FileName = "cmd.exe"
        proc.StartInfo.Arguments = "/c echo 自端末アドレス. " & " >> " & fn
        proc.StartInfo.UseShellExecute = False
        proc.Start()
        proc.WaitForExit()

        proc.StartInfo.FileName = "cmd.exe"
        proc.StartInfo.Arguments = "/c ipconfig >> " & fn
        proc.StartInfo.UseShellExecute = False
        proc.Start()
        proc.WaitForExit()

        proc.Dispose()

        Dim sr As New System.IO.StreamReader(fn, System.Text.Encoding.GetEncoding("shift_jis"))
        Dim s As String = sr.ReadToEnd()
        sr.Close()

        txtOutput.Text = s.Replace("error code 11010", "接続失敗！") _
            .Replace(" size=32", "") _
            .Replace(" TTL=64", "") _
            .Replace(" TTL=128", "")
        btnClose.Focus()

    End Sub
End Class