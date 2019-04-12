// =====================================================================================================================
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
// =====================================================================================================================
using System;
#if UNITY_EDITOR
using UnityEditor;
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
        // TODO : Remove and replace by icon from name of class ... but perhaps used in other windows type?
		public string IconName;
        public Type[] TypeList;
        //-------------------------------------------------------------------------------------------------------------
		public NWDTypeWindowParamAttribute (string sTitle, string sDescription, string sIconName, Type[] sTypeList)
		{
			this.Title = sTitle;
			this.Description = sDescription;
			this.IconName = sIconName;
			this.TypeList = sTypeList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeWindowParamAttribute(string sTitle, string sDescription, Type[] sTypeList)
        {
            this.Title = sTitle;
            this.Description = sDescription;
            this.IconName = null;
            this.TypeList = sTypeList;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDTypeWindow : EditorWindow
	{
		//-------------------------------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
