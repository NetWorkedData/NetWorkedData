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
#if NWD_QUEST
#if UNITY_EDITOR
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCharacter : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonNodalHeight(float sCardWidth)
        {
            return 200f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonNodal(Rect sRect)
        {
            DrawPreviewTexture2D(new Rect(sRect.x + NWDGUI.kFieldMarge, sRect.y + NWDGUI.kFieldMarge, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
#endif
