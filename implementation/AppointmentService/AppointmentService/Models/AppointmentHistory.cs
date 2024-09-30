using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace AppointmentService.Models
{
    public class AppointmentHistory
    {
        [Key]
        public int AppointmentHistoryID { set; get; }
        public int PatientID { set; get; }
        public int DoctorID { set; get; }
        public DateTime DateSelected { set; get; }
        public int StatusID { set; get; }
    }
}
