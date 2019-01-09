//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

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
    public class NWDTradePlaceConnection : NWDConnection <NWDTradePlace> {}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TRW")]
	[NWDClassDescriptionAttribute ("Trade Place descriptions Class")]
	[NWDClassMenuNameAttribute ("Trade Place")]
	public partial class NWDTradePlace : NWDBasis <NWDTradePlace>
	{
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem{ get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Items Authorization",true, true, true)]
		public NWDReferencesListType<NWDWorld> FilterWorlds { get; set; }
		public NWDReferencesListType<NWDCategory> FilterCategories { get; set; }
		public NWDReferencesListType<NWDFamily> FilterFamilies { get; set; }
		public NWDReferencesListType<NWDKeyword>  FilterKeywords { get; set; }
		[NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Money Authorization",true, true, true)]
		public NWDReferencesListType<NWDWorld> FilterMoneyWorlds { get; set; }
		public NWDReferencesListType<NWDCategory> FilterMoneyCategories { get; set; }
		public NWDReferencesListType<NWDFamily> FilterMoneyFamilies { get; set; }
		public NWDReferencesListType<NWDKeyword>  FilterMoneyKeywords { get; set; }
		[NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Trade Detail", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> RequestFixCost { get; set; }
		public NWDReferencesQuantityType<NWDItem> RequestPerItemCost { get; set; }
		public int MaxRequestPerUser { get; set; }
		public int RequestLifeTime { get; set; }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDTradePlace()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTradePlace(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

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
		public static void MyClassMethod ()
		{
			// do something with this class
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Instance methods
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