using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public GameObject MongoContainer;
    private MongoInterface mongo;
    public float x_rotation;
    public float y_rotation;
    public float z_rotation;
    public bool wobble_on;
    private Vector3 rotate;
    private Vector3 wobble;

	// Use this for initialization
	void Start () {
        rotate = new Vector3(x_rotation, y_rotation, z_rotation);
        if (wobble_on) { wobble = new Vector3(0.02f, 0.02f, 0.02f); }
        else { wobble = new Vector3(); }
        print(mongo);
        mongo = MongoContainer.GetComponent<MongoInterface>();
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(rotate * mongo.GetHeartbeat() / 60);
        //transform.Rotate(rotate * Time.deltaTime * (Mathf.Sin(Time.realtimeSinceStartup) + 1.5f));
    }

    void FixedUpdate()
    {
        //print(mongo.GetHeartbeat() / 60 * Time.fixedDeltaTime);
        transform.Rotate(rotate * mongo.GetHeartbeat() / 60 * Time.fixedDeltaTime);
        //print(mongo.GetHearbeat());
        //transform.Rotate(rotate * Time.deltaTime * mongo.GetHeartbeat() / 60.0f);
        //transform.Rotate(rotate * Time.deltaTime * (Mathf.Sin(Time.realtimeSinceStartup) + 1.5f));
        transform.position += (wobble * Mathf.Sin(Time.realtimeSinceStartup * 5));
    }
}
