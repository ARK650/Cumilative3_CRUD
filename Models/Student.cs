using System;
using System.ComponentModel.DataAnnotations.Schema;  
namespace Cumilative3_CRUD.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentFName { get; set; }
        public string StudentLName { get; set; }
        public string StudentNumber { get; set; }

       
        [Column("enroldate")]
        public DateTime EnrollDate { get; set; }

        public string FullName => $"{StudentFName} {StudentLName}";
    }
}
