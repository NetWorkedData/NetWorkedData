//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:43
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
		public void AddWindowInManager (NWDTypeWindow sWindow , List<Type> sType)
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveWindowFromManager (NWDTypeWindow sWindow)
        {
            //NWEBenchmark.Start();
            foreach (KeyValuePair<Type,List<NWDTypeWindow>> tKeyValue in mTypeWindowDico)
			{
				List<NWDTypeWindow> tList = tKeyValue.Value;
				if (tList.Contains (sWindow) == true)
				{
					tList.Remove (sWindow);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RepaintWindowsInManager (Type sType)
        {
            //NWEBenchmark.Start();
            //Debug.Log("RepaintWindowsInManager for type :" + sType.FullName); 
            if (mTypeWindowDico.ContainsKey (sType))
			{
				foreach (NWDTypeWindow tWindow in mTypeWindowDico [sType])
				{
					tWindow.Repaint ();
				}
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDTypeWindow> EditorWindowsInManager(Type sType)
        {
            //NWEBenchmark.Start();
            List<NWDTypeWindow> tReturn = new List<NWDTypeWindow>();
            if (mTypeWindowDico.ContainsKey(sType))
            {
                foreach (NWDTypeWindow tWindow in mTypeWindowDico[sType])
                {
                    tReturn.Add(tWindow);
                }
            }
            //NWEBenchmark.Finish();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif