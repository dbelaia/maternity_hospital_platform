using System.ComponentModel.DataAnnotations;

namespace AnalyticsService.Models
{
    public class Operation
    {
        [Key]
        public int OperationID { set; get; }
        public String OperationName { set; get; }
        public Decimal Price { set; get; }
    }
}
