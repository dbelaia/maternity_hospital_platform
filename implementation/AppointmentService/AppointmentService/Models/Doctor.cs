using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentService.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Occupation { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
