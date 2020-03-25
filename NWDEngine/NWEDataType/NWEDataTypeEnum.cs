

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;

using System.Globalization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Can create basic class working like dynamical enum when generic
    /// </summary>
    [Serializable]
    public class NWEDataTypeEnum : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The value of the static enum instance
        /// </summary>
        public long Value;
        /// <summary>
        /// The name of this enum
        /// </summary>
        public string Name;
        /// <summary>
        /// R-The description of this enum
        /// </summary>
        public string Representation;
        /// <summary>
        /// The type of SQL column to reccord this data linearization
        /// </summary>
        public const string SQLType = "int";
        /// <summary>
        /// Define if this instance can be override by another one. Default is true.
        /// </summary>
        public bool Overridable = true;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Basic constructor
        /// </summary>
        public NWEDataTypeEnum()
        {
            Value = 0;
            Name = null;
            Representation = null;
            Overridable = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Convert enum to string with value as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Convert value of enum instance to long
        /// </summary>
        /// <returns></returns>
        public long ToLong()
        {
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Convert value of enum instance to long
        /// </summary>
        /// <returns></returns>
        public long GetLong()
        {
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set long as value of enum instance
        /// </summary>
        /// <param name="sLong"></param>
        public void SetLong(long sLong)
        {
            Value = sLong;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// If an error is detected this property return true
        /// </summary>
        private bool InError = false;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the height of controlfield in the editor inspector
        /// </summary>
        /// <returns></returns>
        public virtual float ControlFieldHeight()
        {
            return EditorStyles.popup.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the controlfield instance for the inspector
        /// </summary>
        /// <param name="sPosition"></param>
        /// <param name="sEntitled"></param>
        /// <param name="sDisabled"></param>
        /// <param name="sTooltips"></param>
        /// <returns></returns>
        public virtual NWEDataTypeEnum ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "")
        {
            NWEDataTypeEnum tTemporary = new NWEDataTypeEnum();
            tTemporary.Value = Value;
            //FAKE
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return if is in error
        /// </summary>
        /// <returns></returns>
        public virtual bool IsInError()
        {
            return InError;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Analyze if is in error and return the result
        /// </summary>
        /// <returns></returns>
        public virtual bool ErrorAnalyze()
        {
            InError = false;
            return InError;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the default value. 0 by default.
        /// </summary>
        public virtual void Default()
        {
            Value = 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Flush the value and reset to 0 (not th default).
        /// </summary>
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
    /// <summary>
    /// Can create generic class working like dynamical enum
    /// </summary>
    /// <typeparam name="K"></typeparam>
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
        public override NWEDataTypeEnum ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "")
        {
            NWEDataTypeEnum tTemporary = new NWEDataTypeEnum();
            tTemporary.Value = Value;
            List<long> kListIndex = new List<long>();
            List<K> kListK = new List<K>();
            List<string> kListString = new List<string>();
            int tSelection = -1;
            foreach (KeyValuePair<long, K> tKeyPair in kList)
            {
                if (tKeyPair.Key >= 0)
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
                }
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
        /// <summary>
        /// Return static instance for this long value
        /// </summary>
        /// <param name="sID"></param>
        /// <returns></returns>
        public static K GetForValue(long sID, K sDefault)
        {
            K rReturn = sDefault;
            if (kList.ContainsKey(sID))
            {
                rReturn = kList[sID];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetForValue(long sID)
        {
            return GetForValue(sID, None);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return static instance for this string value
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static K GetForValue(string sName, K sDefault)
        {
            K rReturn = sDefault;
            if (kStringList.ContainsKey(sName))
            {
                rReturn = kStringList[sName];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetForValue(string sName)
        {
            return GetForValue(sName, None);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// return all keys in of string
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllValues()
        {
            return kStringList.Keys.ToArray();
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
                K rReturnA = null;
                K rReturnB = null;
                if (kList.ContainsKey(sID))
                    {
                        rReturnA = kList[sID];
                        if (rReturnA.Overridable == true)
                        {
                            kList.Remove(rReturn.Value);
                            kStringList.Remove(rReturn.Name);
                        }
                    }
                if (kStringList.ContainsKey(sName))
                    {
                        rReturnB = kStringList[sName];
                        if (rReturnB.Overridable == true)
                        {
                            kList.Remove(rReturnB.Value);
                            kStringList.Remove(rReturnB.Name);
                        }
                    }
                if (rReturnA!=null && rReturnB!=null)
                    {
                        if (rReturnA.Overridable == true && rReturnB.Overridable == true)
                            {
                                Add(sID, sName, sRepresentation, sOverridable);
                            }
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