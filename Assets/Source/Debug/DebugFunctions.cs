using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFunctions : MonoBehaviour {

    [SerializeField] Text debugText1;
    [SerializeField] Text debugText2;
    [SerializeField] Text debugText3;
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
        debugText1.text = floatName + ": " + number;
    }

    public void DisplayState(string objName, StateRef state)
    {
        debugText2.text = objName + ": " + state;
    }

    public void DisplayVector3(string objName, Vector3 vec)
    {
        debugText3.text = objName + ": " + vec;
    }
}
