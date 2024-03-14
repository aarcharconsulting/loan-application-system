using LoanProcessor.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Lambda.Annotations;
using LoanProcessor.Repository;
using Microsoft.Extensions.Options;
using LoanProcessor.Config;
using LoanProcessor.Interface;
using LoanProcessor.Services;
using LoanProcessor.Models;


namespace LoanProcessor
{
    [LambdaStartup]
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoDbConfiguration>(Configuration.GetSection("MongoDb"));


            services.AddTransient<ILoanProcessorDataRepository>(provider =>
                new MongoDbRepository(provider.GetRequiredService<IOptions<MongoDbConfiguration>>().Value )
           );

            services.AddTransient<IProviderHandler, ApiProviderHandler>();

            services.Configure<SqsServiceOptions>(Configuration.GetSection("SqsService"));


            services.AddTransient<ISqsService, SqsService>();

            services.AddSingleton<ILoanProcessorService, LoanProcessorService>();

        }
    }
}