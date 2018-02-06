using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPM : MonoBehaviour {

    //MongoContainer is a game object with MongoInterface.cs as a component
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
        return 60.0f;
    }

    // Use this for initialization
    void Start () {
        mongo = MongoContainer.GetComponent<MongoInterface>();
        scale_at_start = transform.localScale;
    }

    void FixedUpdate()
    {
        float BPM = updateBPM();
        float frequency = BPM / 60f;
        radians += frequency * Time.fixedDeltaTime;
        float adjustment = (Mathf.Sin(Mathf.PI * radians) + 2) / 2;
        transform.localScale = scale_at_start * adjustment * ScaleConstant;
    }
}
