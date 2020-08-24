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
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDEditorNewClassContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        //public static void GenerateFileUnitTest(string sClassNamePHP, bool sInClassFolder = false)
        //{
        //    // create unit folder
        //    string tTestsFolder = "/Tests";
        //    if (sInClassFolder == true)
        //    {
        //        tTestsFolder = "/" + sClassNamePHP + "/Tests";
        //    }
        //    string tUnitTestFolder = NWDFindOwnerClasses.PathOfPackage(tTestsFolder);
        //    if (Directory.Exists(tUnitTestFolder) == false)
        //    {
        //        Directory.CreateDirectory(tUnitTestFolder);
        //        File.WriteAllText(tUnitTestFolder + "/NetWorkedData_UnitTests_Reference.asmref", "{\n\t\"reference\": \"NWDEditor_UnitTests\"\n}");
        //        AssetDatabase.ImportAsset(tUnitTestFolder);
        //    }
        //    // create unit folder for class test
        //    string tUnitTestClassFolder = tUnitTestFolder + "/" + sClassNamePHP;
        //    if (sInClassFolder == true)
        //    {
        //        tUnitTestClassFolder = tUnitTestFolder;
        //    }
        //    Directory.CreateDirectory(tUnitTestClassFolder);
        //    AssetDatabase.ImportAsset(tUnitTestClassFolder);

        //    // create unit file for class test
        //    StringBuilder tFile = new StringBuilder();
        //    tFile.AppendLine("using NUnit.Framework;");
        //    tFile.AppendLine("using NetWorkedData;");
        //    tFile.AppendLine("namespace NWDEditorTests");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("\tpublic partial class " + sClassNamePHP + "_Tests");
        //    tFile.AppendLine("\t{");
        //    tFile.AppendLine("\t\t[Test]");
        //    tFile.AppendLine("\t\tpublic void TestDuplicate()");
        //    tFile.AppendLine("\t\t{");
        //    tFile.AppendLine("\t\t\tNWDUnitTests.CleanUnitTests(); // clean environment before");
        //    tFile.AppendLine("\t\t\t" + sClassNamePHP + " tItemA = NWDUnitTests.NewData<" + sClassNamePHP + ">();");
        //    tFile.AppendLine("\t\t\ttItemA.UpdateData();");
        //    tFile.AppendLine("\t\t\t" + sClassNamePHP + " tItemB = NWDBasisHelper.DuplicateData(tItemA);");
        //    tFile.AppendLine("\t\t\tAssert.AreNotEqual(tItemA.Reference, tItemB.Reference);");
        //    tFile.AppendLine("\t\t\tNWDUnitTests.CleanUnitTests(); // clean environment after");
        //    tFile.AppendLine("\t\t}");
        //    tFile.AppendLine("\t}");
        //    tFile.AppendLine("}");
        //    string tFilePath = tUnitTestClassFolder + "/" + sClassNamePHP + "_Tests";
        //    int tG = 0;
        //    while (File.Exists(tFilePath + ".cs") == true)
        //    {
        //        tG++;
        //        tFilePath = tFilePath + tG;
        //    }
        //    File.WriteAllText(tFilePath + ".cs", tFile.ToString());
        //    AssetDatabase.ImportAsset(tFilePath + ".cs");
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateFileUnitTest(string sClassNamePHP, string sMacro)
        {
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + sClassNamePHP + "/Tests";
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // create asmref unit tests
            string tPatASMREF = tOwnerClassesFolderPath + "/NetWorkedData_UnitTests_Reference.asmref";
            File.WriteAllText(tPatASMREF, "{\n\t\"reference\": \"NWDEditor_UnitTests\"\n}");
            AssetDatabase.ImportAsset(tPatASMREF);
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file workflow
            string tClassExamplePath_Workflow = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/Tests/NWDExample_UnitTests.cs";
            string tClassExample_UnitTests = File.ReadAllText(tClassExamplePath_Workflow);
            tClassExample_UnitTests = tClassExample_UnitTests.Replace("NWDExample", sClassNamePHP);

            if (string.IsNullOrEmpty(sMacro) == false)
            {
                tClassExample_UnitTests = tClassExample_UnitTests.Replace("NWD_EXAMPLE_MACRO", sMacro);
                tClassExample_UnitTests = tClassExample_UnitTests.Replace("//MACRO_DEFINE ", "");
            }

            string tFilePath_UnitTests = tOwnerClassesFolderPath + "/" + sClassNamePHP + "_UnitTests";
            int tG = 0;
            while (File.Exists(tFilePath_UnitTests + ".cs") == true)
            {
                tG++;
                tFilePath_UnitTests = tFilePath_UnitTests + tG;
            }
            File.WriteAllText(tFilePath_UnitTests + ".cs", tClassExample_UnitTests);
            NWDDebug.Log("Write file " + tFilePath_UnitTests + ".cs");
            AssetDatabase.ImportAsset(tFilePath_UnitTests + ".cs");
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
            string tIconPath = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/NWDExample.psd";
            string tIconPathNew = tOwnerClassesFolderPath + "/Editor/" + sClassNamePHP + ".psd";
            File.Copy(tIconPath, tIconPathNew);
            string tIconPathPro = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/NWDExample_pro.psd";
            string tIconPathNewPro = tOwnerClassesFolderPath + "/Editor/" + sClassNamePHP + "_pro.psd";
            File.Copy(tIconPathPro, tIconPathNewPro);
            AssetDatabase.ImportAsset(tIconPathNew);
            AssetDatabase.ImportAsset(tIconPathNewPro);

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
