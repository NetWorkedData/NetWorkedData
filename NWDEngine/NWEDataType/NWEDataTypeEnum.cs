

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
using SQLite4Unity3d;
using System.Globalization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWEDataTypeEnum : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        public long Value;
        public string Name;
        public string Representation;
        public const string SQLType = "int";
        public bool Overridable = true;
        //-------------------------------------------------------------------------------------------------------------
        public NWEDataTypeEnum()
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
#if UNITY_EDITOR
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
            NWEDataTypeEnum tTemporary = new NWEDataTypeEnum();
            tTemporary.Value = Value;
            //FAKE
            return tTemporary;
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
#endif
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
        public static bool operator ==(NWEDataTypeEnum sA, NWEDataTypeEnum sB)
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
        public static bool operator !=(NWEDataTypeEnum sA, NWEDataTypeEnum sB)
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
            var otherValue = obj as NWEDataTypeEnum;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CompareTo(object sOther) => Value.CompareTo(((NWEDataTypeEnum)sOther).Value);
        //-------------------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWEDataTypeEnumGeneric<K> : NWEDataTypeEnum where K : NWEDataTypeEnumGeneric<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        static readonly Dictionary<long, K> kList = new Dictionary<long, K>();
        static readonly Dictionary<string, K> kStringList = new Dictionary<string, K>();
        public static K None = Add(0, "None", "None", false);
        //-------------------------------------------------------------------------------------------------------------
        public NWEDataTypeEnumGeneric()
        {
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public NWEDataTypeEnumGeneric(long sValue, string sName)
        //{
        //    Value = sValue;
        //    Name = sName;
        //}
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "")
        {
            NWEDataTypeEnum tTemporary = new NWEDataTypeEnum();
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
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static K GetForValue(long sID)
        {
            return kList[sID];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetForValue(string sName)
        {
            return kStringList[sName];
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
            K rReturn = None;
            if (kList.ContainsKey(sID) == false && kStringList.ContainsKey(sName) == false )
            {
                rReturn = Activator.CreateInstance(typeof(K)) as K;
                rReturn.Value = sID;
                rReturn.Name = sName;
                rReturn.Representation = sRepresentation;
                rReturn.Overridable = sOverridable;
                kList.Add(sID, rReturn);
                kStringList.Add(sName, rReturn);
            }
            else
            {
                K rReturnA = kList[sID];
                K rReturnB = kStringList[sName];
                if (rReturnA.Overridable == true)
                {
                    kList.Remove(rReturn.Value);
                    kStringList.Remove(rReturn.Name);
                }
                if (rReturnB.Overridable == true)
                {
                    kList.Remove(rReturnB.Value);
                    kStringList.Remove(rReturnB.Name);
                }
                if (rReturnA.Overridable == true && rReturnB.Overridable == true)
                {
                    Add(sID, sName, sRepresentation, sOverridable);
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
#if UNITY_EDITOR
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomPropertyDrawer(typeof(NWEDataTypeEnumDrawer<>))]
    public class NWEDataTypeEnumDrawer<K> : PropertyDrawer where K : NWEDataTypeEnumGeneric<K>, new()
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            K tTarget = fieldInfo.GetValue(property.serializedObject.targetObject) as K;
            EditorGUI.BeginChangeCheck();
            NWEDataTypeEnum tResult = tTarget.ControlField(position, property.displayName, false, property.tooltip) as NWEDataTypeEnum;
            if (EditorGUI.EndChangeCheck())
            {
                K tTargetFinal = NWEDataTypeEnumGeneric<K>.GetForValue(tResult.Value);
                fieldInfo.SetValue(property.serializedObject.targetObject, tTargetFinal);
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
            EditorGUI.EndProperty();
        }
    }
#endif
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================