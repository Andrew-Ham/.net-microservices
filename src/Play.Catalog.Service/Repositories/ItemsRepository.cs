using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repositories
{

    public class ItemsRepository : IItemsRepository
    {
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> dbCollection;

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        // Constructor of repository. Here we want to construct the instance for DB collection.
        // Dependency injection of IMongoDatabase
        public ItemsRepository(IMongoDatabase database)
        {
            // Got rid of the 2 lines above since ItemsRepository is ready to receive an instance of database via dependency injection.
            // However because we removed those lines we need a way to construct the database object and configure it.
            // The configuration (host/port) that our service needs to connect to, we can declare these settings in appsettings.json file.
            dbCollection = database.GetCollection<Item>(collectionName);
            
        }

        //Return all items in database. ReadOnly because consumer shouldn't need to modify data.
        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }

        // Return one specific item from database
        public async Task<Item> GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity => entity.Id, id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        // Create and add item to database
        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await dbCollection.InsertOneAsync(entity);

        }

        // Update existing item in database
        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);

        }

        // Remove existing item in database
        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(existingEntity => existingEntity.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

    }
}