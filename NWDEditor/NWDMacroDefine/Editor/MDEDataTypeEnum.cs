//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-09-9 18:24:43
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	MacroDefineEditor for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
using System.Globalization;
using UnityEditor;
using UnityEditorInternal;

//=====================================================================================================================
namespace MacroDefineEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class MDEDataTypeEnum : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        public long Value;
        public string Name;
        public string Representation;
        public bool Overridable = true;
        //-------------------------------------------------------------------------------------------------------------
        public MDEDataTypeEnum()
        {
            Value = 0;
            Name = null;
            Representation = null;
            Overridable = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
        //-------------------------------------------------------------------------------------------------------------
        public long ToLong()
        {
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public long GetLong()
        {
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetLong(long sLong)
        {
            Value = sLong;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool InError = false;
        //-------------------------------------------------------------------------------------------------------------
        public virtual float ControlFieldHeight()
        {
            return EditorStyles.popup.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "")
        {
            MDEDataTypeEnum tTemporary = new MDEDataTypeEnum();
            tTemporary.Value = Value;
            //FAKE
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual List<string> StringValuesArray()
        {
            List<string> rList = new List<string>();
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool IsInError()
        {
            return InError;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool ErrorAnalyze()
        {
            InError = false;
            return InError;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void Default()
        {
            Value = 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Flush()
        {
            Value = 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string GetTitle()
        {
			return GetType().Name;
		}
        //-------------------------------------------------------------------------------------------------------------
        public static bool operator ==(MDEDataTypeEnum sA, MDEDataTypeEnum sB)
        {
            if ((object)sA == null && (object)sB == null)
            {
                return true;
            }
            else if ((object)sA == null)
            {
                return false;
            }
            else if ((object)sB == null)
            {
                return false;
            }
            else
            {
                var typeMatches = sA.GetType().Equals(sB.GetType());
                if (typeMatches)
                {
                    return sA.Value.Equals(sB.Value);
                }
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool operator !=(MDEDataTypeEnum sA, MDEDataTypeEnum sB)
        {
            if ((object)sA == null && (object)sB == null)
            {
                return false;
            }
            else if ((object)sA == null)
            {
                return true;
            }
            else if ((object)sB == null)
            {
                return true;
            }
            else
            {
                var typeMatches = sA.GetType().Equals(sB.GetType());
                if (typeMatches)
                {
                    return !sA.Value.Equals(sB.Value);
                }
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            var otherValue = obj as MDEDataTypeEnum;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CompareTo(object sOther) => Value.CompareTo(((MDEDataTypeEnum)sOther).Value);
        //-------------------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class MDEDataTypeEnumGeneric<K> : MDEDataTypeEnum where K : MDEDataTypeEnumGeneric<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        static readonly Dictionary<long, K> kList = new Dictionary<long, K>();
        private static string kTitle;
        //-------------------------------------------------------------------------------------------------------------
        public MDEDataTypeEnumGeneric()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override List<string> StringValuesArray()
        {
            List<string> rList = new List<string>();
            foreach (KeyValuePair<long, K> tKeyPair in kList)
            {
                string tName = string.Empty;
                if (tKeyPair.Value != null)
                {
                    tName = tKeyPair.Value.Name;
                }
                if (string.IsNullOrEmpty(tName) == false)
                {
                    rList.Add(tName);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "")
        {
            MDEDataTypeEnum tTemporary = new MDEDataTypeEnum();
            tTemporary.Value = Value;
            List<long> kListIndex = new List<long>();
            List<K> kListK = new List<K>();
            List<string> kListString = new List<string>();
            int tSelection = -1;
            foreach (KeyValuePair<long, K> tKeyPair in kList)
            {
                kListIndex.Add(tKeyPair.Key);
                kListK.Add(tKeyPair.Value);
                string tName = "error in " + tKeyPair.Key;
                if (tKeyPair.Value == null)
                {
                }
                else
                {
                    tName = tKeyPair.Value.Name;
                    if (string.IsNullOrEmpty(tName))
                    {
                        tName = "???";
                    }
                }
                kListString.Add(tName);
                //Debug.Log("enum listing :" + tKeyPair.Key + " : " + tName);
            }
            tSelection = kListIndex.IndexOf(Value);
            if (tSelection < 0)
            {
                tSelection = 0;
            }
            tSelection = EditorGUI.Popup(sPosition, sEntitled, tSelection, kListString.ToArray());
            tTemporary.Value = kListIndex[tSelection];
            //FAKE
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SetTitle(string sTitle)
        {
            kTitle = sTitle;
            return kTitle;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string GetTitle()
        {
            if (string.IsNullOrEmpty(kTitle))
			{
				kTitle = GetType().Name;
			}
            return kTitle;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetForValue(long sID)
        {
            return kList[sID];
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K AddNone()
        {
            return Add(0, MDEConstants.NONE, MDEConstants.NONE, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K Add(int sID, string sName)
        {
            return Add(sID, sName, null, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K AddProtected(int sID, string sName)
        {
            return Add(sID, sName, null, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K Add(int sID, string sName, string sRepresentation)
        {
            return Add(sID, sName, sRepresentation, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K AddProtected(int sID, string sName, string sRepresentation)
        {
            return Add(sID, sName, sRepresentation, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K AddOverride(int sID, string sName, string sRepresentation)
        {
            return Add(sID, sName, sRepresentation, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K Add(int sID, string sName, string sRepresentation, bool sOverridable)
        {
            sName = MDEMacroDefineEditor.UnixCleaner(sName);
            K rReturn;
            if (kList.ContainsKey(sID) == false)
            {
                rReturn = Activator.CreateInstance(typeof(K)) as K;
                rReturn.Value = sID;
                rReturn.Name = sName;
                rReturn.Representation = sRepresentation;
                rReturn.Overridable = sOverridable;
                kList.Add(sID, rReturn);
            }
            else
            {
                rReturn = kList[sID];
                if (rReturn.Overridable == true)
                {
                    rReturn.Name = sName;
                    rReturn.Representation = sRepresentation;
                }
            }
            return rReturn as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            if (kList.ContainsKey(Value) == true)
            {
                K tParent = kList[Value];
                if (string.IsNullOrEmpty(tParent.Representation))
                {
                    if (string.IsNullOrEmpty(tParent.Name))
                    {
                        return tParent.Value.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return tParent.Name;
                    }
                }
                else
                {
                    return tParent.Representation;
                }
            }
            else
            {
                return Value.ToString(CultureInfo.InvariantCulture);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomPropertyDrawer(typeof(MDEDataTypeEnumDrawer<>))]
    public class MDEDataTypeEnumDrawer<K> : PropertyDrawer where K : MDEDataTypeEnumGeneric<K>, new()
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            K tTarget = fieldInfo.GetValue(property.serializedObject.targetObject) as K;
            EditorGUI.BeginChangeCheck();
            MDEDataTypeEnum tResult = tTarget.ControlField(position, property.displayName, false, property.tooltip) as MDEDataTypeEnum;
            if (EditorGUI.EndChangeCheck())
            {
                K tTargetFinal = MDEDataTypeEnumGeneric<K>.GetForValue(tResult.Value);
                fieldInfo.SetValue(property.serializedObject.targetObject, tTargetFinal);
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
            EditorGUI.EndProperty();
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================