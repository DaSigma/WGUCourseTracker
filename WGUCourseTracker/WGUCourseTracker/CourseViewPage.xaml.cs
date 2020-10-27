using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WGUCourseTracker.Model;
using WGUCourseTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUCourseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseViewPage: ContentPage
    {
        Term term;
        Course currentCourse;
        ObservableCollection<Assessment> courseAssessments;
        //Instructor instructor;
        public CourseViewPage(Course course)
        {
            InitializeComponent();
            currentCourse = course;
        }

      
        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                
                con.CreateTable<Assessment>();
                con.CreateTable<Course>();

                currentCourse = (Course)mainStackLayout.BindingContext;

                var courseID = currentCourse.CourseID;
                var assessmentTable = con.Table<Assessment>().ToList();

                var assessmentList = con.Query<Assessment>($"Select * from Assessment where CourseID = {courseID}");
                courseAssessments = new ObservableCollection<Assessment>(assessmentList);

                var courseInstructor = con.FindWithQuery<Course> ($"Select * from Course where CourseID = {courseID}");

                term = con.FindWithQuery<Term>($"Select * from Term where TermID = {currentCourse.TermID}");

                instructorNameLabel.Text = courseInstructor.InstructorEmail;
                instructorPhoneLabel.Text = courseInstructor.InstructorPhone;
                instructorEmailLabel.Text = courseInstructor.InstructorEmail;

                notesEditor.Text = currentCourse.CourseNotes;
                Status.SelectedItem = currentCourse.CourseStatus;

                PANameLabel.Text = "";
                PADueDateLabel.Text = "";
                OANameLabel.Text = "";
                OADueDateLabel.Text = "";


                foreach (Assessment assessment in assessmentList)
                {
                    if (assessment.AssessmentType == "Performance")
                    {
                        PANameLabel.Text = assessment.AssessmentName;
                        PADueDateLabel.Text = assessment.AssessmentDueDate.ToString("MMM dd, yyyy");
                    }
                    else
                    {
                        OANameLabel.Text = assessment.AssessmentName;
                        OADueDateLabel.Text = assessment.AssessmentDueDate.ToString("MMM dd, yyyy");
                    }

                }

            }

            
        }

        private void AddAssessment_Clicked(object sender, EventArgs e)
        {
            if(courseAssessments.Count == 2)
            {
                MaxAssessments();
            }
            else
            {
                Navigation.PushAsync(new AssessmentAddPage(currentCourse));
            }
            
        }

        async void SaveCourse_Clicked(object sender, EventArgs e)
        {
            if(DateCheck() && NullCheck())
            {
                using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
                {
                    con.CreateTable<Course>();
                    currentCourse = (Course)mainStackLayout.BindingContext;

                    var courseID = currentCourse.CourseID;

                    var selectedCourse = con.FindWithQuery<Course>($"Select * from Course where CourseID = {courseID}");


                    selectedCourse.CourseNotes = notesEditor.Text;
                    selectedCourse.CourseName = courseNameEntry.Text;
                    selectedCourse.CourseStatus = Status.SelectedItem.ToString();
                    selectedCourse.CourseStartDate = startDatePicker.Date;
                    selectedCourse.CourseEndDate = endDatePicker.Date;

                    currentCourse = selectedCourse;

                    con.Update(selectedCourse);

                }
                await DisplayAlert("Saved!", $"'{currentCourse.CourseName}' course updated!", "Ok");
            }

            Page new1 = new CourseViewPage(currentCourse) { BindingContext = currentCourse as Course };
            Navigation.InsertPageBefore(new1, this);
            await Navigation.PopAsync();

        }

        private void InstructerEdit_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InstructorEditPage(currentCourse));
        }

        async void AssessmentEdit_Tapped(object sender, EventArgs e)
        {
            if (courseAssessments.Count != 0)
            {
                await Navigation.PushAsync(new AssessmentViewPage(currentCourse));
                //await DisplayAlert("Assessment Edit", "Select Assessment type to Edit.", "Ok");
            }
            else
            {
                await DisplayAlert("Error","Please Add assessments!", "Ok");
            }
            
        }

        async void MaxAssessments()
        {
            await DisplayAlert("Error!", "Course has maximumum number of assessments.", "Ok");
        }
        private bool NullCheck()
        {
            if (!String.IsNullOrEmpty(courseNameEntry.Text)
                && (Status.SelectedItem != null))
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