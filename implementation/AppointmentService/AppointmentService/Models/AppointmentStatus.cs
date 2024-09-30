using System.ComponentModel.DataAnnotations;

namespace AppointmentService.Models
{
    public class AppointmentStatus
    {
        [Key]
        public int AppointmentStatusID { set; get; }
        public String AppointmentStatusName { set; get; }
    }
}
