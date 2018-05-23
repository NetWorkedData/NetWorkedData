//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections.Generic;
using NetWorkedData;
using UnityEngine;
using UnityEngine.UI;

//=====================================================================================================================
namespace Babaoo
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 
    /// </summary>
    public class QuestTracker : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public Text TextQuestTitle;
        public GameObject RackShowItemList;
        public GameObject ItemPrefab;
        //-------------------------------------------------------------------------------------------------------------
        private Animator Anim;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            // Get animator component
            Anim = GetComponent<Animator>();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetQuest(NWDQuestUserAdvancement data)
        {
            NWDQuest tQuest = data.QuestReference.GetObject();
            //NWDDialog tDialog = data.LastDialogReference.GetObject();

            // Set quest title
            TextQuestTitle.text = tQuest.Title.GetLocalString() + " :";

            // Clear the Rack of all quests
            foreach (Transform child in RackShowItemList.transform)
            {
                Destroy(child.gameObject);
            }

            // Show items need to complet que quest
            Dictionary<NWDItem, int> tItems = tQuest.DesiredItemsToRemove.GetObjectAndQuantity();
            foreach (KeyValuePair<NWDItem, int> pair in tItems)
            {
                NWDItem tItem = pair.Key;
                int tQte = pair.Value;

                // Ownership qte
                int tOwnerQte = NWDOwnership.QuantityForItem(tItem.Reference);

                // Check for quantities in ownership
                GameObject tPrefab = Instantiate(ItemPrefab, RackShowItemList.transform, false);
                Text t = tPrefab.GetComponent<Text>() as Text;
                string tCountable = "";
                if (tItem.Uncountable)
                {
                    if (tOwnerQte >= 1)
                    {
                        tCountable = " - OK";
                    }
                }
                else
                {
                    tCountable = " - " + tOwnerQte + " / " + tQte;
                }
                t.text = ". " + tItem.Name.GetLocalString() + tCountable;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
