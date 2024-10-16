using MongoDB.Driver;

namespace APIAutomation.Helper
{
    public static class Mongo_Helper
    {
        static IMongoCollection<BsonDocument> mongoCollection;

        static Mongo_Helper()
        {
            try
            {
                // Replace with your actual MongoDB connection string
                string conn = "mongodb+srv://username:password@devserverless.dzmnapw.mongodb.net/?retryWrites=true&w=majority";

                // Initialize the MongoDB client
                MongoClient mongoClient = new MongoClient(conn);
                IMongoDatabase mongoDatabase = mongoClient.GetDatabase("database name");  // Database name
                mongoCollection = mongoDatabase.GetCollection<BsonDocument>("collection name"); // Collection name
            }
            catch (MongoConfigurationException ex)
            {
                Console.WriteLine($"MongoDB Configuration Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MongoDB Initialization Error: {ex.Message}");
                throw;
            }
        }

        // Method to get a single document from MongoDB based on key and value
        public static BsonDocument GetDataFromMongo(string key, string value)
        {
            BsonDocument filter = new BsonDocument();
            filter[key] = value;
            BsonDocument result = mongoCollection.Find(filter).FirstOrDefault();

            return result;
        }

        // Method to get a list of documents from MongoDB based on key-value pairs
        public static List<BsonDocument> GetListFromMongo(Dictionary<string, string> query)
        {
            // Create a filter using the provided key-value pairs in the dictionary
            var filter = query.ToBsonDocument();

            // Set the sorting and limit options
            var options = new FindOptions<BsonDocument>()
            {
                Sort = Builders<BsonDocument>.Sort.Descending("_id"), // Sort by _id in descending order
                Limit = 5    // Limit the number of documents returned
            };

            // Perform the query with the filter and options
            var result = mongoCollection.Find(filter).Sort(options.Sort).Limit(options.Limit).ToList();

            return result ?? new List<BsonDocument>();
        }

    }
}
