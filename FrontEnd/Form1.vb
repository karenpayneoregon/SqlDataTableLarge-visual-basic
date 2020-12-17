Imports System.Threading
Imports BackendLibrary
Imports ExtensionsLibrary
Imports FrontEnd.Extensions

Public Class Form1

    Private BindingSource As New BindingSource
    Private _cts As New CancellationTokenSource()

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        AddHandler Operations.OnProcessingEvent, AddressOf OnProcess

        BindingSource.DataSource = Operations.Read()
        DataGridView1.DataSource = BindingSource

    End Sub

    Private Sub OnProcess(status As String)

        StatusLabel.InvokeIfRequired(Sub(item) item.Text = status)

    End Sub

    Private Async Sub ExportButton_Click(sender As Object, e As EventArgs) Handles ExportButton.Click

        Dim dataTable = CType(BindingSource.DataSource, DataTable)

        If dataTable.Rows.Count = 0 Then
            Exit Sub
        End If

        If _cts.IsCancellationRequested = True Then
            _cts.Dispose()
            _cts = New CancellationTokenSource()
        End If


        CancelButton.Enabled = True
        StatusLabel.Visible = True

        Await Task.Delay(1)

        Try
            Await Operations.Export(dataTable, _cts.Token)
            MessageBox.Show("Done")
        Catch cancelled As OperationCanceledException
            MessageBox.Show("Cancelled export")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        _cts.Cancel()
        CancelButton.Enabled = False
        StatusLabel.Visible = False
    End Sub
End Class
