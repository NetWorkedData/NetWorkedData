//=====================================================================================================================
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
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersionHelper : NWDHelper<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDVersion.PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// VERSION");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDVersion>().ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
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
            tFile.AppendLine("$tQuery = 'SELECT * FROM `"+NWDBasisHelper.TableNamePHP<NWDVersion>(sEnvironment)+"` WHERE `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().Version)+"` = \\''.$SQL_CON->real_escape_string($sVersion).'\\' AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().Buildable)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().ActiveDev)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().XX)+"` = 0 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AC)+"` = 1;';");
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($ENV=='Preprod')");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = 'SELECT * FROM `"+NWDBasisHelper.TableNamePHP<NWDVersion>(sEnvironment)+"` WHERE `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().Version)+"` = \\''.$SQL_CON->real_escape_string($sVersion).'\\' AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().Buildable)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().ActivePreprod)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().XX)+"` = 0 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AC)+"` = 1;';");
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($ENV=='Prod')");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = 'SELECT * FROM `"+NWDBasisHelper.TableNamePHP<NWDVersion>(sEnvironment)+"` WHERE `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().Version)+"` = \\''.$SQL_CON->real_escape_string($sVersion).'\\' AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().Buildable)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().ActiveProd)+"` = 1 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().XX)+"` = 0 AND `"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AC)+"` = 1;';");
            tFile.AppendLine("}");
            tFile.AppendLine("$tResult = "+ NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", NWD.K_SQL_CON));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA00));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_array())");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().BlockDataUpdate)+"'] == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA99));
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("if ($tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().BlockApplication)+"'] == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA01));
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertTitle)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertTitle)+"']);");
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertMessage)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertMessage)+"']);");
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertValidation)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertValidation)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().OSXStoreURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().OSXStoreURL)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().IOSStoreURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().IOSStoreURL)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().GooglePlayURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().GooglePlayURL)+"']);");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA02));
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertTitle)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertTitle)+"']);");
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertMessage)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertMessage)+"']);");
            //tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertValidation)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().AlertValidation)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().OSXStoreURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().OSXStoreURL)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().IOSStoreURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().IOSStoreURL)+"']);");
            tFile.AppendLine("respondAdd('"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().GooglePlayURL)+"',$tRow['"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDVersion>().GooglePlayURL)+"']);");
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