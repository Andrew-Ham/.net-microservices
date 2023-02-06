using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories 
{
    public static class Extensions 
    {
        // This method will register the IMongo client and database instance.
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        
            services.AddSingleton(serviceProvider => {
                // Use service provider to request an instance of services for Configuration. 
                var configuration = serviceProvider.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>(); 
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                var database = mongoClient.GetDatabase(serviceSettings.ServiceName);
                return database;
        });
        return services; 
        // The above returns services to enable fluent use of this extension method.
        }

        // This method will register the repository
        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider => 
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });
            return services;
        }
    }
}