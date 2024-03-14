
using LoanSubmission.Models ;   


namespace LoanSubmission.Interfaces
{
    public interface ILoanSubmissionDataRepository
    {
        Task<List<ProviderDetails>> GetProviders();
        Task<string> SaveLoanRequest(LoanApplication loanApplication);

        Task<List<ApiResponse>> GetResponsesByRequestId(string requestId);
    }
}