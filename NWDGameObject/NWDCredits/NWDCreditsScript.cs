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

    // Use this for initialization
    void Start()
    {
        InstallCredits();
    }

    void InstallCredits()
    {
        Credits = CreditsReference.GetObject();
        if (Credits != null)
        {
            Title.text = Credits.Title.GetLocalString();
            // install your company
            // add prefab company
            NWDCreditsCompany tCompany = Credits.Company.GetObject();
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
                foreach (NWDCreditsStuff tStuff in Credits.StuffList.GetObjects())
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
                foreach (NWDCreditsCompany tOtherCompany in Credits.CompanyList.GetObjects())
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
            LegalFooter.text = Credits.LegalFooter.GetLocalString();
            // install the Credits legal copyright 
            Copyright.text = Credits.Copyright.GetLocalString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
