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
using System.IO;
using UnityEditor;
using System.Text;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDEditorNewClassContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileEmptyTemplate(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string[] assetPaths = AssetDatabase.GetAllAssetPaths();
            string assetPathFinal = null;
            string tDir = null;
            foreach (string assetPath in assetPaths)
            {
                if (assetPath.Contains(sClassNamePHP)) // or .js if you want
                {
                    string tFileName = Path.GetFileName(assetPath);
                    tFileName = Path.GetFileName(assetPath);
                    string tExtension = Path.GetExtension(tFileName);
                    tDir = Path.GetPathRoot(tFileName);
                    if (tExtension == ".cs")
                    {
                        if (tFileName == sClassNamePHP + ".cs")
                        {
                            assetPathFinal = assetPath;
                            break;
                        }
                        else if (tFileName == sClassNamePHP + "_Workflow.cs")
                        {
                            assetPathFinal = assetPath;
                        }
                        else
                        {
                            // do nothing
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(assetPathFinal) == false)
            {
                UnityEngine.Object tFile = AssetDatabase.LoadMainAssetAtPath(assetPathFinal);
                AssetDatabase.OpenAsset(tFile);
                EditorGUIUtility.PingObject(tFile);
                Selection.activeGameObject = tFile as GameObject;
            }
            if (tDir != null)
            {
                string tClassExamplePath_Empty = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Empty.cs";
                string tClassExample_Empty = File.ReadAllText(tClassExamplePath_Empty);
                tClassExample_Empty = tClassExample_Empty.Replace("NWDExample", sClassNamePHP);
                tClassExample_Empty = tClassExample_Empty.Replace("NWDBasis", sClassNameBasis);
                if (string.IsNullOrEmpty(sMacro) == false)
                {
                    tClassExample_Empty = tClassExample_Empty.Replace("NWD_EXAMPLE_MACRO", sMacro);
                    tClassExample_Empty = tClassExample_Empty.Replace("//MACRO_DEFINE ", "");
                }
                string tClassExamplePath_EmptyEditor = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_EmptyEditor.cs";
                string tClassExample_EmptyEditor = File.ReadAllText(tClassExamplePath_EmptyEditor);
                tClassExample_EmptyEditor = tClassExample_EmptyEditor.Replace("NWDExample", sClassNamePHP);
                tClassExample_EmptyEditor = tClassExample_EmptyEditor.Replace("NWDBasis", sClassNameBasis);
                if (string.IsNullOrEmpty(sMacro) == false)
                {
                    tClassExample_EmptyEditor = tClassExample_EmptyEditor.Replace("NWD_EXAMPLE_MACRO", sMacro);
                    tClassExample_EmptyEditor = tClassExample_EmptyEditor.Replace("//MACRO_DEFINE ", "");
                }

                if (Directory.Exists(tDir + "/Tests"))
                {
                    Directory.CreateDirectory(tDir + "/Tests");
                }
                if (File.Exists(tDir + sClassNamePHP + "_Workflow.cs") == false)
                {
                    File.WriteAllText(tDir + sClassNamePHP + "_Workflow.cs", tClassExample_Empty);
                    Object tFile = AssetDatabase.LoadMainAssetAtPath(tDir + sClassNamePHP + "_Workflow.cs");
                    if (tFile != null)
                    {
                        AssetDatabase.OpenAsset(tFile);
                        EditorGUIUtility.PingObject(tFile);
                        Selection.activeGameObject = tFile as GameObject;
                    }
                }
                if (File.Exists(tDir + sClassNamePHP + "_Editor.cs") == false)
                {
                    File.WriteAllText(tDir + sClassNamePHP + "_Editor.cs", tClassExample_EmptyEditor);
                    Object tFile = AssetDatabase.LoadMainAssetAtPath(tDir + sClassNamePHP + "_Editor.cs");
                    if (tFile != null)
                    {
                        AssetDatabase.OpenAsset(tFile);
                        EditorGUIUtility.PingObject(tFile);
                        Selection.activeGameObject = tFile as GameObject;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileUnitTest(string sClassNamePHP, string sMacro)
        {
            string tEditorTestClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP + NWDConstants.NWDUnitTestEditModePath;
            Directory.CreateDirectory(tEditorTestClassesFolderPath);
            string tEditorPatASMREF = tEditorTestClassesFolderPath + "/" + NWDConstants.NWDUnitTestEditModeReference + ".asmref";
            File.WriteAllText(tEditorPatASMREF, "{\n\t\"reference\": \""+ NWDConstants.NWDUnitTestEditModeAssembly + "\"\n}");
            AssetDatabase.ImportAsset(tEditorPatASMREF);



            string tEditorClassExamplePath_Workflow_unit = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/UnitTests/EditMode/Tests/NWDExample_EditMode_UnitTests.cs";
            string tEditorClassExample_UnitTests_unit = File.ReadAllText(tEditorClassExamplePath_Workflow_unit);
            tEditorClassExample_UnitTests_unit = tEditorClassExample_UnitTests_unit.Replace("NWDExample", sClassNamePHP);
            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tEditorClassExample_UnitTests_unit = tEditorClassExample_UnitTests_unit.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tEditorClassExample_UnitTests_unit = tEditorClassExample_UnitTests_unit.Replace("//MACRO_DEFINE ", "");
            }
            string tEditorFilePath_UnitTests_unit = tEditorTestClassesFolderPath + "/" + sClassNamePHP + "_EditMode_UnitTests";
            int tEditorG_unit = 0;
            while (File.Exists(tEditorFilePath_UnitTests_unit + ".cs") == true)
            {
                tEditorG_unit++;
                tEditorFilePath_UnitTests_unit = tEditorFilePath_UnitTests_unit + tEditorG_unit;
            }
            File.WriteAllText(tEditorFilePath_UnitTests_unit + ".cs", tEditorClassExample_UnitTests_unit);
            NWDDebug.Log("Write file " + tEditorFilePath_UnitTests_unit + ".cs");
            AssetDatabase.ImportAsset(tEditorFilePath_UnitTests_unit + ".cs");
            string tEditorClassExamplePath_Workflow_Process = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/UnitTests/EditMode/Tests/NWDExample_EditMode_ProcessTests.cs";
            string tEditorClassExample_UnitTests_Process = File.ReadAllText(tEditorClassExamplePath_Workflow_Process);
            tEditorClassExample_UnitTests_Process = tEditorClassExample_UnitTests_Process.Replace("NWDExample", sClassNamePHP);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tEditorClassExample_UnitTests_Process = tEditorClassExample_UnitTests_Process.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tEditorClassExample_UnitTests_Process = tEditorClassExample_UnitTests_Process.Replace("//MACRO_DEFINE ", "");
            }

            string tEditorFilePath_UnitTests_Process = tEditorTestClassesFolderPath + "/" + sClassNamePHP + "_EditMode_ProcessTests";
            int tEditorG_process = 0;
            while (File.Exists(tEditorFilePath_UnitTests_Process + ".cs") == true)
            {
                tEditorG_process++;
                tEditorFilePath_UnitTests_Process = tEditorFilePath_UnitTests_Process + tEditorG_process;
            }
            File.WriteAllText(tEditorFilePath_UnitTests_Process + ".cs", tEditorClassExample_UnitTests_Process);
            NWDDebug.Log("Write file " + tEditorFilePath_UnitTests_Process + ".cs");
            AssetDatabase.ImportAsset(tEditorFilePath_UnitTests_Process + ".cs");




            string tPlayTestClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP + NWDConstants.NWDUnitTestPlayModePath;
            Directory.CreateDirectory(tPlayTestClassesFolderPath);
            string tPlayPatASMREF = tPlayTestClassesFolderPath + "/" + NWDConstants.NWDUnitTestPlayModeReference + ".asmref";
            File.WriteAllText(tPlayPatASMREF, "{\n\t\"reference\": \"" + NWDConstants.NWDUnitTestPlayModeAssembly + "\"\n}");
            AssetDatabase.ImportAsset(tPlayPatASMREF);


            string tPlayClassExamplePath_Workflow_unit = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/UnitTests/PlayMode/Tests/NWDExample_PlayMode_UnitTests.cs";
            string tPlayClassExample_UnitTests_unit = File.ReadAllText(tPlayClassExamplePath_Workflow_unit);
            tPlayClassExample_UnitTests_unit = tPlayClassExample_UnitTests_unit.Replace("NWDExample", sClassNamePHP);
            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tPlayClassExample_UnitTests_unit = tPlayClassExample_UnitTests_unit.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tPlayClassExample_UnitTests_unit = tPlayClassExample_UnitTests_unit.Replace("//MACRO_DEFINE ", "");
            }
            string tPlayFilePath_UnitTests_unit = tPlayTestClassesFolderPath + "/" + sClassNamePHP + "_PlayMode_UnitTests";
            int tPlayG_unit = 0;
            while (File.Exists(tPlayFilePath_UnitTests_unit + ".cs") == true)
            {
                tPlayG_unit++;
                tPlayFilePath_UnitTests_unit = tPlayFilePath_UnitTests_unit + tPlayG_unit;
            }
            File.WriteAllText(tPlayFilePath_UnitTests_unit + ".cs", tPlayClassExample_UnitTests_unit);
            NWDDebug.Log("Write file " + tPlayFilePath_UnitTests_unit + ".cs");
            AssetDatabase.ImportAsset(tPlayFilePath_UnitTests_unit + ".cs");
            string tPlayClassExamplePath_Workflow_Process = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/UnitTests/PlayMode/Tests/NWDExample_PlayMode_ProcessTests.cs";
            string tPlayClassExample_UnitTests_Process = File.ReadAllText(tPlayClassExamplePath_Workflow_Process);
            tPlayClassExample_UnitTests_Process = tPlayClassExample_UnitTests_Process.Replace("NWDExample", sClassNamePHP);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tPlayClassExample_UnitTests_Process = tPlayClassExample_UnitTests_Process.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tPlayClassExample_UnitTests_Process = tPlayClassExample_UnitTests_Process.Replace("//MACRO_DEFINE ", "");
            }

            string tPlayFilePath_UnitTests_Process = tPlayTestClassesFolderPath + "/" + sClassNamePHP + "_PlayMode_ProcessTests";
            int tPlayG_process = 0;
            while (File.Exists(tPlayFilePath_UnitTests_Process + ".cs") == true)
            {
                tPlayG_process++;
                tPlayFilePath_UnitTests_Process = tPlayFilePath_UnitTests_Process + tPlayG_process;
            }
            File.WriteAllText(tPlayFilePath_UnitTests_Process + ".cs", tPlayClassExample_UnitTests_Process);
            NWDDebug.Log("Write file " + tPlayFilePath_UnitTests_Process + ".cs");
            AssetDatabase.ImportAsset(tPlayFilePath_UnitTests_Process + ".cs");



        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileConnection(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file workflow
            string tClassExamplePath_Workflow = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Connection.cs";
            string tClassExample_Connection = File.ReadAllText(tClassExamplePath_Workflow);
            tClassExample_Connection = tClassExample_Connection.Replace("NWDExample", sClassNamePHP);
            tClassExample_Connection = tClassExample_Connection.Replace("NWDBasis", sClassNameBasis);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_Connection = tClassExample_Connection.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_Connection = tClassExample_Connection.Replace("//MACRO_DEFINE ", "");
            }
            string tFilePath_Connection = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_Connection";
            int tG = 0;
            while (File.Exists(tFilePath_Connection + ".cs") == true)
            {
                tG++;
                tFilePath_Connection = tFilePath_Connection + tG;
            }
            File.WriteAllText(tFilePath_Connection + ".cs", tClassExample_Connection);
            NWDDebug.Log("Write file " + tFilePath_Connection + ".cs");
            AssetDatabase.ImportAsset(tFilePath_Connection + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileWorkflow(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file workflow
            string tClassExamplePath_Workflow = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Workflow.cs";
            string tClassExample_Workflow = File.ReadAllText(tClassExamplePath_Workflow);
            tClassExample_Workflow = tClassExample_Workflow.Replace("NWDExample", sClassNamePHP);
            tClassExample_Workflow = tClassExample_Workflow.Replace("NWDBasis", sClassNameBasis);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_Workflow = tClassExample_Workflow.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_Workflow = tClassExample_Workflow.Replace("//MACRO_DEFINE ", "");
            }
            string tFilePath_Workflow = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_Workflow";
            int tG = 0;
            while (File.Exists(tFilePath_Workflow + ".cs") == true)
            {
                tG++;
                tFilePath_Workflow = tFilePath_Workflow + tG;
            }
            File.WriteAllText(tFilePath_Workflow + ".cs", tClassExample_Workflow);
            NWDDebug.Log("Write file " + tFilePath_Workflow + ".cs");
            AssetDatabase.ImportAsset(tFilePath_Workflow + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileEditor(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file editor
            string tClassExamplePath_Editor = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Editor.cs";
            string tClassExample_Editor = File.ReadAllText(tClassExamplePath_Editor);
            tClassExample_Editor = tClassExample_Editor.Replace("NWDExample", sClassNamePHP);
            tClassExample_Editor = tClassExample_Editor.Replace("NWDBasis", sClassNameBasis);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_Editor = tClassExample_Editor.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_Editor = tClassExample_Editor.Replace("//MACRO_DEFINE ", "");
            }
            string tFilePath_Editor = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_Editor";
            int tG = 0;
            while (File.Exists(tFilePath_Editor + ".cs") == true)
            {
                tG++;
                tFilePath_Editor = tFilePath_Editor + tG;
            }
            File.WriteAllText(tFilePath_Editor + ".cs", tClassExample_Editor);
            NWDDebug.Log("Write file " + tFilePath_Editor + ".cs");
            AssetDatabase.ImportAsset(tFilePath_Editor + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileIndex(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file index example
            string tClassExamplePath_Index = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Index.cs";
            string tClassExample_Index = File.ReadAllText(tClassExamplePath_Index);
            tClassExample_Index = tClassExample_Index.Replace("NWDExample", sClassNamePHP);
            tClassExample_Index = tClassExample_Index.Replace("NWDBasis", sClassNameBasis);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_Index = tClassExample_Index.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_Index = tClassExample_Index.Replace("//MACRO_DEFINE ", "");
            }
            string tFilePath_Index = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_Index";
            int tG = 0;
            while (File.Exists(tFilePath_Index + ".cs") == true)
            {
                tG++;
                tFilePath_Index = tFilePath_Index + tG;
            }
            File.WriteAllText(tFilePath_Index + ".cs", tClassExample_Index);
            NWDDebug.Log("Write file " + tFilePath_Index + ".cs");
            AssetDatabase.ImportAsset(tFilePath_Index + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFilePHP(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file PHP extension
            string tClassExamplePath_PHP = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_PHP.cs";
            string tClassExample_PHP = File.ReadAllText(tClassExamplePath_PHP);
            tClassExample_PHP = tClassExample_PHP.Replace("NWDExample", sClassNamePHP);
            tClassExample_PHP = tClassExample_PHP.Replace("NWDBasis", sClassNameBasis);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_PHP = tClassExample_PHP.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_PHP = tClassExample_PHP.Replace("//MACRO_DEFINE ", "");
            }
            string tFilePath_PHP = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_PHP";
            int tG = 0;
            while (File.Exists(tFilePath_PHP + ".cs") == true)
            {
                tG++;
                tFilePath_PHP = tFilePath_PHP + tG;
            }
            File.WriteAllText(tFilePath_PHP + ".cs", tClassExample_PHP);
            NWDDebug.Log("Write file " + tFilePath_PHP + ".cs");
            AssetDatabase.ImportAsset(tFilePath_PHP + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileOverride(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file PHP extension
            string tClassExamplePath_Override = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Override.cs";
            string tClassExample_Override = File.ReadAllText(tClassExamplePath_Override);
            tClassExample_Override = tClassExample_Override.Replace("NWDExample", sClassNamePHP);
            tClassExample_Override = tClassExample_Override.Replace("NWDBasis", sClassNameBasis);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_Override = tClassExample_Override.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_Override = tClassExample_Override.Replace("//MACRO_DEFINE ", "");
            }
            string tFilePath_Override = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_Override";
            int tG = 0;
            while (File.Exists(tFilePath_Override + ".cs") == true)
            {
                tG++;
                tFilePath_Override = tFilePath_Override + tG;
            }
            File.WriteAllText(tFilePath_Override + ".cs", tClassExample_Override);
            NWDDebug.Log("Write file " + tFilePath_Override + ".cs");
            AssetDatabase.ImportAsset(tFilePath_Override + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileExtension(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file PHP extension
            string tClassExamplePath_Extension = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Override.cs";
            string tClassExample_Extension = File.ReadAllText(tClassExamplePath_Extension);
            tClassExample_Extension = tClassExample_Extension.Replace("NWDExample", sClassNamePHP);
            tClassExample_Extension = tClassExample_Extension.Replace("NWDBasis", sClassNameBasis);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_Extension = tClassExample_Extension.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_Extension = tClassExample_Extension.Replace("//MACRO_DEFINE ", "");
            }
            string tFilePath_Extension = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_Extension";
            int tG = 0;
            while (File.Exists(tFilePath_Extension + ".cs") == true)
            {
                tG++;
                tFilePath_Extension = tFilePath_Extension + tG;
            }
            File.WriteAllText(tFilePath_Extension + ".cs", tClassExample_Extension);
            NWDDebug.Log("Write file " + tFilePath_Extension + ".cs");
            AssetDatabase.ImportAsset(tFilePath_Extension + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileHelper(string sClassNamePHP, string sClassNameBasis, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file PHP extension
            string tClassExamplePath_Helper = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Helper.cs";
            string tClassExample_Helper = File.ReadAllText(tClassExamplePath_Helper);
            tClassExample_Helper = tClassExample_Helper.Replace("NWDExample", sClassNamePHP);
            tClassExample_Helper = tClassExample_Helper.Replace("NWDBasis", sClassNameBasis);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_Helper = tClassExample_Helper.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_Helper = tClassExample_Helper.Replace("//MACRO_DEFINE ", "");
            }
            string tFilePath_Helper = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_Helper";
            int tG = 0;
            while (File.Exists(tFilePath_Helper + ".cs") == true)
            {
                tG++;
                tFilePath_Helper = tFilePath_Helper + tG;
            }
            File.WriteAllText(tFilePath_Helper + ".cs", tClassExample_Helper);
            NWDDebug.Log("Write file " + tFilePath_Helper + ".cs");
            AssetDatabase.ImportAsset(tFilePath_Helper + ".cs");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileIcon(string sClassNamePHP)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            Directory.CreateDirectory(tOwnerClassesFolderPath + "/Editor");
            // write icon to modify
            string tIconPath = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/Images/NWDExample.png";
            string tIconPathNew = tOwnerClassesFolderPath + "/Editor/" + sClassNamePHP + ".png";
            File.Copy(tIconPath, tIconPathNew);
            string tIconPathPro = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/Images/NWDExample_pro.png";
            string tIconPathNewPro = tOwnerClassesFolderPath + "/Editor/" + sClassNamePHP + "_pro.png";
            File.Copy(tIconPathPro, tIconPathNewPro);
            string tIconPathAFDesign = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/Natives/NWDExample.afdesign";
            string tIconPathNewAFDesign = tOwnerClassesFolderPath + "/Editor/" + sClassNamePHP + ".afdesign";
            File.Copy(tIconPathAFDesign, tIconPathNewAFDesign);
            // Import Asset
            AssetDatabase.ImportAsset(tIconPathNew);
            AssetDatabase.ImportAsset(tIconPathNewPro);
            AssetDatabase.ImportAsset(tIconPathNewAFDesign);
            // change meta
            TextureImporter tIconPathNewImporter = AssetImporter.GetAtPath(tIconPathNew) as TextureImporter;
            tIconPathNewImporter.textureType = TextureImporterType.GUI;
            tIconPathNewImporter.alphaSource = TextureImporterAlphaSource.FromInput;
            tIconPathNewImporter.alphaIsTransparency = true;
            //tIconPathNewImporter. // remove matte ?
            var tIconPathNewImporterSerialized = new SerializedObject(tIconPathNewImporter);
            tIconPathNewImporterSerialized.FindProperty("m_PSDRemoveMatte").boolValue = true;
            tIconPathNewImporterSerialized.FindProperty("m_PSDShowRemoveMatteOption").boolValue = true; // this is not needed unless you want to show the option (and warning)
            tIconPathNewImporterSerialized.ApplyModifiedProperties();
            AssetDatabase.WriteImportSettingsIfDirty(tIconPathNew);
            // change meta pro
            TextureImporter tIconPathNewProImporter = AssetImporter.GetAtPath(tIconPathNewPro) as TextureImporter;
            tIconPathNewProImporter.textureType = TextureImporterType.GUI;
            tIconPathNewProImporter.alphaSource = TextureImporterAlphaSource.FromInput;
            tIconPathNewProImporter.alphaIsTransparency = true;
            //tIconPathNewImporter. // remove matte ?
            var tIconPathNewProImporterSerialized = new SerializedObject(tIconPathNewProImporter);
            tIconPathNewProImporterSerialized.FindProperty("m_PSDRemoveMatte").boolValue = true;
            tIconPathNewProImporterSerialized.FindProperty("m_PSDShowRemoveMatteOption").boolValue = true; // this is not needed unless you want to show the option (and warning)
            tIconPathNewProImporterSerialized.ApplyModifiedProperties();
            AssetDatabase.WriteImportSettingsIfDirty(tIconPathNewPro);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
