﻿//=====================================================================================================================
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
    /// NWDUserConsolidatedStatsConnexion can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
    /// Use like :
    /// public class MyScriptInGame : MonoBehaviour
    /// { 
    /// [NWDConnexionAttribut (true, true, true, true)] // optional
    /// public NWDUserConsolidatedStatsConnexion MyNetWorkedData;
    /// }
    /// </summary>
    [Serializable]
    public class NWDUserConsolidatedStatsConnexion : NWDConnexion<NWDUserConsolidatedStats>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UCS")]
    [NWDClassDescriptionAttribute("User Consolidated Stats")]
    [NWDClassMenuNameAttribute("User C stats")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDInternalKeyNotEditableAttribute]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD example class. This class is use for (complete description here)
    /// </summary>
    public partial class NWDUserConsolidatedStats : NWDBasis<NWDUserConsolidatedStats>
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
        //PROPERTIES
        public NWDReferenceType<NWDAccount> AccountReference
        {
            get; set;
        }
        public string StringValue
        {
            get; set;
        }
        public bool BoolValue
        {
            get; set;
        }
        public int IntValue
        {
            get; set;
        }
        public float FloatValue
        {
            get; set;
        }

        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserConsolidatedStats()
        {
            Debug.Log("NWDUserConsolidatedStats Constructor");
            //Insert in NetWorkedData;
            NewNetWorkedData();
            //Init your instance here
            // Example : this.MyProperty = true, 1 , "bidule", etc.
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserConsolidatedStats(bool sInsertInNetWorkedData)
        {
            Debug.Log("NWDUserConsolidatedStats Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
            if (sInsertInNetWorkedData == false)
            {
                // do nothing 
                // perhaps the data came from database and is allready in NetWorkedData;
            }
            else
            {
                //Insert in NetWorkedData;
                NewNetWorkedData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public static void MyClassMethod()
        {
            // do something with this class
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        #region override of NetWorkedData addons methods
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
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
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
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats NewIntStat(string sInternalKey, int sInt)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.NewObject();
            rReturn.InternalKey = sInternalKey;
            rReturn.IntValue = sInt;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats NewFloatStat(string sInternalKey, float sFloat)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.NewObject();
            rReturn.InternalKey = sInternalKey;
            rReturn.FloatValue = sFloat;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats NewBoolStat(string sInternalKey, bool sBool)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.NewObject();
            rReturn.InternalKey = sInternalKey;
            rReturn.BoolValue = sBool;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats NewStringStat(string sInternalKey, string sString)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.NewObject();
            rReturn.InternalKey = sInternalKey;
            rReturn.StringValue = sString;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats GetStatByInternalKey(string sInternalKey)
        {
            NWDUserStats rReturn = NWDUserStats.GetObjectByInternalKey(sInternalKey);
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats GetStatByInternalKeyOrCreate(string sInternalKey)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.SaveModifications();
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats ModifyStat(string sInternalKey, int sInt)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.IntValue = sInt;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats ModifyStat(string sInternalKey, float sFloat)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.FloatValue = sFloat;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats ModifyStat(string sInternalKey, bool sBool)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.BoolValue = sBool;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats ModifyStat(string sInternalKey, string sString)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.StringValue = sString;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats ModifyStatByAdd(string sInternalKey, int sAddInt)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.IntValue+= sAddInt;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats ModifyStatByAdd(string sInternalKey, float sAddFloat)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.FloatValue+= sAddFloat;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserConsolidatedStats ModifyStatByAdd(string sInternalKey, string sAddString)
        {
            NWDUserConsolidatedStats rReturn = NWDUserConsolidatedStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.StringValue+= sAddString;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================