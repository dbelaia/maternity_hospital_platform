using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Models
{
    public class OperationHistory
    {
        [Key]
        public int OperationHistoryID { set; get; }
        public int DoctorID { set; get; }
        public int OperationID { set; get; }
        public int PatientID { set; get; }
        public DateTime StartDateTime { set; get; }
        public DateTime EndDateTime { set; get; }
    }
}
