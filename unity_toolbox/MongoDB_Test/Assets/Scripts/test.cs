using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Connect directly to a single MongoDB server
        var client = new MongoClient("mongodb://localhost:27017");

        // Get DB and collection
        var database = client.GetDatabase("randomdata");
        var collection = database.GetCollection<BsonDocument>("randomTest");

        //// Set document filter (find document with given field-value pair)
        //var filter = Builders<BsonDocument>.Filter.Eq("role", "test2");
        //var sort = Builders<BsonDocument>.Sort.Descending("role");

        //// Set projection
        //var projection = Builders<BsonDocument>.Projection.Exclude("_id");

        //// Search for document with given parameters
        //var document = collection.Find(new BsonDocument()).Project(projection).First();

        //// Print the retrieved document to the Unity debug console
        //Debug.Log(document.ToString());
    }
}
