using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateHeaderFields : MonoBehaviour
{
    public GameObject FieldPrefab; // This is our prefab object that will be exposed in the inspector
    public string[] testArray = { "Status", "Disable Visual", "Radio Name", "Last Received", "View" };

    //public int numberToCreate; // number of objects to create. Exposed in inspector

    void Start()
    {
        Populate();
    }

    void Update()
    {

    }

    void Populate()
    {
        GameObject fieldObj; // Create GameObject instance
        Text fieldText;

        for (int i = 0; i < testArray.Length; i++)
        {
            // Create new instances of our prefab until we've created as many as we specified
            fieldObj = (GameObject)Instantiate(FieldPrefab, transform);

            // Set text field info
            fieldText = fieldObj.GetComponent<Button>().transform.Find("Text").GetComponent<Text>();
            fieldText.text = testArray[i];
            fieldText.fontSize = 20;
        }

    }
}