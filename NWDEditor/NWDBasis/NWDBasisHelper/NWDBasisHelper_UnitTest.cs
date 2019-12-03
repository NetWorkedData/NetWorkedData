//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:44
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
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
//using BasicToolBox;
using UnityEditor;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public void AddUnitTestFile()
        {
            // create unit folder
            string tUnitTestFolder = NWDFindOwnerClasses.PathOfPackage("/Tests");
            if (Directory.Exists(tUnitTestFolder) == false)
            {
                Directory.CreateDirectory(tUnitTestFolder);
                File.WriteAllText(tUnitTestFolder + "/NetWorkedData_UnitTests_Reference.asmref", "{\n\t\"reference\": \"NetWorkedData_UnitTests\"\n}");
                AssetDatabase.ImportAsset(tUnitTestFolder);
            }
            // create unit folder for class test
            string tUnitTestClassFolder = tUnitTestFolder+"/" + ClassNamePHP;
            Directory.CreateDirectory(tUnitTestClassFolder);
            AssetDatabase.ImportAsset(tUnitTestClassFolder);

            // create unit file for class test
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("using NUnit.Framework;");
            tFile.AppendLine("using NetWorkedData;");
            tFile.AppendLine("namespace NetWorkedDataTests");
            tFile.AppendLine("{");
            tFile.AppendLine("\tpublic partial class " + ClassNamePHP + "_Tests");
            tFile.AppendLine("\t{");
            tFile.AppendLine("\t\t[Test]");
            tFile.AppendLine("\t\tpublic void TestDuplicate()");
            tFile.AppendLine("\t\t{");
            tFile.AppendLine("\t\t\tNWDUnitTests.CleanUnitTests(); // clean environment before");
            tFile.AppendLine("\t\t\t" + ClassNamePHP + " tItemA = NWDUnitTests.NewData<" + ClassNamePHP + ">();");
            tFile.AppendLine("\t\t\ttItemA.UpdateData();");
            tFile.AppendLine("\t\t\t" + ClassNamePHP + " tItemB = NWDUnitTests.DuplicateData(tItemA);");
            tFile.AppendLine("\t\t\tAssert.AreNotEqual(tItemA.Reference, tItemB.Reference);");
            tFile.AppendLine("\t\t\tNWDUnitTests.CleanUnitTests(); // clean environment after");
            tFile.AppendLine("\t\t}");
            tFile.AppendLine("\t}");
            tFile.AppendLine("}");
            string tFilePath = tUnitTestClassFolder + "/" + ClassNamePHP + "_Tests";
            int tG = 0;
            while (File.Exists(tFilePath + ".cs") ==true)
            {
                tG++;
                tFilePath = tFilePath + tG;
            }
            File.WriteAllText(tFilePath + ".cs", tFile.ToString());
            AssetDatabase.ImportAsset(tFilePath + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif