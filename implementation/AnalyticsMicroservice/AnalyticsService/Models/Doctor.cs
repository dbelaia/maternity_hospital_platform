﻿using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorID { set; get; }
        public String FirstName { set; get; }
        public String LastName { set; get; }
        public String Sex { set; get; }
        public String Occupation { set; get; }
        public String DateOfBirth { set; get; }

    }
}
