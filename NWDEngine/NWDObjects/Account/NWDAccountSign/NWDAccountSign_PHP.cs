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

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
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
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignHelper : NWDHelper<NWDAccountSign>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            // "function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)\n" 
            //"\t ..."
            //"\t\t\t\t$sCsvList = Prepare" + tClassName + "Data($sCsv);\n"
            //"\t ..."
            return "// write your php script string here to update $tReference before sync on server\n";
            //"\t ..."
            //"\t Datas Updated"
            //"\t ..."
            //"\t}\n"
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