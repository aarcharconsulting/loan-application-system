using Amazon.SQS;
using LoanSubmission.Interfaces;
using LoanSubmission.Models;
using Microsoft.Extensions.Options;

namespace LoanSubmission.Services
{
    public class SqsServiceFactory : ISqsServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<SqsServiceOptions> _sqsQueuesConfig;

        public SqsServiceFactory(IServiceProvider serviceProvider, IOptions<SqsServiceOptions> sqsQueuesConfig)
        {
            _serviceProvider = serviceProvider;
            _sqsQueuesConfig = sqsQueuesConfig;
        }

        public ISqsService GetSqsService(string name)
        {

            var queueUrl = name switch
            {
                "RequestQueue" => _sqsQueuesConfig.Value.RequestQueueUrl,
                "ResponseQueue" => _sqsQueuesConfig.Value.ResponseQueueUrl,
                _ => throw new ArgumentException($"Unknown SQS service name: {name}.", nameof(name))
            };

            var sqsClient =  new AmazonSQSClient(new AmazonSQSConfig { ServiceURL = queueUrl });


            return new SqsService(sqsClient, queueUrl);
        }
    }

}
