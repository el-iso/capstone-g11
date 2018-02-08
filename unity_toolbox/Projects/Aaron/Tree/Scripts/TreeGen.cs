using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGen : MonoBehaviour {

    public GameObject segment;
    public float scale = 1.0f;
    public float spacing = 0.2f;
    public int segments = 50;
    public float pulseFreq = 2.0f;
    public bool relativeScale = false;

    public int branches = 3;
    public int branchAngle = 40;
    public float branchAt = 0.5f;
    public int layers = 4;

    private int maxLayersAllowed = 5;
    private float lastActivation = 0.0f;
    private GameObject trunkRoot;

    GameObject makeBranch(int layer, int iteration, GameObject parentBranch, GameObject parentSegment)
    {
        if (layer > layers || layer > maxLayersAllowed) { return null; }
        float local_scale = 0.0f;
        if (!relativeScale) { local_scale = Mathf.Pow(0.5f, layer); }
        else { local_scale = Mathf.Pow(branchAt, layer); }

        GameObject resultBranch = new GameObject();
        resultBranch.transform.SetParent(parentBranch.transform);
        resultBranch.transform.position = parentSegment.transform.position;
        resultBranch.transform.rotation = parentBranch.transform.rotation;
        resultBranch.transform.Rotate(0.0f, iteration * 360.0f / branches, 0.0f);
        resultBranch.transform.Rotate(0.0f, 0.0f, branchAngle);

        GameObject root, previous, current, branchSegment = null;

        root = Instantiate(segment, resultBranch.transform);
        root.GetComponent<PassLights>().setParent(parentSegment);
        root.transform.localScale *= local_scale;
        //root.GetComponent<PassLights>().Activate();
        previous = root;

        for (int i = 1; i < segments; i++)
        {
            current = Instantiate(segment, resultBranch.transform);
            current.transform.localPosition += new Vector3(0.0f, i * spacing * local_scale, 0.0f);
            current.transform.localScale *= local_scale;
            current.GetComponent<PassLights>().setParent(previous);
            previous.GetComponent<PassLights>().addChild(current);
            if ( i == (int)(branchAt * segments))
            {
                branchSegment = current;
                for (int j = 0; j < branches; j++)
                {
                    GameObject childRoot = makeBranch(layer + 1, j, resultBranch, current);
                    current.GetComponent<PassLights>().addChild(childRoot);
                }
            }
            previous = current;
        }
        return root;
    }

	// Use this for initialization
	void Start () {
        GameObject root, previous, current, branchSegment;

        trunkRoot = Instantiate(segment, transform);
        trunkRoot.name = "TrunkRoot";
        trunkRoot.GetComponent<PassLights>().Activate();
        previous = trunkRoot;

        for (int i = 1; i < segments; i++)
        {
            current = Instantiate(segment, transform);
            current.transform.localPosition += new Vector3(0.0f, i * spacing * scale, 0.0f);
            current.transform.localScale *= scale;
            current.GetComponent<PassLights>().setParent(previous);
            previous.GetComponent<PassLights>().addChild(current);
            if (i == (int)(branchAt * segments))
            {
                branchSegment = current;
                for (int j = 0; j < branches; j++)
                {
                    GameObject childRoot = makeBranch(1, j, gameObject, current);
                    current.GetComponent<PassLights>().addChild(childRoot);
                }
            }
            previous = current;
        }
        previous.name = "TrunkTop";

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastActivation > pulseFreq)
        {
            trunkRoot.GetComponent<PassLights>().Activate();
            lastActivation = Time.time;
        }
    }
}
