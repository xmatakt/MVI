using System;
using System.Windows.Forms;

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace firstMCSApp
{
    public partial class Form1 : Form
    {
        private string fileName;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;

                MakeAnalysisRequest(fileName);
            }
        }

        private byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        private async void MakeAnalysisRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers. Replace the second parameter with a valid subscription key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "80c2f6a426164e1c8a8beadd1466e31a");

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "visualFeatures=Description&language=en";
            string uri = "https://westus.api.cognitive.microsoft.com/vision/v1.0/analyze?" + requestParameters;
            System.Diagnostics.Debug.WriteLine(uri);

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);

                string result = await response.Content.ReadAsStringAsync();
                FillRichTextBox(result);
            }
        }

        private void FillRichTextBox(string result)
        {
            resultRichTextBox.Text = result;
        }
    }
}
