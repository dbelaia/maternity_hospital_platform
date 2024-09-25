using System.ComponentModel.DataAnnotations;

namespace MaternityMicroservices.Models
{
    public class AppointmentStatus
    {
        [Key]
        public int AppointmentStatusID { set; get; }
        public String Status { set; get; }
    }
}
