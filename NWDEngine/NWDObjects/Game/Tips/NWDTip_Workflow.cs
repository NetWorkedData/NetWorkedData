//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDTip :NWDBasis <NWDTip>
    {
        //-------------------------------------------------------------------------------------------------------------
        static List<NWDTip> ListForRandom;
        //-------------------------------------------------------------------------------------------------------------
        public NWDTip()
        {
            //Debug.Log("NWDTipsAndTricks Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTip(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDTipsAndTricks Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            Weighting = 1;
            Title = new NWDLocalizableStringType();
            SubTitle = new NWDLocalizableStringType();
            Message = new NWDLocalizableTextType();
        }
		//-------------------------------------------------------------------------------------------------------------
		public static List<NWDTip> PrepareListForRandom ()
		{
			ListForRandom = new List<NWDTip> ();
            foreach (NWDTip tObject in NWDTip.FindDatas()) 
			{
				/* I list the object compatible with request
			 	* I insert in the list  each object (Frequency) times
			 	* I return the List
				*/
				for (int i = 0; i < tObject.Weighting; i++) 
				{
					ListForRandom.Add (tObject);
				}
			}
			return ListForRandom;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDTip SelectRandomTips (bool sAbsoluteRemove = true)
		{
			NWDTip rReturn = null;
			// I select the tick by random 
			int tCount = ListForRandom.Count-1;
			int tIndex  = UnityEngine.Random.Range (0, tCount);
			if (tIndex >=0 && tIndex <= tCount) {
				rReturn = ListForRandom [tIndex];
				if (sAbsoluteRemove == false) {
					ListForRandom.RemoveAt (tIndex);
				} else {
					while (ListForRandom.Contains (rReturn)) {
						ListForRandom.Remove (rReturn);
					}
				}
			}
			return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserTip), typeof(NWDTip) };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================