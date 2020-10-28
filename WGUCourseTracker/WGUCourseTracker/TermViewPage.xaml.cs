using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WGUCourseTracker.Model;
using WGUCourseTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUCourseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermViewPage : ContentPage
    {
        Term selectedTerm;
        Course course;
        ObservableCollection<Course> termCourses;
        public TermViewPage(Term term)
        {
            InitializeComponent();
            term = (Term)mainStackLayout.BindingContext;
            selectedTerm = term;          
            var selectedCourse = (Course)courseListView.SelectedItem;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            courseListView.SelectedItem = null;

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                selectedTerm = (Term)mainStackLayout.BindingContext;
                var termID = selectedTerm.TermID;

                con.CreateTable<Course>();
                var courseTable = con.Table<Course>().ToList();

                var courseList = (from course in courseTable
                                  where course.TermID == selectedTerm.TermID
                                  select course);

                termCourses = new ObservableCollection<Course>(courseList);
                courseListView.ItemsSource = courseList;
            }

        }
        async void SaveTerm_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Term>();

                if (DateCheck() && NullCheck())
                {
                    con.Update(selectedTerm);
                    await DisplayAlert("Success!", $"{selectedTerm.TermName} Saved", "Ok");
                    Page new1 = new TermViewPage(selectedTerm) { BindingContext = selectedTerm as Term };
                    Navigation.InsertPageBefore(new1, this);
                    await Navigation.PopAsync();
                }
                else
                {
                    return;
                }
            }
        }

        async void NewCourse_Clicked(object sender, EventArgs e)
        {
            var selectedTerm = (Term)mainStackLayout.BindingContext;
            if (termCourses.Count < 6)
            {
                await Navigation.PushAsync(new CourseAddPage { BindingContext = selectedTerm as Term });
            }
            else
            {
                await DisplayAlert("Error!", $"{selectedTerm.TermName} Has Maximum number of courses!", "Ok");
            }
            
            
        }

        async void EditCourse_Clicked(object sender, EventArgs e)
        {
            var selectedCourse = (Course)courseListView.SelectedItem;

            if (selectedCourse != null)
            {
                await Navigation.PushAsync(new CourseViewPage(course) { BindingContext = selectedCourse as Course });
            }
            else
            {
                await DisplayAlert("Error!", "Please Select a Course.", "Ok");
            }
        }

        async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var selectedCourse = (Course)courseListView.SelectedItem;

            if (selectedCourse != null)
            {
                using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
                {
                    var courseAssessments = con.Query<Course>($"Select * from Assessment where CourseID = {selectedCourse.CourseID}");
                    
                    if(courseAssessments.Count == 0)
                    {
                        bool deleteCourse = await DisplayAlert("Confirm!", $"Do you wish to delete the '{selectedCourse.CourseName}' course?", "Yes", "No");
                        if (deleteCourse)
                        {
                            con.Execute($"Delete from Course Where {selectedCourse.CourseID} = CourseID ");
                            Page new1 = new TermViewPage(selectedTerm) { BindingContext = selectedTerm as Term };
                            //new1.BindingContext = term as Term;
                            Navigation.InsertPageBefore(new1, this);
                            await Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        await DisplayAlert("Unable to Delete!", $"Course has {courseAssessments.Count} assessment(s) associated with it!", "Ok");
                    }

                }
            }
            else
            {
                await DisplayAlert("Error!", "Please Select a Course to Remove!", "Ok");
            }           
        }

        private bool NullCheck()
        {
            if (!String.IsNullOrEmpty(termEntry.Text))
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
                return true;
            }
            DisplayAlert("Error!", "Start Date must be greater than End Date!", "OK");
            return false;
        }

    }
}