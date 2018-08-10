using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetWorkedData;

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
public class NWDVersionScript : MonoBehaviour {
    //-------------------------------------------------------------------------------------------------------------
	// Use this for initialization
	void Start () {
    }
    //-------------------------------------------------------------------------------------------------------------
	// Update is called once per frame
	void Update () {
    }
    //-------------------------------------------------------------------------------------------------------------
    public void SendAppLinksByEmail ()
    {
        //Debug.Log("NWDVersionScript SendAppLinksByEmail()");
        NWDVersion.RecommendationBy(NWDRecommendationType.Email);
    }
    //-------------------------------------------------------------------------------------------------------------
    public void SendAppLinksByEmailHTML()
    {
        //Debug.Log("NWDVersionScript SendAppLinksByEmailHTML()");
        //Debug.Log("NWDVersionScript SendAppLinksByEmailHTML() NOT AVAILABLE !!!!!!!!");
        //NWDVersion.RecommendationBy(NWDRecommendationType.EmailHTML);
    }
    //-------------------------------------------------------------------------------------------------------------
    public void SendAppLinksBySMS()
    {
        //Debug.Log("NWDVersionScript SendAppLinksBySMS()");
        NWDVersion.RecommendationBy(NWDRecommendationType.SMS);
    }
    //-------------------------------------------------------------------------------------------------------------
}
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++