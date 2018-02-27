using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableButtonBehavior : MonoBehaviour {

    public Sprite buttonImage;

    // Use this for initialization
    void Start () {
        this.GetComponent<Button>().image.sprite = buttonImage;
    }
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Button>().image.sprite = buttonImage;
	}
}
