﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:36
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
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