Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Threading
Imports ExtensionsLibrary

Public Class Operations
    Public Delegate Sub OnProcess(status As String)
    ''' <summary>
    ''' Callback for subscribers to see what is being worked on
    ''' </summary>
    Public Shared Event OnProcessingEvent As OnProcess

    Private Shared ConnectionString As String =
                "Data Source=.\SQLEXPRESS;" &
                "Initial Catalog=PaginationExample;" &
                "Integrated Security=True"

    ''' <summary>
    ''' Read 20,000 rows into a DataTable
    ''' </summary>
    ''' <returns>Populated DataTable</returns>
    Public Shared Function Read() As DataTable

        Dim dataTable As New DataTable

        Using cn As New SqlConnection With {.ConnectionString = ConnectionString}
            Using cmd As New SqlCommand With {.Connection = cn}
                cmd.CommandText =
                    "SELECT Id,FirstName, SequenceNumber, LastName, Street, City, [State], Country, Balance, LastPaid " &
                    "FROM BigTable;"

                cn.Open()
                dataTable.Load(cmd.ExecuteReader())

            End Using
        End Using

        Return dataTable

    End Function
    ''' <summary>
    ''' Export DataTable to CSV
    ''' </summary>
    ''' <param name="dataTable">Populated DataTable</param>
    ''' <param name="ct">Fresh CancellationToken</param>
    ''' <returns>Nothing</returns>
    Public Shared Async Function Export(dataTable As DataTable, ct As CancellationToken, Optional fileName As String = "Exported.csv") As Task

        If File.Exists(fileName) Then
            File.Delete(fileName)
        End If

        Await Task.Run(
            Async Function()

                Dim sb As New StringBuilder

                Dim partitions As List(Of IEnumerable(Of DataRow)) = dataTable.Partition(500).ToList()

                Dim partitionCount = partitions.Count
                Dim index As Integer = 1

                For Each enumerable As IEnumerable(Of DataRow) In partitions

                    RaiseEvent OnProcessingEvent($"Processing partition {index} of {partitionCount}")
                    Await Task.Delay(1)

                    For Each dataRow As DataRow In enumerable
                        sb.AppendLine(String.Join(",", dataRow.ItemArray))
                    Next

                    If ct.IsCancellationRequested Then
                        ct.ThrowIfCancellationRequested()
                    End If

                    index += 1

                Next

                File.AppendAllText(fileName, sb.ToString())

            End Function)

    End Function
End Class
