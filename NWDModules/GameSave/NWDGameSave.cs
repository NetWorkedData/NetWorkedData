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
    public class NWDGameSaveConnection : NWDConnection<NWDGameSave>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("PTY")]
    [NWDClassDescriptionAttribute("Game Save of account")]
    [NWDClassMenuNameAttribute("Game Save of account")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDGameSave : NWDBasis<NWDGameSave>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public int GameSaveTag
        {
            get; set;
        }
        public bool IsCurrent
        {
            get; set;
        }
        public int Difficulty
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDGameSave()
        {
            //Debug.Log("NWDParty Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDGameSave(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDParty Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            GameSaveTag = -1;
            GameSaveTagReevaluate();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GameSaveTagReevaluate()
        {
            int tMax = -1;
            NWDGameSave[] tPartyArray = NWDGameSave.GetAllObjects();
            NWDGameSave tCurrent = null;
            foreach (NWDGameSave tParty in tPartyArray)
            {
                if (tParty != this)
                {
                    if (tParty.GameSaveTag >= tMax)
                    {
                        tMax = tParty.GameSaveTag;
                    }
                }
                if (tParty.IsCurrent == true)
                {
                    tCurrent = tParty;
                }
            }
            if (tCurrent == null)
            {
                IsCurrent = true;
            }
            GameSaveTag = tMax + 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GameSaveTagCheckUnicity()
        {
            bool rReturn = true;
            foreach (NWDGameSave tParty in NWDGameSave.GetAllObjects())
            {
                if (tParty != this)
                {
                    if (tParty.GameSaveTag == GameSaveTag)
                    {
                        // Arghhh error
                        rReturn = false;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GameSaveTagAdjust()
        {
            if (GameSaveTagCheckUnicity() == false)
            {
                GameSaveTagReevaluate();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void ClassInitialization() // call by invoke
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public static NWDGameSave Current()
        {
            NWDGameSave rParty = null;
            foreach (NWDGameSave tParty in NWDGameSave.GetAllObjects())
            {
                if (tParty.IsCurrent == true)
                {
                    rParty = tParty;
                    break;
                }
            }
            if (rParty == null)
            {
                rParty = NWDGameSave.NewObject();
                rParty.Name = "New";
                rParty.IsCurrent = true;
                rParty.SaveModifications();
            }
            return rParty;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static K[] GetAllObjects<K>() where K : NWDBasis<K>
        //{
        //    return K.GetAllObjects();
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void SetCurrent()
        {
            foreach (NWDGameSave tParty in NWDGameSave.GetAllObjects())
            {
                tParty.IsCurrent = false;
                tParty.SaveModificationsIfModified();
            }
            this.IsCurrent = true;
            this.SaveModificationsIfModified();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDGameSave)/*, typeof(NWDUserNickname), etc*/ };
        }
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
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            GameSaveTagAdjust();
            // do something when object will be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        public override void AddonUpdatedMeFromWeb()
        {
            // do something when object finish to be updated from CSV from WebService response
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            GameSaveTagAdjust();
            IsCurrent = false;
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            this.IsCurrent = false;
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            this.IsCurrent = false;
            // do something when object will be put in trash

            //if (false)
            {
                OwnershipTrash();
                UserPreferencesTrash();
                QuestUserAdvancementTrash();
                UserConsolidatedStatsTrash();
                UserStatsTrash();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons the delete me.
        /// </summary>
        public override void AddonDeleteMe()
        {
            this.IsCurrent = false;
            // do something when object will be delete from local base
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            // do something when object will be web service upgrade
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Editor
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
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
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
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
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the width of node draw.
        /// </summary>
        /// <returns>The on node draw width.</returns>
        /// <param name="sDocumentWidth">S document width.</param>
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds color on node.
        /// </summary>
        /// <returns>The on node color.</returns>
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================