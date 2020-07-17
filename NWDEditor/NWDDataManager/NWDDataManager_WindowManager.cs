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
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveWindowFromManager (NWDTypeWindow sWindow)
        {
            //NWDBenchmark.Start();
            foreach (KeyValuePair<Type,List<NWDTypeWindow>> tKeyValue in mTypeWindowDico)
			{
				List<NWDTypeWindow> tList = tKeyValue.Value;
				if (tList.Contains (sWindow) == true)
				{
					tList.Remove (sWindow);
                }
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RepaintWindowsInManager (Type sType)
        {
            //NWDBenchmark.Start();
            //Debug.Log("RepaintWindowsInManager for type :" + sType.FullName); 
            if (mTypeWindowDico.ContainsKey (sType))
			{
				foreach (NWDTypeWindow tWindow in mTypeWindowDico [sType])
				{
					tWindow.Repaint ();
				}
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDTypeWindow> EditorWindowsInManager(Type sType)
        {
            //NWDBenchmark.Start();
            List<NWDTypeWindow> tReturn = new List<NWDTypeWindow>();
            if (mTypeWindowDico.ContainsKey(sType))
            {
                foreach (NWDTypeWindow tWindow in mTypeWindowDico[sType])
                {
                    tReturn.Add(tWindow);
                }
            }
            //NWDBenchmark.Finish();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif