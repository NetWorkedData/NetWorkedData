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
#if NWD_ACCOUNT_IDENTITY

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountAvatar : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountAvatar()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountAvatar(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account NWDItem Avatar
        /// </summary>
        public static NWDItem GetItemAvatar()
        {
            NWDItem rItem = null;
            NWDAccountAvatar[] tAvatars = NWDBasisHelper.GetReachableDatas<NWDAccountAvatar>();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderItem.GetRawData();
            }
            return rItem;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account Sprite Avatar
        /// </summary>
        public static Sprite GetTextureAvatar()
        {
            Sprite rSprite = null;
            NWDAccountAvatar[] tAvatars = NWDBasisHelper.GetReachableDatas<NWDAccountAvatar>();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderTexture.ToSprite();
            }
            return rSprite;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account NWDItem Avatar
        /// </summary>
        public NWDItem GetItem()
        {
            return RenderItem.GetRawData();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account Sprite Avatar
        /// </summary>
        public Sprite GetTexture()
        {
            return RenderTexture.ToSprite();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif