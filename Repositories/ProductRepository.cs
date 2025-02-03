using ProductModule.Interfaces;
using ProductModule.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ProductModule.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly string _tableName;

        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IAmazonDynamoDB dynamoDb, IConfiguration configuration, ILogger<ProductRepository> logger)
        {
            _dynamoDb = dynamoDb;
            _tableName = "ProductCatalog";
            _logger = logger;
        }
        
        public async Task<List<ProductCategory>> GetAllProductCategoriesAsync()
        {
            try
            {
                 var queryRequest = new QueryRequest
                {
                    TableName = _tableName,
                    KeyConditionExpression = "PK = :pk",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":pk", new AttributeValue { S = "CATEGORY" }}
                    }
                };

                var response = await _dynamoDb.QueryAsync(queryRequest);
                return response.Items.Select(item => new ProductCategory
                {
                    PK = item["PK"].S,
                    SK = item["SK"].S,
                    ProductCategoryName = item["category_name"].S,
                    ProductCategoryId = item["category_id"].S,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }           
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryId)
        {
            var queryRequest = new QueryRequest
            {
                TableName = _tableName,
                KeyConditionExpression = "PK = :pk AND begins_with(SK, :sk)",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":pk", new AttributeValue { S = $"CATEGORY#{categoryId}" }},
                    {":sk", new AttributeValue { S = "PRODUCT#" }}
                }
            };

            var response = await _dynamoDb.QueryAsync(queryRequest);
            return response.Items.Select(item => new Product
            {
                PK = item["PK"].S,
                SK = item["SK"].S,
                ProductId = item["product_id"].S,
                ProductName = item["product_name"].S,
                Price = decimal.Parse(item["productCost"].N),
                StockQuantity = int.Parse(item["numberInStock"].N),
                ProductCategoryId = item["category_id"].S,
            });
        }
    }
}