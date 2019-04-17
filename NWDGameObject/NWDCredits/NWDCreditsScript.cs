// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:36
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

using NetWorkedData;
using UnityEngine.UI;

public class NWDCreditsScript : MonoBehaviour
{

    public NWDCreditsConnection CreditsReference;
    private NWDCredits Credits;
    public GameObject CompanyPrefab;
    public GameObject StuffPrefab;
    public GameObject MemberPrefab;
    public GameObject MemberJobPrefab;
    public GameObject MemberActorPrefab;
    public GameObject OtherCompanyPrefab;

    public Text Title;
    public Text LegalFooter;
    public Text Copyright;

    public GameObject CompanyPanel;
    public GameObject StuffListPanel;
    public GameObject CompanyListPanel;

    public GameObject SeparatorOne;
    public GameObject SeparatorTwo;
    // Use this for initialization
    void Start()
    {
        InstallCredits();
    }
    public void RefreshCredits()
    {
        RemoveCredits();
        InstallCredits();
    }
    void RemoveCredits()
    {
        foreach (Transform tChild in CompanyPanel.transform)
        {
            GameObject.Destroy(tChild.gameObject);
        }
        foreach (Transform tChild in StuffListPanel.transform)
        {
            GameObject.Destroy(tChild.gameObject);
        }
        foreach (Transform tChild in CompanyListPanel.transform)
        {
            GameObject.Destroy(tChild.gameObject);
        }
    }
    void InstallCredits()
    {
        Credits = CreditsReference.GetData();
        if (Credits != null)
        {
            Title.text = Credits.Title.GetLocalString();
            // install your company
            // add prefab company
            NWDCreditsCompany tCompany = Credits.Company.GetData();
            if (tCompany != null && CompanyPanel != null)
            {
                GameObject tPrefab = Instantiate(CompanyPrefab, CompanyPanel.transform, false);
                NWDCreditsCompanyScript tCompanyScript = tPrefab.GetComponent<NWDCreditsCompanyScript>() as NWDCreditsCompanyScript;
                tCompanyScript.Install(tCompany);
            }
            // install the Credits Stuff 
            // add prefab Stuff
            // Install Stuff Member
            if (StuffListPanel != null)
            {
                foreach (NWDCreditsStuff tStuff in Credits.StuffList.GetReachableDatas())
                {

                    GameObject tPrefabData = null;
                    if (tStuff.Prefab != null)
                    {
                        tPrefabData =  tStuff.Prefab.ToPrefab();
                    }
                    GameObject tMemberPrefabData = null;
                    if (tStuff.MemberPrefab!=null)
                    {
                        tStuff.MemberPrefab.ToPrefab();
                    }
                    if (tMemberPrefabData == null)
                    {
                        tMemberPrefabData = MemberPrefab;
                    }
                    if (tPrefabData != null)
                    {
                        GameObject tPrefab = Instantiate(tPrefabData, StuffListPanel.transform, false);
                        NWDCreditsStuffScript tStuffScript = tPrefab.GetComponent<NWDCreditsStuffScript>() as NWDCreditsStuffScript;
                        tStuffScript.Install(tStuff, tMemberPrefabData);
                    }
                    else if (StuffPrefab != null)
                    {
                        GameObject tPrefab = Instantiate(StuffPrefab, StuffListPanel.transform, false);
                        NWDCreditsStuffScript tStuffScript = tPrefab.GetComponent<NWDCreditsStuffScript>() as NWDCreditsStuffScript;
                        tStuffScript.Install(tStuff, tMemberPrefabData);
                    }
                }
            }

            // install the Credits Company
            // add prefab Company
            // Install Companies logo, etc.
            if (CompanyListPanel != null)
            {
                foreach (NWDCreditsCompany tOtherCompany in Credits.CompanyList.GetReachableDatas())
                {
                    if (OtherCompanyPrefab != null)
                    {
                        GameObject tPrefab = Instantiate(OtherCompanyPrefab, CompanyListPanel.transform, false);
                        NWDCreditsCompanyScript tCompanyScript = tPrefab.GetComponent<NWDCreditsCompanyScript>() as NWDCreditsCompanyScript;
                        tCompanyScript.Install(tOtherCompany);
                    }
                }
            }

            // install the Credits legal footer 
            string tLegalFooterText  = Credits.LegalFooter.GetLocalString();
            if (string.IsNullOrEmpty(tLegalFooterText))
            {
                SeparatorOne.SetActive(false);
            }
            else
            {
                SeparatorOne.SetActive(true);
            }
                LegalFooter.text = tLegalFooterText;
            // install the Credits legal copyright 
            string tCopyrightText = Credits.Copyright.GetLocalString();
            if (string.IsNullOrEmpty(tCopyrightText))
            {
                SeparatorTwo.SetActive(false);
            }
            else
            {
                SeparatorTwo.SetActive(true);
            }
            Copyright.text = tCopyrightText;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
