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
    public partial class TermListPage : ContentPage
    {

        public TermListPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            using(SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Term>();

                var termList = con.Table<Term>().ToList();
                termsListView.ItemsSource = termList;
            }
            
        }
        private void AddTerm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TermEditPage());
        }

        async void EditTerm_Clicked(object sender, EventArgs e)
        {
            var selectedTerm = (Term)termsListView.SelectedItem;

            await Navigation.PushAsync(new TermViewPage {BindingContext = selectedTerm as Term });

            //using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            //{
            //    Page newMainPage = new TermViewPage();
                
            //    Navigation.InsertPageBefore(newMainPage, this);
            //    //await Navigation.PopAsync();
            //}

        }

        async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                var selectedTerm = (Term)termsListView.SelectedItem;

                bool deleteTerm = await DisplayAlert("Delete?", $"Do you wish to delete '{selectedTerm.TermName}'?","Yes","No");
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
    }
}