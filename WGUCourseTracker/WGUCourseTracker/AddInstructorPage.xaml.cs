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
    public partial class AddInstructorPage : ContentPage
    {
        public AddInstructorPage()
        {
            InitializeComponent();
        }

        private void SaveInstructor_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}