namespace PayrollPrinterApp.Models
{
    public class PayrollRecord
    {
        public string SSN { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public decimal FWTax { get; set; }
        public decimal WageTax { get; set; }
    }
}
