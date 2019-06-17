//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:36
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
    public partial class NWDUserTransaction : NWDBasis<NWDUserTransaction>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTransaction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTransaction(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            InAppApprouved = NWDTransactionCheckStatut.NotInApp;
        }
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
        public static List<NWDUserTransaction> GetTransactionsByShopAndType(NWDShop sShop, List<NWDRack> sRacks, NWDTransactionType sType)
        {
            // Create Transaction array
            List<NWDUserTransaction> rTransactionList = new List<NWDUserTransaction>();

            // Init all transactions done by the user for selected shop and type
            NWDUserTransaction[] tList = GetReachableDatas();
            foreach (NWDUserTransaction transaction in tList)
            {
                // Verify we are in the right Shop
                if (transaction.ShopReference.ContainsData(sShop))
                {
                    // Parse selected Shop Racks
                    foreach (NWDRack tRack in sRacks)
                    {
                        // Verify the Rack
                        if (transaction.RackReference.ContainsData(tRack))
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================