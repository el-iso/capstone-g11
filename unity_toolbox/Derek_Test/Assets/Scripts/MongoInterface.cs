using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

public class MongoInterface : MonoBehaviour {

    // Database variables
    private static MongoClient Client { get; set; }
    private static IMongoDatabase Database { get; set; }
    private static IMongoCollection<BsonDocument> Collection { get; set; }
    private static string database = "randomdata";
    private static string collection = "randomTest";


    //Exposed Variables
    private static float heartrate;
    public static float heartRate
    {
        get { return heartRate; }
        set { MongoInterface.heartrate = value; }
    }

    // Unity Function : Use this for initialization
    void Start() {
        //Hardcoded connection to MongoDB server
        EstablishConnection("mongodb://localhost:27017", MongoInterface.database, MongoInterface.collection);
    }

    // Unity Function : Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Initializes the MongoDB connection. Automatically called through Start()
    /// </summary>
    /// <param name="connectionString">IP address for connection</param>
    /// <param name="dbName">Database to Use</param>
    /// <param name="collectionName">Collection to Use</param>
    private static void EstablishConnection(string connectionString, string dbName, string collectionName)
    {
        // Set database variables
        MongoInterface.Client = new MongoClient(connectionString);
        MongoInterface.Database = MongoInterface.Client.GetDatabase(dbName);
        MongoInterface.Collection = MongoInterface.Database.GetCollection<BsonDocument>(collectionName);
    }

    /// <summary>
    /// Drops the database passed as a parameter. Returns true if database dropped, false otherwise.
    /// </summary>
    /// <param name="dbName">Database to Drop</param>
    private static bool DropDatabase(string dbName)
    {
        // Check if database exists
        if (MongoInterface.Client.GetDatabase(dbName) == null)
            return false;

        // Remove old references
        if (String.Equals(dbName, MongoInterface.Database.DatabaseNamespace.DatabaseName))
        {
            // Remove Collection reference if within database
            if (System.Object.ReferenceEquals(MongoInterface.Collection, MongoInterface.Database.GetCollection<BsonDocument>(MongoInterface.Collection.CollectionNamespace.CollectionName)))
                MongoInterface.Collection = null;
            Database = null;
        }

        // Drop database
        MongoInterface.Client.DropDatabase(dbName);

        // Verify database was dropped
        if (MongoInterface.Client.GetDatabase(dbName) == null)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Drops the collection in the current database. Returns true if collection dropped, false otherwise.
    /// </summary>
    /// <param name="dbName"></param>
    private static bool DropCollection(string collectionName)
    {
        return DropCollection(MongoInterface.Database.DatabaseNamespace.DatabaseName, collectionName);
    }

    /// <summary>
    /// Drops the collection in the specified database. Returns true if collection dropped, false otherwise.
    /// </summary>
    /// <param name="dbName"></param>
    private static bool DropCollection(string dbName, string collectionName)
    {
        var DB = MongoInterface.Client.GetDatabase(dbName);

        // Check if collection exists
        if (DB == null || DB.GetCollection<BsonDocument>(collectionName) == null)
            return false;

        // Remove reference if same as Collection
        if (String.Equals(collectionName, DB.DatabaseNamespace.DatabaseName))
            Collection = null;

        // Drop collection
        DB.DropCollection(collectionName);

        // Verify collection was dropped
        if (DB.GetCollection<BsonDocument>(collectionName) == null)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Insert a JSON-encoded document into the current database and collection.
    /// </summary>
    /// <param name="dbName"></param>
    /// <param name="collectionName"></param>
    /// <param name="json"></param>
    private static async void InsertDocument(string json)
    {
        await MongoInterface.Collection.InsertOneAsync(BsonDocument.Parse(json));
    }

    /// <summary>
    /// Insert a JSON-encoded document into the specified database and collection.
    /// </summary>
    /// <param name="dbName"></param>
    /// <param name="collectionName"></param>
    /// <param name="json"></param>
    private static async void InsertDocument(string dbName, string collectionName, string json)
    {
        await Client.GetDatabase(dbName).GetCollection<BsonDocument>(collectionName).InsertOneAsync(BsonDocument.Parse(json));
    }

    /// <summary>
    /// Returns up to limit number of recent documents in the current database matching deviceID.
    /// TODO: Use Async calls? Probably dont need in this case
    /// </summary>
    /// <param name="deviceID"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    private static List<BsonDocument> SearchRecentByDeviceID(int deviceID, int limit = 10000)
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var sort = Builders<BsonDocument>.Sort.Descending("sent");
        var result = Collection.Find(filter).Limit(limit).Sort(sort).ToList();
        return result;
    }


    //Implementation Specific

    private static void RetrievePublicValues()
    {
        var filter = Builders<BsonDocument>.Filter.Empty;
        var sort = Builders<BsonDocument>.Sort.Descending("sent");
        var result = Collection.Find(filter).Limit(1).Sort(sort).ToList();

        float h = float.Parse(result[0]["h"].AsString);
    }
}

