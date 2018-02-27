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
    /// NWDUserNicknameConnexion can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
    /// Use like :
    /// public class MyScriptInGame : MonoBehaviour
    /// { 
    /// [NWDConnexionAttribut (true, true, true, true)] // optional
    /// public NWDUserNicknameConnexion MyNetWorkedData;
    /// }
    /// </summary>
    [Serializable]
    public class NWDUserNicknameConnexion : NWDConnexion<NWDUserNickname>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UNN")]
    [NWDClassDescriptionAttribute("User Nickname")]
    [NWDClassMenuNameAttribute("User Nickname")]
    [NWDClassMenuNameAttribute("User Nickname")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference when update by sync ... for example verif unique ID of an attribute and return it\n" +
                                       "\n "+
                                       "if (UniquePropertyValueFromValue($ENV.'_NWDUserNickname', 'Nickname', 'UniqueNickname', $tReference) == true)\n"+
                                       "\t{\n"+
                                       "\t\tmyLog('YES YESY YESY YEESSSSSSS', __FILE__, __FUNCTION__, __LINE__);\n"+
                                       "\t\tIntegrityNWDUserNicknameReevalue($tReference);\n"+
                                       "\t}\n")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDInternalKeyNotEditableAttribute]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD example class. This class is use for (complete description here)
    /// </summary>
    public partial class NWDUserNickname : NWDBasis<NWDUserNickname>
    {
        //#warning YOU MUST FOLLOW THIS INSTRUCTIONS
        //-------------------------------------------------------------------------------------------------------------
        // YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
        // YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
        // YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
        //-------------------------------------------------------------------------------------------------------------
        #region Class Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your static properties
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        [NWDHeader("Player Informations")]
        public NWDReferenceType<NWDAccount> AccountReference {get; set;}
        public string Nickname {get; set;}
        public string UniqueNickname {get; set;}
        //-------------------------------------------------------------------------------------------------------------
        public delegate void SyncNickNameBlock(bool error, NWDOperationResult result = null);
        public SyncNickNameBlock SyncNickNameBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserNickname NewNickName(string name)
        {
            NWDUserNickname rNickName = NewObject();
            rNickName.InternalKey = name;
            rNickName.InternalDescription = "";
            rNickName.Nickname = name;
            rNickName.SaveModifications();
            return rNickName;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sync NickName if needed
        /// </summary>
        public void SyncNickName()
        {
            // Sync with the server
            List<Type> tList = new List<Type>
            {
                typeof(NWDUserNickname)
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (SyncNickNameBlockDelegate != null)
                {
                    SyncNickNameBlockDelegate(false);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = bInfos as NWDOperationResult;
                if (SyncNickNameBlockDelegate != null)
                {
                    SyncNickNameBlockDelegate(true, tInfos);
                }
            };
            NWDDataManager.SharedInstance.AddWebRequestSynchronizationWithBlock(tList, tSuccess, tFailed);
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
        //private static string kNickNameToTest = "";
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

            //kNickNameToTest = EditorGUI.TextField(new Rect(tX, tYadd, tWidth, tTextFieldStyle.fixedHeight), "Nickname to test", kNickNameToTest, tTextFieldStyle);
            //tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            //EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(kNickNameToTest) || kNickNameToTest.Length < 4);
            //if (GUI.Button(new Rect(tX, tYadd, tWidth, tMiniButtonStyle.fixedHeight), "Validate nickname", tMiniButtonStyle))
            //{
            //    BTBConsole.Clean();
            //    this.AskValidationOfNickname(kNickNameToTest);
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
        //    NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        //}
        //-------------------------------------------------------------------------------------------------------------
    }

    //-------------------------------------------------------------------------------------------------------------
    #region Connexion NWDUserNickname with Unity MonoBehavior
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// NWDUserNickname connexion.
    /// In your MonoBehaviour Script connect object with :
    /// <code>
    ///	[NWDConnexionAttribut(true,true, true, true)]
    /// public NWDUserNicknameConnexion MyNWDUserNicknameObject;
    /// </code>
    /// </summary>
    //-------------------------------------------------------------------------------------------------------------
    // CONNEXION STRUCTURE METHODS
    //-------------------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------------------
    //public class NWDUserNicknameMonoBehaviour: NWDMonoBehaviour<NWDUserNicknameMonoBehaviour> {}
    #endregion
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================