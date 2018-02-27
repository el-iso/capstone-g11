using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataImporter : MonoBehaviour {

    //Configuration Variables
    public static float poll_interval = 1.0f;

    private static float time_of_last_poll = 0.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (Time.time - time_of_last_poll >= poll_interval)
        {
            Debug.Log(Time.time.ToString());

            time_of_last_poll = Time.time;
        }
    }
}
