using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Linq;

namespace bonusCv8
{
    public partial class Form1 : Form
    {
        private bool isLeftLabelFilled = false;
        private bool isRightLabelFilled = false;
        private int timeToSleep = 100;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                pictureBoxLeft.Image = new Bitmap(fileName);
                pictureBoxLeft.Update();
                pictureBoxLeft.Refresh();
                MakeRequest(fileName, labelLeft);
            }
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                pictureBoxRight.Image = new Bitmap(fileName);
                pictureBoxRight.Update();
                pictureBoxRight.Refresh();
                MakeRequest(fileName, labelRight);
            }
        }

        private byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        private async void MakeRequest(string imageFilePath, Label label)
        {
            var client = new HttpClient();

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e00b84f9752b4f409fb62216616fa6c0");

            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            //A peak at the JSON response.
            label.Text = "Happiness: " + responseContent.Split(',').FirstOrDefault(x => x.Contains("happiness")).Split(':')[1];
            //richTextBox.Text = responseContent;
        }

        private void labelLeft_TextChanged(object sender, EventArgs e)
        {
            isLeftLabelFilled = true;
            GetWinner();
        }

        private void labelRight_TextChanged(object sender, EventArgs e)
        {
            isRightLabelFilled = true;
            GetWinner();
        }

        private void GetWinner()
        {
            if(isLeftLabelFilled && isRightLabelFilled)
            {
                var leftHappiness = float.Parse(labelLeft.Text.Split(' ')[1]);
                var rightHappiness = float.Parse(labelRight.Text.Split(' ')[1]);

                if(leftHappiness > rightHappiness)
                {
                    var originalImage = pictureBoxLeft.Image;
                    for (int i = 0; i < 5; i++)
                    {
                        pictureBoxLeft.Image = Properties.Resources.winner;
                        pictureBoxLeft.Refresh();
                        System.Threading.Thread.Sleep(timeToSleep);
                        pictureBoxLeft.Image = originalImage;
                        pictureBoxLeft.Refresh();
                        System.Threading.Thread.Sleep(timeToSleep);
                    }
                    return;
                }

                if (leftHappiness < rightHappiness)
                {
                    var originalImage = pictureBoxRight.Image;
                    for (int i = 0; i < 5; i++)
                    {
                        pictureBoxRight.Image = Properties.Resources.winner;
                        pictureBoxRight.Refresh();
                        System.Threading.Thread.Sleep(timeToSleep);
                        pictureBoxRight.Image = originalImage;
                        pictureBoxRight.Refresh();
                        System.Threading.Thread.Sleep(timeToSleep);
                    }
                    return;
                }

                MessageBox.Show("Obe osoby su rovnako stastne!");              
            }
        }
    }
}
