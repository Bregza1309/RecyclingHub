using System.ComponentModel.DataAnnotations;
namespace RecyclingHub.Models.DbModels
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is Required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Product Price is Required")]
        public string Price { get; set; } = string.Empty;
    
        public string Image { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
    }
}
