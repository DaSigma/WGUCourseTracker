using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUCourseTracker.Model;
using WGUCourseTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUCourseTracker
{

    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class TermListPage : ContentPage
    {
        ObservableCollection<Course> courses;
        ObservableCollection<Assessment> assessments;
        bool start = true;

        public TermListPage()
        {
            InitializeComponent();
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            termsListView.SelectedItem = null;

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Term>();

                var termList = con.Table<Term>().ToList();
                termsListView.ItemsSource = termList;

                con.CreateTable<Course>();
                var courseTable = con.Table<Course>().ToList();
                courses = new ObservableCollection<Course>(courseTable);

                con.CreateTable<Assessment>();
                var asssessmentTable = con.Table<Assessment>().ToList();
                assessments = new ObservableCollection<Assessment>(asssessmentTable);
                
            }

            if (start)
            {
                start = false;
                CourseStartNotify();
                AssessmentStartNotify();
            }

        }
        private void AddTerm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermAddPage());
        }

        async void EditTerm_Clicked(object sender, EventArgs e)
        {
            
            if (termsListView.SelectedItem != null)
            {
                var selectedTerm = (Term)termsListView.SelectedItem;

                await Navigation.PushAsync(new TermViewPage { BindingContext = selectedTerm as Term });
            }
            else
            {
                await DisplayAlert("Error!", "Please Select a Term to View/Edit!", "Ok");
            }

        }

        async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var selectedTerm = (Term)termsListView.SelectedItem;

            if(selectedTerm != null)
            {
                using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
                {
                    bool deleteTerm = await DisplayAlert("Confirm!", $"Do you wish to delete '{selectedTerm.TermName}'?", "Yes", "No");
                    if (deleteTerm)
                    {
                        con.Execute($"Delete from Term Where {selectedTerm.TermID} = TermID ");
                        Page new1 = new TermListPage();
                        Navigation.InsertPageBefore(new1, this);
                        await Navigation.PopAsync();
                    }
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Error!", "Please Select a Term to Remove!", "Ok");
            }
            

        }
        private void CourseStartNotify()
        {
            int notifynumber = 0;
            foreach (Course course in courses)
            {
                notifynumber++;

                if (course.CourseStartDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Course Start Reminder", $"{course.CourseName} begins today.", notifynumber);
                }

                if (course.CourseEndDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Course Ending Reminder", $"{course.CourseName} finishes today.",notifynumber);
                }
            }

        }

        private void AssessmentStartNotify()
        {
            int notifynumber = 0;
            foreach (Assessment assessment in assessments)
            {
                notifynumber++;

                if (assessment.AssessmentDueDate == DateTime.Today)
                {
                    CrossLocalNotifications.Current.Show("Assessment Start Reminder", $"{assessment.AssessmentName} begins today.", notifynumber);
                }
            }

        }
    }
}