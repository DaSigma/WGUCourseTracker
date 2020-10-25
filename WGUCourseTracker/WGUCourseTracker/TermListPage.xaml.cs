﻿using SQLite;
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
            termsListView.SelectedItem = null;

            using (SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                con.CreateTable<Term>();

                var termList = con.Table<Term>().ToList();
                termsListView.ItemsSource = termList;
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
    }
}