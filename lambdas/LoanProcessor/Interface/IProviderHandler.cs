using LoanProcessor.Models;
using LoanProcessor.Models;

namespace LoanProcessor.Interface
{
    public interface IProviderHandler
    {
        Task<ProviderResponse> GetAsync(ProviderDetails providerDetails, int requestedAmount, int loanDurationYears);
    }
}