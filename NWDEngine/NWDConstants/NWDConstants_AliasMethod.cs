//=====================================================================================================================
//
// ideMobi copyright 2019 
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using BasicToolBox;
using System.Globalization;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        //public const string M_OverrideClasseInThisSync = "OverrideClasseInThisSync_1zsF4754";
        //public const string M_ClasseInThisSync = "ClasseInThisSync_1zsF4754";
        //public const string M_ClassInitialization = "ClassInitialization_df4sDe75";
        //public const string M_ErrorRegenerate = "ErrorRegenerate_dzz4Eze79";
        //-------------------------------------------------------------------------------------------------------------
        //public const string M_AddonPhpPreCalculate = "AddonPhpPreCalculate_dxEyEsrd";
        //public const string M_AddonPhpPostCalculate = "AddonPhpPostCalculate_dez4Erze";
        //public const string M_AddonPhpGetCalculate = "AddonPhpGetCalculate_Edk77aOp";
        //public const string M_AddonPhpSpecialCalculate = "AddonPhpSpecialCalculate_re5RT778";
        //public const string M_AddonPhpFunctions = "AddonPhpFunctions_erE78erz";
        //-------------------------------------------------------------------------------------------------------------
        //public const string M_ApplyAllModifications = "ApplyAllModifications";
        //-------------------------------------------------------------------------------------------------------------
        public const string M_DrawInEditor = "DrawInEditor";
        //-------------------------------------------------------------------------------------------------------------
        //public const string M_RestaureConfigurations = "M_RestaureConfigurations";
        //public const string M_PrepareToProdPublish = "M_PrepareToProdPublish";
        //public const string M_PrepareToPreprodPublish = "M_PrepareToPreprodPublish";
        //public const string M_DrawTypeInInspector = "M_DrawTypeInInspector";
        //public const string M_DrawObjectEditor = "M_DrawObjectEditor";
        public const string M_EditorGetObjects = "M_EditorGetObjects";
        public const string M_ChangeReferenceForAnotherInAllObjects = "M_ChangeReferenceForAnotherInAllObjects";
        public const string M_ChangeReferenceForAnother = "M_ChangeReferenceForAnother";
        //public const string M_ReferenceConnectionHeightSerializedString = "M_ReferenceConnectionHeightSerializedString";
        //public const string M_ReferenceConnectionFieldSerialized= "M_ReferenceConnectionFieldSerialized";
        //public const string M_ReOrderAllLocalizations= "M_ReOrderAllLocalizations";
        //public const string M_ChangeAssetPath= "M_ChangeAssetPath";
        //public const string M_SynchronizationSetToZeroTimestamp= "M_SynchronizationSetToZeroTimestamp";
        //public const string M_SynchronizationUpadteTimestamp= "M_SynchronizationUpadteTimestamp";
        //public const string M_CreateTable= "M_CreateTable";
        //public const string M_CleanTable= "M_CleanTable";
        //public const string M_PurgeTable= "M_PurgeTable";
        //public const string M_UpdateDataTable= "M_UpdateDataTable";
        //public const string M_ResetTable= "M_ResetTable";
        //public const string M_LoadFromDatabase= "M_LoadFromDatabase";
        //public const string M_IndexAll = "M_IndexAll";
        //public const string M_Informations= "M_Informations";
        public const string M_TryToChangeUserForAllObjects= "M_TryToChangeUserForAllObjects";
        public const string M_SynchronizationPullData= "M_SynchronizationPullData";
        public const string M_SynchronizationPushData= "M_SynchronizationPushData";
        //public const string M_CheckoutPushData= "M_CheckoutPushData";
        //public const string M_DeleteUser= "M_DeleteUser";
        //public const string M_UpdateMe= "M_UpdateMe";
        //public const string M_CheckError = "M_CheckError";
        //public const string M_NewObject= "M_NewObject";
        //public const string M_ClassDeclare = "M_ClassDeclare";
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //public const string M_ExportLocalizationInCSV = "M_ExportLocalizationInCSV";
        //public const string M_ImportAllLocalizations = "M_ImportAllLocalizations";
        //public const string M_RestaureObjectInEdition = "M_RestaureObjectInEdition";
        //public const string M_CreateErrorsAndMessages = "M_CreateAllError";
        public const string M_ModelReset = "M_ModelReset";
        //public const string M_CreateAllPHP = "M_CreateAllPHP";
        public const string M_BasisCreatePHP = "M_BasisCreatePHP";
        //public const string M_NodeCardAnalyze = "M_NodeCardAnalyze";
        //public const string M_AddOnNodeDraw = "M_AddOnNodeDraw";
        //public const string M_AddOnNodePropertyDraw = "M_AddOnNodePropertyDraw";
        //public const string M_EditorAddNewObject = "M_EditorAddNewObject";
        //public const string M_PathOfPackage = "M_PathOfPackage";
        public const string M_ModelAnalyze = "M_ModelAnalyze";
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================