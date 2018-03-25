using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NetWorkedData;

public class NWDUserNetWorkingScript : MonoBehaviour {

	// Use this for initialization
    void Start () {
        Debug.Log("NWDUserNetWorkingScript Start()");
        NWDUserNetWorking.PrepareUpdate(20, null);
        StartCoroutine(UserNetworkinUpdate());
	}

    IEnumerator UserNetworkinUpdate()
    {
        while (true)
        {
            Debug.Log("NWDUserNetWorkingScript UserNetworkinUpdate()");
            NWDUserNetWorking.NetworkingUpdate();
            yield return new WaitForSeconds(NWDUserNetWorking.UpdateDelayInSeconds - 10);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
