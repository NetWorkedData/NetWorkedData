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

using System;
using System.Text;
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
    public class NWEDataTypeMask : IComparable
    {
        //-------------------------------------------------------------------------------------------------------------
        public long Value;
        public string Name;
        public string Representation;
        public const string SQLType = "int";
        public bool Overridable = true;
        //-------------------------------------------------------------------------------------------------------------
        protected static GUIStyle BinaryLabel;
        //-------------------------------------------------------------------------------------------------------------
        public NWEDataTypeMask()
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
            return EditorStyles.layerMaskField.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "", GUIStyle sStyle = null)
        {
            NWEDataTypeMask tTemporary = new NWEDataTypeMask();
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
        public override bool Equals(object obj)
        {
            var otherValue = obj as NWEDataTypeMask;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CompareTo(object sOther) => Value.CompareTo(((NWEDataTypeMask)sOther).Value);
        //-------------------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWEDataTypeMask operator |(NWEDataTypeMask sA, NWEDataTypeMask sB)
        //{
        //    NWEDataTypeMask rReturn = Activator.CreateInstance(typeof(NWEDataTypeMask)) as NWEDataTypeMask;
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWEDataTypeMask operator +(NWEDataTypeMask obj1, NWEDataTypeMask obj2)
        //{
        //    NWEDataTypeMask rReturn = Activator.CreateInstance(typeof(NWEDataTypeMask)) as NWEDataTypeMask;
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWEDataTypeMaskGeneric<K> : NWEDataTypeMask where K : NWEDataTypeMaskGeneric<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        static readonly Dictionary<long, K> kList = new Dictionary<long, K>();
        static public K Nothing = new K();
        static public K Everythings = new K();
        //-------------------------------------------------------------------------------------------------------------
        static NWEDataTypeMaskGeneric()
        {
            Nothing.Value = 0;
            Nothing.Name = "Nothing";
            Nothing.Representation = Nothing.Name;
            Nothing.Overridable = false;
            Everythings.Value = long.MaxValue;
            Everythings.Name = "Everythings";
            Everythings.Representation = Nothing.Name;
            Everythings.Overridable = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWEDataTypeMaskGeneric()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "", GUIStyle sStyle = null)
        {
            if (BinaryLabel == null)
            {
                BinaryLabel = new GUIStyle(EditorStyles.label);
                BinaryLabel.alignment = TextAnchor.MiddleRight;
                BinaryLabel.normal.textColor = Color.gray;
                BinaryLabel.fixedHeight = EditorStyles.layerMaskField.fixedHeight;
            }
            if (sStyle == null)
            {
                sStyle = EditorStyles.layerMaskField;
            }
            NWEDataTypeMaskGeneric<K> tTemporary = Activator.CreateInstance(typeof(K)) as K;
            tTemporary.Value = Value;
            //StringBuilder stringLog = new StringBuilder();
            //stringLog.AppendLine(sEntitled);
            //stringLog.AppendLine("Start with tTemporary.Value = " + tTemporary.Value);
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
                //kListString.Add(tName + " (" + Convert.ToString(tKeyPair.Value.Value, 2) + ")");
                kListString.Add(tName);
            }
            tSelection = kListIndex.IndexOf(Value);
            if (tSelection < 0)
            {
                tSelection = 0;
            }
            // transform to list for fictive mask
            int tTemporaryFlags = 0;
            //stringLog.AppendLine("init tTemporaryFlags = " + Convert.ToString(tTemporaryFlags, 2));
            for (int ti = 0; ti < kListString.Count; ti++)
            {
                if (tTemporary.ContainsMask(kListK[ti]))
                {
                    //stringLog.AppendLine("containts = " + kListK[ti].Name + "( " + Convert.ToString(kListK[ti].Value, 2)+" )");
                    tTemporaryFlags = tTemporaryFlags | (1 << ti);
                }
            }
            //stringLog.AppendLine("before field tTemporaryFlags = " + Convert.ToString(tTemporaryFlags, 2));
          
            //tTemporaryFlags = EditorGUI.MaskField(sPosition, new GUIContent(sEntitled, ConcatRepresentations()), tTemporaryFlags, kListString.ToArray(), sStyle);
            tTemporaryFlags = EditorGUI.MaskField(sPosition, sEntitled, tTemporaryFlags, kListString.ToArray(), sStyle);
            sPosition.y += EditorStyles.layerMaskField.fixedHeight;

            //stringLog.AppendLine("after field tTemporaryFlags = " + Convert.ToString(tTemporaryFlags, 2));

            tTemporary.Value = 0;
            for (int ti = 0; ti < kListString.Count; ti++)
            {
                if ((tTemporaryFlags & (1 << ti)) == (1 << ti))
                {
                    tTemporary.SetFlag(kListK[ti]);
                }
            }
            //StringBuilder tBinary = new StringBuilder();
            //string tB = Convert.ToString(tTemporary.Value, 2);
            //while (tBinary.Length<(32- tB.Length))
            //{
            //    tBinary.Append("0");
            //}
            //tBinary.Append(tB);
            //EditorGUI.LabelField(sPosition, " ", tBinary.ToString(), BinaryLabel);
            //sPosition.y += BinaryLabel.fixedHeight;
            //FAKE
            //stringLog.AppendLine("Finish with tTemporary.Value = " + tTemporary.Value);
            //Debug.Log(stringLog.ToString());
            return tTemporary;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        protected static K Add(int sBinaryIndex, string sName)
        {
            return Add(sBinaryIndex, sName, null, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K AddProtected(int sBinaryIndex, string sName)
        {
            return Add(sBinaryIndex, sName, null, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K Add(int sBinaryIndex, string sName, string sRepresentation)
        {
            return Add(sBinaryIndex, sName, sRepresentation, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K AddProtected(int sBinaryIndex, string sName, string sRepresentation)
        {
            return Add(sBinaryIndex, sName, sRepresentation, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K AddOverride(int sBinaryIndex, string sName, string sRepresentation)
        {
            return Add(sBinaryIndex, sName, sRepresentation, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K Add(int sBinaryIndex, string sName, string sRepresentation, bool sOverridable)
        {
            K rReturn;
            if (kList.ContainsKey(sBinaryIndex) == false)
            {
                rReturn = Activator.CreateInstance(typeof(K)) as K;
                rReturn.Value = 1 << sBinaryIndex;
                rReturn.Name = sName;
                rReturn.Representation = sRepresentation;
                rReturn.Overridable = sOverridable;
                kList.Add(sBinaryIndex, rReturn);
            }
            else
            {
                rReturn = kList[sBinaryIndex];
                if (rReturn.Overridable == true)
                {
                    rReturn.Name = sName;
                    rReturn.Representation = sRepresentation;
                }
            }
            return rReturn as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string ConcatRepresentations()
        {
            return string.Join(",", Representations());
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] Representations()
        {
            List<string> rResult = new List<string>();
            foreach (KeyValuePair<long, K> tValuePair in kList)
            {
                if (this.ContainsMask(tValuePair.Value))
                {
                    rResult.Add(tValuePair.Value.Representation);
                }
            }
            return rResult.ToArray();
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
        public static K operator |(NWEDataTypeMaskGeneric<K> sA, NWEDataTypeMaskGeneric<K> sB)
        {
            K rReturn = Activator.CreateInstance(typeof(K)) as K;
            rReturn.Value = sA.Value | sB.Value;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K ToogleFlag(NWEDataTypeMaskGeneric<K> sA, NWEDataTypeMaskGeneric<K> sB)
        {
            K rReturn = Activator.CreateInstance(typeof(K)) as K;
            rReturn.Value = sA.Value ^ sB.Value;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IntersectsMask(NWEDataTypeMaskGeneric<K> sMask)
        {
            return (Value & sMask.Value) != 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainsMask(NWEDataTypeMaskGeneric<K> sMask)
        {
            long tResult = Value & sMask.Value;
            //Debug.Log(Convert.ToString(Value, 2) + " contains " + Convert.ToString(sMask.Value, 2) + "? => " + Convert.ToString(tResult, 2));
            return (Value & sMask.Value) == sMask.Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ExcludesMask(NWEDataTypeMaskGeneric<K> sMask)
        {
            return (Value & (~sMask.Value)) == Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFlag(NWEDataTypeMaskGeneric<K> sFlag)
        {
            Value = Value | sFlag.Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UnsetFlag(NWEDataTypeMaskGeneric<K> sFlag)
        {
            Value = Value & (~sFlag.Value);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================