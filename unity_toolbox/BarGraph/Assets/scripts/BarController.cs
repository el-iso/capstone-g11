using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(0, 1.0f, 0);
	}
}
