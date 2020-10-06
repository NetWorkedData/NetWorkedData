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
    public partial class NWDItem : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem() {}
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) {}
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            Uncountable = false;
            HiddenInGame = false;
            Usable = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Enrichment(string sText, int sCpt = 0, string sLanguage = null, bool sBold = true)
        {
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }

            // Replace Tag by Item Name
            string rText = sText.Replace("#I" + sCpt + NWEConstants.K_HASHTAG, tBstart + Name + tBend);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDiscovered(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            NWDUserOwnership.SetDiscovered(this, sWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
