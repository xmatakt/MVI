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
    public partial class TemplatesPage : ContentPage
    {
        private List<Template> templates;
        private const string allCourses = "Všetky zamerania";
        private const string allDegrees = "Všetky stupne";
        private const string autoInstitute = "Ústav automobilovej mechatroniky";
        private const string infInstitute = "Ústav informatiky a matematiky";
        private const string bachelorsDegree = "Bakalárska práca";
        private const string diplomDegree = "Diplomová práca";

        public TemplatesPage()
        {
            InitializeComponent();
            InitializeTemplates();
            InitializeCoursePicker();
            InitializeDegreePicker();

            TemplatesListView.ItemsSource = templates;
        }

        private void InitializeTemplates()
        {
            templates = new List<Template>()
            {
                new Template()
                {
                    Course = autoInstitute,
                    Degree = bachelorsDegree,
                    TemplateUrl = "http://uamt.fei.stuba.sk/web/sites/subory/sablony_prac/Sablona_BP_UAMT.docx"
                },
                new Template()
                {
                    Course = autoInstitute,
                    Degree = diplomDegree,
                    TemplateUrl = "http://uamt.fei.stuba.sk/web/sites/subory/sablony_prac/Sablona_DP_UAMT-2.docx"
                },
                new Template()
                {
                    Course = infInstitute,
                    Degree = bachelorsDegree,
                    TemplateUrl = "http://www.kaivt.elf.stuba.sk/kaivt/Predmety/Sablony?action=AttachFile&do=get&target=sablonaZP.dotx"
                },
                new Template()
                {
                    Course = infInstitute,
                    Degree = diplomDegree,
                    TemplateUrl = "http://www.kaivt.elf.stuba.sk/kaivt/Predmety/Sablony?action=AttachFile&do=get&target=sablonaZP.dotx"
                },
            };
        }

        private void InitializeCoursePicker()
        {
            CoursePicker.Items.Add(allCourses);
            CoursePicker.Items.Add(autoInstitute);
            CoursePicker.Items.Add(infInstitute);
            CoursePicker.SelectedIndex = 0;
        }

        private void InitializeDegreePicker()
        {
            DegreePicker.Items.Add(allDegrees);
            DegreePicker.Items.Add(bachelorsDegree);
            DegreePicker.Items.Add(diplomDegree);
            DegreePicker.SelectedIndex = 0;
        }

        private void FilterTemplates()
        {
            var selectedCourse = CoursePicker.Items[CoursePicker.SelectedIndex];
            var selectedDegree = DegreePicker.Items[DegreePicker.SelectedIndex];
            var filteredTemplates = templates;

            if (selectedCourse != allCourses)
                filteredTemplates = filteredTemplates.Where(x => x.Course == selectedCourse).ToList();

            if (selectedDegree != allDegrees)
                filteredTemplates = filteredTemplates.Where(x => x.Degree == selectedDegree).ToList();

            TemplatesListView.ItemsSource = filteredTemplates;
        }

        private void Pickers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CoursePicker.SelectedIndex >= 0 && DegreePicker.SelectedIndex >= 0)
                FilterTemplates();
        }

        private void TextCell_Tapped(object sender, EventArgs e)
        {
            var textCell = (TextCell)sender;
            var template = templates.Find(x =>
                x.Course == textCell.Text &&
                x.Degree == textCell.Detail);

            Device.OpenUri(new Uri(template.TemplateUrl));
        }
    }
}
