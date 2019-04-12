// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:29
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
public class NWDCreditsCompanyScript : MonoBehaviour
{
    //public NWDCreditsCompanyConnection CompanyReference;
    //private NWDCreditsCompany Company;
    public Text Title;
    public Image Logo;
    // Use this for initialization
    void Start()
    {
        
    }

    public void Install(NWDCreditsCompany sCompany)
    {
        if (sCompany != null)
        {
            Title.text = sCompany.Title.GetLocalString();
            Sprite tSprite = sCompany.Logo.ToSprite();
            if (tSprite == null)
            {
                Logo.gameObject.SetActive(false);
            }
            else
            {
                Logo.gameObject.SetActive(true);
                Logo.sprite = sCompany.Logo.ToSprite();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
