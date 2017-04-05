Imports System.Windows.Forms

Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If OpenFileDialog.ShowDialog() = DialogResult.OK Then
            Dim fileName = OpenFileDialog.FileName
            MakeAnalysisRequest(fileName)
        End If
    End Sub

    Private Function GetImageAsByteArray(imageFilePath As String) As Byte()
        Dim fileStream As New FileStream(imageFilePath, FileMode.Open, FileAccess.Read)
        Dim binaryReader As New BinaryReader(fileStream)
        Return binaryReader.ReadBytes(CInt(fileStream.Length))
    End Function

    Private Async Sub MakeAnalysisRequest(imageFilePath As String)
        Dim client = New HttpClient()

        ' Request headers. Replace the second parameter with a valid subscription key.
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "80c2f6a426164e1c8a8beadd1466e31a")

        ' Request parameters. A third optional parameter is "details".
        Dim requestParameters As String = "visualFeatures=Description&language=en"
        Dim uri As String = Convert.ToString("https://westus.api.cognitive.microsoft.com/vision/v1.0/analyze?") & requestParameters
        System.Diagnostics.Debug.WriteLine(uri)

        Dim response As HttpResponseMessage

        ' Request body. Try this sample with a locally stored JPEG image.
        Dim byteData As Byte() = GetImageAsByteArray(imageFilePath)

        Using content = New ByteArrayContent(byteData)
            ' This example uses content type "application/octet-stream".
            ' The other content types you can use are "application/json" and "multipart/form-data".
            content.Headers.ContentType = New MediaTypeHeaderValue("application/octet-stream")
            response = Await client.PostAsync(uri, content)

            Dim result As String = Await response.Content.ReadAsStringAsync()
            FillRichTextBox(result)
        End Using
    End Sub

    Private Sub FillRichTextBox(result As String)
        resultRichTextBox.Text = result
    End Sub
End Class
