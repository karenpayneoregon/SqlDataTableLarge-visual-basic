Imports System.Runtime.CompilerServices
Imports System.Data

Public Module DataExtensions
    ''' <summary>
    ''' Partition DataTable rows by partitionSize parameter
    ''' </summary>
    ''' <param name="dataTable">Populated DataTable</param>
    ''' <param name="partitionSize">Integer to chunk/partition DataRows</param>
    ''' <returns></returns>
    <Extension>
    Public Iterator Function Partition(dataTable As DataTable, partitionSize As Integer) As IEnumerable(Of IEnumerable(Of DataRow))

        Dim numberOfDataRows = Math.Ceiling(CDbl(dataTable.Rows.Count))
        Dim rowIndex = 0

        Do While rowIndex < numberOfDataRows / partitionSize
            Yield Partition(dataTable, rowIndex * partitionSize, rowIndex * partitionSize + partitionSize)
            rowIndex += 1
        Loop
    End Function
    Private Iterator Function Partition(dataTable As DataTable, index As Integer, endIndex As Integer) As IEnumerable(Of DataRow)

        Dim indexer = index

        Do While indexer < endIndex AndAlso indexer < dataTable.Rows.Count
            Yield dataTable.Rows(indexer)
            indexer += 1
        Loop

    End Function
End Module
