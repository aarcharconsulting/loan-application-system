using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LoanSubmission.Interfaces;
using LoanSubmission.Models;
using Amazon.SQS.Model;
using MongoDB.Bson.IO;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LoanSubmission
{
    public class LoanSubmissionFunction
    {
        private static readonly IServiceProvider ServiceProvider;
        private readonly ILoanSubmissionDataRepository _repository;
        private readonly ISqsServiceFactory _sqsServiceFactory;

        static LoanSubmissionFunction()
        {
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var startup = new Startup(configuration);
            startup.ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public LoanSubmissionFunction()
        {
            _repository = ServiceProvider.GetService<ILoanSubmissionDataRepository>();
            _sqsServiceFactory = ServiceProvider.GetService<ISqsServiceFactory>();
        }

        public async Task<APIGatewayProxyResponse> LoanSubmissionFunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogInformation($"Received input: {request.Body}");

            var loanApplication = JsonSerializer.Deserialize<LoanApplication>(request.Body);

            return await SubmitLoanRequest(context, loanApplication);
        }

        private async Task<APIGatewayProxyResponse> SubmitLoanRequest(ILambdaContext context, LoanApplication? loanApplication)
        {
            try
            {
                if (loanApplication != null)
                {
                    context.Logger.LogInformation("Loan application is not null.");

                    var requestQueueSqsService = _sqsServiceFactory.GetSqsService("RequestQueue");

                    var providers = await _repository.GetProviders();

                    var requestId = await _repository.SaveLoanRequest(loanApplication);

                    context.Logger.LogInformation($"SaveLoanRequest successful. Request ID: {requestId}");

                    foreach (var provider in providers)
                    {
                        string message = JsonSerializer.Serialize(new ProcessLoanRequest
                        {
                            RequestId = requestId,
                            ProviderId = provider.ProviderId,
                            ProviderName = provider.Name,
                            LoanDurationYears = loanApplication.LoanDetails.Years,
                            RequestedAmount = loanApplication.LoanDetails.Amount

                        });
                        await requestQueueSqsService.SendMessageAsync(message);
                    }

                    context.Logger.LogInformation("Completed processing.");

                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 200,
                        Body = JsonSerializer.Serialize(new { RequestId = requestId })
                    };
                }
                else
                {
                    var message = "Loan application cannot be null";
                    context.Logger.LogError($"Validation error: {message}");

                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 400,
                        Body = JsonSerializer.Serialize(new { Messages = new[] { message } })
                    };
                }
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error processing input: {ex.Message}");
                throw;
            }
        }

        public async Task<APIGatewayProxyResponse> LoanCheckerFunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                context.Logger.LogInformation($"Received input: {JsonSerializer.Serialize(request)}");

                if (!request.QueryStringParameters.TryGetValue("guid", out var requestId))
                {
                    var message = "Missing or invalid 'guid' query parameter.";
                    context.Logger.LogError(message + " Request: " + JsonSerializer.Serialize(request));

                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 400,
                        Body = JsonSerializer.Serialize(new { Message = message })
                    };
                }


                context.Logger.LogInformation($"trying to fetch messages");

                var response = await _repository.GetResponsesByRequestId(requestId);

                if (response != null && response.Any())
                {
                    context.Logger.LogInformation($"Total messages present - {response?.Count ?? 0}");
                    
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = 200,
                        Body = JsonSerializer.Serialize(response)
                    };
                }
                context.Logger.LogWarning("No message present for GUID: " + requestId + ". Returning 204.");
                return new APIGatewayProxyResponse
                {
                    StatusCode = 204
                };
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error processing input: {ex.Message}. inner exception - {ex.InnerException?.Message}");

                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = JsonSerializer.Serialize(new { Error = "An error occurred while processing your request." })
                };
            }
        }

        private static ApiResponse? TryParseApiResponse(Message? message, ILambdaContext context)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiResponse>(message?.Body);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error deserializing API response: {ex.ToString()} Message: {message?.Body}");
                return null;
            }
        }
        
    }
}
