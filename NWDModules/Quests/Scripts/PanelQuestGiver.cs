//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using NetWorkedData;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace Babaoo
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 
    /// </summary>
    public class PanelQuestGiver : MonoBehaviour
    {
        void HandleBTBNotificationBlock(BTBNotification sNotification)
        {
        }


        //-------------------------------------------------------------------------------------------------------------
        [Header("GameObject's connections")]
        public GameObject PanelShowQuestList;
        public GameObject PanelShowQuestDialogue;
        public GameObject QuestPrefab;
        public GameObject RackShowQuestList;
        public Text TextNoQuestAvailable;
        public Text TextDialogue;
        public Text TextButtonChoiceA;
        public Text TextButtonChoiceB;
        public Text TextButtonChoiceC;
        public Text TextButtonChoiceD;
        public Text TextButtonChoiceE;
        public Button ButtonChoiceA;
        public Button ButtonChoiceB;
        public Button ButtonChoiceC;
        public Button ButtonChoiceD;
        public Button ButtonChoiceE;
        //-------------------------------------------------------------------------------------------------------------
        [Header("Bubble design")]
        public Image ImageDialogBubble;
        public bool UseDialogBubbleImage = false;
        public Sprite SpeechBubbleSprite;
        public Sprite WhisperBubbleSprite;
        public Sprite ThoughtBubbleSprite;
        public Sprite ScreamBubbleSprite;
        public Sprite NarrativeBubbleSprite;
        public Sprite DivineBubbleSprite;
        public Sprite SubconscientBubbleSprite;
        //-------------------------------------------------------------------------------------------------------------
        public Color CharacterDisable = Color.gray;
        public Color CharacterEnable = Color.white;
        public Image ImageNPCLeft;
        public Image ImageNPCMiddle;
        public Image ImageNPCRight;
        public Text NicknameNPCLeft;
        public Text NicknameNPCMiddle;
        public Text NicknameNPCRight;
        //-------------------------------------------------------------------------------------------------------------
        private NWDCharacter CharacterLeft = null;
        private NWDCharacter CharacterMiddle = null;
        private NWDCharacter CharacterRight = null;
        private Animator PanelAnim;
        private bool DestroyAfterClosing = false;
        private NWDQuest ActiveQuest;
        private NWDDialog NextDialogueA;
        private NWDDialog NextDialogueB;
        private NWDDialog NextDialogueC;
        private NWDDialog NextDialogueD;
        private NWDDialog NextDialogueE;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            // Localized text
            NWDLocalization.AutoLocalize(TextNoQuestAvailable);

            // Get animator component
            PanelAnim = GetComponent<Animator>();

            // Show Panel Quest List if NPC have more than one quest
            VerifyQuestList();

            BTBNotificationManager.SharedInstance().AddObserver(this, NWDNotificationConstants.K_LANGUAGE_CHANGED, delegate (BTBNotification sNotification)
            {
                LanguageReload();
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        // Use this for destroy
        void OnDestroy()
        {
            BTBNotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LanguageReload()
        {
            TextNoQuestAvailable.text = NWDLocalization.GetLocalText("•NO-MORE-QUEST•");
        }
        //-------------------------------------------------------------------------------------------------------------
        void VerifyQuestList()
        {
            //PanelShowQuestList.SetActive(true);
        }
        //-------------------------------------------------------------------------------------------------------------
        void ShowFirstdialogue(NWDDialog sDialog)
        {
            if (sDialog != null)
            {
                NextDialogueA = sDialog;
                // reset characters in dialog
                ImageNPCLeft.gameObject.SetActive(false);
                ImageNPCRight.gameObject.SetActive(false);
                ImageNPCMiddle.gameObject.SetActive(false);

                PanelShowQuestList.SetActive(false);
                PanelShowQuestDialogue.SetActive(true);
                if (PanelAnim != null)
                {
                    PanelAnim.SetTrigger("ShowDialogue");
                }
                GoNextDialogue();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowPanel(NWDSetOfQuests sQuests)
        {
            gameObject.SetActive(true);
            DestroyAfterClosing = true;
            SetQuestList(sQuests);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetQuest(NWDQuest sQuest)
        {
            ActiveQuest = sQuest;
            NWDQuestUserAdvancement tQuestUserAdvancement = NWDQuestUserAdvancement.GetAdvancementForQuest(ActiveQuest);
            NWDDialog tDialog = ActiveQuest.FirstDialogOnShowQuest(tQuestUserAdvancement);
            ShowFirstdialogue(tDialog);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDialog(NWDDialog sDialog)
        {
            ActiveQuest = null;
            ShowFirstdialogue(sDialog);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetQuestList(NWDSetOfQuests sQuests)
        {
            // Show NPC image
            NWDCharacter tCharacter = sQuests.CharacterReference.GetObject();
            if (tCharacter != null)
            {
                Sprite tSprite = tCharacter.NormalState.ToSprite();
                if (tSprite != null)
                {
                    ImageNPCLeft.sprite = tSprite;
                }
                else
                {
                    ImageNPCLeft.sprite = null;
                }
            }

            // Clear the Rack of all quest
            foreach (Transform child in RackShowQuestList.transform)
            {
                Destroy(child.gameObject);
            }

            // Init all quest given by npc
            NWDQuest[] tQuests = sQuests.QuestsList.GetObjects();
            bool isAnyQuestToShow = false;

            List<NWDQuest> tQuestList = new List<NWDQuest>();


            foreach (NWDQuest tQuest in tQuests)
            {
                // Get current Started quest by active user
                NWDQuestUserAdvancement tQuestAdvanacement = NWDQuestUserAdvancement.GetAdvancementForQuest(tQuest);
                if (tQuestAdvanacement.AvailableQuest())
                {
                    tQuestList.Add(tQuest);
                }
            }

            if (tQuestList.Count == 0)
            {
                isAnyQuestToShow = false;
            }
            else if (tQuestList.Count == 1)
            {
                PanelShowQuestList.SetActive(false);
                isAnyQuestToShow = false;
                SetQuest(tQuestList[0]);
            }
            else
            {
                PanelShowQuestList.SetActive(true);
                isAnyQuestToShow = false;
                foreach (NWDQuest tQuest in tQuestList)
                {
                    // Get current Started quest by active user
                    NWDQuestUserAdvancement tQuestAdvanacement = NWDQuestUserAdvancement.GetAdvancementForQuest(tQuest);
                    if (tQuestAdvanacement.AvailableQuest())
                    {
                        GameObject tPrefab = Instantiate(QuestPrefab, RackShowQuestList.transform, false);
                        Quest t = tPrefab.GetComponent<Quest>() as Quest;
                        t.questBlockDelegate = delegate (NWDQuest sQuest)
                        {
                            SetQuest(sQuest);
                        };
                        t.SetQuest(tQuest);
                        isAnyQuestToShow = true;
                    }
                }
            }
            TextNoQuestAvailable.gameObject.SetActive(false);
            if (!isAnyQuestToShow)
            {
                TextNoQuestAvailable.gameObject.SetActive(true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GoNextDialogue(int buttonId = 0)
        {
            NWDDialog tDialogue = null;
            if (buttonId == 0)
            {
                tDialogue = NextDialogueA;
            }
            else if (buttonId == 1)
            {
                tDialogue = NextDialogueB;
            }
            else if (buttonId == 2)
            {
                tDialogue = NextDialogueC;
            }
            else if (buttonId == 3)
            {
                tDialogue = NextDialogueD;
            }
            else if (buttonId == 4)
            {
                tDialogue = NextDialogueE;
            }
            if (tDialogue != null)
            {
                // Progress in quest with this dialog
                if (ActiveQuest != null)
                {
                    NWDQuestUserAdvancement.AdvancementDialog(ActiveQuest, tDialogue);
                }
                // Use character
                NWDCharacter tCharacter = tDialogue.CharacterReference.GetObject();
                if (tCharacter != null)
                {
                    ImageNPCLeft.color = CharacterDisable;
                    ImageNPCRight.color = CharacterDisable;
                    ImageNPCMiddle.color = CharacterDisable;

                    Sprite tCharacterSprite = tCharacter.GetSpriteForEmotion(tDialogue.CharacterEmotion);
                    switch (tDialogue.CharacterPosition)
                    {
                        case NWDCharacterPositionType.Left:
                            {
                                CharacterLeft = tCharacter;
                                ImageNPCLeft.sprite = tCharacterSprite;
                                ImageNPCLeft.gameObject.SetActive(true);
                                ImageNPCLeft.color = CharacterEnable;
                                NicknameNPCLeft.text = tCharacter.NickName.GetLocalString();
                                if (CharacterMiddle == tCharacter)
                                {
                                    CharacterMiddle = null;
                                    ImageNPCMiddle.gameObject.SetActive(false);
                                }
                                if (CharacterRight == tCharacter)
                                {
                                    CharacterRight = null;
                                    ImageNPCRight.gameObject.SetActive(false);
                                }
                            }
                            break;
                        case NWDCharacterPositionType.Middle:
                            {
                                CharacterMiddle = tCharacter;
                                ImageNPCMiddle.sprite = tCharacterSprite;
                                ImageNPCMiddle.gameObject.SetActive(true);
                                ImageNPCMiddle.color = CharacterEnable;
                                NicknameNPCMiddle.text = tCharacter.NickName.GetLocalString();
                                if (CharacterLeft == tCharacter)
                                {
                                    CharacterLeft = null;
                                    ImageNPCLeft.gameObject.SetActive(false);
                                }
                                if (CharacterRight == tCharacter)
                                {
                                    CharacterRight = null;
                                    ImageNPCRight.gameObject.SetActive(false);
                                }
                            }
                            break;
                        case NWDCharacterPositionType.Right:
                            {
                                CharacterRight = tCharacter;
                                ImageNPCRight.sprite = tCharacterSprite;
                                ImageNPCRight.gameObject.SetActive(true);
                                ImageNPCRight.color = CharacterEnable;
                                NicknameNPCRight.text = tCharacter.NickName.GetLocalString();
                                if (CharacterLeft == tCharacter)
                                {
                                    CharacterLeft = null;
                                    ImageNPCLeft.gameObject.SetActive(false);
                                }
                                if (CharacterMiddle == tCharacter)
                                {
                                    CharacterMiddle = null;
                                    ImageNPCMiddle.gameObject.SetActive(false);
                                }
                            }
                            break;
                    }
                }

                // Check action in dialogue answer
                NWDAction[] tActionsOnAnswer = tDialogue.ActionOnAnswer.GetObjects();
                foreach (NWDAction tAction in tActionsOnAnswer)
                {
                    // Trigger action
                    tAction.PostNotification();
                }

                // Set dialogue to show
                string tNextDialog = tDialogue.DialogRichText(true);

                if (string.IsNullOrEmpty(tNextDialog) == false)
                {
                    // change bubble design 
                    if (UseDialogBubbleImage)
                    {
                        switch (tDialogue.BubbleStyle)
                        {
                            case NWDBubbleStyleType.Speech:
                                {
                                    ImageDialogBubble.sprite = SpeechBubbleSprite;
                                }
                                break;
                            case NWDBubbleStyleType.Whisper:
                                {
                                    ImageDialogBubble.sprite = WhisperBubbleSprite;
                                }
                                break;
                            case NWDBubbleStyleType.Thought:
                                {
                                    ImageDialogBubble.sprite = ThoughtBubbleSprite;
                                }
                                break;
                            case NWDBubbleStyleType.Scream:
                                {
                                    ImageDialogBubble.sprite = ScreamBubbleSprite;
                                }
                                break;
                            case NWDBubbleStyleType.Narrative:
                                {
                                    ImageDialogBubble.sprite = NarrativeBubbleSprite;
                                }
                                break;
                            case NWDBubbleStyleType.Divine:
                                {
                                    ImageDialogBubble.sprite = DivineBubbleSprite;
                                }
                                break;
                            case NWDBubbleStyleType.Subconscient:
                                {
                                    ImageDialogBubble.sprite = SubconscientBubbleSprite;
                                }
                                break;
                        }
                    }

                    TextDialogue.text = tNextDialog;

                    // Check action in dialogue answer
                    NWDAction[] tActionsOnDialog = tDialogue.ActionOnDialog.GetObjects();
                    foreach (NWDAction tAction in tActionsOnDialog)
                    {
                        // Trigger action
                        tAction.PostNotification();
                    }

                    // Hide all buttons
                    ButtonChoiceA.gameObject.SetActive(false);
                    ButtonChoiceB.gameObject.SetActive(false);
                    ButtonChoiceC.gameObject.SetActive(false);
                    ButtonChoiceD.gameObject.SetActive(false);
                    ButtonChoiceE.gameObject.SetActive(false);

                    // Check for next dialogue
                    List<NWDDialog> tDialogues = tDialogue.GetNextDialogs();
                    if (tDialogues.Count > 0)
                    {
                        if (tDialogues.Count > 0)
                        {
                            ButtonChoiceA.gameObject.SetActive(true);
                            NextDialogueA = tDialogues[0];
                            TextButtonChoiceA.text = NextDialogueA.AnswerRichText(false);
                        }
                        if (tDialogues.Count > 1)
                        {
                            ButtonChoiceB.gameObject.SetActive(true);
                            NextDialogueB = tDialogues[1];
                            TextButtonChoiceB.text = NextDialogueB.AnswerRichText(false);
                        }
                        if (tDialogues.Count > 2)
                        {
                            ButtonChoiceC.gameObject.SetActive(true);
                            NextDialogueC = tDialogues[2];
                            TextButtonChoiceC.text = NextDialogueC.AnswerRichText(false);
                        }
                        if (tDialogues.Count > 3)
                        {
                            ButtonChoiceD.gameObject.SetActive(true);
                            NextDialogueD = tDialogues[3];
                            TextButtonChoiceD.text = NextDialogueD.AnswerRichText(false);
                        }
                        if (tDialogues.Count > 4)
                        {
                            ButtonChoiceE.gameObject.SetActive(true);
                            NextDialogueE = tDialogues[4];
                            TextButtonChoiceE.text = NextDialogueE.AnswerRichText(false);
                        }
                    }
                }
                else
                {
                    ClosePanel();
                }
            }
            else
            {
                ClosePanel();
                #if UNITY_EDITOR
                // Dialog is null ?
                UnityEditor.EditorUtility.DisplayDialog("NWDDIALOG", "Dialogue is null, must be an error, forgoten Dialogue to show", "OK");
                #endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ClosePanel()
        {
            if (PanelShowQuestList.activeInHierarchy)
            {
                PanelAnim.SetTrigger("Hide");
            }
            else
            {
                PanelAnim.SetTrigger("HideDialogue");
            }
            if (GameManager.GetInstance().CurrentPortal == null && InputManager.GetInstance() != null && SceneManager.GetSceneByName("MainWorld_Demo").IsValid())
            {
                InputManager.GetInstance().SetInputMode(InputManager.EInputMode.NONE);
                InputManager.GetInstance().SetNextInputMode(InputManager.EInputMode.GAME);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DestroyPanel()
        {
            gameObject.SetActive(false);
            if (DestroyAfterClosing)
            {
                Destroy(gameObject);
            }
            else
            {
                PanelShowQuestList.SetActive(true);
                PanelShowQuestDialogue.SetActive(false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================