﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallDataPointBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, -10, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(this.gameObject.transform.localPosition.y < -230)
        {
            Destroy(this.gameObject);
        }
	}
}
