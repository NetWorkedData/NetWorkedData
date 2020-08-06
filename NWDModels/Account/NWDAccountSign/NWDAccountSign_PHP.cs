//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignHelper : NWDHelper<NWDAccountSign>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            string tSignReference = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference);
            string tSignStatusKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus);
            string tXXKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().XX);
            string tSignHashKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash);
            string tRescueHashKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash);
            string tLoginHashKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().LoginHash);
            string tInternalDescription = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalDescription);
            int t_Index_XXKey = CSV_IndexOf(tXXKey);
            int t_Index_SignActionKey = CSV_IndexOf(tSignStatusKey);
            int t_Index_SignHashKey = CSV_IndexOf(tSignHashKey);
            int t_Index_RescueHashKey = CSV_IndexOf(tRescueHashKey);
            int t_Index_LoginHashKey = CSV_IndexOf(tLoginHashKey);
            int t_Index_InternalDescription = CSV_IndexOf(tInternalDescription);
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("// analyze the sign ");
            tFile.AppendLine("if ($sCsvList[" + t_Index_SignActionKey + "] == " + ((int)NWDAccountSignAction.TryToAssociate).ToString() + ")");
            tFile.AppendLine("{");
            {
                tFile.Append("$tQueryRequest = 'SELECT * FROM `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` WHERE ");
                tFile.AppendLine(" ( `" + tSignHashKey + "` = \\''.EscapeString($sCsvList[" + t_Index_SignHashKey + "]).'\\'';");
                tFile.AppendLine("if ($sCsvList[" + t_Index_RescueHashKey + "]!='')");
                tFile.AppendLine("{");
                {
                    tFile.Append("$tQueryRequest .= ' OR `" + tRescueHashKey + "` = \\''.EscapeString($sCsvList[" + t_Index_RescueHashKey + "]).'\\'';");

                }
                tFile.AppendLine("}");
                tFile.AppendLine("if ($sCsvList[" + t_Index_LoginHashKey + "]!='')");
                tFile.AppendLine("{");
                {
                    tFile.Append("$tQueryRequest .= ' OR `" + tLoginHashKey + "` = \\''.EscapeString($sCsvList[" + t_Index_LoginHashKey + "]).'\\'';");

                }
                tFile.AppendLine("}");
                tFile.Append("$tQueryRequest .= ' ) AND `" + tSignReference + "` != \\''.EscapeString($tReference).'\\' ");
                tFile.Append("AND `AC` = 1");
                tFile.AppendLine(";';");
                tFile.AppendLine("$tResultRequest = SelectFromAllConnexions($tQueryRequest);");
                tFile.AppendLine("if ($tResultRequest['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResultRequest['error_log'].'"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("if ($tResultRequest['count'] > 0)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$sReplaces[" + t_Index_SignActionKey + "] = " + ((int)NWDAccountSignAction.ErrorAssociated).ToString() + ";");
                        tFile.AppendLine("$sReplaces[" + t_Index_SignHashKey + "] = '';");
                        tFile.AppendLine("$sReplaces[" + t_Index_RescueHashKey + "] = '';");
                        tFile.AppendLine("$sReplaces[" + t_Index_InternalDescription + "] = 'Error';");
                        tFile.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$sReplaces[" + t_Index_SignActionKey + "]=" + ((int)NWDAccountSignAction.Associated).ToString() + ";");
                        tFile.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($sCsvList[" + t_Index_SignActionKey + "] == " + ((int)NWDAccountSignAction.TryToDissociate).ToString() + ")");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("$sReplaces[" + t_Index_SignActionKey + "] = " + ((int)NWDAccountSignAction.Dissociated).ToString() + ";");
                tFile.AppendLine("$sReplaces[" + t_Index_SignHashKey + "] = '';");
                tFile.AppendLine("$sReplaces[" + t_Index_RescueHashKey + "] = '';");
                tFile.AppendLine("$sReplaces[" + t_Index_InternalDescription + "] = 'Dissociated';");
                tFile.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " (GetCurrentConnexion(), $tReference);");
                tFile.AppendLine("return;");
            }
            tFile.AppendLine("}");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif