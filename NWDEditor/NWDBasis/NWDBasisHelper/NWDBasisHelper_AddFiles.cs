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
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileUnitTest()
        {
            NWDEditorNewClass.GenerateFileUnitTest(ClassNamePHP);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileConnection()
        {
            Debug.Log("Basis class is " + ClassType.BaseType.Name);
            NWDEditorNewClass.GenerateFileConnection(ClassNamePHP, ClassType.BaseType.Name);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileWorkflow()
        {
            NWDEditorNewClass.GenerateFileWorkflow(ClassNamePHP, ClassType.BaseType.Name);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileHelper()
        {
            NWDEditorNewClass.GenerateFileHelper(ClassNamePHP, ClassType.BaseType.Name);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileEditor()
        {
            NWDEditorNewClass.GenerateFileEditor(ClassNamePHP, ClassType.BaseType.Name);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileIndex()
        {
            NWDEditorNewClass.GenerateFileIndex(ClassNamePHP, ClassType.BaseType.Name);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFilePHP()
        {
            NWDEditorNewClass.GenerateFilePHP(ClassNamePHP, ClassType.BaseType.Name);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileIcon()
        {
            NWDEditorNewClass.GenerateFileIcon(ClassNamePHP);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileExtension()
        {
            NWDEditorNewClass.GenerateFileExtension(ClassNamePHP, ClassType.BaseType.Name);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileOverride()
        {
            NWDEditorNewClass.GenerateFileOverride(ClassNamePHP, ClassType.BaseType.Name);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif