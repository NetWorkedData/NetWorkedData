//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[Serializable]
    public class NWDLevelConnection : NWDConnection<NWDLevel>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("LVL")]
    [NWDClassDescriptionAttribute("Level descriptions Class")]
    [NWDClassMenuNameAttribute("Level")]
    public partial class NWDLevel : NWDBasis<NWDLevel>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalizableTextType Title
        {
            get; set;
        }
        public int Order
        {
            get; set;
        }
        public NWDJsonType JSON
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDLevel()
        {
            //Debug.Log("NWDLevel Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDLevel(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDLevel Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserLevelScore), typeof(NWDLevel) };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================