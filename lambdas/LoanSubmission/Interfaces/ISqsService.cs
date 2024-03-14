using Amazon.SQS.Model;

namespace LoanSubmission.Interfaces
{
    public interface ISqsService
    {
        Task DeleteMessageAsync(string receiptHandle);
        Task<ReceiveMessageResponse> ReceiveMessageAsync();
        Task SendMessageAsync(string messageBody);
    }
}