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
    public partial class CourseViewPage: ContentPage
    {
        Course course;
        Instructor instructor;
        public CourseViewPage()
        {
            InitializeComponent();
        }

        

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                course = (Course)mainStackLayout.BindingContext;
                

                var courseID = course.CourseID;

                con.CreateTable<Course>();
                con.CreateTable<Assessment>();
                con.CreateTable<Instructor>();

                var courseTable = con.Table<Course>().ToList();

                var instructorTable = con.Table<Instructor>().ToList();

                var courseInstructor = (from instructor in instructorTable
                                  where instructor.CourseID == course.CourseID
                                  select instructor);

                instructorStackLayout.BindingContext = courseInstructor;
            }

        }

        private void AddAssessment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentViewPage());
        }

        private void AddInstructor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddInstructorPage());
        }

        private void SaveCourse_Clicked(object sender, EventArgs e)
        {

        }
    }
}