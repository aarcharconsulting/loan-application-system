using LoanSubmission.Interfaces;
using LoanSubmission.Models;
using MongoDB.Driver;


namespace LoanSubmission.Repository
{
    public class MongoDbRepository : ILoanSubmissionDataRepository
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        public MongoDbRepository(MongoDbConfiguration options)
        {
            _mongoClient = new MongoClient(options.ConnectionString);
            _database = _mongoClient.GetDatabase(options.DatabaseName);
        }


        public async Task<List<ProviderDetails>> GetProviders()
        {
            var collection = _database.GetCollection<ProviderDetails>("Providers");
            var filter = Builders<ProviderDetails>.Filter.Empty;
            var providers = await collection.Find(filter).ToListAsync();
            return providers;
        }

        public async Task<string> SaveLoanRequest(LoanApplication loanApplication)
        {
            var collection = _database.GetCollection<LoanApplication>("LoanRequests");
            await collection.InsertOneAsync(loanApplication);
            return loanApplication.Id.ToString();
        }
        public async Task<List<ApiResponse>> GetResponsesByRequestId(string requestId)
        {
            var collection = _database.GetCollection<ApiResponse>("responses");
            var filter = Builders<ApiResponse>.Filter.Eq(r => r.RequestId, requestId);
            var responses = await collection.Find(filter).ToListAsync();
            return responses;
        }

    }
}
