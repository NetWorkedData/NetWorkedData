//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
using UnityEngine;

namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisGameSavePublish : NWDBasisAccountPublish
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Data is editable by this account. This account can read, write (insert, update) and trash this data.
        /// And this data is readeable by 
        /// </summary>
        [NWDInspectorGroupOrder(NWD.InspectorBasisHeader, -2)]
        [NWDCertified]
        public NWDReferencesArrayType<NWDGameSave> GameSavesArray { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor basic.
        /// </summary>
        public NWDBasisGameSavePublish()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor from database.
        /// </summary>
        public NWDBasisGameSavePublish(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsReacheableBy(string sGameSaveReference, string sAccountReference = null)
        {
            if (string.IsNullOrEmpty(sAccountReference))
            {
                sAccountReference = NWDAccount.CurrentReference();
            }
            if (sGameSaveReference == null)
            {
                return (Account.GetReference() == sAccountReference);
            }
            else
            {
                return (Account.GetReference() == sAccountReference && GameSavesArray.ContainsReference(sGameSaveReference));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsWritableBy(string sGameSaveReference, string sAccountReference = null)
        {
            if (string.IsNullOrEmpty(sAccountReference))
            {
                sAccountReference = NWDAccount.CurrentReference();
            }
            if (sGameSaveReference == null)
            {
                return (Account.GetReference() == sAccountReference);
            }
            else
            {
                return (Account.GetReference() == sAccountReference && GameSavesArray.ContainsReference(sGameSaveReference));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PropertiesAutofill()
        {
            base.PropertiesAutofill();
            PropertiesMinimal();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PropertiesMinimal()
        {
            base.PropertiesMinimal();
            if (GameSavesArray == null)
            {
                GameSavesArray = new NWDReferencesArrayType<NWDGameSave>();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            //Debug.Log("AddonInsertMe " + BasisHelper().ClassNamePHP);
            base.AddonInsertMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            //Debug.Log("AddonUpdateMe " + BasisHelper().ClassNamePHP);
            base.AddonUpdateMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            //Debug.Log("AddonDuplicateMe " + BasisHelper().ClassNamePHP);
            base.AddonDuplicateMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Assign the good <see cref="RangeAccess"/> for this data for this <see cref="Account"/>
        /// </summary>
        private void AssignGameSave()
        {
            //Debug.Log("AssignRange " + BasisHelper().ClassNamePHP);
            //only if data was not sync ... else it need to use the define RangeAccess
            if (DevSync <= 1 && ProdSync <= 1 && PreprodSync <= 1)
            {
                if (GameSavesArray == null)
                {
                    GameSavesArray = new NWDReferencesArrayType<NWDGameSave>();
                }
                GameSavesArray.AddData(NWDGameSave.CurrentData());
            }
            //Debug.Log("AssignRange RangeAccess = " + RangeAccess + "    " + BasisHelper().ClassNamePHP);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================