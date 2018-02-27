using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusButtonBehavior : MonoBehaviour {

    public Sprite goodStatus, tentativeStatus, badStatus;
    private enum Status { GOOD, TENTATIVE, BAD };
    private Status currStatus;

	// Use this for initialization
	void Start () {
        this.currStatus = Status.GOOD;
	}
	
	// Update is called once per frame
	void Update () {
		switch (this.currStatus)
        {
            case Status.GOOD:
                this.GetComponent<Button>().image.sprite = goodStatus;
                break;
            case Status.TENTATIVE:
                this.GetComponent<Button>().image.sprite = tentativeStatus;
                break;
            case Status.BAD:
                this.GetComponent<Button>().image.sprite = badStatus;
                break;
        }
	}
}
