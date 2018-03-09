using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallInitializer : MonoBehaviour {

    public List<int> devices = new List<int>();
    private List<GameObject> particleObjects = new List<GameObject>();

    public GameObject glowSpray;

    public float offset = 2;
    public Vector3 axis = new Vector3(0, 0, 1);

    // Use this for initialization
    void Start () {
        for (int dID = 0; dID < devices.Count; dID++)
        {
            GameObject g = Instantiate(glowSpray);
            var rotateConfig = g.GetComponent<CirclularMovementRadialOut>();
            rotateConfig.device = dID;
            //rotateConfig.useMongo = true;
            g.transform.SetParent(transform);
            g.transform.position = transform.position + new Vector3(0, offset, 0);
            float angle = 360.0f * dID / devices.Count;
            print("Init at " + angle);
            g.transform.RotateAround(transform.position, new Vector3(0,0,1), angle);
            particleObjects.Add(g);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
