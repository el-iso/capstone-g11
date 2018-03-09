using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclularMovementRadialOut : MonoBehaviour {
    
    public float speed = 30;
    public int device = 0;
    public bool useMongo = false;
    public Vector3 axis = new Vector3(0,0,1);
    private float currData = 0.0f;
    private float prevData = 0.0f;
    private float delta = 0.0f;

    private Vector3 origin;

	// Use this for initialization
	void Start () {
        origin = transform.parent ? transform.parent.position : Vector3.zero;
        if (useMongo)
        {
            prevData = MongoInterface.GetHeartbeat(device);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if (useMongo)
        {
            currData = MongoInterface.GetHeartbeat(device);
            delta = currData - prevData;
            prevData = currData;

            transform.RotateAround(origin, axis, speed * delta);
        }
        else
        {
            transform.RotateAround(origin, axis, speed * Time.deltaTime);
        }
        
	}
}
