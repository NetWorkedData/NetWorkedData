using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetWorkedData;

public class NWDSwitchTestResult : MonoBehaviour {

    public NWDSwitch TheSwitch;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchAction()
    {
        Debug.Log("Switch click, new value is : " + TheSwitch.SwitchState.ToString()); 
    }
}
