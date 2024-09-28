using System.ComponentModel.DataAnnotations;

namespace MaternityMicroservices.Models
{
    public class AppointmentTypes
    {
        [Key]
        public int AppointmentTypeID { set; get; }
        public String AppointmentTypeDescription { set; get; }
        public Decimal Price { set; get; }
    }
}
