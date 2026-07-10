using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int UserID { get; set; }
        public string EmployeeName { get; set; }
        public string Role { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; } // Present, Absent, Leave
    }

    // Form se list submit karne ke liye ViewModel
    public class AttendanceSubmitViewModel
    {
        public DateTime AttendanceDate { get; set; }
        public List<Attendance> AttendanceList { get; set; }
    }
}