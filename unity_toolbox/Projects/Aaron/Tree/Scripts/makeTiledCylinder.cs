using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeTiledCylinder : MonoBehaviour {

    public GameObject segment;
    public float scale = 1.0f;
    public float spacing;
    public int segments = 10;
    public float pulseFreq = 2.0f;
    public bool loop = false;
    

    private GameObject root;
    private GameObject previous;
    private GameObject current;
    private float lastActivation = 0.0f;



	// Use this for initialization
	void Start () {
        segment.GetComponent<PassLights>();
        root = Instantiate(segment, transform);
        root.GetComponent<PassLights>().Activate();
        previous = root;
        for (int i = 1; i < segments; i++)
        {
            current = Instantiate(segment, transform);
            current.transform.localPosition += new Vector3(0.0f, i * spacing * scale, 0.0f);
            current.transform.localScale *= scale;
            current.GetComponent<PassLights>().setParent(previous);
            previous.GetComponent<PassLights>().addChild(current);
            previous = current;
        }
        if (loop) { previous.GetComponent<PassLights>().addChild(root); }
        else { previous.GetComponent<PassLights>().addChild(null); }
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastActivation > pulseFreq)
        {
            root.GetComponent<PassLights>().Activate();
            lastActivation = Time.time;
        }
	}
}