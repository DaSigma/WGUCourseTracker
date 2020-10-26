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
    public partial class AssessmentViewPage : ContentPage
    {
        Course currentCourse;
        public AssessmentViewPage(Course course)
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
                con.CreateTable<Assessment>();
                
            }
        }

        async void SaveAssessment_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                var courseID = currentCourse.CourseID;
                con.CreateTable<Assessment>();
                var type = TypePicker.SelectedItem.ToString();

                var assessment = con.FindWithQuery<Assessment>($"Select * from Assessment where CourseID = {courseID} And AssessmentType = '{type}'");

                assessment.AssessmentName = AssessmentName.Text;
                assessment.AssessmentDueDate = DueDatePicker.Date;
                con.Update(assessment);
                await Navigation.PopAsync();

            }
        }

        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                var courseID = currentCourse.CourseID;
                con.CreateTable<Assessment>();
                string type = TypePicker.SelectedItem.ToString();

                var assessment = con.FindWithQuery<Assessment>($"Select * from Assessment where CourseID = {courseID} And AssessmentType = '{type}'");

                AssessmentName.Text = assessment.AssessmentName;
                DueDatePicker.Date = assessment.AssessmentDueDate;

            }
        }
    }
}