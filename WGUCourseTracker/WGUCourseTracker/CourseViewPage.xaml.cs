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

                notesEditor.Text = course.CourseNotes;
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
            }

        }

        private void AddAssessment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentAddPage(course));
        }

        private void SaveCourse_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Course>();
                course = (Course)mainStackLayout.BindingContext;

                var courseID = course.CourseID;

                var selectedCourse = con.FindWithQuery<Course>($"Select * from Course where CourseID = {courseID}");

                selectedCourse.CourseNotes = notesEditor.Text;

                con.Update(selectedCourse);

            }
        }

        private void InstructerEdit_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InstructorEditPage(course));
        }

        private void AssessmentEdit_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentViewPage(course));
        }

        private void AssessmentDelete_Tapped(object sender, EventArgs e)
        {
            Page new1 = new AssessmentViewPage(course);
            

            var deleteToolBarItem = new ToolbarItem
            {
                Text = "Delete"
            };
            new1.ToolbarItems.Clear();
            Navigation.PushAsync(new1);
            new1.ToolbarItems.Add(deleteToolBarItem);

        }
    }
}