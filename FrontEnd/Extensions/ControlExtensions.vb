Imports System.Runtime.CompilerServices

Namespace Extensions

    Public Module ControlExtensions
        ''' <summary>
        ''' Used to prevent cross-threading violation
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="control"></param>
        ''' <param name="action"></param>
        ''' <returns></returns>
        <Extension>
        Public Function InvokeIfRequired(Of T As Control)(control As T, action As Action(Of T)) As IAsyncResult

            If control.InvokeRequired Then

                Try

                    Return control.BeginInvoke(
                        New Action(Of T, Action(Of T))(AddressOf InvokeIfRequired),
                        New Object() {control, action})

                Catch ex As Exception

                    Return Nothing

                End Try
            Else

                action(control)
                Return Nothing

            End If

        End Function
    End Module
End Namespace