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
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDConsent : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDConsent[] SelectCurrentDatas()
        {
            List<NWDConsent> rList = new List<NWDConsent>();
            Dictionary<string, NWDConsent> tDico = new Dictionary<string, NWDConsent>(new StringIndexKeyComparer());
            NWDConsent[] tConsentList = NWDBasisHelper.GetReachableDatas<NWDConsent>();
            foreach (NWDConsent tConsent in tConsentList)
            {
                if (tDico.ContainsKey(tConsent.KeyOfConsent) == false)
                {
                    tDico.Add(tConsent.KeyOfConsent, tConsent);
                }
                else
                {
                    if (tDico[tConsent.KeyOfConsent].Version.ToInt() < tConsent.Version.ToInt())
                    {
                        tDico[tConsent.KeyOfConsent] = tConsent;
                    }
                }
            }
            foreach (KeyValuePair<string, NWDConsent> tConsentKeyValue in tDico)
            {
                rList.Add(tConsentKeyValue.Value);
            }
            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================