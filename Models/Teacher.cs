using Microsoft.AspNetCore.Mvc;

namespace Cumilative3_CRUD.Models
{
 
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherFName { get; set; }
        public string TeacherLName { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string FullName => $"{TeacherFName} {TeacherLName}";
    }
}
