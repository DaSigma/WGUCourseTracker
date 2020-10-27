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
                var courseAssessments = con.Query<Assessment>($"Select AssessmentType from Assessment where CourseID = {courseID}");
                TypePicker.Items.Clear();

                foreach (Assessment assessment in courseAssessments)
                {
                    if (assessment.AssessmentType == "Performance")
                    {
                        TypePicker.Items.Add("Performance");
                    }
                    else
                    {
                        TypePicker.Items.Add("Objective");

                    }
                }


            }


        }

        async void SaveAssessment_Clicked(object sender, EventArgs e)
        {
            if(TypePicker.SelectedItem != null)
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
            else
            {
                await DisplayAlert("Action needed!", "Select Assessment type to Edit.", "Ok");
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

        async void Delete_Clicked(object sender, EventArgs e)
        {
            if(TypePicker.SelectedItem != null)
            {
                bool deleteAssessment = await DisplayAlert("Confirm!", $"Do you wish to delete the {TypePicker.SelectedItem.ToString()} Assessment?", "Yes", "No");

                if (deleteAssessment)
                {
                    using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
                    {
                        var courseID = currentCourse.CourseID;
                        con.CreateTable<Assessment>();
                        var type = TypePicker.SelectedItem.ToString();

                        var assessment = con.FindWithQuery<Assessment>($"Select * from Assessment where CourseID = {courseID} And AssessmentType = '{type}'");

                        assessment.AssessmentName = AssessmentName.Text;
                        assessment.AssessmentDueDate = DueDatePicker.Date;
                        con.Delete(assessment);
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                await DisplayAlert("Action needed!", "Select Assessment type to Delete.", "Ok");
            }

        }

    }
}