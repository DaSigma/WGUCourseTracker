using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUCourseTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUCourseTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermViewPage : ContentPage
    {
        Term term;
        public TermViewPage()
        {
            InitializeComponent();
            
        }
        async void SaveTerm_Clicked(object sender, EventArgs e)
        {

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Term>();

                if (startDatePicker.Date < endDatePicker.Date && termEntry != null)
                {
                    term = (Term)mainStackLayout.BindingContext;
                    con.Update(term);
                    await DisplayAlert("Success!", $"{term.TermName} Updated", "Ok");
                    Page new1 = new TermListPage();
                    Navigation.InsertPageBefore(new1, this);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Term not Created", "Ok");
                }
            }
        }

        private void NewCourse_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CoursePage());
        }

        private void EditCourse_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CoursePage());

        }
    }
}