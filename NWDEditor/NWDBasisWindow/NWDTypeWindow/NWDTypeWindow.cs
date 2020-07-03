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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[AttributeUsage (AttributeTargets.Class, AllowMultiple = true)]
	public class NWDTypeWindowParamAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
		public string Title;
		public string Description;
        public Type[] TypeList;
        //-------------------------------------------------------------------------------------------------------------
        [Obsolete]
		public NWDTypeWindowParamAttribute (string sTitle, string sDescription, string sIconName, Type[] sTypeList)
		{
			this.Title = sTitle;
			this.Description = sDescription;
			this.TypeList = sTypeList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeWindowParamAttribute(string sTitle, string sDescription, Type[] sTypeList)
        {
            this.Title = sTitle;
            this.Description = sDescription;
            this.TypeList = sTypeList;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDTypeWindow : NWDEditorWindow
	{
        //-------------------------------------------------------------------------------------------------------------
        public virtual void SelectTab(Type tType)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDTypeWindow));
            foreach (NWDTypeWindow tWindow in tWindows)
            {
                tWindow.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
