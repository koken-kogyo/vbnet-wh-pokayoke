Public Class FormDialog


    Private _msg As String
    Private cnt As Integer

    Public Sub setMessage(ByVal msg As String)
        _msg = msg
        lblMessage.Text = _msg
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        cnt = cnt + 1
        lblMessage.Text = lblMessage.Text.Replace(".", "") + New String(".", cnt)
        Refresh()
        If cnt > 5 Then cnt = 0
    End Sub

    Private Sub FormDialog_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Timer1.Enabled = True
    End Sub

    Private Sub FormDialog_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Timer1.Enabled = False
    End Sub

End Class