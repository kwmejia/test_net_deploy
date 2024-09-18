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

        // Configuración de SSL
        settings.SslSettings = new SslSettings
        {
            CheckCertificateRevocation = false,
            EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
        };
        
        MongoClient mongoClient = new(settings);
        
        _database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName); 
    }

    public IMongoCollection<ToDo> ToDos => _database.GetCollection<ToDo>("ToDos"); 
}
