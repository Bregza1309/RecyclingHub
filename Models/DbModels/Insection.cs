namespace RecyclingHub.Models.DbModels
{
    public class Insection
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Amount { get;set; }
        public double ExpensesAmount { get; set; } = 0;
        public string? ExpensesDescription { get; set; }
        public double SalesTotal { get; set; }
        public double CarryOver { get; set; }
        public DateTime Date {  get; set; } = DateTime.Now;
        public string CompanyName { get; set; } = string.Empty;

    }
}
