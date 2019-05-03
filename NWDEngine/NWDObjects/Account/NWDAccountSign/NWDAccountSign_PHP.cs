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
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            string tSignStatusKey = NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().SignStatus);
            string tSignHashKey = NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().SignHash);
            string tRescueHashKey = NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().RescueHash);
            string tInternalKeyKey = NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().InternalKey);
            int t_Index_SignActionKey = New_CSV_IndexOf(tSignStatusKey);
            int t_Index_SignHashKey = New_CSV_IndexOf(tSignHashKey);
            int t_Index_RescueHashKey = New_CSV_IndexOf(tRescueHashKey);
            int t_Index_InternalKeyKey = New_CSV_IndexOf(tInternalKeyKey);
            StringBuilder sScript = new StringBuilder(string.Empty);
            sScript.AppendLine("// analyze the sign ");
            sScript.AppendLine("if ($sCsvList[" + t_Index_SignActionKey + "] == " + ((int)NWDAccountSignAction.TryToAssociate).ToString()+")");
            sScript.AppendLine("{");
            sScript.Append("$tQueryRequest = 'SELECT * FROM `'.$ENV.'_" + NWDAccountSign.BasisHelper().ClassNamePHP + "` WHERE ");
            sScript.AppendLine(" ( `" + tSignHashKey + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_SignHashKey + "]).'\\'';");
            sScript.AppendLine("if ($sCsvList[" + t_Index_RescueHashKey + "]!='')");
            sScript.AppendLine("{");
            sScript.Append("$tQueryRequest .= ' OR `" + tRescueHashKey + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_RescueHashKey + "]).'\\'';");
            sScript.AppendLine("}");
            sScript.Append("$tQueryRequest .= ' ) AND `Reference` != \\''.$SQL_CON->real_escape_string($tReference).'\\' ");
            sScript.Append("AND `AC` = 1");
            sScript.AppendLine(";';"); 
            sScript.AppendLine("myLog('query = '.$tQueryRequest.'', __FILE__, __FUNCTION__, __LINE__);");
            sScript.AppendLine("$tResultRequest = $SQL_CON->query($tQueryRequest);");

                sScript.AppendLine("if (!$tResultRequest)");
                sScript.AppendLine("{");
                sScript.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryRequest.'', __FILE__, __FUNCTION__, __LINE__);");
                sScript.AppendLine("error('SERVER');");
                sScript.AppendLine("}");
                sScript.AppendLine("else");
                sScript.AppendLine("{");

                    sScript.AppendLine("if ($tResultRequest->num_rows > 0)");
                    sScript.AppendLine("{");
                    sScript.AppendLine("myLog('find sign in another data', __FILE__, __FUNCTION__, __LINE__);");
                    sScript.AppendLine("$sReplaces[" + t_Index_SignActionKey + "] = " + ((int)NWDAccountSignAction.ErrorAssociated).ToString()+";");
                    sScript.AppendLine("$sReplaces[" + t_Index_SignHashKey + "] = '';");
                    sScript.AppendLine("$sReplaces[" + t_Index_RescueHashKey + "] = '';");
                    sScript.AppendLine("$sReplaces[" + t_Index_InternalKeyKey + "] = 'Error';");
                    sScript.AppendLine("$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);");
                    sScript.AppendLine("}");
                    sScript.AppendLine("else");
                    sScript.AppendLine("{");
                    sScript.AppendLine("$sReplaces[" + t_Index_SignActionKey + "]=" + ((int)NWDAccountSignAction.Associated).ToString()+";");
                    sScript.AppendLine("$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);");
                    sScript.AppendLine("}");

            sScript.AppendLine("}");

            sScript.AppendLine("}");
            sScript.AppendLine("else if ($sCsvList[" + t_Index_SignActionKey + "] == " + ((int)NWDAccountSignAction.TryToDissociate).ToString() + ")");
            sScript.AppendLine("{");
            sScript.AppendLine("$sReplaces[" + t_Index_SignActionKey + "] = " + ((int)NWDAccountSignAction.Dissociated).ToString() + ";");
            sScript.AppendLine("$sReplaces[" + t_Index_SignHashKey + "] = '';");
            sScript.AppendLine("$sReplaces[" + t_Index_RescueHashKey + "] = '';");
                    sScript.AppendLine("$sReplaces[" + t_Index_InternalKeyKey + "] = 'Dissociated';");
            sScript.AppendLine("$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);");
            sScript.AppendLine("}");
            sScript.AppendLine("else");
            sScript.AppendLine("{");
            sScript.AppendLine("GetDatas" + ClassNamePHP + "ByReference ($tReference);");
            sScript.AppendLine("return;");
            sScript.AppendLine("}");

            return sScript.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPostCalculate(NWDAppEnvironment AppEnvironment)
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
        public override string New_AddonPhpGetCalculate(NWDAppEnvironment AppEnvironment)
        {
            //"while($tRow = $tResult->fetch_row()")
            //"{"
            return "// write your php script string here to special operation, example : \n$REP['" + ClassName + " After Get'] ='success!!!';\n";
            //"\t}\n"
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpSpecialCalculate(NWDAppEnvironment AppEnvironment)
        {
            //"function Special" + tClassName + " ($sTimeStamp, $sAccountReferences)\n" 
            //"\t{\n" 
            return "// write your php script string here to special operation, example : \n$REP['" + ClassName + " Special'] ='success!!!';\n";
            //"\t}\n"
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpFunctions(NWDAppEnvironment AppEnvironment)
        {
            return "// write your php script string here to add function in php file;\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif