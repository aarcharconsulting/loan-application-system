using LoanProcessor.Models;
using LoanProcessor.Models;
using LoanProcessor.Interface;

namespace LoanProcessor.Implementation
{
    public class ApiProviderHandler : IProviderHandler
    {
        // todo: inject http client factory to make actual api calls
        private readonly Random _random = new Random(); 

        public ApiProviderHandler()
        {
        }

        public async Task<ProviderResponse> GetAsync(ProviderDetails providerDetails, int requestedAmount, int loanDurationYears)
        {
            await Task.Delay(_random.Next(100, 201));
         
            var interestRate = _random.NextDouble() * 5 + 5; 
            var monthlyInterestRate = interestRate / 12 / 100;

            var monthlyPayment = CalculateMonthlyPayment(requestedAmount, interestRate, loanDurationYears);
            var totalInterest = monthlyPayment * loanDurationYears * 12 - requestedAmount;

            var loanDetails =  new LoanDetails
            {
                RequestedAmount = requestedAmount,
                LoanDurationYears = loanDurationYears,
                InterestRate = interestRate,
                MonthlyInterestRate = monthlyInterestRate,
                MonthlyPayment = monthlyPayment,
                TotalInterest = totalInterest,
                TotalAmount = requestedAmount + totalInterest,
                
            };

            return new ProviderResponse()
            {
                Success = true,
                LoanDetails = loanDetails,
                ProviderId = providerDetails.ProviderId,
                ProviderName = providerDetails.Name
            };
        }

     

        private double CalculateMonthlyPayment(double principle, double interestRate, int durationYears)
        {
            var monthlyRate = interestRate / 100 / 12;
            var durationMonths = durationYears * 12;
            return principle * monthlyRate / (1 - Math.Pow(1 + monthlyRate, -durationMonths));
        }
    }
}
