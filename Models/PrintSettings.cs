namespace PayrollPrinterApp.Models
{
    public class PrintSettings 
    { 
        public int MarginLeft { get; set; } = 40; 
        public int MarginTop { get; set; } = 60; 
        public int LineHeight { get; set; } = 14; 
        public double PageWidthInches { get; set; } = 7.5; 
        public double PageHeightInches { get; set; } = 3.25; // Employer header info 
        public string EmployerTaxId { get; set; } = "2249-03"; 
        public string EmployerName { get; set; } = "COM-FSM"; 
        public string EmployerAddress { get; set; } = "P.O. Box 159"; 
    }
}
