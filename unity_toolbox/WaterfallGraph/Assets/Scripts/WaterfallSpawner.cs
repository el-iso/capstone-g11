using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallSpawner : MonoBehaviour {

    int pos = 0;
    int spawnRate = 0;
	// Use this for initialization
	void Start () {

    }

    public GameObject point;
    // Update is called once per frame
    void Update () {
        
        if (spawnRate > 0)
        {
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.AddComponent<Rigidbody>();
            //cube.transform.position = new Vector3(pos, 0, 0);
            //Rigidbody rb = cube.GetComponent<Rigidbody>();
            //rb.useGravity = false;
            //rb.velocity = new Vector3(0, -5, 0);
            Instantiate(point, new Vector3(0, 0, pos), Quaternion.identity);

            if (pos == 100)
            {
                pos = 0;
            }
            else
            {
                pos++;
            }
            spawnRate = 0;
        }
        else
        {
            spawnRate++;
        }
       
    }
}
