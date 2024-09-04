namespace RecyclingHub.Models.DbModels
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = new Product();
        public double WeightAvailable { get; set; } = 0;
        public string CompanyName { get; set; } = string.Empty;
    }
}
