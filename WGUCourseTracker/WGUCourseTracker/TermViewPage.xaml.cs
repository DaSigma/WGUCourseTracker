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
        Term term;
        Course course;

        public TermViewPage()
        {
            InitializeComponent();
            term = (Term)mainStackLayout.BindingContext;
            var selectedCourse = (Course)courseListView.SelectedItem;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            courseListView.SelectedItem = null;


            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                //course = (Course)courseStackLayout.BindingContext;
                term = (Term)mainStackLayout.BindingContext;
                var termID = term.TermID;

                con.CreateTable<Course>();
                var courseTable = con.Table<Course>().ToList();

                var courseList = (from course in courseTable
                                  where course.TermID == term.TermID
                                  select course);        
               
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
                    con.Update(term);
                    await DisplayAlert("Success!", $"{term.TermName} Saved", "Ok");
                    Page new1 = new TermListPage();
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

                await Navigation.PushAsync(new CourseAddPage { BindingContext = selectedTerm as Term });
        }

        async void EditCourse_Clicked(object sender, EventArgs e)
        {
            var selectedCourse = (Course)courseListView.SelectedItem;

            if (selectedCourse != null)
            {
                await Navigation.PushAsync(new CourseViewPage(course) { BindingContext = selectedCourse as Course });
                //await Navigation.PushAsync(new CourseViewPage { BindingContext = selectedCourse as Course });
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
                    bool deleteCourse = await DisplayAlert("Confirm!", $"Do you wish to delete the '{selectedCourse.CourseName}' course?", "Yes", "No");
                    if (deleteCourse)
                    {
                        con.Execute($"Delete from Course Where {selectedCourse.CourseID} = CourseID ");
                        Page new1 = new TermViewPage();
                        new1.BindingContext = term as Term;
                        Navigation.InsertPageBefore(new1, this);
                        await Navigation.PopAsync();
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