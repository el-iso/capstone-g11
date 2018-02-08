using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassLights : MonoBehaviour {

    public float delay = 0.5f;
    public float speed = 1.0f;
    public Color color = new Color(1, 1, 1, 1);

    private GameObject parent;
    private List<GameObject> children = new List<GameObject>();
    private bool pulseFlag = false;
    private bool childActivationFlag = false;
    private Renderer rend;
    private Material mat;
    private float activationTime;
    private bool root = false;
    private Color defaultColor = new Color(0, 0, 0, 1);

    private static float[] sinCache;
    private static int sinSteps = 60;
    private static bool cacheDone = false;

    public void addChild(GameObject g)
    {
        if (g) { children.Add(g); }
    }

    public void setParent(GameObject g, bool isroot=false)
    {
        if (g) { parent = g; }
        root = isroot;
    }

    public void Activate()
    {
        pulseFlag = true;
        childActivationFlag = true;
        activationTime = Time.time;
    }

    // Use this for initialization
    void Start() {
        if (!cacheDone)
        {
            cacheDone = true;
            sinCache = new float[sinSteps];
            float radianStep = 1.0f / sinSteps;
            for(int i = 0; i < sinSteps; i++)
            {
                sinCache[i] = Mathf.Sin(i * radianStep * Mathf.PI);
            }
            print(sinCache.Length);
        }
        rend = gameObject.GetComponent<Renderer>();
        mat = rend.material;
        rend.enabled = false;
        //mat.SetColor("_EmissionColor", defaultColor);
    }

    void Update()
    {
        float timeSinceActivation = Time.time - activationTime;
        if (pulseFlag)
        {
            if (timeSinceActivation < speed)
            {
                rend.enabled = true;
                float progress = timeSinceActivation / speed;
                float intensity = Mathf.Sin(progress * Mathf.PI);
                mat.SetColor("_EmissionColor", color * intensity);
            }
            else
            {
                pulseFlag = false;
                rend.enabled = false;
                //mat.SetColor("_EmissionColor", defaultColor);
            }
        }

        if (childActivationFlag)
        {
            if (timeSinceActivation > delay)
            {
                foreach (GameObject child in children)
                {
                    if (child) { child.GetComponent<PassLights>().Activate(); }
                }
                childActivationFlag = false;
            }
        }
    }
}

internal static class sinCache
{
    private static int steps = 60;
    private static List<float> cache;

    
}