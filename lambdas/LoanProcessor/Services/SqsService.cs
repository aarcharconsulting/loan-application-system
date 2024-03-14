using Amazon.Runtime;
using Amazon.SQS.Model;
using Amazon.SQS;
using LoanProcessor.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using LoanProcessor.Models;

namespace LoanProcessor.Services
{
    public class SqsService : ISqsService
    {
        private readonly AmazonSQSClient _sqsClient;
        private readonly string _queueUrl;

        public SqsService(IOptions<SqsServiceOptions> options)
        {
            var settings = options.Value;
            _queueUrl = settings.QueueUrl;

            var awsCreds = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
            _sqsClient = new AmazonSQSClient(awsCreds, new AmazonSQSConfig { ServiceURL = _queueUrl });
        }

        public async Task SendMessageAsync(ApiResponse response)
        {
            if (!string.IsNullOrEmpty(_queueUrl) && _sqsClient != null)
            {
                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = _queueUrl,
                    MessageBody = System.Text.Json.JsonSerializer.Serialize(response)
                };
                await _sqsClient.SendMessageAsync(sendMessageRequest);
            }
        }
    }
}
