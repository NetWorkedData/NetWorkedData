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
using SQLite.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDTransactionCheckStatut : int
    {
        NotInApp = -1,
        Unknow = 0,
        Approuved = 1,
        Refused = 2,

        Error = 9
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDTransactionType
    {
        None,
        Daily,
        Weekly,
        Monthly
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UTT")]
    [NWDClassDescriptionAttribute("User Transaction descriptions Class")]
    [NWDClassMenuNameAttribute("User Transaction")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserTransaction : NWDBasis<NWDUserTransaction>
    {
        //-----------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }

        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Items in transaction", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemsReceived
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemsSpent
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Place ", true, true, true)]
        public NWDReferenceType<NWDBarterPlace> BarterPlaceReference
        {
            get; set;
        }
        public NWDReferenceType<NWDTradePlace> TradePlaceReference
        {
            get; set;
        }
        public NWDReferenceType<NWDShop> ShopReference
        {
            get; set;
        }
        public NWDReferenceType<NWDRack> RackReference
        {
            get; set;
        }
        public NWDReferenceType<NWDPack> PackReference
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Other", true, true, true)]
        public string Platform
        {
            get; set;
        }
        public NWDReferenceType<NWDInAppPack> InAppReference
        {
            get; set;
        }
        public string InAppTransaction
        {
            get; set;
        }
        public NWDTransactionCheckStatut InAppApprouved
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTransaction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTransaction(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Add a new transaction to user account.
        /// </summary>
        /// <returns>The transaction.</returns>
        /// <param name="sItem">NWDItem to description the transaction.</param>
        /// <param name="sShop">NWDShop from where we buy the NWDPack.</param>
        /// <param name="sRack">NWDRack from where we buy the NWDPack.</param>
        /// <param name="sPack">NWDPack the pack we just buy.</param>
        public static NWDUserTransaction AddTransactionToAccount(NWDItem sItem, NWDShop sShop, NWDRack sRack, NWDPack sPack)
        {
            // Set a NWDTransaction
            NWDUserTransaction rTransaction = NewData();
            #if UNITY_EDITOR
            rTransaction.InternalKey = NWDUserNickname.GetNickname() + " - " + sItem.Name.GetBaseString();
            #endif
            rTransaction.Tag = NWDBasisTag.TagUserCreated;
            rTransaction.ShopReference.SetReference(sShop.Reference);
            rTransaction.RackReference.SetReference(sRack.Reference);
            rTransaction.PackReference.SetReference(sPack.Reference);
            rTransaction.SaveData();

            return rTransaction;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            InAppApprouved = NWDTransactionCheckStatut.NotInApp;
        }
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonSyncForce()
        {
            bool rReturn = false;
            if (InAppApprouved == NWDTransactionCheckStatut.Unknow)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
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
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_AddonPhpPreCalculate)]
        public static string AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {

            //string tTradeStatus = NWDUserTradeRequest.FindAliasName("TradeStatus");
            //string tLimitDayTime = NWDUserTradeRequest.FindAliasName("LimitDayTime");
            //string tTradePlaceRequest = NWDUserTradeRequest.FindAliasName("TradePlace");

            //string tTradeRequestsList = NWDUserTradeFinder.FindAliasName("TradeRequestsList");
            //string tTradePlace = NWDUserTradeFinder.FindAliasName("TradePlace");
            //int tIndex_TradeRequestsList = CSVAssemblyIndexOf(tTradeRequestsList);
            //int tIndex_TradePlace = CSVAssemblyIndexOf(tTradePlace);

            string sScript = "" +
                "// debut find \n" +
                "// JE DOIS VERIFIER AVEC LES ERVEUR APPLE OU GOOGLE DE LA VALIDITEE DE LA TRANSACTION ET METTRE InAppApprouved EN Approuved OU Refused!\n" +
                "// fin find \n";

            return sScript;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpPostCalculate(NWDAppEnvironment AppEnvironment)
        {
            return "\n" +
                "\n";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpSpecialCalculate(NWDAppEnvironment AppEnvironment)
        {
            return "// write your php script here to special operation, example : \n$REP['" + BasisHelper().ClassName + " Special'] ='success!!!';\n";
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Static methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDUserTransaction> GetTransactionsByShopAndType(NWDShop sShop, List<NWDRack> sRacks, NWDTransactionType sType)
        {
            // Create Transaction array
            List<NWDUserTransaction> rTransactionList = new List<NWDUserTransaction>();

            // Init all transactions done by the user for selected shop and type
            NWDUserTransaction[] tList = FindDatas();
            foreach (NWDUserTransaction transaction in tList)
            {
                // Verify we are in the right Shop
                if (transaction.ShopReference.ContainsObject(sShop))
                {
                    // Parse selected Shop Racks
                    foreach (NWDRack tRack in sRacks)
                    {
                        // Verify the Rack
                        if (transaction.RackReference.ContainsObject(tRack))
                        {
                            // Take only transaction filter by ShopType
                            bool isValidate = false;
                            switch (sType)
                            {
                                case NWDTransactionType.Daily:
                                    double tLocalDateStart = BTBDateHelper.ConvertToTimestamp(DateTime.Today);
                                    double tLocalDateEnd = BTBDateHelper.ConvertToTimestamp(DateTime.Today.AddDays(1));
                                    if (transaction.DC >= tLocalDateStart && transaction.DC <= tLocalDateEnd)
                                    {
                                        isValidate = true;
                                    }
                                    break;
                                case NWDTransactionType.Weekly:
                                    //TODO: GetTransactionsByShopAndType weekly not implemented
                                    isValidate = true;
                                    break;
                                case NWDTransactionType.Monthly:
                                    //TODO: GetTransactionsByShopAndType monthly not implemented
                                    isValidate = true;
                                    break;
                                case NWDTransactionType.None:
                                    isValidate = true;
                                    break;
                            }

                            // Transaction found (shop and shop type match) and validate
                            if (isValidate)
                            {
                                rTransactionList.Add(transaction);
                            }
                            break;
                        }
                    }
                }
            }
            return rTransactionList;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================