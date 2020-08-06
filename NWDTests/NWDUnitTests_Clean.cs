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
using System;
using System.Collections.Generic;
using UnityEngine;
using NetWorkedData;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Clean all local datas and put synchronized data in trash state.
        /// </summary>
        /// <param name="sLocalDataOnly"></param>
        public static void CleanUnitTests(bool sLocalDataOnly = true)
        {
            List<NWDTypeClass> tToDelete = new List<NWDTypeClass>();
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                foreach (NWDTypeClass tObject in tHelper.Datas)
                {
                    if (tObject.Tag == NWDBasisTag.UnitTestToDelete)
                    {
                        //if (tObject.DevSync < 0 && tObject.PreprodSync < 0 && tObject.ProdSync < 0)
                        if (tObject.DevSync < 1 && tObject.PreprodSync < 1 && tObject.ProdSync < 1)
                            {
                            // local ... delete now
                            tToDelete.Add(tObject);
                        }
                        else
                        {
                            // not local data ... put in trash and Sync on server
                            if (sLocalDataOnly == false)
                            {
                                tObject.TrashData();
                            }
                        }
                    }
                }
            }
            foreach (NWDTypeClass tObject in tToDelete)
            {
                tObject.DeleteData(NWDWritingMode.MainThread);
            }
            if (sLocalDataOnly == false)
            {
                // sync with server now?
                // TODO : force sync on server? And Trash on server?
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================