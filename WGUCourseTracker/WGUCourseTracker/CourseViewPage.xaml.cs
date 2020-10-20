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
    public partial class CourseViewPage: ContentPage
    {
        public CourseViewPage()
        {
            InitializeComponent();
        }

        private void AddAssessment_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssessmentViewPage());
        }

        private void AddInstructor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddInstructorPage());
        }

        private void SaveCourse_Clicked(object sender, EventArgs e)
        {

        }
    }
}