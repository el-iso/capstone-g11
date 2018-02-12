using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    public GameObject wall;
    public Vector3 dimensions = new Vector3(10, 10, 10);
    public float thickness = 0.1f;

    private GameObject[] walls = new GameObject[6];

    private GameObject makeWall(int index)
    {
        if (index > 6 || index < 0) { throw new System.ArgumentOutOfRangeException("Wall index must be 0 < x < 6, received "+index); }
        int xyzTarget = index / 2;
        int posNeg = (index % 2 == 0) ? 1 : -1;

        GameObject resultWall = Instantiate(wall, this.transform);
        resultWall.transform.SetParent(this.transform);
        Vector3 v = new Vector3();
        v[xyzTarget] = dimensions[xyzTarget] * posNeg / 2;
        resultWall.transform.position = v;

        Vector3 scale = new Vector3();
        for(int i = 0; i < 3; i++){
            if (i == xyzTarget){ scale[i] = thickness; }

            else { scale[i] = dimensions[i] + thickness; }
        }

        resultWall.transform.localScale = scale;

        return resultWall;
    }

	// Use this for initialization
	void Start () {
		for(int i = 0; i < 6; i++)
        {
            walls[i] = makeWall(i);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
