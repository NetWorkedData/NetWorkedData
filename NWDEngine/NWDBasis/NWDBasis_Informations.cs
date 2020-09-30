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

using UnityEngine;



//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
	{
//		//-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Class informations.
//        /// </summary>
//        /// <param name="sString">S string.</param>
//		public static void ClassInformations (string sString)
//        {
//			Debug.Log ("From " + sString + " real [" + typeof(K).Name + "] = > " + NWDBasisHelper.Informations (typeof(K)) + "' ");
//		}
//		//-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Informations about this instance.
//        /// </summary>
//        /// <returns>The informations.</returns>
//        [NWDAliasMethod(NWDConstants.M_Informations)]

//        public static string Informations ()
//		{
//#if UNITY_EDITOR
//            int tCount = BasisHelper().Datas.Count;
//			if (tCount == 0) {
//                return string.Empty + BasisHelper().ClassNamePHP + " " + NWDConstants.K_APP_BASIS_NO_OBJECT + " (sync at " + BasisHelper().New_SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment())+  ")\n";
//			} else if (tCount == 1) {
//                return string.Empty + BasisHelper().ClassNamePHP + " : " + tCount + " " + NWDConstants.K_APP_BASIS_ONE_OBJECT + " (sync at " + BasisHelper().New_SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment()) + ")\n";
//			} else {
//                return string.Empty + BasisHelper().ClassNamePHP + " : " + tCount + " " + NWDConstants.K_APP_BASIS_X_OBJECTS + " (sync at " + BasisHelper().New_SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment()) + ")\n";
//			}
//#else
//            return string.Empty;
//#endif

        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
