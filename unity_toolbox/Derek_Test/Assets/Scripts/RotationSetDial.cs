using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSetDial : MonoBehaviour {

    private Transform tr;
    private Vector3 startRotation;

    // Use this for initialization
    void Start () {
        this.tr = GetComponent<Transform>();
        this.startRotation = tr.localEulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
        //Get some data value here
        float data_value = 100;

        Vector3 newRotation = this.startRotation;
        newRotation.y += data_value;
        tr.localEulerAngles = newRotation;
        
    }
}
