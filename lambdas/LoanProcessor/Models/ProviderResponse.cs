namespace LoanProcessor.Models
{
    public class ProviderResponse
    {
        public bool Success { get;  set; }
        public LoanDetails LoanDetails { get;  set; }
        public int ProviderId { get; set; }
        public string RequestId { get; set; }

        public string ProviderName { get; set; }
    }
}