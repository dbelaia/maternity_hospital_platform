﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentService.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { set; get; }
        public String FirstName { set; get; }
        public String LastName { set; get; }
        public String Sex { set; get; }
        public DateTime DateOfBirth { set; get; }
    }
}
