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

                if (startDatePicker.Date < endDatePicker.Date && courseEntry != null)
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