using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WGUCourseTracker.Model;
using WGUCourseTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUCourseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermViewPage : ContentPage
    {
        Term term;
        Course course;
        public TermViewPage()
        {
            InitializeComponent();


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            courseListView.SelectedItem = null;

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                //course = (Course)courseStackLayout.BindingContext;
                term = (Term)mainStackLayout.BindingContext;
                var termID = term.TermID;

                con.CreateTable<Course>();
                var courseTable = con.Table<Course>().ToList();

                var courseList = (from course in courseTable
                                  where course.TermID == term.TermID
                                  select course);        
               
                courseListView.ItemsSource = courseList;
            }

        }
        async void SaveTerm_Clicked(object sender, EventArgs e)
        {

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Term>();

                if (startDatePicker.Date < endDatePicker.Date && termEntry != null)
                {
                    con.Update(term);
                    await DisplayAlert("Success!", $"{term.TermName} Updated", "Ok");
                    Page new1 = new TermListPage();
                    Navigation.InsertPageBefore(new1, this);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Term not Created", "Ok");
                }
            }
        }

        async void NewCourse_Clicked(object sender, EventArgs e)
        {
                var selectedTerm = (Term)mainStackLayout.BindingContext;

                await Navigation.PushAsync(new CourseAddPage { BindingContext = selectedTerm as Term });
        }

        private void EditCourse_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CourseViewPage());

        }
    }
}