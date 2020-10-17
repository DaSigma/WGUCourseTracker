using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WGUCourseTracker.Model
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int InstructorID { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorPhone { get; set; }

    }
}
