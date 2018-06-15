using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NetWorkedData;
using UnityEngine.UI;

public class NWDCreditsScript : MonoBehaviour
{

    public NWDCreditsConnection CreditsReference;

    public GameObject StuffPrefab;

    public Text Title;
    public Text LegalFooter;
    public Text Copyright;

    private NWDCredits Credits;
    private NWDCreditsCompanyScript Company;

    private List<NWDCreditsStuffScript> StuffList = new List<NWDCreditsStuffScript> ();
    private List<NWDCreditsCompanyScript> CompanyList = new List<NWDCreditsCompanyScript>();

    // Use this for initialization
    void Start()
    {
        InstallCredits();
        ResizeCredits();
    }

    void InstallCredits()
    {
        Credits = CreditsReference.GetObject();
        if (Credits != null)
        {
            Title.text = Credits.Title.GetLocalString();
            // install your company
            // add prefab company

            // install the Credits Stuff 
            // add prefab Stuff
            // Install Stuff Member

            // install the Credits Company
            // add prefab Company
            // Install Companies logo, etc.

            // install the Credits legal footer 
            LegalFooter.text = Credits.LegalFooter.GetLocalString();
            // install the Credits legal copyright 
            Copyright.text = Credits.Copyright.GetLocalString();
        }
    }
    void ResizeCredits()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
