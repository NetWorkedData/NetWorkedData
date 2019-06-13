// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:15
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using BasicToolBox;
using UnityEditor;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignHelper : NWDHelper<NWDAccountSign>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            string tSignStatusKey = NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().SignStatus);
            string tSignHashKey = NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().SignHash);
            string tRescueHashKey = NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().RescueHash);
            string tInternalDescription = NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().InternalDescription);
            int t_Index_SignActionKey = New_CSV_IndexOf(tSignStatusKey);
            int t_Index_SignHashKey = New_CSV_IndexOf(tSignHashKey);
            int t_Index_RescueHashKey = New_CSV_IndexOf(tRescueHashKey);
            int t_Index_InternalDescription= New_CSV_IndexOf(tInternalDescription);
            StringBuilder sScript = new StringBuilder(string.Empty);
            sScript.AppendLine("// analyze the sign ");
            sScript.AppendLine("if ($sCsvList[" + t_Index_SignActionKey + "] == " + ((int)NWDAccountSignAction.TryToAssociate).ToString()+")");
            sScript.AppendLine("{");
            sScript.Append("$tQueryRequest = 'SELECT * FROM `" + NWDAccountSign.TableNamePHP(sEnvironment) + "` WHERE ");
            sScript.AppendLine(" ( `" + tSignHashKey + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_SignHashKey + "]).'\\'';");
            sScript.AppendLine("if ($sCsvList[" + t_Index_RescueHashKey + "]!='')");
            sScript.AppendLine("{");
            sScript.Append("$tQueryRequest .= ' OR `" + tRescueHashKey + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_RescueHashKey + "]).'\\'';");
            sScript.AppendLine("}");
            sScript.Append("$tQueryRequest .= ' ) AND `Reference` != \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' ");
            sScript.Append("AND `AC` = 1");
            sScript.AppendLine(";';");
            sScript.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRequest"));
            sScript.AppendLine("$tResultRequest = "+NWD.K_SQL_CON+"->query($tQueryRequest);");
            sScript.AppendLine("if (!$tResultRequest)");
            sScript.AppendLine("{");
            sScript.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRequest"));
            sScript.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            sScript.AppendLine("}");
            sScript.AppendLine("else");
            sScript.AppendLine("{");
            sScript.AppendLine("if ($tResultRequest->num_rows > 0)");
            sScript.AppendLine("{");
            sScript.AppendLine("$sReplaces[" + t_Index_SignActionKey + "] = " + ((int)NWDAccountSignAction.ErrorAssociated).ToString()+";");
            sScript.AppendLine("$sReplaces[" + t_Index_SignHashKey + "] = '';");
            sScript.AppendLine("$sReplaces[" + t_Index_RescueHashKey + "] = '';");
            sScript.AppendLine("$sReplaces[" + t_Index_InternalDescription + "] = 'Error';");
            sScript.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            sScript.AppendLine("}");
            sScript.AppendLine("else");
            sScript.AppendLine("{");
            sScript.AppendLine("$sReplaces[" + t_Index_SignActionKey + "]=" + ((int)NWDAccountSignAction.Associated).ToString()+";");
            sScript.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            sScript.AppendLine("}");
            sScript.AppendLine("}");
            sScript.AppendLine("}");
            sScript.AppendLine("else if ($sCsvList[" + t_Index_SignActionKey + "] == " + ((int)NWDAccountSignAction.TryToDissociate).ToString() + ")");
            sScript.AppendLine("{");
            sScript.AppendLine("$sReplaces[" + t_Index_SignActionKey + "] = " + ((int)NWDAccountSignAction.Dissociated).ToString() + ";");
            sScript.AppendLine("$sReplaces[" + t_Index_SignHashKey + "] = '';");
            sScript.AppendLine("$sReplaces[" + t_Index_RescueHashKey + "] = '';");
            sScript.AppendLine("$sReplaces[" + t_Index_InternalDescription + "] = 'Dissociated';");
            sScript.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            sScript.AppendLine("}");
            sScript.AppendLine("else");
            sScript.AppendLine("{");
            sScript.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            sScript.AppendLine("return;");
            sScript.AppendLine("}");

            return sScript.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            // "function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)\n" 
            //"\t{\n" 
            //"\t ..."
            //"\t\t\t\t$sCsvList = Prepare" + tClassName + "Data($sCsv);\n"
            //"\t ..."
            //"\t Datas Updated"
            //"\t ..."
            return "// write your php script string here to update afetr sync on server\n";
            //"\t ..."
            //"\t}\n"
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpGetCalculate(NWDAppEnvironment sEnvironment)
        {
            //"while($tRow = $tResult->fetch_row()")
            //"{"
            return "// write your php script string here to special operation, example : \n$REP['" + ClassName + " After Get'] ='success!!!';\n";
            //"\t}\n"
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)
        {
            //"function Special" + tClassName + " ($sTimeStamp, $sAccountReferences)\n" 
            //"\t{\n" 
            return "// write your php script string here to special operation, example : \n$REP['" + ClassName + " Special'] ='success!!!';\n";
            //"\t}\n"
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpFunctions(NWDAppEnvironment sEnvironment)
        {
            return "// write your php script string here to add function in php file;\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif