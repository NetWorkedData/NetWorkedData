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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserAvatar : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserAvatar()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserAvatar(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDItem GetNWDItemAvatar()
        {
            NWDItem rItem = null;
            NWDUserAvatar[] tAvatars = NWDBasisHelper.GetReachableDatas<NWDUserAvatar>();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderItem.GetRawData();
            }
            return rItem;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Sprite GetTextureAvatar()
        {
            Sprite rSprite = null;
            NWDUserAvatar[] tAvatars = NWDBasisHelper.GetReachableDatas<NWDUserAvatar>();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderTexture.ToSprite();
            }
            return rSprite;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem GetNWDItem()
        {
            return RenderItem.GetRawData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public Sprite GetTexture()
        {
            return RenderTexture.ToSprite();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================