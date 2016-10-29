Module Module1

    Sub Main()
        Dim logger As New System.IO.StreamWriter("logs.log")
        Try
            Dim configFile As New System.IO.StreamReader("autoupdate.conf")
            Dim line As String = configFile.ReadLine
            While configFile.Peek >= 0
                If line.StartsWith("<UpdateDNS>") Then
                    Dim mode As String = configFile.ReadLine
                    mode = mode.Trim
                    Dim DNS As String = configFile.ReadLine
                    DNS = DNS.Trim
                    Dim password As String = configFile.ReadLine
                    password = password.Trim
                    If mode = "CurrentIP" Then
                        updateWithCurrentIp(DNS, password)
                        logger.WriteLine(time() & " [INFO]: Updated " & DNS & " ip with current IP")
                    ElseIf mode = "OtherIP" Then
                        Dim ip As String = configFile.ReadLine
                        ip = ip.Trim
                        updateWithIp(DNS, password, ip)
                        logger.WriteLine(time() & " [INFO]: Updated " & DNS & " ip with ip " & ip)
                    Else
                        logger.WriteLine(time() & " [ERROR]: Unknown mode: " & mode)
                    End If
                End If
                line = configFile.ReadLine
            End While
            logger.WriteLine(time() & " [INFO]: Finished reading configuration file and updating DNS")
        Catch ex As Exception
            logger.WriteLine(time() & " [ERROR]: Failed to read configuration and execute configuration file: " & ex.Message)
        End Try
        logger.Close()
        End
    End Sub

    Function time() As String
        Dim h As String = My.Computer.Clock.LocalTime.Hour
        Dim m As String = My.Computer.Clock.LocalTime.Minute
        Dim s As String = My.Computer.Clock.LocalTime.Second
        If h < 10 Then
            h = "0" & h
        End If
        If m < 10 Then
            m = "0" & m
        End If
        If s < 10 Then
            s = "0" & s
        End If
        Return h & ":" & m & ":" & s
    End Function

    Function updateWithCurrentIp(ByVal DNS As String, ByVal password As String) As String
        Dim wc As New Net.WebClient
        Dim ip As String = wc.DownloadString("http://tools.feron.it/php/ip.php")
        Return wc.DownloadString("http://www.dtdns.com/api/autodns.cfm?id=" & DNS & "&pw=" & password & "&client=OpenUpdate&ip=" & ip)
    End Function

    Function updateWithIp(ByVal DNS As String, ByVal password As String, ByVal ip As String) As String
        Dim wc As New Net.WebClient
        Return wc.DownloadString("http://www.dtdns.com/api/autodns.cfm?id=" & DNS & "&pw=" & password & "&client=OpenUpdate&ip=" & ip)
    End Function

End Module
