// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:38
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetWorkedData;

public class NWDCreditsStuffScript : MonoBehaviour {

    public NWDCreditsStuffConnection StuffReference;
    private NWDCreditsStuff Stuff;
    public Text Title;
    public GameObject PanelGrid;
    public Image PanelImage;
	// Use this for initialization
	void Start () {
        Stuff = StuffReference.GetData();
        if (Stuff != null)
        {
            Title.text = Stuff.Title.GetLocalString();
        }
    }
    public void Install(NWDCreditsStuff sStuff, GameObject sMemberPrefab)
    {
        if (sStuff != null && sMemberPrefab!=null && PanelGrid!=null)
        {
            Title.text = sStuff.Title.GetLocalString();
            // install the member
            PanelImage.color = sStuff.Tint.GetColor();
            foreach (NWDCreditsMember tMember in sStuff.MemberList.GetObjects())
            {
                GameObject tPrefab = Instantiate(sMemberPrefab, PanelGrid.transform, false);
                NWDCreditsMemberScript tMemberScript = tPrefab.GetComponent<NWDCreditsMemberScript>() as NWDCreditsMemberScript;
                tMemberScript.Install(tMember,sStuff.Tint.GetColor());
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
