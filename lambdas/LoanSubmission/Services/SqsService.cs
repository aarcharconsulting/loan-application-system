using Amazon.SQS;
using Amazon.SQS.Model;
using LoanSubmission.Interfaces;

namespace LoanSubmission.Services
{
    public class SqsService : ISqsService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;

        public SqsService(IAmazonSQS client, string url)
        {
            _queueUrl = url;
            _sqsClient = client;
        }

        public async Task DeleteMessageAsync(string receiptHandle)
        {
            await _sqsClient.DeleteMessageAsync(new DeleteMessageRequest()
            {
                QueueUrl = _queueUrl,
                ReceiptHandle = receiptHandle
            });
        }

        public async Task<ReceiveMessageResponse> ReceiveMessageAsync()
        {

            return await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 0 
            });
        }

        public async Task SendMessageAsync(string messageBody)
        {
            await _sqsClient.SendMessageAsync(new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = messageBody
            });
        }
    }
}
