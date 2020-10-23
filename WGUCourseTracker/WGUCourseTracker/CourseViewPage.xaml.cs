using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public List<Assessment> courseAssessments { get; set; }

        //Instructor instructor;
        public CourseViewPage()
        {
            InitializeComponent();
        }

        

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Assessment>();
                course = (Course)mainStackLayout.BindingContext;

                var courseID = course.CourseID;
                var assessmentTable = con.Table<Assessment>().ToList();

                courseAssessments = con.Query<Assessment>($"Select * from Assessment where CourseID = {courseID}");

                foreach(Assessment assessment in courseAssessments)
                {
                    if(assessment.AssessmentType == "Performance")
                    {
                        PAName.Text = assessment.AssessmentName;
                        PADueDate.Text = assessment.AssessmentDueDate.ToString();
                    }
                    else
                    {
                        OAName.Text = assessment.AssessmentName;
                        OADueDate.Text = assessment.AssessmentDueDate.ToString();
                    }
                }
                //if(courseAssessments.)

                //PAName = courseAssessments.
                //var courseTable = con.Table<Course>().ToList();

            }

        }

        private void AddAssessment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentAddPage(course));
        }

        private void AddInstructor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddInstructorPage());
        }

        private void SaveCourse_Clicked(object sender, EventArgs e)
        {

        }

        private void InstructerEdit_Tapped(object sender, EventArgs e)
        {

        }

        private void AssessmentEdit_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentViewPage(course));
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}