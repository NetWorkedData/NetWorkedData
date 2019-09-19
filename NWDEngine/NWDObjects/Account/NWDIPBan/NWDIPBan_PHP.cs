//=====================================================================================================================
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
//=====================================================================================================================

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
    public partial class NWDIPBanHelper : NWDHelper<NWDIPBan>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            // "function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)\n" 
            //"\t ..."
            //"\t\t\t\t$sCsvList = Prepare" + tClassName + "Data($sCsv);\n"
            //"\t ..."
            rReturn.AppendLine("// write your php script string here to update $tReference before sync on server");
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
            // "function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)\n" 
            //"\t{\n" 
            //"\t ..."
            //"\t\t\t\t$sCsvList = Prepare" + tClassName + "Data($sCsv);\n"
            //"\t ..."
            //"\t Datas Updated"
            //"\t ..."
            rReturn.AppendLine("// write your php script string here to update after sync on server");
            //"\t ..."
            //"\t}\n"
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpGetCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("// use $tRow)");
            rReturn.AppendLine("// write your php script string here to special operation, example : $REP['" + ClassNamePHP + " After Get'] ='success!!!';");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("// in function " + PHP_FUNCTION_SPECIAL() + " ($sTimeStamp, $sAccountReferences)");
            rReturn.AppendLine("// write your php script string here to special operation, example : $REP['" + ClassName + " Special'] ='success!!!';");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpFunctions(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("function AddOnOne" + ClassNamePHP + " ($sTimeStamp, $sAccountReferences)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("// write your php script string here to special operation, example : $REP['" + ClassName + " Special'] ='success!!!';");
            rReturn.AppendLine("}");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif