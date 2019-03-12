﻿//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<Type,List<NWDTypeWindow>> mTypeWindowDico = new Dictionary<Type,List<NWDTypeWindow>>();
        //-------------------------------------------------------------------------------------------------------------
		public void AddWindowInManager (NWDTypeWindow sWindow , Type[] sType)
		{
			foreach (Type tType in sType) 
			{
				if (mTypeWindowDico.ContainsKey (tType))
				{
					List<NWDTypeWindow> tList = mTypeWindowDico [tType];
					if (tList.Contains (sWindow) == false)
					{
						tList.Add (sWindow);
					}
				} 
				else 
				{
					List<NWDTypeWindow> tList = new List<NWDTypeWindow> ();
					tList.Add (sWindow);
					mTypeWindowDico.Add (tType, tList);
						
				}
			}
        }
        //-------------------------------------------------------------------------------------------------------------
		public void RemoveWindowFromManager (NWDTypeWindow sWindow)
		{
			foreach (KeyValuePair<Type,List<NWDTypeWindow>> tKeyValue in mTypeWindowDico) 
			{
				List<NWDTypeWindow> tList = tKeyValue.Value;
				if (tList.Contains (sWindow) == true) 
				{
					tList.Remove (sWindow);
				}
			}
        }
        //-------------------------------------------------------------------------------------------------------------
		public void RepaintWindowsInManager (Type sType)
		{
            Debug.Log("RepaintWindowsInManager for type :" + sType.FullName); 
			if (mTypeWindowDico.ContainsKey (sType))
			{
				foreach (NWDTypeWindow tWindow in mTypeWindowDico [sType])
				{
					tWindow.Repaint ();
				}
			}
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void RepaintWindowForData(Type sType)
        //{
        //    //Debug.Log("RepaintWindowsInManager for type :" + sType.FullName); 
        //    if (mTypeWindowDico.ContainsKey(sType))
        //    {
        //        foreach (NWDTypeWindow tWindow in mTypeWindowDico[sType])
        //        {
        //            tWindow.Repaint();
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif