//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[SerializeField]
	public class NWDSynchronizedClassesListType: BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDSynchronizedClassesListType ()
		{
			Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
		//-------------------------------------------------------------------------------------------------------------
		public bool ContainsClasse (string sClasse)
		{
			if (sClasse == null) {
				return false;
			}
			return Value.Contains (sClasse);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetClasses (string[] sClasses)
		{
			List<string> tList = new List<string> ();
			foreach (string tClasse in sClasses) {
				tList.Add (tClasse);
			}
			string[] tNextValueArray = tList.Distinct ().ToArray ();
			Value = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AddClasses (string[] sClasses)
		{
			List<string> tList = new List<string> ();
			if (Value != null && Value != string.Empty) 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tList = new List<string> (tValueArray);
			}
			foreach (string tClasse in sClasses) {
				tList.Add (tClasse);
			}
			string[] tNextValueArray = tList.Distinct ().ToArray ();
			Value = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void RemoveClasses (string[] sClasses)
		{
			List<string> tList = new List<string> ();
			if (Value != null && Value != string.Empty) 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tList = new List<string> (tValueArray);
			}
			foreach (string tClasse in sClasses) {
				tList.Remove (tClasse);
			}
			string[] tNextValueArray = tList.Distinct ().ToArray ();
			Value = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
		}
		//-------------------------------------------------------------------------------------------------------------
		public string[] GetClasses ()
		{
			return Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] GetSortedClasses()
        {
            string[] tResult = GetClasses();
            Array.Sort(tResult, StringComparer.InvariantCulture);
            return tResult;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        static List<string> kClassesPossibilities;
        //-------------------------------------------------------------------------------------------------------------
        public List<string> ClassesPossibilities()
        {
            if (kClassesPossibilities == null)
            {
                kClassesPossibilities = new List<string>();
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    if (tHelper != null)
                    {
                        kClassesPossibilities.Add(tHelper.ClassNamePHP);
                    }
                }
                kClassesPossibilities.Sort();
            }
            return kClassesPossibilities;
        }
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
		public List<string> ClasseInError( List<string> sClassesList) {
			List<string> rReturn = new List<string> ();
			foreach (string tClasse in sClassesList) {
                if (ClassesPossibilities().Contains(tClasse) == false) {
					rReturn.Add (tClasse);
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			int tRow = 1;

			int tConnection = 0;
			if (Value != null && Value != string.Empty) 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tRow += tValueArray.Count ();
				List<string> tValueListERROR = ClasseInError (new List<string> (tValueArray));
				if (tValueListERROR.Count > 0) {
					tConnection = 1;
				}
			}

            float tHeight = (NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge) * tRow - NWDConstants.kFieldMarge + 
                             tConnection*(NWDConstants.kRedLabelStyle.fixedHeight+NWDConstants.kFieldMarge+
                                          NWDConstants.kMiniButtonStyle.fixedHeight+NWDConstants.kFieldMarge);


			// test if error in reference and add button height
			if (Value != null && Value != string.Empty) 
			{
				if (ClasseInError (new List<string> (Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries))).Count > 0) {
                    tHeight = tHeight + NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
				}
			}

			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
        {
            //NWDConstants.LoadImages();
            //NWDConstants.LoadStyles();
            NWDSynchronizedClassesListType tTemporary = new NWDSynchronizedClassesListType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
			tTemporary.Value = Value;
			//Type sFromType = typeof(K);

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;
			float tEditWidth = NWDConstants.kEditWidth;
			bool tConnection = true;


			List<string> tClasseList = new List<string> ();
			List<string> tInternalNameList = new List<string> ();

            tClasseList.Add(NWDConstants.kFieldSeparatorA);
            tInternalNameList.Add(NWDConstants.kFieldNone);

            foreach (string  tKey in ClassesPossibilities())
            {
                tClasseList.Add(tKey);
                tInternalNameList.Add(tKey);
            }

            List<GUIContent> tContentFuturList = new List<GUIContent>();
            foreach (string tS in tInternalNameList.ToArray())
            {
                tContentFuturList.Add(new GUIContent(tS));
            }


			List<string> tValueList = new List<string> ();
			if (Value != null && Value != string.Empty) 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tValueList = new List<string> (tValueArray);

			}

			List<string> tValueListERROR = ClasseInError (tValueList);
			if (tValueListERROR.Count > 0) {
				tConnection = false;
			}

			EditorGUI.BeginDisabledGroup (!tConnection);

            bool tUp = false;
            bool tDown = false;
            int tIndexToMove = -1;

			tValueList.Add (string.Empty);
            string tNewClasse= string.Empty;
			for (int i = 0; i < tValueList.Count; i++) 
			{
				//string tFieldName = sEntitled;
				if (i > 0) 
				{
					//tFieldName = "   ";
                    tContent = new GUIContent("   ");
				}
				string tV = tValueList.ElementAt (i);
				int tIndex = tClasseList.IndexOf (tV);
                tIndex = EditorGUI.Popup (new Rect (tX, tY, tWidth, NWDConstants.kPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray (), NWDConstants.kPopupdStyle);

				if (tValueListERROR.Contains (tV)) {
                    GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth+NWDConstants.kFieldMarge, tY+1, tWidth - EditorGUIUtility.labelWidth -NWDConstants.kFieldMarge*4 - tEditWidth, NWDConstants.kGrayLabelStyle.fixedHeight), "? <"+tV+">", NWDConstants.kGrayLabelStyle);
				}

                if (tIndex >= 0) {
                   
                    if (i > 0)
                    {
                        GUIContent tUpContent = new GUIContent(NWDConstants.kImageUp, "up");
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (tEditWidth + NWDConstants.kFieldMarge)*2, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tUpContent, NWDConstants.kPopupButtonStyle))
                        {
                            tUp = true;
                            tIndexToMove = i;
                        }
                    }
                    if (i < tValueList.Count-2)
                    {
                        GUIContent tDownContent = new GUIContent(NWDConstants.kImageDown, "down");
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (tEditWidth + NWDConstants.kFieldMarge), tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tDownContent, NWDConstants.kPopupButtonStyle))
                        {
                            tDown = true;
                            tIndexToMove = i;
                        }
                    }
				}

                tY += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
				if (tIndex >= 0 && tIndex < tClasseList.Count) 
				{
					tValueList [i] = tClasseList.ElementAt (tIndex);
				} 
				else 
				{
					tValueList [i] = string.Empty;
				}
			}
            if (tDown == true)
            {
                int tNewIndex = tIndexToMove + 1;
                string tP = tValueList[tIndexToMove];
                tValueList.RemoveAt(tIndexToMove);
                if (tNewIndex >= tValueList.Count)
                {
                    tNewIndex = tValueList.Count-1;
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

			string[] tNextValueArray = tValueList.Distinct ().ToArray ();
            string tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray)+tNewClasse;
			tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
			tTemporary.Value = tNextValue;

			EditorGUI.EndDisabledGroup ();

			if (tConnection == false) {
				tTemporary.Value = Value;

                GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, NWDConstants.kRedLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_LIST_ERROR, NWDConstants.kRedLabelStyle);
                tY = tY + NWDConstants.kFieldMarge + NWDConstants.kRedLabelStyle.fixedHeight;
                //				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
                //				tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
                NWDConstants.GUIRedButtonBegin();
                if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, NWDConstants.kDeleteButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, NWDConstants.kDeleteButtonStyle)) {
					foreach (string tDeleteClasse in tValueListERROR) {
						tValueList.Remove (tDeleteClasse);
					}
					tNextValueArray = tValueList.Distinct ().ToArray ();
					tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
					tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
					tTemporary.Value = tNextValue;
                }
                NWDConstants.GUIRedButtonEnd();
                tY = tY + NWDConstants.kFieldMarge + NWDConstants.kMiniButtonStyle.fixedHeight;
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