using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailChanger : MonoBehaviour
{
    private System.Random r = new System.Random();

    // Use this for initialization
    void Start()
    {
        TrailRenderer trail = gameObject.GetComponent<TrailRenderer>();
        Color c = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
        trail.material.SetColor("_Color", c);
        trail.material.EnableKeyword("_EMISSION");
        trail.material.SetColor("_EmissionColor", c*1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}