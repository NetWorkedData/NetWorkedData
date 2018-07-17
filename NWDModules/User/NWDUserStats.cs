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
	public class NWDUserStatsConnection : NWDConnection <NWDUserStats> {}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("UST")]
	[NWDClassDescriptionAttribute ("User stats")]
	[NWDClassMenuNameAttribute ("User stats")]
	public partial class NWDUserStats : NWDBasis<NWDUserStats>
	{
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
		public NWDReferenceType<NWDAccount> Account {get; set;}
        public string StringValue {get; set;}
		public bool BoolValue {get; set;}
		public int IntValue {get; set;}
        public float FloatValue {get; set;}
        //public double DoubleValue {get; set;}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDUserStats()
        {
            //Debug.Log("NWDUserStats Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserStats Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Exampel of implement for class method.
		/// </summary>
		public static void MyClassMethod ()
		{
			// do something with this class
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Exampel of implement for instance method.
		/// </summary>
		public void MyInstanceMethod ()
		{
			// do something with this object
		}
		//-------------------------------------------------------------------------------------------------------------
		#region NetWorkedData addons methods
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just after loaded from database.
		/// </summary>
		public override void AddonLoadedMe ()
		{
			// do something when object was loaded
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before unload from memory.
		/// </summary>
		public override void AddonUnloadMe ()
		{
			// do something when object will be unload
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before insert.
		/// </summary>
		public override void AddonInsertMe ()
		{
			// do something when object will be inserted
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before update.
		/// </summary>
		public override void AddonUpdateMe ()
		{
			// do something when object will be updated
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when updated.
		/// </summary>
		public override void AddonUpdatedMe ()
		{
			// do something when object finish to be updated
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when updated me from Web.
		/// </summary>
        public override void AddonUpdatedMeFromWeb ()
		{
			// do something when object finish to be updated from CSV from WebService response
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before dupplicate.
		/// </summary>
		public override void AddonDuplicateMe ()
		{
			// do something when object will be dupplicate
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before enable.
		/// </summary>
		public override void AddonEnableMe ()
		{
			// do something when object will be enabled
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before disable.
		/// </summary>
		public override void AddonDisableMe ()
		{
			// do something when object will be disabled
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before put in trash.
		/// </summary>
		public override void AddonTrashMe ()
		{
			// do something when object will be put in trash
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before remove from trash.
		/// </summary>
		public override void AddonUnTrashMe ()
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
		public override bool AddonEdited( bool sNeedBeUpdate)
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
		public override float AddonEditor (Rect sInRect)
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
		public override float AddonEditorHeight ()
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


        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats NewIntStat(string sInternalKey, int sInt)
        {
            NWDUserStats rReturn = NWDUserStats.NewObject();
            rReturn.InternalKey = sInternalKey;
            rReturn.IntValue = sInt;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats NewFloatStat(string sInternalKey, float sFloat)
        {
            NWDUserStats rReturn = NWDUserStats.NewObject();
            rReturn.InternalKey = sInternalKey;
            rReturn.FloatValue = sFloat;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats NewBoolStat(string sInternalKey, bool sBool)
        {
            NWDUserStats rReturn = NWDUserStats.NewObject();
            rReturn.InternalKey = sInternalKey;
            rReturn.BoolValue = sBool;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats NewStringStat(string sInternalKey, string sString)
        {
            NWDUserStats rReturn = NWDUserStats.NewObject();
            rReturn.InternalKey = sInternalKey;
            rReturn.StringValue = sString;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats GetStatByInternalKey(string sInternalKey)
        {
            NWDUserStats rReturn = NWDUserStats.GetObjectByInternalKey(sInternalKey);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats GetStatByInternalKeyOrCreate(string sInternalKey)
        {
            NWDUserStats rReturn = NWDUserStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.SaveModifications();
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats ModifyStat(string sInternalKey, int sInt)
        {
            NWDUserStats rReturn = NWDUserStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.IntValue = sInt;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats ModifyStat(string sInternalKey, float sFloat)
        {
            NWDUserStats rReturn = NWDUserStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.FloatValue = sFloat;
            rReturn.SaveModifications();
            return rReturn;
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWDUserStats ModifyStat(string sInternalKey, double sDouble)
        //{
        //    NWDUserStats rReturn = NWDUserStats.GetObjectByInternalKeyOrCreate(sInternalKey);
        //    rReturn.DoubleValue = sDouble;
        //    rReturn.SaveModifications();
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats ModifyStat(string sInternalKey, bool sBool)
        {
            NWDUserStats rReturn = NWDUserStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.BoolValue = sBool;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserStats ModifyStat(string sInternalKey, string sString)
        {
            NWDUserStats rReturn = NWDUserStats.GetObjectByInternalKeyOrCreate(sInternalKey);
            rReturn.StringValue = sString;
            rReturn.SaveModifications();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================