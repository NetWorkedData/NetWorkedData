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
    public partial class NWDBasisAccountDependent : NWDBasisBundled
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Data is editable by this account. This account can read, write (insert, update) and trash this data.
        /// </summary>
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart("Account Informations")]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        //[NWDNotEditable]
        //[NWDCertified]
        //[NWDHidden]
        //public int RangeAccess
        //{
        //    get; set;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor basic.
        /// </summary>
        public NWDBasisAccountDependent()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor from database.
        /// </summary>
        public NWDBasisAccountDependent(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
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
            //Debug.Log("PropertiesAutofill " + BasisHelper().ClassNamePHP);
            Account.SetValue(NWDAccount.CurrentReference());
            //GameSave.SetValue(NWDGameSave.CurrentData().Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            //Debug.Log("AddonInsertMe " + BasisHelper().ClassNamePHP);
            base.AddonInsertMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            //Debug.Log("AddonUpdateMe " + BasisHelper().ClassNamePHP);
            base.AddonUpdateMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            //Debug.Log("AddonDuplicateMe " + BasisHelper().ClassNamePHP);
            base.AddonDuplicateMe();
            AssignRange();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Assign the good <see cref="RangeAccess"/> for this data for this <see cref="Account"/>
        /// </summary>
        private void AssignRange()
        {
            //Debug.Log("AssignRange " + BasisHelper().ClassNamePHP);
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
            //Debug.Log("AssignRange RangeAccess = " + RangeAccess + "    " + BasisHelper().ClassNamePHP);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void ChangeUser(string sOldUser, string sNewUser)
        {
            //Debug.Log("ChangeUser(string sOldUser, string sNewUser) in " + BasisHelper().ClassNamePHP);
            if (IntegrityIsValid() == true)
            {
                if (Account.GetValue() == sOldUser)
                {
#if UNITY_EDITOR
                    //prevent corrupt in 
                    BasisHelper().SetObjectInEdition(null);
                    NWDDataInspector.InspectNetWorkedData(null, true, false);
#endif
                    Account.SetValue(sNewUser);
                    UpdateData();
                    AnalyzeData();
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