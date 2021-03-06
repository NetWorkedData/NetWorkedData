//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisAccountDependent : NWDBasisBundled
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Data is editable by this account. This account can read, write (insert, update) and trash this data.
        /// </summary>
        [NWDInspectorGroupOrder(NWD.InspectorBasisHeader,-9)]
        [NWDCertified]
        //[NWDIndexedAttribut(NWD.K_ACCOUNT_INDEX)]
        [NWDIndexedAttribut(NWD.K_BASIS_INDEX)]
        public NWDReferenceType<NWDAccount> Account { get; set; }
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
            if (string.IsNullOrEmpty(sAccountReference))
            {
                sAccountReference = NWDAccount.CurrentReference();
            }
            return (Account.GetReference() == sAccountReference);
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
            if (Account == null)
            {
                Account = new NWDReferenceType<NWDAccount>();
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
                Account.SetValue(NWDAccount.CurrentReference());
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
        protected override string GetEventualAccount()
        {
            return Account.GetValue();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
