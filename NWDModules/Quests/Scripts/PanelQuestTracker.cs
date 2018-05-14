//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using NetWorkedData;
using UnityEngine.UI;

//=====================================================================================================================
namespace Babaoo
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 
    /// </summary>
    public class PanelQuestTracker : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public GameObject RackQuestTracker;
        public GameObject QuestTrackerPrefab;
        public GameObject PanelTracker;
        public Text TextButtonShowTracker;
        //-------------------------------------------------------------------------------------------------------------
        private static PanelQuestTracker Instance = null;
        private Animator Anim;
        //-------------------------------------------------------------------------------------------------------------
        public static PanelQuestTracker UnityShareInstance()
        {
            return Instance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RefreshTracker()
        {
            InitQuestTracker();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowQuestTracker()
        {
            // Show messages panel
            PanelTracker.SetActive(!PanelTracker.activeInHierarchy);

            // Change button show tracker symbol
            TextButtonShowTracker.text = "+";
            if (PanelTracker.activeInHierarchy)
            {
                TextButtonShowTracker.text = "-";
                RefreshTracker();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            //Check if there is already an instance
            if (Instance == null)
                //if not, set it to this.
                Instance = this;
            //If instance already exists:
            else if (Instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one instance.
                Destroy(gameObject);

            Anim = GetComponent<Animator>();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            InitQuestTracker();
        }
        //-------------------------------------------------------------------------------------------------------------
        void InitQuestTracker()
        {
            // Clear the Rack of all Quest been tracked
            foreach (Transform child in RackQuestTracker.transform)
            {
                Destroy(child.gameObject);
            }

            // Init quest tracked (accepted)
            NWDQuestUserAdvancement[] tQuests = NWDQuestUserAdvancement.GetAllObjects();
            foreach (NWDQuestUserAdvancement tQuest in tQuests)
            {
                if (tQuest.QuestState == NWDQuestState.Accept)
                {
                    GameObject tPrefab = Instantiate(QuestTrackerPrefab, RackQuestTracker.transform, false);
                    QuestTracker t = tPrefab.GetComponent<QuestTracker>() as QuestTracker;
                    t.SetQuest(tQuest);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
