using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseBehavior : MonoBehaviour {

    
    [Range(0.0f, 10.0f)]
    public float endSize;
    [Range(0, 60)]
    public int frameDelay;
    [Range(0, 2)]
    public int Heart_BloodOX_Resp;

    public float speed = 0.0f;

    private Transform tr;
    private float timer;
    private float startSize;
    private float yScale;
    private bool delayActive;
    

    // Use this for initialization
    void Start () {
        this.tr = GetComponent<Transform>();
        this.startSize = Mathf.Min(this.tr.localScale.x, this.tr.localScale.z);
        this.timer = 0;
        this.yScale = this.tr.localScale.y;
        if (this.frameDelay > 0) this.delayActive = true; else this.delayActive = false;
	}

    void FixedUpdate()
    {
        switch (this.Heart_BloodOX_Resp)
        {
            case 0:
                this.speed = MongoInterface.GetHeartbeat();
                break;
            case 1:
                this.speed = MongoInterface.GetBloodOxygen();
                break;
            case 2:
                this.speed = MongoInterface.GetRespiration();
                break;
            default:
                this.speed = MongoInterface.GetHeartbeat();
                break;
        }
        this.timer += speed/3600.0f;
        if (this.delayActive && this.timer > this.frameDelay)
        {
            this.timer = 0;
            this.delayActive = false;
        }
        
        if (this.delayActive)
        {
            return;
        }

        float newSize = this.startSize + ( ( 0.5f * (this.endSize - this.startSize) * (Mathf.Cos( ( this.timer * 2 * Mathf.PI ) + Mathf.PI) + 1) ) );
        this.tr.localScale = new Vector3(newSize, this.yScale, newSize);
    }
}
