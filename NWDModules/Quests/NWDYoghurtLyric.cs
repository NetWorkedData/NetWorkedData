//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDYoghurtLyricConnection : NWDConnection<NWDYoghurtLyric>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("YLC")]
    [NWDClassDescriptionAttribute("yoghurt lyrics")]
    [NWDClassMenuNameAttribute("yoghurt lyrics")]
    public partial class NWDYoghurtLyric : NWDBasis<NWDYoghurtLyric>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAudioClipType Prefab
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDYoghurtLyric()
        {
            //Debug.Log("NWDYoghurtLyric Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDYoghurtLyric(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDYoghurtLyric Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================