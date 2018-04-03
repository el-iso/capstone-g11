using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialWaterfallBehavior : MonoBehaviour
{

    public float speed = 30;
    public bool x = false;
    public bool y = false;
    public bool z = true;

    public bool heart = true;
    public bool respiration = true;
    public bool oxidization = true;
    public GameObject prefab = null;

    public int numPoints = 1;

    private List<GameObject> points;
    private List<float> prevData;
    private List<float> curData;
    private List<float> delta;
    


    private List<float> updateData(int size)
    {
        List<float> list = new List<float>();
        if (heart)
        {
            for (int i = 0; i < size; i++)
            {
                list.Add(MongoInterface.GetHeartbeat(i));
            }

            return list;
        }
        if (respiration)
        {
            for (int i = 0; i < size; i++)
            {
                list.Add(MongoInterface.GetRespiration(i));
            }

            return list;
        }
        if (oxidization)
        {
            for (int i = 0; i < size; i++)
            {
                list.Add(MongoInterface.GetBloodOxygen(i));
            }

            return list;
        }

        return new List<float>();
    }


    // Use this for initialization
    void Start()
    {
        prevData = new List<float>();
        delta = new List<float>();
        points = new List<GameObject>();
        Vector3 axis = new Vector3(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0);
        for (int i = 0; i < numPoints; i++)
        {
            GameObject g = Instantiate(prefab);
            g.transform.SetParent(transform);
            points.Add(g);

            points[i].transform.RotateAround(transform.position, axis, i*(360/numPoints));
        }

        for (int i = 0; i < numPoints; i++)
        {
            prevData.Add(0.0f);
            delta.Add((float)i*(360/numPoints));
        }
    }

    // Update is called once per frame
    void Update()
    {
        curData = updateData(numPoints);
        for(int i = 0; i < numPoints; i++)
        {
            delta.Insert(i, curData[i] - prevData[i]);
        }
        //curData = updateData();
        //delta = curData - prevData;

        //Transform parent_t = transform.parent ? transform.parent : transform;
        Vector3 point = transform.position;
        Vector3 axis = new Vector3(x ? 1 : 0, y ? 1 : 0, z ? 1 : 0);
        //transform.RotateAround(point, axis, speed * Time.deltaTime);
        for(int i = 0; i < numPoints; i++)
        {
            points[i].transform.RotateAround(point, axis, delta[i]);
        }

        for(int i = 0; i < numPoints; i++)
        {
            prevData[i] = curData[i];
        }
    }
}
