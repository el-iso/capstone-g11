using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallSpawner : MonoBehaviour {

    int pos = 0;
    int spawnRate = 0;

    //prefab to spawn 
    public GameObject prefab;
    //color to apply to prefab
    public Color color;

    //MongoContainer is a game object with MongoInterface.cs as a component
    public GameObject MongoContainer;
    private MongoInterface mongo;

    public float ScaleConstant = 1.0f;
    public bool heart = true;
    public bool respiration = true;
    public bool oxidization = true;

    private Vector3 scale_at_start;
    private float radians = 0.0f;


    private float updateData()
    {
        if (heart)
        {
            float data = mongo.GetHeartbeat() % 100;

            return data;
        }
        if (respiration)
        {
            float data = mongo.GetRespiration() % 100;
            return data;
        }
        if (oxidization)
        {
            float data = mongo.GetBloodOxygen() % 100;
            return data;
        }

        return 60.0f;
    }

    // Use this for initialization
    void Start()
    {
       
        //Renderer rend = prefab.GetComponent(typeof(Renderer)) as Renderer;
        //rend.sharedMaterial.color = color;

        mongo = MongoContainer.GetComponent<MongoInterface>();
        scale_at_start = transform.localScale;
    }

    void Update () {
        
        if (spawnRate > 0)
        {
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //cube.AddComponent<Rigidbody>();
            //cube.transform.position = new Vector3(pos, 0, 0);
            //Rigidbody rb = cube.GetComponent<Rigidbody>();
            //rb.useGravity = false;
            //rb.velocity = new Vector3(0, -5, 0);
            GameObject clone = Instantiate(prefab, new Vector3(0, 0, updateData()), Quaternion.identity);
            Renderer rend = clone.GetComponent(typeof(Renderer)) as Renderer;
            rend.material.color = color;

            spawnRate = 0;
        }
        else
        {
            spawnRate++;
        }
       
    }

}
