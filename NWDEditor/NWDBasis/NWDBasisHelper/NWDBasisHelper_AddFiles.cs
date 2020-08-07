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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileUnitTest()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFileUnitTest(ClassNamePHP, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileConnection()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFileConnection(ClassNamePHP, ClassType.BaseType.Name, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileWorkflow()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFileWorkflow(ClassNamePHP, ClassType.BaseType.Name, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileHelper()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFileHelper(ClassNamePHP, ClassType.BaseType.Name, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileEditor()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFileEditor(ClassNamePHP, ClassType.BaseType.Name, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileIndex()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFileIndex(ClassNamePHP, ClassType.BaseType.Name, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFilePHP()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFilePHP(ClassNamePHP, ClassType.BaseType.Name, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileIcon()
        {
            NWDEditorNewClass.GenerateFileIcon(ClassNamePHP);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileExtension()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFileExtension(ClassNamePHP, ClassType.BaseType.Name, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateFileOverride()
        {
            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            NWDEditorNewClass.GenerateFileOverride(ClassNamePHP, ClassType.BaseType.Name, tMacro);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif