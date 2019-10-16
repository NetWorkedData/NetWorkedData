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
//using BasicToolBox;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignHelper : NWDHelper<NWDAccountSign>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDAccountSign.PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSign : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_CONSTANTS_PATH(sEnvironment) + ");");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function CreateAccountSign($sAccountReference, $sSDKt, $sSDKv, $sSDKr)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine("global $ACC_TMP, $TIME_SYNC, $ACC_NEED_USER_TRANSFERT;");
            tFile.AppendLine("global $shs, $ereg_token;");
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD;");
            tFile.AppendLine("$tInternalKey = '';");
            tFile.AppendLine("$tInternalDescription = '';");
            tFile.AppendLine("$tInsertSQL='';");
            tFile.AppendLine("$tInsertSQLValue='';");
            tFile.AppendLine("$tReference = referenceGenerate ('" + NWDBasisHelper.BasisHelper<NWDAccountSign>().ClassTrigramme + "', '" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "', '" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "');");
            tFile.AppendLine("$tInsertSQL.='INSERT INTO `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` (';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "`, '; $tInsertSQLValue.= '\\''.$tReference.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ServerHash) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ServerLog) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DM) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DS) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");

            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Account) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($sAccountReference).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignType) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($sSDKt).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "`, ';$tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($sSDKv).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "`, ';$tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($sSDKr).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "`, ';$tInsertSQLValue.= '\\'" + (int)NWDAccountSignAction.Associated + "\\', ';");

            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'SignUp Sign\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\'Dev AccounSign'.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DevSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'SignUp Certified\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\'Preprod AccountSign'.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ProdSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            }
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "`, '; $tInsertSQLValue.= '\\'1\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DC) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DD) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InError) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().CheckList) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Preview) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Tag) + "`, '; $tInsertSQLValue.= '\\'" + ((int)NWDBasisTag.TagServerCreated).ToString() + "\\', '; // " + NWDBasisTag.TagServerCreated.ToString() + "");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().WebModel) + "`, '; $tInsertSQLValue.= '\\'" + NWDBasisHelper.BasisHelper<NWDAccount>().LastWebBuild + "\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().XX) + "` '; $tInsertSQLValue.= '\\'0\\'';");
            tFile.AppendLine("$tInsertSQL.=')';");
            tFile.AppendLine("$tInsertSQL.=' VALUES ('.$tInsertSQLValue.');';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tInsertSQL);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tInsertSQL"));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$rReturn = true;");
            tFile.AppendLine(""+NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_FUNCTION_INTEGRITY_REEVALUATE()+"($tReference);");
            tFile.AppendLine("}");
            tFile.AppendLine("return $rReturn;");
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