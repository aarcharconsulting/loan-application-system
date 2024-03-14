using LoanSubmission.Interfaces;
using LoanSubmission.Models;
using LoanSubmission.Repository;
using LoanSubmission.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LoanSubmission
{
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

            services.Configure<SqsServiceOptions>(Configuration.GetSection("SqsService"));

            services.AddSingleton<ISqsServiceFactory, SqsServiceFactory>();

            services.AddTransient<ILoanSubmissionDataRepository>(provider =>
                     new MongoDbRepository( provider.GetRequiredService<IOptions<MongoDbConfiguration>>().Value )
            );

        

        }
    }
}