//using Amazon.DynamoDBv2.DataModel;
//using Amazon.DynamoDBv2;
//using LoanProcessor.Interface;
//using LoanProcessor.Models;
//using Amazon.DynamoDBv2.DocumentModel;

//namespace LoanProcessor.Repository
//{
//    public class DynamoDbRepository : ILoanDataRepository
//    {
//        private readonly DynamoDBContext _context;


//        public DynamoDbRepository()
//        {
//            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
//            clientConfig.ServiceURL = "http://localhost:4566";
//            AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);

//            _context = new DynamoDBContext(client);
//        }

//        public async Task<ProviderDetails> GetProviderDetailsByNameAsync(string providerName)
//        {
//            var scanConditions = new List<ScanCondition>
//            {
//                new ScanCondition(nameof(ProviderDetails.Name), ScanOperator.Equal, providerName)
//            };

//            var results = await _context.ScanAsync<ProviderDetails>(scanConditions).GetRemainingAsync();
//            return results.FirstOrDefault();
//        }



//        public async Task SaveApiResponseAsync(ApiResponseDetails responseDetails)
//        {
//            await _context.SaveAsync(responseDetails);
//        }
//    }
//}


////    public class DynamoDbRepository : ILoanDataRepository
////    {
////        private readonly IAmazonDynamoDB client;

////        public DynamoDbRepository()
////        {
////            var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://localhost:4566" };
////            client = new AmazonDynamoDBClient(clientConfig);
////        }

////        public async Task<ProviderDetails> GetProviderDetailsByNameAsync(string providerName)
////        {
////            var request = new ScanRequest
////            {
////                TableName = "Providers",
////                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
////            {
////                {":v_providerName", new AttributeValue { S = providerName }}
////            },
////                FilterExpression = "Name = :v_providerName"
////            };

////            var response = await client.ScanAsync(request);
////            if (response.Items.Count > 0)
////            {
////                var item = response.Items.First();
////                return new ProviderDetails
////                {
////                    // Assuming these are your item attributes, adjust accordingly
////                    ObjectId = item["ObjectId"].S,
////                    ProviderId = int.Parse(item["ProviderId"].N),
////                    Name = item["Name"].S,
////                    ApiKey = item["ApiKey"].S,
////                    ApiEndpoint = item["ApiEndpoint"].S
////                };
////            }
////            return null; // Or throw an appropriate exception
////        }

////        public async Task SaveApiResponseAsync(ApiResponseDetails responseDetails)
////        {
////            var item = new Dictionary<string, AttributeValue>
////        {
////            // Assuming these are your ApiResponseDetails attributes, adjust accordingly
////            {"ResponseId", new AttributeValue { S = responseDetails.ResponseId }},
////            {"ProviderId", new AttributeValue { N = responseDetails.ProviderId.ToString() }},
////            {"Data", new AttributeValue { S = responseDetails.Data }},
////            // Add other attributes here
////        };

////            var request = new PutItemRequest
////            {
////                TableName = "ApiResponses",
////                Item = item
////            };

////            await client.PutItemAsync(request);
////        }
////    }

////}