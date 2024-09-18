using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ToDoList.Models;

namespace ToDoList.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<DatabaseSettings> databaseSettings)
    {
        var settings = MongoClientSettings.FromUrl(new MongoUrl(databaseSettings.Value.ConnectionString));

       
        
        MongoClient mongoClient = new(settings);
        
        _database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName); 
    }

    public IMongoCollection<ToDo> ToDos => _database.GetCollection<ToDo>("ToDos"); 
}
