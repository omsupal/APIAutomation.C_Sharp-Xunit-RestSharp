using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIAutomation.Helper
{
    public static class Mongo_Helper
    {
        static IMongoCollection<BsonDocument> mongoCollection;

        static Mongo_Helper()
        {
            string conn = "mongoconnection String";
            MongoClient mongoClient = new MongoClient(conn); // reader connection string
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase("Database Name");  //Database name
            mongoCollection = mongoDatabase.GetCollection<BsonDocument>("Collection name"); //add your collction name here
        }

        /// <summary>
        /// this method return one single document basis the provided key and value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonDocument GetDataFromMongo(string key, string value)
        {
            BsonDocument filter = new BsonDocument();
            filter[key] = value;
            BsonDocument bookings = mongoCollection.Find(filter).FirstOrDefault();

            if (bookings != null)
                return bookings;
            else
                return null;
        }

        /// <summary>
        /// this method returns the list of bsondocument on basis of provided key value pair and sorting options
        /// </summary>
        /// <param name="query"></param>
        /// <returns>list of BsonDocument</returns>
        public static List<BsonDocument> GetListFromMongo(Dictionary<string, string> query)
        {
            BsonDocument sort = new BsonDocument();
            sort.Add("_id", -1.0);
            var options = new FindOptions<BsonDocument>()
            {
                Sort = sort,
                Limit = 5    //no of documents to return
            };

            var result = mongoCollection.FindAsync(query.ToBsonDocument(), options).Result.ToList();
            if (result != null)
                return result;
            else
                return null;
        }
    }
}
