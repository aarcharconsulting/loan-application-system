
namespace LoanProcessor.Models
{
    public class ProcessRequest
    {
        public int ProviderId { get; set; }
        public string? ProviderName { get; set; }
        public string RequestId { get; set; }
        public int RequestedAmount { get; set; }
        public int LoanDurationYears { get; set; }
        public string LoanRequest { get; set; }
    }
}