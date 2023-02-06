using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Configure ServiceSetting
var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

// AddSingleton allows us to register a type or an object and makes sure there is ONLY ONE instance of it in the entire microservice.
builder.Services.AddSingleton(serviceProvider => {
    var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
    BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
    var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
    var database = mongoClient.GetDatabase(serviceSettings.ServiceName);
    return database;
});

// Register ItemsRepository Dependency
// As you can tell the below AddSingleton is different from the previous one above.
// It is different because the one above we are explicitly constructing our mongoClient and the IMongoDatabase before registering it with the service container
// In the case below, we are just declaring what is the type that we want to register and which interface implements it. And the ASP.NET Core runtime
// will construct that instance whenever it is needed.
builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();



builder.Services.AddControllers(options => {options.SuppressAsyncSuffixInActionNames = false;});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
