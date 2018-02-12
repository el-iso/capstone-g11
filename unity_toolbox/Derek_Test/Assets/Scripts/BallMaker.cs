using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMaker : MonoBehaviour {

    public GameObject spawn;
    [Range(0,1)]
    public float frequency = 0.5f;
    [Range(0, 1000)]
    public int numBalls = 100;
    [Range(0, 5)]
    public float radius = 1.0f;

    private Transform tr;
    private int timer;
    private int ballsSpawned;

	// Use this for initialization
	void Start () {
        this.tr = this.gameObject.transform;
        this.timer = 0;
        this.ballsSpawned = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer++;
        float secs_elapsed = timer / 60.0f;
        if (secs_elapsed > frequency)
        {
            timer = 0;
            if (ballsSpawned <= numBalls)
            {
                GameObject obj = Instantiate(spawn, new Vector3(this.tr.position.x + Random.Range(0, this.radius) - (this.radius/2.0f), this.tr.position.y, this.tr.position.z + Random.Range(0, this.radius) - (this.radius / 2.0f)), Quaternion.identity);
                obj.GetComponent<SelfDescruct>().range = 15.0f;
            }
        }
	}
}
