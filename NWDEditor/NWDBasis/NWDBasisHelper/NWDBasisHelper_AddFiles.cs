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