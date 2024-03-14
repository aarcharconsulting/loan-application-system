namespace LoanProcessor.Config;

public class DynamoDBConfiguration
{
    public string ServiceURL { get; set; }
    public string Region { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}