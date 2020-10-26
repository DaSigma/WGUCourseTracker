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
    public partial class AssessmentAddPage : ContentPage
    {
        Course currentCourse;
        public AssessmentAddPage(Course course)
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

                foreach (Assessment assessment in courseAssessments)
                {

                    if (assessment.AssessmentType == "Performance")
                    {
                        TypePicker.Items.Remove("Performance");
                    }
                    else
                    {
                        TypePicker.Items.Remove("Objective");

                    }
                }

            }
        }
            async void SaveAssessment_Clicked(object sender, EventArgs e)
            {
                using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
                {
                    var courseID = currentCourse.CourseID;
                    con.CreateTable<Assessment>();

                    Assessment assessment = new Assessment()
                    {
                        AssessmentType = TypePicker.SelectedItem.ToString(),
                        AssessmentName = AssessmentName.Text,
                        AssessmentDueDate = DueDatePicker.Date,
                        CourseID = courseID

                    };

                con.Insert(assessment);
                await Navigation.PopAsync();

                }
            }
        }
    }