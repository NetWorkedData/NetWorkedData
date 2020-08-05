//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-09-9 18:24:43
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	MacroDefineEditor for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
//=====================================================================================================================
namespace MacroDefineEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class MDEConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string Path = "Assets/pref.MDE";
        public const float RowHeight = 16.0F;
        public const float ManagementWidth = 100.0F;
        public const string NONE = "NONE";
        //-------------------------------------------------------------------------------------------------------------
        public const string Management = "Management";
        public const string Remove = "Remove";
        public const string MacroDefineEditor = "Macro Define Editor";
        public const string Menu = "Window/" + MacroDefineEditor;
        public const string EnumArea = "Macro add by enum";
        public const string BoolArea = "Macro add by boolean";
        public const string ErrorInLoading = "... error in loading ...";
        public const string Reload = "Reload";
        public const string Result = "Result in build settings";
        public const string NewMacroArea = "Define new macro here";
        public const string NewMacro = "Macro";
        public const string NewMacroButton = "Add Macro";
        public const string SaveArea = "Save configuration and compile if necessairy";
        public const string Save = "Save and compile";
        //-------------------------------------------------------------------------------------------------------------
        public const string AlertTitle = "Warning";
        public const string AlertMessage = "You will save and compile another configuration, are you sure?";
        public const string AlertOK = "Yes, save";
        public const string AlertCancel = "No, cancel";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif