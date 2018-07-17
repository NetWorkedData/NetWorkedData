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
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDExampleConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDExample tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDUserNicknameConnection : NWDConnection<NWDUserNickname>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UNN")]
    [NWDClassDescriptionAttribute("User Nickname")]
    [NWDClassMenuNameAttribute("User Nickname")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference when update by sync ... for example verif unique ID of an attribute and return it\n" +
                                       "\n "+
                                       "if (UniquePropertyValueFromValue($ENV.'_NWDUserNickname', 'Nickname', 'UniqueNickname', $tReference) == true)\n"+
                                       "\t{\n"+
                                       "\t\tIntegrityNWDUserNicknameReevalue($tReference);\n"+
                                       "\t}\n")]
    public partial class NWDUserNickname : NWDBasis<NWDUserNickname>
    {
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        #region Class Properties
        // Your static properties
        #endregion
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        #region Instance Properties
        [NWDHeader("Player Informations")]
        public NWDReferenceType<NWDAccount> Account {get; set;}
        public string Nickname {get; set;}
        public string UniqueNickname {get; set;}
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        public delegate void SyncNicknameBlock(bool error, NWDOperationResult result = null);
        public SyncNicknameBlock SyncNicknameBlockDelegate;
        #endregion
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNickname()
        {
            //Debug.Log("NWDUserNickname Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNickname(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserNickname Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Enrichment the specified sText, with sLanguage and sBold.
        /// </summary>
        /// <returns>The enrichment.</returns>
        /// <param name="sText">S text.</param>
        /// <param name="sLanguage">S language.</param>
        /// <param name="sBold">If set to <c>true</c> s bold.</param>
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            string rText = sText;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = "";
                tBend = "";
            }
            if (sLanguage == null)
            {
                sLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            }
            // Replace the nickname
            NWDUserNickname tNicknameObject = NWDUserNickname.GetFirstObject();
            string tNickname = "";
            string tNicknameID = "";
            if (tNicknameObject != null)
            {
                tNickname = tNicknameObject.Nickname;
                tNicknameID = tNicknameObject.UniqueNickname;
            }
            // Replace the old tag nickname
            //rText = rText.Replace("@nickname@", tBstart + tNickname + tBend);
            //rText = rText.Replace("@nicknameid@", tBstart + tNicknameID + tBend);
            // Replace the new tag nickname
            rText = rText.Replace("#Nickname#", tBstart + tNickname + tBend);
            rText = rText.Replace("#UniqueNickname#", tBstart + tNicknameID + tBend);
            //// Replace the standard tag nickname
            //rText = rText.Replace("{Nickname}", tBstart + tNickname + tBend);
            //rText = rText.Replace("{UniqueNickname}", tBstart + tNicknameID + tBend);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserNickname NewNickname(string name)
        {
            NWDUserNickname rNickname = NewObject();
            rNickname.InternalKey = name;
            rNickname.InternalDescription = "";
            rNickname.Nickname = name;
            rNickname.SaveModifications();
            return rNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetNickname()
        {
            string rNickname = "";
            NWDUserNickname[] tNickname = GetAllObjects();
            if (tNickname.Length > 0)
            {
                rNickname = tNickname[0].Nickname;
            }
            return rNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetUniqueNickname()
        {
            string rUniqueNickname = "";
            NWDUserNickname[] tNickname = GetAllObjects();
            if (tNickname.Length > 0)
            {
                rUniqueNickname = tNickname[0].UniqueNickname;
            }
            return rUniqueNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sync Nickname if needed
        /// </summary>
        public void SyncNickname()
        {
            // Sync with the server
            List<Type> tList = new List<Type>
            {
                typeof(NWDUserNickname)
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (SyncNicknameBlockDelegate != null)
                {
                    SyncNicknameBlockDelegate(false);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = bInfos as NWDOperationResult;
                if (SyncNicknameBlockDelegate != null)
                {
                    SyncNicknameBlockDelegate(true, tInfos);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tList, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //private static string kNicknameToTest = "";
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override float AddonEditor(Rect sInRect)
        {
            float tYadd = 0.0f;
            // Draw the interface addon for editor
            //float tWidth = sInRect.width;
            //float tX = sInRect.x;
            //tYadd = sInRect.y;

            //GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            //tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            //GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            //tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), tWidth);

            //GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            //tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), tWidth);

            //EditorGUI.DrawRect(new Rect(tX, tYadd + NWDConstants.kFieldMarge, tWidth, 1), kRowColorLine);
            //tYadd += NWDConstants.kFieldMarge * 2;

            //EditorGUI.LabelField(new Rect(tX, tYadd, tWidth, tTextFieldStyle.fixedHeight), "Tools box", tLabelStyle);
            //tYadd += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            //kNicknameToTest = EditorGUI.TextField(new Rect(tX, tYadd, tWidth, tTextFieldStyle.fixedHeight), "Nickname to test", kNicknameToTest, tTextFieldStyle);
            //tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            //EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(kNicknameToTest) || kNicknameToTest.Length < 4);
            //if (GUI.Button(new Rect(tX, tYadd, tWidth, tMiniButtonStyle.fixedHeight), "Validate nickname", tMiniButtonStyle))
            //{
            //    BTBConsole.Clean();
            //    this.AskValidationOfNickname(kNicknameToTest);
            //}
            //tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            //EditorGUI.EndDisabledGroup();
            //if (GUI.Button(new Rect(tX, tYadd, tWidth, tMiniButtonStyle.fixedHeight), "ReValidate", tMiniButtonStyle))
            //{
            //    BTBConsole.Clean();
            //    this.AskValidationOfNickname(Nickname);
            //}
            //tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            //GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            //tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100);

            //GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            //tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100);

            //GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            //tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), 100);

            //tYadd = NWDConstants.kFieldMarge;

            //tYadd += NWDConstants.kFieldMarge * 2;
            //tYadd += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            //tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            //tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            //tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            //tYadd += NWDConstants.kFieldMarge;

            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        //public void AskValidationOfNickname(string sNickname,
        //                                    BTBOperationBlock sSuccessBlock = null,
        //                                    BTBOperationBlock sErrorBlock = null,
        //                                    BTBOperationBlock sCancelBlock = null,
        //                                    BTBOperationBlock sProgressBlock = null,
        //                                    bool sPriority = true,
        //                                    NWDAppEnvironment sEnvironment = null)
        //{
        //    SaveModificationsIfModified();
        //    // Start webrequest
        //    NWDOperationWebUserNickname sOperation = NWDOperationWebUserNickname.Create("Nickname with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
        //    // Enter the informations
        //    sOperation.Action = "nickname";
        //    sOperation.Nickname = sNickname;
        //    sOperation.UserNicknameReference = this;
        //    // add request!
        //    NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================