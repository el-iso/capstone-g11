using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

public class ConnectionTest : MonoBehaviour {

    // Database variables
    public static MongoClient Client { get; set; }
    public static IMongoDatabase Database { get; set; }
    public static IMongoCollection<BsonDocument> Collection { get; set; }


    // Unity Function : Use this for initialization
    void Start() {
        //Hardcoded connection to MongoDB server
        EstablishConnection("mongodb://localhost:27017", "randomdata", "randomTest");

        // Testing purposes only:
        //var result = SearchRecentByDeviceID(0, 10);
        //foreach (var thing in result)
        //{
        //    Debug.Log(thing.ToJson());
        //}
    }

    // Unity Function : Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Initializes the MongoDB connection. Automatically called through Start()
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="dbName"></param>
    /// <param name="collectionName"></param>
    public static void EstablishConnection(string connectionString, string dbName, string collectionName)
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
    public static async void InsertDocument(string json)
    {
        await Collection.InsertOneAsync(BsonDocument.Parse(json));
    }

    /// <summary>
    /// Insert a JSON-encoded document into the specified database and collection.
    /// </summary>
    /// <param name="dbName"></param>
    /// <param name="collectionName"></param>
    /// <param name="json"></param>
    public static async void InsertDocument(string dbName, string collectionName, string json)
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
    public static List<BsonDocument> SearchRecentByDeviceID(int deviceID, int limit = 10000)
    {
        //var filter = Builders<BsonDocument>.Filter.Eq("did", deviceID);
        //var sort = Builders<BsonDocument>.Sort.Ascending("sent");
        //var result = Collection.Find(filter).Sort(sort).ToList();
        //return result;

        var filter = Builders<BsonDocument>.Filter.Empty;
        var result = Collection.Find(filter).ToList();
        return result;
    }

    //static BsonDocument SearchByTimeRange(int timeStart, int timeEnd, int deviceID, int limit = 10000)
    //{
    //    //var builder = Builders<BsonDocument>.Filter;
    //    //builder.And(builder.Eq("did", deviceID), builder.Lt("sent", timeEnd));
    //    var filter = Builders<BsonDocument>.Filter.Eq("did", deviceID);
    //    var sort = Builders<BsonDocument>.Sort.Ascending("sent");
    //    var result = Collection.Find(filter).Sort(sort).ToBsonDocument();
    //    return result;
    //}

    //public static String getLastHeartRate()
    //{
    //    //// Connect directly to a single MongoDB server
    //    //var client = new MongoClient("mongodb://localhost:27017");

    //    // Get DB and collection
    //    //var database = client.GetDatabase("randomData");
    //    //var collection = database.GetCollection<BsonDocument>("randomTest");

    //    // Set document filter (find document with given field-value pair)
    //    //var filter = Builders<BsonDocument>.Filter.Eq("role", "test2");
    //    var sort = Builders<BsonDocument>.Sort.Ascending("sent");

    //    // Set projection
    //    //var projection = Builders<BsonDocument>.Projection.Exclude("_id");
    //    var projection = Builders<BsonDocument>.Projection.Include("h").Exclude("b").Exclude("r").Exclude("did").Exclude("sent");

    //    // Search for document with given parameters
    //    //var str = "";
    //    //while (cursor.hasNext())
    //    //{
    //    //    items.append(cursor.next().toJson());

    //    //}

    //    var document = Collection.Find(new BsonDocument()).Project(projection).Sort(sort);
    //    // Print the retrieved document to the Unity debug console
    //    //Debug.Log(document.ToString());
    //    return document.ToString();
    //}
}
