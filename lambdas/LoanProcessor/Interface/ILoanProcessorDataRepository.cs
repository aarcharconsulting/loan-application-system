using LoanProcessor.Models;

namespace LoanProcessor.Interface
{
    public interface ILoanProcessorDataRepository
    {
        Task<ProviderDetails> GetProviderDetailsByNameAsync(string providerName);
        Task SaveApiResponseAsync(ApiResponse responseDetails);
    }
}
