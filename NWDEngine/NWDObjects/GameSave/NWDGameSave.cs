//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("GSV")]
    [NWDClassDescriptionAttribute("Game Save")]
    [NWDClassMenuNameAttribute("Game Save")]
    public partial class NWDGameSave : NWDBasis<NWDGameSave>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public bool IsCurrent
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDGameSave()
        {
            //Debug.Log("NWDGameSave Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDGameSave(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDGameSave Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            //Debug.Log("NWDGameSave Initialization()");
            //GameSaveTag = -1;
            //GameSaveTagReevaluate();
            Name = " * GameSave " + DateTime.Today.ToShortDateString();
            Tag = NWDBasisTag.TagAdminCreated;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave NewCurrent()
        {
            NWDGameSave rGameSave = null;
            rGameSave = NewData();
            //rGameSave.InternalKey = NWDAccount.GetCurrentAccountReference();
            rGameSave.Name = "GameSave " + DateTime.Today.ToShortDateString();
            rGameSave.Tag = NWDBasisTag.TagUserCreated;
            rGameSave.IsCurrent = true;
            NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
            if (tAccountInfos != null)
            {
                tAccountInfos.CurrentGameSave.SetReference(rGameSave.Reference);
                tAccountInfos.SaveData();
            }
            rGameSave.SaveData();
            return rGameSave;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave Current()
        {
            NWDGameSave rParty = null;
            NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
            if (tAccountInfos != null)
            {
                if (tAccountInfos.CurrentGameSave != null)
                {
                    NWDGameSave tParty = NWDGameSave.FindDataByReference(tAccountInfos.CurrentGameSave.GetReference());
                    if (tParty != null)
                    {
                        rParty = tParty;
                    }
                }
                else
                {
                }
            }
            if (rParty == null)
            {
                NWDGameSave[] tParties = NWDGameSave.FindDatas(NWDAccount.GetCurrentAccountReference(), null);
                foreach (NWDGameSave tPart in tParties)
                {
                    if (tPart != null)
                    {
                        rParty = tPart;
                        if (tAccountInfos != null)
                        {
                            if (tAccountInfos.CurrentGameSave == null)
                            {
                                tAccountInfos.CurrentGameSave = new NWDReferenceFreeType<NWDGameSave>();
                            }
                            tAccountInfos.CurrentGameSave.SetReference(rParty.Reference);
                            tAccountInfos.SaveData();
                        }
                        break;
                    }
                }
            }
            if (rParty == null)
            {
                rParty = NewCurrent();
            }
            return rParty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave CurrentForAccount(string sAccountReference)
        {
            NWDGameSave rParty = null;
            foreach (NWDGameSave tParty in BasisHelper().Datas)
            {
                if (tParty.Account.GetReference() == sAccountReference)
                {
                    if (tParty.IsCurrent == true && tParty.IsEnable() && tParty.IsTrashed() == false && tParty.TestIntegrity() == true)
                    {
                        rParty = tParty;
                        break;
                    }
                }
            }
            return rParty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetCurrent()
        {
            foreach (NWDGameSave tParty in BasisHelper().Datas)
            {
                if (tParty.Account.GetReference() == NWDAccount.GetCurrentAccountReference() && tParty.IsEnable() && tParty.IsTrashed() == false && tParty.TestIntegrity() == true)
                {
                    tParty.IsCurrent = false;
                    tParty.SaveDataIfModified();
                }
            }
            NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
            if (tAccountInfos != null)
            {
                if (tAccountInfos.CurrentGameSave == null)
                {
                    tAccountInfos.CurrentGameSave = new NWDReferenceFreeType<NWDGameSave>();
                }
                tAccountInfos.CurrentGameSave.SetReference(Reference);
                tAccountInfos.SaveData();
            }
            IsCurrent = true;
            SaveDataIfModified();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDAccountInfos), typeof(NWDGameSave) };
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tY = sInRect.y;
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            // draw line 
            EditorGUI.DrawRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1), NWDConstants.kRowColorLine);
            tY += NWDConstants.kFieldMarge * 2;

            EditorGUI.LabelField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Tools box", tLabelStyle);
            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            // Draw the interface addon for editor
            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                {
                    NWDBasisHelper.FindTypeInfos(tType).m_SearchAccount = Account.GetReference();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                {
                    NWDBasisHelper.FindTypeInfos(tType).m_SearchGameSave = Reference;
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), "Set current"))
            {
                SetCurrent();
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

            tYadd += tLabelStyle.fixedHeight = tMiniButtonStyle.fixedHeight * 3 + NWDConstants.kFieldMarge * 5;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override Color AddOnNodeColor()
        {
            return Color.gray;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = false;
            // check if you found error in Data values.
            // normal way is return false!
            return rReturnErrorFound;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================