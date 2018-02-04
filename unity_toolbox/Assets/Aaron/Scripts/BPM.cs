using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPM : MonoBehaviour {

    public GameObject MongoContainer;
    private MongoInterface mongo;

    public float ScaleConstant = 1.0f;
    public bool heart = true;
    public bool respiration = true;

    private Vector3 scale_at_start;
    private float radians = 0.0f;


    private float updateBPM()
    {
        if (heart)
        {
            return mongo.GetHeartbeat();
        }
        if (respiration)
        {
            return mongo.GetRespiration();
        }
        return 100.0f;
    }

    // Use this for initialization
    void Start () {
        mongo = MongoContainer.GetComponent<MongoInterface>();
        scale_at_start = transform.localScale;
        print(Mathf.Sin(Mathf.PI * 1.5f));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        float BPM = updateBPM();
        float frequency = BPM / 60f;
        float period;
        if (frequency - 0.01f < 0.0f)
        {
            period = 0.0f;
        }
        else if (frequency < 0.0f)
        {
            period = 0.0f;
        }
        else
        {
            period = 1.0f / frequency;
        }
        //print("Freq="+frequency.ToString()+"\tPeriod=" + period.ToString() + "\tRadians=" + radians.ToString());
        radians += frequency * Time.fixedDeltaTime;
        //print(radians);


        float adjustment = (Mathf.Sin(Mathf.PI * radians) + 2) / 2;
        transform.localScale = scale_at_start * adjustment;
    }
}
