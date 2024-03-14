using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LoanProcessor.Interface;
using Amazon.Lambda.SQSEvents;


[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LoanProcessor
{
    public class LoanProcessorFunction
    {
        private readonly ILoanProcessorService _loanProcessorService;
        private static readonly IServiceProvider ServiceProvider;


        static LoanProcessorFunction()
        {

            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appSettings.json")
                                .Build();

            var startup = new Startup(configuration);
            startup.ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
        public LoanProcessorFunction()
        {
            
            _loanProcessorService = ServiceProvider?.GetService<ILoanProcessorService>();
        }

        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            context.Logger.LogInformation($"triggering loan processor function");

            await _loanProcessorService?.ProcessLoanRequestAsync(sqsEvent, context);
        }
    }
}


