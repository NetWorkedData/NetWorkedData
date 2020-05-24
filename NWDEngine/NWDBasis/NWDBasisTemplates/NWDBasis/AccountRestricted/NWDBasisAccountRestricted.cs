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
    /// <summary>
    /// This class is used for the <see cref="NWDAccount"/> and <see cref="NWDRequestToken"/> only.
    /// </summary>
    public partial class NWDBasisAccountRestricted : NWDBasis
    {
        //--------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder(NWD.InspectorBasisHeader,-1)]
        [NWDCertified]
        [NWDNotEditable]
        //[NWDIndexedAttribut(NWD.K_ACCOUNT_INDEX)]
        [NWDIndexedAttribut(NWD.K_BASIS_INDEX)]
        [NWDVarChar(256)]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisAccountRestricted()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisAccountRestricted(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsReacheableBy(string sGameSaveReference, string sAccountReference = null)
        {
            return (Account.GetReference() == sAccountReference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsWritableBy(string sGameSaveReference, string sAccountReference = null)
        {
            return (Account.GetReference() == sAccountReference);
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
            if (Account == null)
            {
                Account = new NWDReferenceType<NWDAccount>();
            }
            Account.SetValue(NWDAccount.CurrentReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            base.AddonInsertMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            base.AddonDuplicateMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Assign the good <see cref="RangeAccess"/> for this data for this <see cref="Account"/>
        /// </summary>
        private void AssignRange()
        {
            //only if data was not sync ... else it need to use the define RangeAccess
            if (DevSync <= 1 && ProdSync <= 1 && PreprodSync <= 1)
            {
                if (Account == null)
                {
                    Account = new NWDReferenceType<NWDAccount>();
                }
                    string[] tAccountExplode = Account.GetValue().Split(new char[] { '-' });
                    if (tAccountExplode.Length > 1)
                    {
                        int tRange;
                        int.TryParse(tAccountExplode[1], out tRange);
                        RangeAccess = tRange;
                    }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void ChangeUser(string sOldUser, string sNewUser)
        {
            Debug.Log("ChangeUser(string sOldUser, string sNewUser) in " + BasisHelper().ClassNamePHP);
            if (IntegrityIsValid() == true)
            {
                if (Account == null)
                {
                    Account = new NWDReferenceType<NWDAccount>();
                }
                if (Account.GetValue() == sOldUser)
                {
                    Account.SetValue(sNewUser);
                    UpdateData();
                }
            }
            else
            {
                Debug.Log("ChangeUser INTEGRITY ERROR " + Reference + "in " + BasisHelper().ClassNamePHP);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================