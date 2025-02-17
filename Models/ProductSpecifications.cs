using Amazon.DynamoDBv2.DataModel;

namespace ProductModule.Models
{
    [DynamoDBTable("ProductCatalog")]
    public class ProductSpecifications
    {
        [DynamoDBHashKey("PK")]
        public string PK { get; set; } = "";

        [DynamoDBRangeKey("SK")]
        public string SK { get; set; } = "";

        [DynamoDBProperty]
        public required string ProductId { get; set; }

        [DynamoDBProperty]
        public Dictionary<string, string> Specifications { get; set;} = new Dictionary<string, string>();

        public static string CreatePK(string productId) => $"PRODUCT#{productId}";
        public static string CreateMetadataSK(string productId) => $"METADATA#{productId}";
    }
}