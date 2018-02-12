using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSetDial : MonoBehaviour {

    [Range(0, 2)]
    public int Heart_BloodOX_Resp;

    private Transform tr;
    private Vector3 startRotation;

    // Use this for initialization
    void Start () {
        this.tr = GetComponent<Transform>();
        this.startRotation = tr.localEulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
        //Get some data value here
        //float data_value = 100;
        float data_value = 0;

        switch (this.Heart_BloodOX_Resp)
        {
            case 0:
                data_value = MongoInterface.GetHeartbeat();
                break;
            case 1:
                data_value = MongoInterface.GetBloodOxygen();
                break;
            case 2:
                data_value = MongoInterface.GetRespiration();
                break;
            default:
                data_value = MongoInterface.GetHeartbeat();
                break;
        }

        Vector3 newRotation = this.startRotation;
        newRotation.y += data_value;
        tr.localEulerAngles = newRotation;
        
    }
}
