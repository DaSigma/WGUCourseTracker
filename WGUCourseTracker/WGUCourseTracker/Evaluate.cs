using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using WGUCourseTracker.Model;
using WGUCourseTracker.Models;

namespace WGUCourseTracker
{
    public class Evaluate
    {
        public static void AddData()
        {
            using(SQLiteConnection con = new SQLiteConnection(App.DbLocation))
            {
                //Evalluation data          
                Term myTerm = new Term();
                myTerm.TermName = "Spring Term";
                myTerm.TermStartDate = DateTime.Today;
                myTerm.TermEndDate = DateTime.Today.AddMonths(6);
                con.Insert(myTerm);

                Course myCourse = new Course();
                myCourse.CourseName = "Mobile App Dev";
                myCourse.CourseStartDate = DateTime.Today;
                myCourse.CourseEndDate = DateTime.Today.AddDays(30);
                myCourse.CourseStatus = "Active";
                myCourse.CourseNotes = "This is a great course";
                myCourse.InstructorName = "Delvon Fontenot";
                myCourse.InstructorPhone = "337-418-0664";
                myCourse.InstructorEmail = "dfonte3@wgu.edu";
                myCourse.TermID = myTerm.TermID;
                con.Insert(myCourse);

                Assessment myAssessment = new Assessment();
                myAssessment.AssessmentName = "Mobile App Performance";
                myAssessment.AssessmentType = "Performance";
                myAssessment.AssessmentDueDate = DateTime.Today;
                myAssessment.CourseID = myCourse.CourseID;
                myAssessment.notify = true;
                con.Insert(myAssessment);

                Assessment myAssessment2 = new Assessment();
                myAssessment2.AssessmentName = "Mobile App Objective";
                myAssessment2.AssessmentType = "Objective";
                myAssessment2.AssessmentDueDate = DateTime.Today;
                myAssessment2.CourseID = myCourse.CourseID;
                myAssessment2.notify = false;
                con.Insert(myAssessment2);
            }

        }



    }
}
