Imports System.Net.Sockets
Imports System.IO

Public Class TCP_Server

    Public Event getMessage(ByRef rClient As TcpClient, ByVal str As String)
    Public Event clientLogout(ByVal client As TCP_Server)
    Private sendMessage As StreamWriter
    Public listClient As TcpClient

    Sub New(ByVal forClient As TcpClient)
        listClient = forClient
        listClient.GetStream.BeginRead(New Byte() {0}, 0, 0, AddressOf ReadAllClient, Nothing)
    End Sub

    Private Sub ReadAllClient()
        Try
            Dim bufferSize As Integer = 1024
            Dim myBufferBytes(bufferSize) As Byte

            If listClient.Available Then
                listClient.GetStream.Read(myBufferBytes, 0, myBufferBytes.Length)
                RaiseEvent getMessage(listClient, System.Text.Encoding.ASCII.GetString(myBufferBytes))
                listClient.GetStream.BeginRead(New Byte() {0}, 0, 0, AddressOf ReadAllClient, Nothing)
            End If

        Catch ex As Exception
            RaiseEvent clientLogout(Me)
        End Try
    End Sub

    Public Sub Send(ByVal Messsage As String)
        sendMessage = New StreamWriter(listClient.GetStream)
        sendMessage.WriteLine(Messsage)
        sendMessage.Flush()
    End Sub

    Public Sub Close()
        listClient.GetStream().Close()
        listClient.Close()
    End Sub

End Class
