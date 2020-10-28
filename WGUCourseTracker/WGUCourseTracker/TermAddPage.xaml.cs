using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGUCourseTracker.Models;
using Xamarin.Forms;
using WGUCourseTracker.Model;
using SQLite;

namespace WGUCourseTracker
{
    public partial class TermAddPage : ContentPage
    {
        public TermAddPage()
        {
            InitializeComponent();           
        }
        async void SaveTerm_Clicked(object sender, EventArgs e)
        {
            Term term = new Term()
            {
                TermName = termEntry.Text,
                TermStartDate = startDatePicker.Date,
                TermEndDate = endDatePicker.Date
            };

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Term>();

                if (DateCheck() && NullCheck())
                {
                    con.Insert(term);
                    await DisplayAlert("Success!", $"{term.TermName} Created", "Ok");
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
