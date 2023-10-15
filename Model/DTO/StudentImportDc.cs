using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Model
{
    public class StudentImportDc
    {
        public int StudentId { get; set; }=0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Mobile { get; set; }
        public string EnrollmentDate { get; set; }
        public string AcademicYear { get; set; }
        public string Grade { get; set; }
        public string Division { get; set; }
        public string Parent { get; set; }
        public string ParentNumber { get; set; }
    }
}
