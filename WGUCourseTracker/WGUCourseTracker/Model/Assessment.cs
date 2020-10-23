using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WGUCourseTracker.Model
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentID { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentType { get; set; }
        public DateTime AssessmentDueDate { get; set; }
        public int CourseID { get; set; }
    }
}
