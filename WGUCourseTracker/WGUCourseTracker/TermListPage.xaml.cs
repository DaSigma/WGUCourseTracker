using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void NewItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void EditItem_Clicked(object sender, EventArgs e)
        {

        }

    }
}