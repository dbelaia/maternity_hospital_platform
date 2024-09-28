using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace MaternityMicroservices.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { set; get; }
        public String FirstName { set; get; }
        public String LastName { set; get; }
        public String Sex { set; get; }
        public String DateOfBirth { set; get; }
    }
}
