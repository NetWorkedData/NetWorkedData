//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
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
