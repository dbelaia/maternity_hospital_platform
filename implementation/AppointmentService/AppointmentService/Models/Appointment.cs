using System.ComponentModel.DataAnnotations;

namespace AppointmentService.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { set; get; }
        public String AppointmentName { set; get; }
        public Decimal Price { set; get; }
    }
}
