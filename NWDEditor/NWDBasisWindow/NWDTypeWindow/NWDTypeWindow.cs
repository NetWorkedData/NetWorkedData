﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:21
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
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
