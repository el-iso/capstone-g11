using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclularMovementRadialOut : MonoBehaviour {

    public float speed = 30;
    public bool x = false;
    public bool y = false;
    public bool z = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Transform parent_t = transform.parent ? transform.parent : transform;
        Vector3 point = parent_t.position;
        Vector3 axis = new Vector3(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0);
        transform.RotateAround(point, axis, speed * Time.deltaTime);
	}
}
