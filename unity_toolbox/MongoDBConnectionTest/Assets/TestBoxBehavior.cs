using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
//using ConnectionTest;
using UnityEngine;

public class TestBoxBehavior : MonoBehaviour
{


    [Range(0.0f, 10.0f)]
    public float endSize;
    [Range(0, 100)]
    public int frameDelay;
    [Range(0.0f, 6.0f)]
    public float speed;
    public int dbDelay = 100;

    private Transform tr;
    private float timer;
    private float dbTimer;
    private float startSize;

    private float yScale;
    private bool delayActive;

    // Use this for initialization
    void Start()
    {
        this.tr = GetComponent<Transform>();
        this.startSize = Mathf.Min(this.tr.localScale.x, this.tr.localScale.z);
        this.timer = 0;
        this.dbTimer = 0;
        this.yScale = this.tr.localScale.y;
        if (this.frameDelay > 0) this.delayActive = true; else this.delayActive = false;
    }

    void FixedUpdate()
    {
        this.dbTimer += 1;
        if (this.dbTimer > this.dbDelay)
        {
            this.dbTimer = 0;
            getNewDBValue();
            print(this.speed);
        }



        this.timer += speed;
        if (this.delayActive && this.timer > this.frameDelay)
        {
            this.timer = 0;
            this.delayActive = false;
        }

        if (this.delayActive)
        {
            return;
        }

        //var dbValues = ConnectionTest.SearchRecentByDeviceID(0, 10);
        //foreach (var thing in dbValues)
        //{
        //    Debug.Log(thing.ToJson());
        //}
        //speed = (float) ((dbValues[0]["r"]).AsDouble) % 6;
        
        float newSize = this.startSize + ((0.5f * (this.endSize - this.startSize) * (Mathf.Cos((this.timer / 60.0f) + Mathf.PI) + 1)));
        //double newSize = this.startSize + obj;
        var scale = new Vector3(((float)newSize % float.MaxValue), this.yScale, ((float)newSize % float.MaxValue));
        this.tr.localScale = scale;
    }

    void getNewDBValue()
    {
        var dbValues = ConnectionTest.SearchRecentByDeviceID(0, 10);
        print(dbValues[0]["r"].AsDouble%6.0);
        this.speed = (float)((dbValues[0]["r"]).AsDouble) % 6;
    }
}