using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewGraphBehavior : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().transform.Find("Text").GetComponent<Text>().text = ">";
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Button>().transform.Find("Text").GetComponent<Text>().text = ">";
    }
}
