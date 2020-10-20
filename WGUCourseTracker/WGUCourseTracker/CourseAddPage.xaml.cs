using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUCourseTracker.Model;
using WGUCourseTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUCourseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseAddPage : ContentPage
    {
        Term term;
        public CourseAddPage()
        {
            InitializeComponent();
        }

        private void AddAssessment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentViewPage());
        }

        private void AddInstructor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddInstructorPage());
        }

        async void SaveCourse_Clicked(object sender, EventArgs e)
        {
            term = (Term)mainStackLayout.BindingContext;

            Course course = new Course()
            {
                CourseName = courseEntry.Text,
                CourseStartDate = startDatePicker.Date,
                CourseEndDate = endDatePicker.Date,
                CourseStatus = statusPicker.SelectedItem.ToString(),
                TermID = term.TermID

            };

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Course>();

                if (startDatePicker.Date < endDatePicker.Date && courseEntry != null)
                {
                    term = (Term)mainStackLayout.BindingContext;
                    con.Insert(course);
                    await DisplayAlert("Success!", $"{course.CourseName} Created", "Ok");
                    Page new1 = new TermViewPage();
                    Navigation.InsertPageBefore(new1, this);
                    new1.BindingContext = term as Term;
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Failure", "Course not Created", "Ok");
                }
            }
        }
    }
}