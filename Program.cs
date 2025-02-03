using Amazon.DynamoDBv2;
using ProductModule.Interfaces;
using ProductModule.Repository;
using ProductModule.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAddProductsRepository, AddProductsRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAddProductsService, AddProductsService>();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseMiddleware<CatchApplicationLevelExceptions>();


app.MapControllers();


app.Run();
