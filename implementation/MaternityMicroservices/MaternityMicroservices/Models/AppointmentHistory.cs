using System.ComponentModel.DataAnnotations;

namespace MaternityMicroservices.Models
{
    public class AppointmentHistory
    {
        [Key]
        public int AppointmentHistoryRecordID { set; get; }
        public int AppointmentTypeID { set; get; }
        public int PatientID { set; get; }
        public int DoctorID { set; get; }
        public DateOnly DateSelected { set; get; }
        public TimeOnly TimeSelected { set; get; }
        public int StatusID { set; get; }
    }
}
