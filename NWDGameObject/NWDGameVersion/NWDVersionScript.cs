//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================
using System;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
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
}
//=====================================================================================================================