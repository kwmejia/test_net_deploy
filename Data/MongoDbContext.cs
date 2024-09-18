using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ToDoList.Models;

namespace ToDoList.Data
{
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
    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11
};

            // Establecer tiempos de espera (por si acaso hay timeouts)
            settings.ConnectTimeout = TimeSpan.FromSeconds(30);  // Tiempo de espera para conexión
            settings.SocketTimeout = TimeSpan.FromSeconds(30);   // Tiempo de espera para los sockets

            MongoClient mongoClient = new MongoClient(settings);

            _database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        }

        public IMongoCollection<ToDo> ToDos => _database.GetCollection<ToDo>("ToDos");
    }
}
