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
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart("Account Informations")]
        [NWDCertified]
        [NWDNotEditable]
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
            Account.SetValue(NWDAccount.CurrentReference());
            //GameSave.SetValue(NWDGameSave.CurrentData().Reference);
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
                if (Account != null)
                {
                    string[] tAccountExplode = Account.GetValue().Split(new char[] { '-' });
                    if (tAccountExplode.Length > 1)
                    {
                        int tRange;
                        int.TryParse(tAccountExplode[1], out tRange);
                        RangeAccess = tRange;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void ChangeUser(string sOldUser, string sNewUser)
        {
            Debug.Log("ChangeUser(string sOldUser, string sNewUser) in " + BasisHelper().ClassNamePHP);
            if (IntegrityIsValid() == true)
            {
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