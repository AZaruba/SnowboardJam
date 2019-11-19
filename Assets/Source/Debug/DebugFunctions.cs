using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFunctions : MonoBehaviour {

    [SerializeField] Text state_1;
    [SerializeField] Text float_1;
    [SerializeField] Text vector3_1;
    [SerializeField] Text vector3_2;
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
        float_1.text = floatName + ": " + number;
    }

    public void DisplayState(string objName, StateRef state)
    {
        state_1.text = objName + ": " + state;
    }

    public void DisplayVector3(string objName, Vector3 vec, int vectorLine = 0)
    {
        if (vectorLine > 1)
        {
            return;
        }
        if (vectorLine == 0)
        {
            vector3_1.text = objName + ": " + vec;
        }
        else if (vectorLine == 1)
        {
            vector3_2.text = objName + ": " + vec;
        }
    }
}
