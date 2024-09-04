namespace RecyclingHub.Models.DbModels
{
    public class Disposal
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public string MannerOfVerification { get; set; } = string.Empty;
        public string TelephoneNumbers { get; set; } = string.Empty;
        public string ContactAddress { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string IntermediaryFullName { get; set; } = string.Empty;
        public string IntermediarySignature { get; set; } = string.Empty;
        public string MannerOfDisposal { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

    }
}
