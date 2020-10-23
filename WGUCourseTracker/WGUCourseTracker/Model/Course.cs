using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WGUCourseTracker.Model
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public DateTime  CourseStartDate { get; set; }
        public DateTime CourseEndDate { get; set; }
        public string CourseStatus { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorPhone { get; set; }
        public string CourseNotes { get; set; }
        public int TermID { get; set; }

    }
}
