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
    public class NWDTransactionConnection : NWDConnection<NWDTransaction>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum TransactionType
    {
        None,
        Daily,
        Weekly,
        Monthly
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("TRS")]
    [NWDClassDescriptionAttribute("Transaction descriptions Class")]
    [NWDClassMenuNameAttribute("Transaction")]
    [NWDForceSecureDataAttribute]
    public partial class NWDTransaction : NWDBasis<NWDTransaction>
    {
        //-----------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> AccountReference
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
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Other", true, true, true)]
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
        public NWDTransaction()
        {
            //Debug.Log("NWDTransaction Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTransaction(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDTransaction Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
        public static NWDTransaction AddTransactionToAccount(NWDItem sItem, NWDShop sShop, NWDRack sRack, NWDPack sPack)
        {
            // Set a NWDTransaction
            NWDTransaction rTransaction = NewData();

#if UNITY_EDITOR
            rTransaction.InternalKey = sItem.Name.GetBaseString();
            rTransaction.InternalDescription = NWDAccountNickname.GetNickname();
#endif
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
        public static string AddonPhpPreCalculate()
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
        public static string AddonPhpPostCalculate()
        {
            return "\n" +
                "\n";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpSpecialCalculate()
        {
            return "// write your php script here to special operation, example : \n$REP['" + Datas().ClassName + " Special'] ='success!!!';\n";
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
        public static List<NWDTransaction> GetTransactionsByShopAndType(NWDShop sShop, List<NWDRack> sRacks, TransactionType sType)
        {
            // Create Transaction array
            List<NWDTransaction> rTransactionList = new List<NWDTransaction>();

            // Init all transactions done by the user for selected shop and type
            NWDTransaction[] tList = FindDatas();
            foreach (NWDTransaction transaction in tList)
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
                                case TransactionType.Daily:
                                    double tLocalDateStart = BTBDateHelper.ConvertToTimestamp(DateTime.Today);
                                    double tLocalDateEnd = BTBDateHelper.ConvertToTimestamp(DateTime.Today.AddDays(1));
                                    if (transaction.DC >= tLocalDateStart && transaction.DC <= tLocalDateEnd)
                                    {
                                        isValidate = true;
                                    }
                                    break;
                                case TransactionType.Weekly:
                                    //TODO: GetTransactionsByShopAndType weekly not implemented
                                    isValidate = true;
                                    break;
                                case TransactionType.Monthly:
                                    //TODO: GetTransactionsByShopAndType monthly not implemented
                                    isValidate = true;
                                    break;
                                case TransactionType.None:
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