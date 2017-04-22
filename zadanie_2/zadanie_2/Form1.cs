using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace zadanie_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            difficultyComboBox.Items.Add(1);
            difficultyComboBox.Items.Add(2);
            difficultyComboBox.Items.Add(3);
            difficultyComboBox.Items.Add(4);
            difficultyComboBox.SelectedIndex = 0;
        }

        static async Task InvokeRequestResponseService(string name, string difficulty, string hours)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            new StringTable() 
                            {
                                ColumnNames = new string[] {"Meno", "Obtiaznost predmetu", "Pocet hodin ucenia", "Znamka"},
                                Values = new string[,] {  { name, difficulty, hours, "value" }}
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "AiKP754aiJ8/qwkwvxraICFYgfey/u1Zq22t2z2lwNNIge2hG0hwUvGB8hDJH+slkz3T2G4zZf5pCQI95QcNnQ=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress =
                    new Uri(
                        "https://ussouthcentral.services.azureml.net/workspaces/fd1b75cf334c42a7a7350be5d86ef80d/services/294f012c5c6948bb985cc0641aef58a8/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)


                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Result: " + result);
                }
                else
                {
                    MessageBox.Show(string.Format("The request failed with status code: {0}\nCHECK THE OUTPUT WINDOW!", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    System.Diagnostics.Debug.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(responseContent);
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await InvokeRequestResponseService(nameTextBox.Text, difficultyComboBox.Items[difficultyComboBox.SelectedIndex].ToString(), hoursNumericUpDown.Value.ToString());
        }
    }
}
