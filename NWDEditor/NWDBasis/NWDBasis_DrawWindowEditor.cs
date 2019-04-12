// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:21:4
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        ////-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_DrawInEditor)]
        //public static void DrawInEditor(EditorWindow sEditorWindow, bool sAutoSelect = false)
        //{
        //    BasisHelper().New_DrawTableEditor(sEditorWindow);
        //    if (sAutoSelect == true)
        //    {
        //        BasisHelper().New_SelectedFirstObjectInTable(sEditorWindow);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif