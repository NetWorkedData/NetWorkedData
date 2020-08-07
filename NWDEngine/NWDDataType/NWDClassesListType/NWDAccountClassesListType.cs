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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
   //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
   [SerializeField]
   public class NWDAccountClassesListType : NWEDataType
   {
       //-------------------------------------------------------------------------------------------------------------
       public NWDAccountClassesListType()
       {
           Value = string.Empty;
       }
       //-------------------------------------------------------------------------------------------------------------
       public override void Default()
       {
           Value = string.Empty;
       }
       //-------------------------------------------------------------------------------------------------------------
       public bool ContainsClasse(string sClasse)
       {
           if (sClasse == null)
           {
               return false;
           }
           return Value.Contains(sClasse);
       }
       //-------------------------------------------------------------------------------------------------------------
       public void SetClasses(string[] sClasses)
       {
           List<string> tList = new List<string>();
           foreach (string tClasse in sClasses)
           {
               tList.Add(tClasse);
           }
           string[] tNextValueArray = tList.Distinct().ToArray();
           Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
       }
       //-------------------------------------------------------------------------------------------------------------
       public void AddClasses(string[] sClasses)
       {
           List<string> tList = new List<string>();
           if (Value != null && Value != string.Empty)
           {
               string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
               tList = new List<string>(tValueArray);
           }
           foreach (string tClasse in sClasses)
           {
               tList.Add(tClasse);
           }
           string[] tNextValueArray = tList.Distinct().ToArray();
           Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
       }
       //-------------------------------------------------------------------------------------------------------------
       public void RemoveClasses(string[] sClasses)
       {
           List<string> tList = new List<string>();
           if (Value != null && Value != string.Empty)
           {
               string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
               tList = new List<string>(tValueArray);
           }
           foreach (string tClasse in sClasses)
           {
               tList.Remove(tClasse);
           }
           string[] tNextValueArray = tList.Distinct().ToArray();
           Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
       }
       //-------------------------------------------------------------------------------------------------------------
       public string[] GetClasses()
       {
           return Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
       }
       //-------------------------------------------------------------------------------------------------------------
       public List<Type> GetClassesTypeList()
       {
           ClassesPossibilities();
           List<Type> rReturn = new List<Type>();
           foreach (string tString in GetClasses())
           {
               if (kClassesInvert.ContainsKey(tString))
               {
                   rReturn.Add(kClassesInvert[tString]);
               }
           }
           return rReturn;
       }
       //-------------------------------------------------------------------------------------------------------------
       public string[] GetSortedClasses()
       {
           string[] tResult = GetClasses();
           Array.Sort(tResult, StringComparer.InvariantCulture);
           return tResult;
       }
       //-------------------------------------------------------------------------------------------------------------
       static List<string> kClassesPossibilities;
       //-------------------------------------------------------------------------------------------------------------
       static Dictionary<string, Type> kClassesInvert;
       //-------------------------------------------------------------------------------------------------------------
       public List<string> ClassesPossibilities()
       {
           if (kClassesPossibilities == null)
           {
               kClassesPossibilities = new List<string>();
               kClassesInvert = new Dictionary<string, Type>();
               foreach (Type tType in NWDDataManager.SharedInstance().ClassAccountDependentList)
               {
                   NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                   if (tHelper != null)
                   {
                       kClassesPossibilities.Add(tHelper.ClassNamePHP);
                       kClassesInvert.Add(tHelper.ClassNamePHP, tType);
                   }
               }
               kClassesPossibilities.Sort();
           }
           return kClassesPossibilities;
       }
       //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
       //-------------------------------------------------------------------------------------------------------------
       public override bool ErrorAnalyze()
       {
           bool rReturn = false;
           if (string.IsNullOrEmpty(Value) == false)
           {
               List<string> tValueListERROR = ClasseInError(new List<string>(GetClasses()));
               if (tValueListERROR.Count > 0)
               {
                   rReturn = true;
               }
           }
           InError = rReturn;
           return rReturn;
       }
       //-------------------------------------------------------------------------------------------------------------
       public List<string> ClasseInError(List<string> sClassesList)
       {
           List<string> rReturn = new List<string>();
           foreach (string tClasse in sClassesList)
           {
               if (ClassesPossibilities().Contains(tClasse) == false)
               {
                   rReturn.Add(tClasse);
               }
           }
           return rReturn;
       }
       //-------------------------------------------------------------------------------------------------------------
       public override float ControlFieldHeight()
       {
           int tRow = 1;

           int tConnection = 0;
           if (Value != null && Value != string.Empty)
           {
               string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
               tRow += tValueArray.Count();
               List<string> tValueListERROR = ClasseInError(new List<string>(tValueArray));
               if (tValueListERROR.Count > 0)
               {
                   tConnection = 1;
               }
           }

           float tHeight = (NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge) * tRow - NWDGUI.kFieldMarge +
                            tConnection * (NWDGUI.kRedLabelStyle.fixedHeight + NWDGUI.kFieldMarge +
                                         NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge);


           // test if error in reference and add button height
           if (Value != null && Value != string.Empty)
           {
               if (ClasseInError(new List<string>(Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries))).Count > 0)
               {
                   tHeight = tHeight + NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
               }
           }

           return tHeight;
       }
       //-------------------------------------------------------------------------------------------------------------
       public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
       {
           //NWDConstants.LoadImages();
           //NWDGUI.LoadStyles();
           NWDAccountClassesListType tTemporary = new NWDAccountClassesListType();
           GUIContent tContent = new GUIContent(sEntitled, sTooltips);
           tTemporary.Value = Value;
           //Type sFromType = typeof(K);

           float tWidth = sPosition.width;
           float tHeight = sPosition.height;
           float tX = sPosition.position.x;
           float tY = sPosition.position.y;
           bool tConnection = true;


           List<string> tClasseList = new List<string>();
           List<string> tInternalNameList = new List<string>();

           tClasseList.Add(NWDConstants.kFieldSeparatorA);
           tInternalNameList.Add(NWDConstants.kFieldNone);

           foreach (string tKey in ClassesPossibilities())
           {
               tClasseList.Add(tKey);
               tInternalNameList.Add(tKey);
           }

           List<GUIContent> tContentFuturList = new List<GUIContent>();
           foreach (string tS in tInternalNameList.ToArray())
           {
               tContentFuturList.Add(new GUIContent(tS));
           }


           List<string> tValueList = new List<string>();
           if (Value != null && Value != string.Empty)
           {
               string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
               tValueList = new List<string>(tValueArray);

           }

           List<string> tValueListERROR = ClasseInError(tValueList);
           if (tValueListERROR.Count > 0)
           {
               tConnection = false;
           }

           EditorGUI.BeginDisabledGroup(!tConnection);

           bool tUp = false;
           bool tDown = false;
           int tIndexToMove = -1;

           tValueList.Add(string.Empty);
           string tNewClasse = string.Empty;
           for (int i = 0; i < tValueList.Count; i++)
           {
               //string tFieldName = sEntitled;
               if (i > 0)
               {
                   //tFieldName = "   ";
                   tContent = new GUIContent("   ");
               }
               string tV = tValueList.ElementAt(i);
               int tIndex = tClasseList.IndexOf(tV);
               tIndex = EditorGUI.Popup(new Rect(tX, tY, tWidth, NWDGUI.kPopupStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDGUI.kPopupStyle);

               if (tValueListERROR.Contains(tV))
               {
                   GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth + NWDGUI.kFieldMarge, tY + 1, tWidth - EditorGUIUtility.labelWidth - NWDGUI.kFieldMarge * 4 - NWDGUI.kUpDownWidth, NWDGUI.kGrayLabelStyle.fixedHeight), "? <" + tV + ">", NWDGUI.kGrayLabelStyle);
               }

               if (tIndex >= 0)
               {
                   if (i > 0)
                   {
                       if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDGUI.kUpDownWidth + NWDGUI.kFieldMarge), tY + NWDGUI.kDatasSelectorYOffset, NWDGUI.kIconButtonStyle.fixedHeight, NWDGUI.kIconButtonStyle.fixedHeight - 2), NWDGUI.kUpContentIcon, NWDGUI.kIconButtonStyle))
                       {
                           tUp = true;
                           tIndexToMove = i;
                       }
                       if (i < tValueList.Count - 2)
                       {
                           if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDGUI.kUpDownWidth + NWDGUI.kFieldMarge) * 2, tY + NWDGUI.kDatasSelectorYOffset, NWDGUI.kIconButtonStyle.fixedHeight, NWDGUI.kIconButtonStyle.fixedHeight - 2), NWDGUI.kDownContentIcon, NWDGUI.kIconButtonStyle))
                           {
                               tDown = true;
                               tIndexToMove = i;
                           }
                       }
                   }
                   tValueList[i] = tV;
               }

               tY += NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge;
               if (tIndex >= 0 && tIndex < tClasseList.Count)
               {
                   tValueList[i] = tClasseList.ElementAt(tIndex);
               }
               else
               {
                   tValueList[i] = string.Empty;
               }
           }
           if (tDown == true)
           {
               int tNewIndex = tIndexToMove + 1;
               string tP = tValueList[tIndexToMove];
               tValueList.RemoveAt(tIndexToMove);
               if (tNewIndex >= tValueList.Count)
               {
                   tNewIndex = tValueList.Count - 1;
               }
               if (tNewIndex < 0)
               {
                   tNewIndex = 0;
               }
               tValueList.Insert(tNewIndex, tP);
           }
           if (tUp == true)
           {
               int tNewIndex = tIndexToMove - 1;
               string tP = tValueList[tIndexToMove];
               tValueList.RemoveAt(tIndexToMove);
               if (tNewIndex < 0)
               {
                   tNewIndex = 0;
               }
               tValueList.Insert(tNewIndex, tP);
           }

           tValueList.Remove(NWDConstants.kFieldSeparatorA);
           List<string> tDistinct = new List<string>();
           foreach (string tVD in tValueList)
           {
               if (tDistinct.Contains(tVD) == false)
               {
                   tDistinct.Add(tVD);
               }
           }
           string[] tNextValueArray = tDistinct.ToArray();
           string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray) + tNewClasse;
           tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
           tTemporary.Value = tNextValue;

           EditorGUI.EndDisabledGroup();

           if (tConnection == false)
           {
               tTemporary.Value = Value;

               GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, NWDGUI.kRedLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_LIST_ERROR, NWDGUI.kRedLabelStyle);
               tY = tY + NWDGUI.kFieldMarge + NWDGUI.kRedLabelStyle.fixedHeight;
               //				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
               //				tY = tY + NWDGUI.kFieldMarge + tLabelAssetStyle.fixedHeight;
               NWDGUI.BeginRedArea();
               if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY, 60.0F, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, NWDGUI.kMiniButtonStyle))
               {
                   foreach (string tDeleteClasse in tValueListERROR)
                   {
                       tValueList.Remove(tDeleteClasse);
                   }
                   tNextValueArray = tValueList.Distinct().ToArray();
                   tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
                   tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                   tTemporary.Value = tNextValue;
               }
               NWDGUI.EndRedArea();
               tY = tY + NWDGUI.kFieldMarge + NWDGUI.kMiniButtonStyle.fixedHeight;
           }

           return tTemporary;
       }
       //-------------------------------------------------------------------------------------------------------------
#endif
       //-------------------------------------------------------------------------------------------------------------
   }
   //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================