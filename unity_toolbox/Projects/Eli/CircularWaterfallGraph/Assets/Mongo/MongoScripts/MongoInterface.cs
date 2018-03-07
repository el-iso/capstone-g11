using MongoDB.Driver;
using MongoDB.Bson;
using UnityEngine;
using MongoDB.Driver.Builders;
using System.Collections.Generic;

public class MongoInterface : MonoBehaviour {

    //Inspector Window
    public float HeartBeat;
    public float ResperationRate;
    public float BloodOX;

    //Configuration Variables
    public static string database = "Production";
    public static string collection = "LiveData";
    public static float poll_interval = .1f;

    // Database variables
    private static MongoClient Client { get; set; }
    private static MongoServer Server { get; set; }
    private static MongoDatabase Database { get; set; }
    private static MongoCollection<BsonDocument> Collection { get; set; }

    private static float heartbeat = -1.0f;
    private static float respiration = -1.0f;
    private static float bloodOxygen = -1.0f;

    private static float time_of_last_poll = 0.0f;
    private static Dictionary<int, BsonDocument> deviceData = new Dictionary<int, BsonDocument>(); 

    // Unity Function : Use this for initialization
    void Start() {
        //Hardcoded connection to MongoDB server
        EstablishConnection("mongodb://localhost:27017", database, collection);
    }

    //Called once per fixed physics time step
    void FixedUpdate()
    {
        if (Time.time - time_of_last_poll >= poll_interval)
        {
            List<int> keys = new List<int>(deviceData.Keys);
            foreach(int dID in keys)
            {
                deviceData[dID] = SearchRecentByDeviceID(dID, 1);
            }
            BsonDocument results = SearchRecentByDeviceID(0, 1);
            heartbeat = float.Parse(results["h"].ToString());
            respiration = float.Parse(results["r"].ToString());
            bloodOxygen = float.Parse(results["b"].ToString());

            //Set Inspector Window Variables
            this.HeartBeat = MongoInterface.heartbeat;
            this.ResperationRate = MongoInterface.respiration;
            this.BloodOX = MongoInterface.bloodOxygen;

            time_of_last_poll = Time.time;
        }
    }

    private static BsonDocument getRecentByDevice(int deviceID = 0)
    {
        if (deviceData.ContainsKey(deviceID))
        {
            return deviceData[deviceID];
        }
        else
        {
            deviceData[deviceID] = SearchRecentByDeviceID(deviceID, 1);
            return deviceData[deviceID];
        }
    }

    /// <summary>
    /// Returns the most recent value of heartbeat pulled from MongoDB
    /// </summary>
    /// <returns></returns>
    public static float GetHeartbeat(int deviceID=0)
    {
        return float.Parse(getRecentByDevice(deviceID)["h"].ToString());
        return heartbeat;
    }

    /// <summary>
    /// Returns the most recent value of respiration pulled from MongoDB
    /// </summary>
    /// <returns></returns>
    public static float GetRespiration(int deviceID = 0)
    {
        return float.Parse(getRecentByDevice(deviceID)["r"].ToString());
        return respiration;
    }

    /// <summary>
    /// Returns the most recent value of blood oxygenation pulled from MongoDB
    /// </summary>
    /// <returns></returns>
    public static float GetBloodOxygen(int deviceID = 0)
    {
        return float.Parse(getRecentByDevice(deviceID)["b"].ToString());
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
        Server = Client.GetServer();
        Database = Server.GetDatabase(dbName);
        Collection = Database.GetCollection<BsonDocument>(collectionName);
    }

    /// <summary>
    /// Returns up to limit number of recent documents in the current database matching deviceID.
    /// TODO: Use Async calls?
    /// </summary>
    /// <param name="deviceID"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    private static BsonDocument SearchRecentByDeviceID(int deviceID, int limit = 1)
    {
        //var filter = Builders<BsonDocument>.Filter.Empty;
        //var sort = Builders<BsonDocument>.Sort.Descending("sent");
        //var result = Collection.Find(filter).Limit(limit).Sort(sort).ToList();

        var query = new QueryDocument("did", deviceID);
        BsonDocument result = new BsonDocument();
        var search = Collection.Find(MongoDB.Driver.Builders.Query.EQ("did", deviceID)).SetLimit(limit).SetSortOrder(SortBy.Descending("sent"));
        foreach (BsonDocument doc in search)
        {
            result = doc;
            break;
        }
        
        
        return result;
    }
}
