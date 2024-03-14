using LoanProcessor.Models;

namespace LoanProcessor.Interface
{
    public interface ISqsService
    {
        Task SendMessageAsync(ApiResponse response);

        Task DeleteMessageAsync(string receiptHandle);
    }
}
