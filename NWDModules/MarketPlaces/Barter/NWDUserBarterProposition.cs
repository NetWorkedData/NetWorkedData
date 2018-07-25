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
using SQLite4Unity3d;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDBarterState : int
    {
        InWaiting, // the request or proposition is waiting
        Refused,// the request or proposition is refused
        Accepted,// the request or proposition is accepted ... item can be distrib
        ItemDistribute,// the request or proposition 's items are distribute
    }
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
    public class NWDUserBarterPropositionConnection : NWDConnection <NWDUserBarterProposition> {}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("BPR")]
	[NWDClassDescriptionAttribute ("Barter Proposition descriptions Class")]
    [NWDClassMenuNameAttribute ("Barter Proposition")]
	public partial class NWDUserBarterProposition :NWDBasis <NWDUserBarterProposition>
	{
		//-------------------------------------------------------------------------------------------------------------
		#region Properties
		//-------------------------------------------------------------------------------------------------------------
        // Your properties
        [NWDTooltips("Barter reference")]
        [NWDNeedReferenceAttribute(typeof(NWDUserBarterRequest))]
        public NWDReferenceType<NWDUserBarterRequest> BarterRequest { get; set; }
        [NWDNeedUserAvatarAttribute()]
        [NWDNeedAccountNicknameAttribute()]
        public NWDReferenceType<NWDAccount> BarterAccount { get; set; }
		[Indexed ("AccountIndex", 0)]
        [NWDTooltips("Account reference")]
        [NWDNeedUserAvatarAttribute()]
        [NWDNeedAccountNicknameAttribute()]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        [NWDTooltips("Items to pay the bater zone trader( can be empty)")]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed { get; set; }
        [NWDTooltips("State of proposition. You can refound your item only when the barter is 'Refused'. If refused/accpetd the state can be nerver change.")]
        public NWDBarterState PropositionState { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDUserBarterProposition()
        {
            //Debug.Log("NWDBarterProposition Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterProposition(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDBarterProposition Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> {typeof(NWDUserBarterProposition), typeof(NWDUserBarterRequest), typeof(NWDAccountNickname), typeof(NWDUserAvatar)};
        }
		//-------------------------------------------------------------------------------------------------------------
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
            PropositionState = NWDBarterState.InWaiting;
        }
		//-------------------------------------------------------------------------------------------------------------
		public void MyInstanceMethod ()
		{
			// do something with this object
		}
		//-------------------------------------------------------------------------------------------------------------
		#region NetWorkedData addons methods
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonInsertMe ()
		{
			// do something when object will be inserted
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdateMe ()
		{
			// do something when object will be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdatedMe ()
		{
			// do something when object finish to be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDuplicateMe ()
		{
			// do something when object will be dupplicate
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonEnableMe ()
		{
			// do something when object will be enabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDisableMe ()
		{
			// do something when object will be disabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonTrashMe ()
		{
			// do something when object will be put in trash
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUnTrashMe ()
		{
			// do something when object will be remove from trash
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		//Addons for Edition
		//-------------------------------------------------------------------------------------------------------------
		public override bool AddonEdited( bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) 
			{
				// do something
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditor (Rect sInRect)
		{
			// Draw the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================