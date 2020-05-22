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
    public partial class NWDBasisAccountPublish : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Data is editable by this account. This account can read, write (insert, update) and trash this data.
        /// And this data is readeable by 
        /// </summary>
        [NWDInspectorGroupOrder(NWD.InspectorBasisHeader,-8)]
        [NWDCertified]
        public NWDReferencesArrayType<NWDAccount> ReaderAccountsArray { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor basic.
        /// </summary>
        public NWDBasisAccountPublish()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor from database.
        /// </summary>
        public NWDBasisAccountPublish(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsReacheableBy(string sGameSaveReference, string sAccountReference = null)
        {
            if (string.IsNullOrEmpty(sAccountReference))
            {
                sAccountReference = NWDAccount.CurrentReference();
            }
            return ReaderAccountsArray.ContainsReference(sAccountReference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsWritableBy(string sGameSaveReference, string sAccountReference = null)
        {
            if (string.IsNullOrEmpty(sAccountReference))
            {
                sAccountReference = NWDAccount.CurrentReference();
            }
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
            if (ReaderAccountsArray == null)
            {
                ReaderAccountsArray = new NWDReferencesArrayType<NWDAccount>();
            }
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
                    ReaderAccountsArray.RemoveReferences(new string[] { sOldUser });
                    ReaderAccountsArray.AddReferences(new string[] { sNewUser });
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