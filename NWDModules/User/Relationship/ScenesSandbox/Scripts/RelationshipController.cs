//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetWorkedData;

//=====================================================================================================================
namespace SpiritOfBottle
{
    public class RelationshipController : SceneController
    {
        //-------------------------------------------------------------------------------------------------------------
        public Text TextBack;
        public Text TextAddRelationship;
        //-------------------------------------------------------------------------------------------------------------
        public GameObject RackRelationship;
        public GameObject RelationshipPrefab;
        //-------------------------------------------------------------------------------------------------------------
        public GameObject PanelAddRelationship;
        //-------------------------------------------------------------------------------------------------------------
        public void AddRelationship()
        {
            GameObject tPanel = Instantiate(PanelAddRelationship, Canvas, false);
            PanelAddRelationship k = tPanel.GetComponent<PanelAddRelationship>() as PanelAddRelationship;
            k.validateRelationshipBlockDelegate = delegate(bool result) {
                if(result)
                {
                    ShowNotification("RELATIONSHIP","You've got a new Relationship!");
                }
                else
                {
                    ShowNotification("RELATIONSHIP", "New Relationship aborded!");
                }
            };
            k.ShowPanel();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            // Localized text
            NWDLocalization.AutoLocalize(TextBack);
            NWDLocalization.AutoLocalize(TextAddRelationship);

            InitRelationship();
        }
        //-------------------------------------------------------------------------------------------------------------
        void InitRelationship() 
        {
            
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================