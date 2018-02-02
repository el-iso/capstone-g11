using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

public class MongoInterface : MonoBehaviour {

    public string database = "randomdata";
    public string collection = "randomTest";
    public float poll_interval = 1.0f;

    // Database variables
    private static MongoClient Client { get; set; }
    private static IMongoDatabase Database { get; set; }
    private static IMongoCollection<BsonDocument> Collection { get; set; }

    private float heartbeat = 100.0f;
    private float respiration = 100.0f;
    private float bloodOxygen = 100.0f;

    private float time_of_last_poll = 0.0f;

    // Unity Function : Use this for initialization
    void Start() {
        //Hardcoded connection to MongoDB server
        EstablishConnection("mongodb://localhost:27017", database, collection);
    }

    // Unity Function : Update is called once per frame
    void Update() {
        if(Time.time - time_of_last_poll >= poll_interval)
        {
            var results = SearchRecentByDeviceID(0, 1);
            heartbeat = float.Parse(results[0]["h"].ToString());
            respiration = float.Parse(results[0]["r"].ToString());
            bloodOxygen = float.Parse(results[0]["b"].ToString());
            print(heartbeat);
            time_of_last_poll = Time.time;
        }
    }

    /// <summary>
    /// Returns the most recent value of heartbeat pulled from MongoDB
    /// </summary>
    /// <returns></returns>
    public float GetHeartbeat()
    {
        return heartbeat;
    }

    /// <summary>
    /// Returns the most recent value of respiration pulled from MongoDB
    /// </summary>
    /// <returns></returns>
    public float GetRespiration()
    {
        return respiration;
    }

    /// <summary>
    /// Returns the most recent value of blood oxygenation pulled from MongoDB
    /// </summary>
    /// <returns></returns>
    public float GetBloodOxygen()
    {
        return bloodOxygen;
    }

    /// <summary>
    /// Initializes the MongoDB connection. Automatically called through Start()
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="dbName"></param>
    /// <param name="collectionName"></param>
    private static void EstablishConnection(string connectionString, string dbName, string collectionName)
    {
        // Set database variables
        Client = new MongoClient(connectionString);
        Database = Client.GetDatabase(dbName);
        Collection = Database.GetCollection<BsonDocument>(collectionName);
    }

    /// <summary>
    /// Drops the database passed as a parameter. Returns true if database dropped, false otherwise.
    /// </summary>
    /// <param name="dbName"></param>
    public static bool DropDatabase(string dbName)
    {
        // Check if database exists
        if (Client.GetDatabase(dbName) == null)
            return false;

        // Remove old references
        if (String.Equals(dbName, Database.DatabaseNamespace.DatabaseName))
        {
            // Remove Collection reference if within database
            if (System.Object.ReferenceEquals(Collection, Database.GetCollection<BsonDocument>(Collection.CollectionNamespace.CollectionName)))
                Collection = null;
            Database = null;
        }

        // Drop database
        Client.DropDatabase(dbName);

        // Verify database was dropped
        if (Client.GetDatabase(dbName) == null)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Drops the collection in the current database. Returns true if collection dropped, false otherwise.
    /// </summary>
    /// <param name="dbName"></param>
    public static bool DropCollection(string collectionName)
    {
        return DropCollection(Database.DatabaseNamespace.DatabaseName, collectionName);
    }

    /// <summary>
    /// Drops the collection in the specified database. Returns true if collection dropped, false otherwise.
    /// </summary>
    /// <param name="dbName"></param>
    public static bool DropCollection(string dbName, string collectionName)
    {
        var DB = Client.GetDatabase(dbName);

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
        await Collection.InsertOneAsync(BsonDocument.Parse(json));
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
    /// TODO: Use Async calls?
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
}
