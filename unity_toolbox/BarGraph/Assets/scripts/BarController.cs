using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour {

    private float startX, startY, startZ;
    private float basePos;

    private void Start()
    {
        startX = transform.localPosition.x;
        startY = transform.localPosition.y;
        startZ = transform.localPosition.z;
    }

    // Update is called once per frame
    void Update () {
        transform.localScale += new Vector3(0, 0, 1.0f);
        basePos = transform.localScale.z / 2;
        transform.localPosition = new Vector3(startX, startY, startZ + basePos);

    }
}
