using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using ProductModule.Interfaces;
using ProductModule.Models;

namespace ProductModule.Repository
{
    public class AddProductsRepository : IAddProductsRepository
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;
        private readonly IDynamoDBContext _dynamoDbContext;
        private const string TableName = "ProductCatalog";
        public AddProductsRepository(IAmazonDynamoDB amazonDynamoDB)
        {
            _amazonDynamoDB = amazonDynamoDB;
            _dynamoDbContext = new DynamoDBContext(_amazonDynamoDB);
        }
        public async Task<ProductCategory> AddProductCategoryAsync(string categoryName)
        {
            var categoryId = GenerateUniqueCategoryId().ToString();
            var item = new Dictionary<string, AttributeValue>
            {
                ["PK"] = new AttributeValue { S = "CATEGORY" },
                ["SK"] = new AttributeValue { S = $"CATEGORY#{categoryName}" },
                ["category_name"] = new AttributeValue { S = categoryName },
                ["category_id"] = new AttributeValue { S = categoryId }
            };
            var request = new PutItemRequest
            {
                TableName = TableName,
                Item = item,
                ConditionExpression = "attribute_not_exists(PK) AND attribute_not_exists(SK)"
            };

            try
            {
                await _amazonDynamoDB.PutItemAsync(request);
                return new ProductCategory
                {
                    ProductCategoryId = categoryId,
                    ProductCategoryName = categoryName,
                    PK = "CATEGORY",
                    SK = $"CATEGORY#{categoryName}"
                };
            }
            catch(ConditionalCheckFailedException)
            {
                throw new DuplicateItemException("Duplicate item");
            }
        }

        public async Task<bool> AddProductsToCategoryAsync(List<Product> products)
        {
            try
            {
                const int batchSize = 25;
                for(int i = 0; i < products.Count; i += batchSize)
                {
                    var batch = products.Skip(i).Take(batchSize);
                    var writeRequests = new List<WriteRequest>();
                    foreach(var product in batch)
                    {
                        var productId = GenerateUniqueCategoryId();
                        var item = new Dictionary<string, AttributeValue>
                        {
                            ["PK"] = new AttributeValue { S = $"CATEGORY#{product.ProductCategoryId}" },
                            ["SK"] = new AttributeValue { S = $"PRODUCT#{productId}" },
                            ["product_name"] = new AttributeValue { S = product.ProductName },
                            ["product_id"] = new AttributeValue { S = productId },
                            ["numberInStock"] = new AttributeValue { N = product.StockQuantity.ToString() },
                            ["productCost"] = new AttributeValue { N = product.Price.ToString() },
                            ["category_id"] = new AttributeValue { S = product.ProductCategoryId }
                        };
                        writeRequests.Add(new WriteRequest 
                        {
                            PutRequest = new PutRequest 
                            {
                                Item = item
                            }
                        });
                    }

                    var batchRequest = new BatchWriteItemRequest()
                    {
                        RequestItems = new Dictionary<string, List<WriteRequest>>
                        {
                            { TableName, writeRequests },
                        }
                    };
                    var response = await _amazonDynamoDB.BatchWriteItemAsync(batchRequest);
                    while (response.UnprocessedItems.Count > 0)
                    {
                        var retryRequest = new BatchWriteItemRequest
                        {
                            RequestItems = response.UnprocessedItems
                        };
                        response = await _amazonDynamoDB.BatchWriteItemAsync(retryRequest);
                    }
                }
                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductSpecifications>> AddProductSpecificationsAsync(List<ProductSpecifications> productSpecs)
        {
            var batchWrite = _dynamoDbContext.CreateBatchWrite<ProductSpecifications>();
        
            foreach (var product in productSpecs)
            {
                product.PK = ProductSpecifications.CreatePK(product.ProductId);
                product.SK = ProductSpecifications.CreateMetadataSK(product.ProductId);
            }
            
            batchWrite.AddPutItems(productSpecs);
            await batchWrite.ExecuteAsync();
            return productSpecs;
        }

        private string GenerateUniqueCategoryId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }
    }
}