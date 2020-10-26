using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUCourseTracker.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUCourseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InstructorEditPage : ContentPage
    {
        Course currentCourse;
        public InstructorEditPage(Course course)
        {
            InitializeComponent();
            currentCourse = course;
        }

        protected override void OnAppearing()
        {
            var courseID = currentCourse.CourseID;
            base.OnAppearing();

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {

                currentCourse = con.FindWithQuery<Course>($"Select * from Course where CourseID = {courseID}");

                InstructorName.Text = currentCourse.InstructorName;
                InstructorPhone.Text = currentCourse.InstructorPhone;
                InstructorEmail.Text = currentCourse.InstructorEmail;

            }
        }

        async void SaveInstructor_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                var courseID = currentCourse.CourseID;

                var assessment = con.FindWithQuery<Assessment>($"Select * from Course where CourseID = {courseID}");

                currentCourse.InstructorName = InstructorName.Text;
                currentCourse.InstructorPhone = InstructorPhone.Text;
                currentCourse.InstructorEmail = InstructorEmail.Text;

                con.Update(currentCourse);
                Page new1 = new CourseViewPage(currentCourse);
                new1.BindingContext = currentCourse as Course;
                Navigation.InsertPageBefore(new1, this);
                await Navigation.PopAsync();

            }
        }
    }
}