using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FEINews.Data;

namespace FEINews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExamsPage : ContentPage
    {
        private const string allSubjects = "Všetky predmety";
        private const string fileName = "exams.csv";
        private List<Exam> exams;

        public ExamsPage()
        {
            InitializeComponent();
            InitializePage();
        }

        private void InitializePage()
        {
            if(DependencyService.Get<Interfaces.IFileController>().FileExists(fileName))
            {
                InitializeExams();
                InitializeSubjectPicker();
            }
            else
            {
                DisplayAlert("CSV súbor sa nenašiel!", @"Súbor s rozvrhom skúšok nebol nájdený. Pre stiahnutie tohoto súboru stlačte tlačidlo Obnoviť CSV súbor", "OK");
            }
        }

        private void InitializeExams()
        {
            exams = new List<Exam>();
            var csvContent = DependencyService.Get<Interfaces.IFileController>().GetFileString(fileName);
            if(csvContent == null)
            {
                DisplayAlert("CSV súbor je prázdny!", @"CSV súbor je prázdny, alebo sa pri jeho načítaní vyskytla chyba", "OK");
                return;
            }

            csvContent = csvContent.Trim();
            var csvLines = csvContent.Split('\n');
            var counter = 0;
            // prvy riadok vynecham ponevadz to su iba nazvy stlpcov
            for (int i = 1; i < csvLines.Length; i++)
            {
                var columns = csvLines[i].Split(';');
                if (columns.Length >= 9)
                {
                    var newExam = new Exam()
                    {
                        ID = ++counter,
                        Workplace = columns[0],
                        Code = columns[1],
                        SubjectName = columns[2],
                        RTDate = GetExamDateString(columns[3]),
                        RTTime = columns[4],
                        RTPlace = columns[5],
                        OTDate = GetExamDateString(columns[6]),
                        OTTime = columns[7],
                        OTPlace = columns[8]
                    };
                    exams.Add(newExam);
                }
            }

            ExamsListView.ItemsSource = exams;
        }

        private void InitializeSubjectPicker()
        {
            var subjectNames = exams.Select(x => x.SubjectName).Distinct();

            SubjectPicker.Items.Add(allSubjects);
            foreach (var subjectName in subjectNames)
                SubjectPicker.Items.Add(subjectName);

            SubjectPicker.SelectedIndex = 0;
        }

        private void ExamsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (Exam)e.Item;
            DisplayAlert("Popis udalosti", item.Description, "OK");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var itemID = ((Button)sender).Text;
            var exam = exams.Find(x => x.ID == int.Parse(itemID.Trim()));
            DependencyService.Get<Interfaces.ICalendarEventCreator>().CreateEventForExam(exam);
        }

        private void ReloadFileButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<Interfaces.IFileController>()
                .DownloadFileAsync("http://www.uamt.fei.stuba.sk/MVI/sites/default/files/noviny/Terminy_skusok_FEI_ZS_2016_17f.csv", fileName);

            WaitForDownloadAsync();
        }

        private async void WaitForDownloadAsync()
        {
            //TODO: ak sa nepodari stiahnut, bude tu nekonecna slucka
            //http://stackoverflow.com/questions/5071076/downloadstringasync-wait-for-request-completion
            while (!DependencyService.Get<Interfaces.IFileController>().FileExists(fileName))
            {
                await Task.Delay(100); // wait for respsonse
            }

            InitializeExams();
            InitializeSubjectPicker();
        }

        private string GetExamDateString(string date)
        {
            if(date.Contains("."))
            {
                var splittedDate = date.Split('.');
                if (splittedDate.Length < 2)
                    return date;

                var switchedDate = splittedDate[1] + "." + splittedDate[0] + ".";
                DateTime dateString;
                if (DateTime.TryParse(switchedDate, out dateString))
                {
                    return date + "2017";
                }
            }

            return date;
        }

        private void SubjectPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = SubjectPicker.SelectedIndex;
            if(SubjectPicker.Items[selectedIndex] == allSubjects)
                ExamsListView.ItemsSource = exams;
            else
                ExamsListView.ItemsSource = exams.Where(x=> x.SubjectName == SubjectPicker.Items[selectedIndex]);
        }

        private void OrderResults_Clicked(object sender, EventArgs e)
        {
            if (exams == null || exams.Count <= 0)
            {
                DisplayAlert("Zoznam skúšok je prázdny!", @"Pre načítanie zoznamu skúšok stlačte tlačidlo Obnoviť CSV súbor", "OK");
                return;
            }

            var button = (Button)sender;

            if(button.Text == "Abecedne")
            {
                ExamsListView.ItemsSource = GetFilteredExams().OrderBy(x => x.SubjectName);
                button.Text = "Abecedne zostupne";
            }
            else if (button.Text == "Abecedne zostupne")
            {
                ExamsListView.ItemsSource = GetFilteredExams().OrderByDescending(x => x.SubjectName);
                button.Text = "Pôvodné poradie";
            }
            else if (button.Text == "Pôvodné poradie")
            {
                ExamsListView.ItemsSource = GetFilteredExams();
                button.Text = "Abecedne";
            }
        }

        private List<Exam> GetFilteredExams()
        {
            var selectedIndex = SubjectPicker.SelectedIndex;
            if (selectedIndex < 0)
                return exams;
            if ( SubjectPicker.Items[selectedIndex] == allSubjects)
               return exams;
            else
                return exams.Where(x => x.SubjectName == SubjectPicker.Items[selectedIndex]).ToList();
        }
    }
}
