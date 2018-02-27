using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataRowBehavior : MonoBehaviour
{
    public GameObject FieldPrefab, ViewGraphPrefab, StatusPrefab, DisablePrefab, EnablePrefab; // This is our prefab object that will be exposed in the inspector
    public string[] testArray = { "Data", "Data", "Data", "Data", "Data" };
    public Sprite buttonImage;

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
        GameObject currObj; // Create GameObject instance
        Text currText;

        // Field 0: Status button
        currObj = (GameObject)Instantiate(StatusPrefab, transform);
        currText = currObj.GetComponent<Button>().transform.Find("Text").GetComponent<Text>();
        currText.text = "";

        // Field 1: Disable button
        currObj = (GameObject)Instantiate(DisablePrefab, transform);
        currText = currObj.GetComponent<Button>().transform.Find("Text").GetComponent<Text>();
        currText.text = "";

        // Field 2: Field button (radio name)
        currObj = (GameObject)Instantiate(FieldPrefab, transform);
        currText = currObj.GetComponent<Button>().transform.Find("Text").GetComponent<Text>();
        currText.text = "Radio Name";

        // Field 3: Field button (last received transmission)
        currObj = (GameObject)Instantiate(FieldPrefab, transform);
        currText = currObj.GetComponent<Button>().transform.Find("Text").GetComponent<Text>();
        currText.text = "--- seconds";

        // Field 4: View graph button
        currObj = (GameObject)Instantiate(ViewGraphPrefab, transform);
        currText = currObj.GetComponent<Button>().transform.Find("Text").GetComponent<Text>();
        currText.text = "";

    }
}

//Sprite myImage = Resources.Load<Sprite>("/ControlCenter/Pictures/Green_Check_Mark");
//Image test = Resources.Load<Image>("/Green_check_mark");

//GameObject test = GameObject.Find("/Canvas/TestButton");
//Button test2 = test.GetComponent<Button>();
//Sprite test3 = test2.image.sprite;
//Debug.Log(test3.ToString());
//Debug.Log(test3 == null);

//fieldObj.GetComponent<Button>().image.sprite = Resources.Load<Sprite>("Green_check_mark");
//Debug.Log(fieldObj.GetComponent<Button>().image.sprite == null);