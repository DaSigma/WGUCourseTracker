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

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {

                if (DateCheck() && NullCheck())

                    {
                        con.CreateTable<Course>();
                        con.CreateTable<Instructor>();
                        con.CreateTable<Assessment>();

                        Course course = new Course()
                        {
                            CourseName = courseEntry.Text, 
                            CourseStartDate = startDatePicker.Date,
                            CourseEndDate = endDatePicker.Date,
                            CourseStatus = statusPicker.SelectedItem.ToString(),
                            InstructorName = instructorNameEntry.Text,
                            InstructorEmail = instructorEmailEntry.Text,
                            InstructorPhone = instructorPhoneEntry.Text,
                            TermID = term.TermID
                        };

                        con.Insert(course);
                        var courseId = course.CourseID;

                        term = (Term)mainStackLayout.BindingContext;
                    
                    
                        await DisplayAlert("Success!", $"{course.CourseName} course Created successfully!", "Ok");
                        Page new1 = new TermViewPage();
                        new1.BindingContext = term as Term;
                        //Navigation.InsertPageBefore(new1, this);
                        await Navigation.PopAsync();
                }
                else
                {
                    return;
                }
            }
        }
        private bool NullCheck()
        {
            if (!String.IsNullOrEmpty(courseEntry.Text)
                && (statusPicker.SelectedItem != null)
                && !String.IsNullOrEmpty(instructorEmailEntry.Text)
                && !String.IsNullOrEmpty(instructorNameEntry.Text)
                && !String.IsNullOrEmpty(instructorPhoneEntry.Text))
            {
                return true;
            }
            DisplayAlert("Error!", "Please fill in all information!", "Ok");
            return false;
        }
        private bool DateCheck()
        {


            if (startDatePicker.Date < endDatePicker.Date)
            {
                if (term.TermStartDate <= startDatePicker.Date && term.TermEndDate >= endDatePicker.Date)
                {
                    return true;
                }
                DisplayAlert("Error!", $"Course should be between term Start " +
                    $"({term.TermStartDate.ToString("MMM dd, yyyy")}) and End ({term.TermEndDate.ToString("MMM dd, yyyy")}) dates!", "OK");
                return false;

            }
            DisplayAlert("Error!", "Start Date must be greater than End Date!", "OK");
            return false;
        }

    }
}