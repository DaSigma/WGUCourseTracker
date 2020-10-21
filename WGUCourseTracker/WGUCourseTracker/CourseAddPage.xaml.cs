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

            Instructor instructor = new Instructor()
            {
                InstructorName = instructorNameEntry.Text,
                InstructorPhone = instructorPhoneEntry.Text,
                InstructorEmail = instructorEmailEntry.Text
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
                    new1.BindingContext = term as Term;
                    //Navigation.InsertPageBefore(new1, this);
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