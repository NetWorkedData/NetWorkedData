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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    public partial class NWDBasisGameSaveDependent : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Data is editable by this account. This account can read, write (insert, update) and trash this data.
        /// </summary>
        [NWDInspectorGroupOrder(NWD.InspectorBasisHeader,-2)]
        [NWDCertified]
        public NWDReferenceType<NWDGameSave> GameSave { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor basic.
        /// </summary>
        public NWDBasisGameSaveDependent()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor from database.
        /// </summary>
        public NWDBasisGameSaveDependent(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
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
            if (GameSave == null)
            {
                GameSave = new NWDReferenceType<NWDGameSave>();
            }

            // Very slow when extract data from database
            // removed... need new process
            /*
                NWDGameSave tCurrentGameSave = NWDGameSave.CurrentData();
                if (tCurrentGameSave != null)
                {
                    if (tCurrentGameSave.Account.GetValue() != null)
                    {
                        Account.SetValue(tCurrentGameSave.Account.GetValue());
                    }
                    else
                    {
                        Account.SetValue(NWDAccount.CurrentReference());
                    }
                    GameSave.SetValue(tCurrentGameSave.Reference);
                }
            */
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
                return (Account.GetReference() == sAccountReference && GameSave.GetReference() == sGameSaveReference);
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
                return (Account.GetReference() == sAccountReference && GameSave.GetReference() == sGameSaveReference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            base.AddonInsertMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            base.AddonDuplicateMe();
            AssignGameSave();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Assign the good <see cref="RangeAccess"/> for this data for this <see cref="Account"/>
        /// </summary>
        private void AssignGameSave()
        {
            //only if data was not sync ... else it need to use the define GameSave
            if (DevSync <= 1 && ProdSync <= 1 && PreprodSync <= 1)
            {
                if (GameSave == null)
                {
                    GameSave = new NWDReferenceType<NWDGameSave>();
                }
                GameSave.SetData(NWDGameSave.CurrentData());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        protected override string GetEventualGameSave()
        {
            return GameSave.GetValue();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
