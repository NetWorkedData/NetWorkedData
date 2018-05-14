//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using NetWorkedData;
using UnityEngine.UI;
using UnityEngine;

//=====================================================================================================================
namespace Babaoo
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 
    /// </summary>
    public class QuestGiverTest : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public Text TextQuestGiver;
        public NWDCharacterConnection CharacterConnection;
        public NWDSetOfQuestsConnection SetOfQuests;
        public NWDQuestConnection Quest;
        public NWDDialogConnection Dialog;
        //-------------------------------------------------------------------------------------------------------------
        public GameObject PanelQuestGiver;
        //-------------------------------------------------------------------------------------------------------------
        private Animator Anim;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Anim = GetComponent<Animator>();

            SetQuestGiver();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetQuestGiver()
        {
            NWDCharacter tNPC = CharacterConnection.GetObject();
            if (tNPC != null)
            {
                TextQuestGiver.text = tNPC.FirstName.GetLocalString() + " " + tNPC.LastName.GetLocalString() + " (" + tNPC.NickName.GetLocalString() + ")";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowQuest(bool isFullScreen = true)
        {
            if (isFullScreen)
            {
                PanelQuestGiver tScript = PanelQuestGiver.GetComponent<PanelQuestGiver>();
                if (SetOfQuests.GetObject() != null)
                {
                    PanelQuestGiver.SetActive(true);
                    tScript.SetQuestList(SetOfQuests.GetObject());
                }
                else if (Quest.GetObject() != null)
                {
                    PanelQuestGiver.SetActive(true);
                    tScript.SetQuest(Quest.GetObject());
                }
                else if (Dialog.GetObject() != null)
                {
                    PanelQuestGiver.SetActive(true);
                    tScript.SetDialog(Dialog.GetObject());
                }
            }
            else
            {
                GameObject tPanel = Instantiate(PanelQuestGiver, transform, false);
                PanelQuestGiver k = tPanel.GetComponent<PanelQuestGiver>() as PanelQuestGiver;
                k.ShowPanel(SetOfQuests.GetObject());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================