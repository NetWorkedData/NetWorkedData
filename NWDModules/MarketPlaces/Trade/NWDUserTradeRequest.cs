//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using SQLite.Attribute;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronize(true)]
	[NWDClassTrigramme("UTR")]
	[NWDClassDescription("User Trade Request descriptions Class")]
	[NWDClassMenuName("User Trade Request")]
	[NWDForceSecureDataAttribute]
	public partial class NWDUserTradeRequest : NWDBasis<NWDUserTradeRequest>
	{
		//-------------------------------------------------------------------------------------------------------------
		[NWDInspectorGroupStart("Trade Detail", true, true, true)]
		[Indexed("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> Account
		{
			get; set;
		}
		public NWDReferenceType<NWDGameSave> GameSave
		{
			get; set;
		}
		[NWDAlias("TradePlace")]
		public NWDReferenceType<NWDTradePlace> TradePlace
		{
			get; set;
		}
		[NWDInspectorGroupEnd]

		[NWDInspectorGroupStart("For Relationship Only", true, true, true)]

		[NWDAlias("ForRelationshipOnly")]
		public bool ForRelationshipOnly
		{
			get; set;
		}
		[NWDAlias("RelationshipAccountReferences")]
		public string RelationshipAccountReferences
		{
			get; set;
		}
		[NWDInspectorGroupEnd]

		[NWDInspectorGroupStart("Trade References", true, true, true)]
		[NWDAlias("ItemsProposed")]
		public NWDReferencesQuantityType<NWDItem> ItemsProposed
		{
			get; set;
		}
		[NWDAlias("ItemsAsked")]
		public NWDReferencesQuantityType<NWDItem> ItemsAsked
		{
			get; set;
		}
		[NWDAlias("TradeStatus")]
		public NWDTradeStatus TradeStatus
		{
			get; set;
        }
        [NWDNotEditable]
        [NWDAlias("TradeHash")]
		public string TradeHash
		{
			get; set;
		}
		[NWDAlias("LimitDayTime")]
		public NWDDateTimeUtcType LimitDayTime
		{
			get; set;
		}
		[NWDAlias("WinnerProposition")]
		public NWDReferenceType<NWDUserTradeProposition> WinnerProposition
		{
			get; set;
		}
		[NWDInspectorGroupEnd]

		[NWDInspectorGroupStart("Tags", true, true, true)]
		public NWDReferencesListType<NWDWorld> TagWorlds
		{
			get; set;
		}
		public NWDReferencesListType<NWDCategory> TagCategories
		{
			get; set;
		}
		public NWDReferencesListType<NWDFamily> TagFamilies
		{
			get; set;
		}
		public NWDReferencesListType<NWDKeyword> TagKeywords
		{
			get; set;
		}
		//-------------------------------------------------------------------------------------------------------------
		public delegate void tradeRequestBlock(bool error, NWDTradeStatus status, NWDOperationResult result);
		public tradeRequestBlock tradeRequestBlockDelegate;
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================