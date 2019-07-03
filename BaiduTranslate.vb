Public Function BaiduTranslate(szContent As String, nFlag As Integer) As String
        If szContent = "" Then Return "" : Exit Function
        Dim result As String = ""
        Dim fromstr = "auto"
        Dim tostr = ""
        If nFlag = 1 Then
            tostr = "zh"
        Else
            tostr = "en"
        End If
        Dim appid = "20190503000293652"
        Dim r As System.Random = New Random(System.Environment.TickCount)
        Dim salt = r.Next(10000, 99999)
        Dim pass = "cVY1eN2GPyvyoBXR1RwO"
        Dim md5 As MD5 = System.Security.Cryptography.MD5.Create()
        Dim str1 = appid & szContent & salt & pass
        Dim inputBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(str1)
        Dim hash As Byte() = md5.ComputeHash(inputBytes)
        Dim sb As New StringBuilder()
        For i As Integer = 0 To hash.Length - 1
            sb.Append(hash(i).ToString("x2"))
        Next
        Dim szUrl = "http://api.fanyi.baidu.com/api/trans/vip/translate?q=" & szContent & "&from=" & fromstr & "&to=" & tostr & "&appid=" & appid & "&salt=" & salt & "&sign=" & sb.ToString
        Dim request As HttpWebRequest = WebRequest.Create(szUrl)
        request.KeepAlive = True
        request.Accept = "*/*"
        request.Method = "GET"
        Dim szResult As String = ""
        Try
            Using stream As Stream = request.GetResponse().GetResponseStream()
                Using reader As New StreamReader(stream)
                    result = reader.ReadToEnd()
                    reader.Close()
                End Using
                stream.Close()
            End Using

            If result = "" Then Return "" : Exit Function
            Dim jsons As JObject = JObject.Parse(result)

            Dim n = jsons.SelectToken("trans_result").Count
            For i = 0 To n - 1
                szResult = szResult + vbNewLine + jsons.SelectToken("trans_result(" & i & ").dst").ToString
            Next
        Catch ex As Exception

        End Try
        Return szResult
    End Function
