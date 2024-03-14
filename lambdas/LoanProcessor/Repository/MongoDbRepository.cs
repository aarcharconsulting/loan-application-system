using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanProcessor.Config;
using LoanProcessor.Interface;
using LoanProcessor.Models;

namespace LoanProcessor.Repository
{
    public class MongoDbRepository : ILoanProcessorDataRepository
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _database;
        public MongoDbRepository(MongoDbConfiguration options)
        {
            _mongoClient = new MongoClient(options.ConnectionString);
            _database = _mongoClient.GetDatabase(options.DatabaseName);
        }
        public async Task<ProviderDetails> GetProviderDetailsByNameAsync(string providerName)
        {
            var collection = _database.GetCollection<ProviderDetails>("Providers");
            var filter = Builders<ProviderDetails>.Filter.Eq("Name", providerName);
            var provider = await collection.Find(filter).FirstOrDefaultAsync();
            return provider;
        }


        public async Task SaveApiResponseAsync(ApiResponse responseDetails)
        {
            var collection = _database.GetCollection<ApiResponse>("responses");
            await collection.InsertOneAsync(responseDetails);
        }
    }
}
