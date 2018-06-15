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
