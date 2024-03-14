namespace LoanProcessor.Models
{
    public class LoanDetails
    {
        public int RequestedAmount { get;  set; }
        public int LoanDurationYears { get;  set; }
        public double InterestRate { get;  set; }
        public double MonthlyInterestRate { get;  set; }
        public double MonthlyPayment { get;  set; }
        public double TotalInterest { get;  set; }
        public double TotalAmount { get;  set; }
    }
}