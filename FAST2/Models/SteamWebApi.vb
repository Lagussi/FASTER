﻿Imports System.IO
Imports System.Net
Imports Newtonsoft.Json.Linq

Namespace Models
    Public Class SteamWebApi

        Private Const SteamApiKey = "1669DCBF5FD494B07B85358D12FFB85B"

        'Gets mod info from Steam using Steam Web API
        Public Shared Function GetFileDetails(modIds As List(Of Int32)) As List(Of JObject)

            Dim mods As String = String.Empty
            For Each modId In modIds
                mods = mods & "&publishedfileids[" & modIds.IndexOf(modId) & "]=" & modId
            Next

            Dim response = ApiCall("https://api.steampowered.com/IPublishedFileService/GetDetails/v1?key=" & SteamApiKey & mods)
            
            Return response.SelectTokens("response.publishedfiledetails[*]").Cast (Of JObject)().ToList()
        End Function

        Public Shared Function GetPlayerSummaries(playerId As Integer) As JObject
            Dim response = ApiCall("https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v1?key=" & SteamApiKey & "&steamids=" & playerId)

            Return response.SelectToken("response.players.player[0]")
        End Function

        Private Shared Function ApiCall(uri As String) As JObject
            ' Create a request for the URL. 
            Dim request As WebRequest = WebRequest.Create(uri)
            ' Get the response.
            Dim response As WebResponse = request.GetResponse()
            ' Display the status.
            Console.WriteLine(CType(response, HttpWebResponse).StatusDescription)
            ' Get the stream containing content returned by the server.
            Dim dataStream As Stream = response.GetResponseStream()
            ' Open the stream using a StreamReader for easy access.
            Dim reader As New StreamReader(dataStream)
            ' Read the content.
            Dim responseFromServer = reader.ReadToEnd()
            ' Clean up the streams and the response.
            reader.Close()
            response.Close()
            ' Return the response
            Return JObject.Parse(responseFromServer)
        End Function
    End Class
End NameSpace