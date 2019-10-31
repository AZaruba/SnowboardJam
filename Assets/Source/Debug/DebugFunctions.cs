using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFunctions : MonoBehaviour {

    [SerializeField] Text debugText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    public void DisplayFloat(string floatName, float number)
    {
        debugText.text = floatName + ": " + number;
    }

    public void DisplayState(string objName, StateRef state)
    {
        debugText.text = objName + ": " + state;
    }
}
