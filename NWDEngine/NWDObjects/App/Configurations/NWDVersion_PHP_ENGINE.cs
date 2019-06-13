// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:25
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersionHelper : NWDHelper<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDVersion.New_PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string New_PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// VERSION");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDVersion.BasisHelper().ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function versionTest($sVersion)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $ENV;");
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $admin;");
            tFile.AppendLine("$return = true;");
            tFile.AppendLine("$tTested = false;");
            tFile.AppendLine("if ($ENV=='Dev')");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = 'SELECT * FROM `"+NWDVersion.TableNamePHP(sEnvironment)+"` WHERE `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().Version)+"` = \\''.$SQL_CON->real_escape_string($sVersion).'\\' AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().Buildable)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().ActiveDev)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().XX)+"` = 0 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AC)+"` = 1;';");
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($ENV=='Preprod')");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = 'SELECT * FROM `"+NWDVersion.TableNamePHP(sEnvironment)+"` WHERE `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().Version)+"` = \\''.$SQL_CON->real_escape_string($sVersion).'\\' AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().Buildable)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().ActivePreprod)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().XX)+"` = 0 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AC)+"` = 1;';");
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($ENV=='Prod')");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = 'SELECT * FROM `"+NWDVersion.TableNamePHP(sEnvironment)+"` WHERE `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().Version)+"` = \\''.$SQL_CON->real_escape_string($sVersion).'\\' AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().Buildable)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().ActiveProd)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().XX)+"` = 0 AND `"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AC)+"` = 1;';");
            tFile.AppendLine("}");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA00));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_array())");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().BlockDataUpdate)+"'] == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA99));
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("if ($tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().BlockApplication)+"'] == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA01));
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertTitle)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertTitle)+"']);");
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertMessage)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertMessage)+"']);");
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertValidation)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertValidation)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().OSXStoreURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().OSXStoreURL)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().IOSStoreURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().IOSStoreURL)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().GooglePlayURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().GooglePlayURL)+"']);");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA02));
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertTitle)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertTitle)+"']);");
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertMessage)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertMessage)+"']);");
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertValidation)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().AlertValidation)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().OSXStoreURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().OSXStoreURL)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().IOSStoreURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().IOSStoreURL)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().GooglePlayURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDVersion.FictiveData().GooglePlayURL)+"']);");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("return $return;");
            tFile.AppendLine("}");

            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif