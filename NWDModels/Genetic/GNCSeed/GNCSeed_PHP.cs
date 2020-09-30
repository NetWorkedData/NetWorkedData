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
#endif
#if GNC_GENETIC
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class GNCSeedHelper : NWDHelper<GNCSeed>
    {
        public override Dictionary<string, string> CreatePHPAddonFiles(NWDAppEnvironment sEnvironment, bool sWriteOnDisk = true)
        {
            Dictionary<string, string> rReturn = new Dictionary<string, string>(new StringIndexKeyComparer());
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// EXAMPLE ENGINE FILE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            rReturn.Add(sEnvironment.EnvFolder(sWriteOnDisk) + "/GNCSeed_engine.php", tFileFormatted);
            //NWD.K_DB
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.Append(base.AddonPhpPreCalculate(sEnvironment));
            // "function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)\n" 
            //"\t ..."
            //"\t\t\t\t$sCsvList = Prepare" + tClassName + "Data($sCsv);\n"
            //"\t ..."
            rReturn.AppendLine("// write your php script string here to update $tReference before sync on server");
            rReturn.AppendLine("// use public override string AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)");
            //"\t ..."
            //"\t Datas Updated"
            //"\t ..."
            //"\t}\n"
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.Append(base.AddonPhpPostCalculate(sEnvironment));
            // "function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)\n" 
            //"\t{\n" 
            //"\t ..."
            //"\t\t\t\t$sCsvList = Prepare" + tClassName + "Data($sCsv);\n"
            //"\t ..."
            //"\t Datas Updated"
            //"\t ..."
            rReturn.AppendLine("// write your php script string here to update after sync on server");
            rReturn.AppendLine("// use public override string AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)");
            //"\t ..."
            //"\t}\n"
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpGetCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.Append(base.AddonPhpGetCalculate(sEnvironment));
            rReturn.AppendLine("// use $tRow");
            rReturn.AppendLine("// write your php script string here to special operation, example : $REP['" + ClassNamePHP + " After Get'] ='success!!!';");
            rReturn.AppendLine("// use public override string  AddonPhpGetCalculate(NWDAppEnvironment sEnvironment)");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.Append(base.AddonPhpSpecialCalculate(sEnvironment));
            rReturn.AppendLine("// in function " + PHP_FUNCTION_SPECIAL() + " ($sAccountReferences)");
            rReturn.AppendLine("// write your php script string here to special operation, example : $REP['" + ClassNamePHP + " Special'] ='success!!!';");
            rReturn.AppendLine("// use public override string  AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpFunctions(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.Append(base.AddonPhpFunctions(sEnvironment));
            rReturn.AppendLine("function AddOnOne" + ClassNamePHP + " ($sTimeStamp, $sAccountReferences)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("// write your php script string here to special operation, example : $REP['" + ClassNamePHP + " Special'] ='success!!!';");
            rReturn.AppendLine("// use public override string  AddonPhpFunctions(NWDAppEnvironment sEnvironment)");
            rReturn.AppendLine("}");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
#endif //GNC_GENETIC
