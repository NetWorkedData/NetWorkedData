// =====================================================================================================================
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
// =====================================================================================================================
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
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveWindowFromManager (NWDTypeWindow sWindow)
        {
            //BTBBenchmark.Start();
            foreach (KeyValuePair<Type,List<NWDTypeWindow>> tKeyValue in mTypeWindowDico) 
			{
				List<NWDTypeWindow> tList = tKeyValue.Value;
				if (tList.Contains (sWindow) == true) 
				{
					tList.Remove (sWindow);
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RepaintWindowsInManager (Type sType)
        {
            //BTBBenchmark.Start();
            //Debug.Log("RepaintWindowsInManager for type :" + sType.FullName); 
            if (mTypeWindowDico.ContainsKey (sType))
			{
				foreach (NWDTypeWindow tWindow in mTypeWindowDico [sType])
				{
					tWindow.Repaint ();
				}
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<EditorWindow> EditorWindowsInManager(Type sType)
        {
            //BTBBenchmark.Start();
            List<EditorWindow> tReturn = new List<EditorWindow>();
            if (mTypeWindowDico.ContainsKey(sType))
            {
                foreach (NWDTypeWindow tWindow in mTypeWindowDico[sType])
                {
                    tReturn.Add(tWindow);
                }
            }
            //BTBBenchmark.Finish();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif