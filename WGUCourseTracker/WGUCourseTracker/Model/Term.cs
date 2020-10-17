using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WGUCourseTracker.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int TermID { get; set; }
        public string TermName { get; set; }
        public DateTime TermStartDate { get; set; }
        public DateTime TermEndDate { get; set; }

    }
}
