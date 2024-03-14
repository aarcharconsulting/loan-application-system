using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LoanSubmission.Models;

public class LoanApplication
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public LoanDetails LoanDetails { get; set; }
    public PersonalDetails PersonalDetails { get; set; }
    public FinanceDetails FinanceDetails { get; set; }
}