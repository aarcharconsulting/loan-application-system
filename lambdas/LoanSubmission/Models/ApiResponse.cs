namespace LoanSubmission.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class ApiResponse
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string ResponseId { get; set; }
        public int ProviderId { get; set; }

        public string ProviderName { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string RequestId { get; set; }

        public string[] ErrorMessages { get; set; }
        public LoanResponse ResponseData { get; set; }
    }
}