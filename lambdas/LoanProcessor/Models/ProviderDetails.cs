namespace LoanProcessor.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using ThirdParty.Json.LitJson;

    public class ProviderDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string ApiEndpoint { get; set; }
    }
}