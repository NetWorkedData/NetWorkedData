//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:34
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetWorkedData;

public class NWDCreditsMemberScript : MonoBehaviour {
    public Text Names;
    public Text Office;
    public Image Character;
    // Use this for initialization
    private void Start()
    {
        
    }
    public void Install(NWDCreditsMember sMember, Color sTint)
    {
        if (sMember != null)
        {
            Office.text = sMember.Office.GetLocalString();
            Office.color = sTint;
            if (string.IsNullOrEmpty(sMember.Nickname.GetLocalString()))
            {
                Names.text = sMember.Lastname.GetLocalString() + " " + sMember.Firstname.GetLocalString();
            }
            else
            {
                Names.text = sMember.Lastname.GetLocalString() + " " + sMember.Firstname.GetLocalString() + "(" + sMember.Nickname.GetLocalString() + ")";
            }
        }
    }
	
	// Update is called once per frame
    private void Update () {
		
	}
}
