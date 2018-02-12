using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDescruct : MonoBehaviour {

    
    public float fuse = 0.0f;
    public float range = 0.0f;

    private float timeCreated;
    private Transform tr;
    private Vector3 startPos;

	// Use this for initialization
	void Start () {
        this.timeCreated = Time.time;
        this.tr = this.gameObject.transform;
        this.startPos = this.tr.position;
	}
	
	// Update is called once per frame
	void Update () {
        if ((fuse > 0.0f && Time.time - this.timeCreated > fuse) || (range > 0.0f && (this.tr.position - this.startPos).magnitude > range))
        {
            Destroy(this.gameObject);
        }
	}
}
