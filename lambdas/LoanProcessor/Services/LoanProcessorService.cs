using Amazon.Lambda.SQSEvents;
using LoanProcessor.Models;
using LoanProcessor.Interface;
using System.Text.Json;
using Amazon.Lambda.Core;

namespace LoanProcessor.Services
{
    public class LoanProcessorService : ILoanProcessorService
    {
        private readonly IProviderHandler _providerHandler;
        private readonly ILoanProcessorDataRepository _repository;
        private readonly ISqsService _sqsService;

        public LoanProcessorService(IProviderHandler providerHandler, ILoanProcessorDataRepository repository, ISqsService sqsService)
        {
            _providerHandler = providerHandler;
            _repository = repository;
            _sqsService = sqsService;
        }

        public async Task ProcessLoanRequestAsync(SQSEvent sqsEvent, ILambdaContext context)
        {
            context.Logger.LogInformation($"ProcessLoanRequestAsync executing with records count - {sqsEvent.Records.Count}");
            foreach (var record in sqsEvent.Records)
            {
                try
                {
                    context.Logger.LogInformation($"ProcessLoanRequestAsync record body - {record.Body}");

                    var snsMessage = JsonSerializer.Deserialize<ProcessRequest>(record.Body);
                    if (snsMessage == null || snsMessage.RequestedAmount < 1 || snsMessage.LoanDurationYears < 1 || string.IsNullOrEmpty(snsMessage.ProviderName) || string.IsNullOrEmpty(snsMessage.RequestId))
                    {
                        context.Logger.LogError($"ProcessLoanRequestAsync required parameters are not completed properly for record - {record.Body}");
                        throw new ArgumentNullException("Please fill all required parameters");
                    }

                    ProviderDetails providerDetails = await _repository.GetProviderDetailsByNameAsync(snsMessage.ProviderName);
                    ProviderResponse providerResponse = await _providerHandler.GetAsync(providerDetails, snsMessage.RequestedAmount, snsMessage.LoanDurationYears);

                    if (providerResponse.Success)
                    {
                        var apiResponse = new ApiResponse
                        {
                            ResponseId = Guid.NewGuid().ToString(),
                            ProviderId = providerResponse.ProviderId,
                            ResponseData = providerResponse.LoanDetails,
                            ProviderName = providerResponse.ProviderName,
                            RequestId = snsMessage.RequestId
                        };

                        context.Logger.LogInformation($"ProcessLoanRequestAsync - provider response is  success , response id = {apiResponse.ResponseId}");

                        await _repository.SaveApiResponseAsync(apiResponse);

                       // await _sqsService.SendMessageAsync(apiResponse);

                        context.Logger.LogInformation($"ProcessLoanRequestAsync - save success, send message to response queue succcess");

                    }
                    else
                    {
                        context.Logger.LogError("ProcessLoanRequestAsync - provider response is not success");
                    }
                }catch(Exception ex)
                {
                    context.Logger.LogError($"ProcessLoanRequestAsync error occured  {ex.Message}. inner exception ${ex.InnerException?.Message}");
                }
            }
        }
    }
}
