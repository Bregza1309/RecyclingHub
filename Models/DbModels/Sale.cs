using System.ComponentModel.DataAnnotations;
namespace RecyclingHub.Models.DbModels
{
    public class Sale
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Customer Name is Required")]
        public string CustomerName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string PricePerKg { get; set; } = string.Empty;
        public double WeightInKgs { get; set; } 
        public double GrossWeight { get; set; }
        public double TareWeight { get; set; }
        public double CashAmount { get; set; }
        public DateTime Date {  get; set; } = DateTime.Now;
        public string CompanyName { get; set; } = string.Empty;
    }
}
