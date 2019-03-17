//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInspectorHeaderAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string mHeader;
        //-------------------------------------------------------------------------------------------------------------
        public NWDInspectorHeaderAttribute(string sHeader)
        {
            this.mHeader = sHeader;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInspectorGroupResetAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInspectorGroupStartAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string mGroupName = string.Empty;
        public string mToolsTips = string.Empty;
        public bool mBoldHeader;
        public bool mReducible;
        public bool mOpen;
        public NWDInspectorGroupStartAttribute Parent;
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        private string Key(String sClassName)
        {
            return sClassName + mGroupName;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsDrawable(String sClassName)
        {
            bool rReturn = GetDrawable(sClassName);
            if (Parent != null)
            {
                rReturn = IsDrawable(sClassName);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDrawable(String sClassName, bool sBool)
        {
            string tKey = Key(sClassName);
            EditorPrefs.HasKey(tKey);
            EditorPrefs.SetBool(tKey, sBool);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetDrawable(String sClassName)
        {
            string tKey = Key(sClassName);
            EditorPrefs.HasKey(tKey);
            return EditorPrefs.GetBool(tKey);
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public NWDInspectorGroupStartAttribute(string sGroupName, bool sBoldHeader = false, bool sReducible = true, bool sOpen = true)
        {
            this.mGroupName = sGroupName;
            this.mBoldHeader = sBoldHeader;
            this.mOpen = sOpen;
            this.mReducible = sReducible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDInspectorGroupStartAttribute(string sGroupName, string sToolsTips, bool sBoldHeader = false, bool sReducible = true, bool sOpen = true)
        {
            this.mGroupName = sGroupName;
            this.mToolsTips = sToolsTips;
            this.mBoldHeader = sBoldHeader;
            this.mOpen = sOpen;
            this.mReducible = sReducible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public GUIContent Content()
        {
            return new GUIContent(mGroupName, mToolsTips);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInspectorGroupEndAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInspectorGroupOrderAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string mGroupName = string.Empty;
        public int mGroupOrder = 0;
        public string mEntitled = string.Empty;
        public string mToolsTips = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public NWDInspectorGroupOrderAttribute(string sGroupName, int sGroupOrder = 0, string sToolsTips = "", string sEntitled = "")
        {
            this.mGroupName = sGroupName;
            this.mGroupOrder = sGroupOrder;
            this.mToolsTips = sToolsTips;
            this.mEntitled = sEntitled;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInspectorSeparatorAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDNotEditableAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDNotVisible : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDHidden : Attribute
    {
        // = NWDNotVisible
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDNotWorking : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string ToolsTips = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public NWDNotWorking()
        {
            this.ToolsTips = "[NOT WORKING]";
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDNotWorking(string sToolsTips = "[NOT WORKING]")
        {
            this.ToolsTips = sToolsTips;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDCertified : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDCertified()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInDevelopment : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string ToolsTips = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public NWDInDevelopment()
        {
            this.ToolsTips = "[IN DEV]";
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDInDevelopment(string sToolsTips = "[IN DEV]")
        {
            this.ToolsTips = sToolsTips;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInformationAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string mText = string.Empty;
        public string mToolsTips = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public NWDInformationAttribute(string sText)
        {
            mText = sText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public GUIContent Content()
        {
            return new GUIContent(mText, mToolsTips);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDSpaceAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public float mSize;
        //-------------------------------------------------------------------------------------------------------------
        public NWDSpaceAttribute()
        {
            this.mSize = 10.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDSpaceAttribute(float sSize)
        {
            this.mSize = sSize;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDEntitledAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Entitled = string.Empty;
        public string ToolsTips = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public NWDEntitledAttribute(string sEntitled)
        {
            this.Entitled = sEntitled;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDEntitledAttribute(string sEntitled, string sToolsTips)
        {
            this.Entitled = sEntitled;
            this.ToolsTips = sToolsTips;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDTooltipsAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string ToolsTips = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public NWDTooltipsAttribute(string sToolsTips)
        {
            this.ToolsTips = sToolsTips;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDOrderAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public int Order = 0;
        //-------------------------------------------------------------------------------------------------------------
        public NWDOrderAttribute(int sOrder)
        {
            this.Order = sOrder;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDIfType
    {
        Equal,
        Range,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDIfAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string mPropertyName;
        public string[] mValues;
        public NWDIfType TypeOfCompare = NWDIfType.Equal;
        public bool mVisible;
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public bool IsDrawable(System.Object sObject)
        {
            bool rReturn = true;
            if (TypeOfCompare == NWDIfType.Equal)
            {
                PropertyInfo tInfo = sObject.GetType().GetProperty(mPropertyName, BindingFlags.Public | BindingFlags.Instance);
                if (tInfo != null)
                {
                    //Debug.Log("analyze if " + mPropertyName + " is equal...");
                    List<string> tList = new List<string>(mValues);
                    object tObject = tInfo.GetValue(sObject, null);
                    string tV = tObject.ToString();

                    if (tObject.GetType().IsEnum)
                    {
                        int tvv = (int)tObject;
                        tV = tvv.ToString();
                        //Debug.Log("analyze if " + mPropertyName + " value " + tV + " equal in " + string.Join(",", tList));
                    }
                    else
                    {
                        //Debug.Log("analyze if " + mPropertyName + " value " + tV + " equal in " + string.Join(",", tList));
                    }
                    if (!tList.Contains(tV))
                    {
                        rReturn = false;
                    }
                }
            }
            else if (TypeOfCompare == NWDIfType.Range)
            {
                PropertyInfo tInfo = sObject.GetType().GetProperty(mPropertyName, BindingFlags.Public | BindingFlags.Instance);
                if (tInfo != null)
                {
                    //Debug.Log("analyze if " + mPropertyName + " is in Range...");
                    object tObject = tInfo.GetValue(sObject, null);

                    float tValue = 0;
                    float.TryParse(tObject.ToString(), out tValue);

                    float tValueMin = 0;
                    float.TryParse(mValues[0], out tValueMin);

                    float tValueMax = 0;
                    float.TryParse(mValues[1], out tValueMax);

                    if (tValue < tValueMin || tValue > tValueMax)
                    {
                        rReturn = false;
                    }
                }
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, string sValue, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValue };
            this.TypeOfCompare = NWDIfType.Equal;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, string[] sValues, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = sValues;
            this.TypeOfCompare = NWDIfType.Equal;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, bool sValue, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValue.ToString() };
            this.TypeOfCompare = NWDIfType.Equal;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, int sValue, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValue.ToString() };
            this.TypeOfCompare = NWDIfType.Equal;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, int[] sValues, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            List<string> tValues = new List<string>();
            foreach (int ti in sValues)
            {
                tValues.Add(ti.ToString());
            }
            this.mValues = tValues.ToArray();
            this.TypeOfCompare = NWDIfType.Equal;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, int sValueMin, int sValueMax, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValueMin.ToString(), sValueMax.ToString() };
            this.TypeOfCompare = NWDIfType.Range;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, float sValue, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValue.ToString() };
            this.TypeOfCompare = NWDIfType.Equal;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, float[] sValues, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            List<string> tValues = new List<string>();
            foreach (int ti in sValues)
            {
                tValues.Add(ti.ToString());
            }
            this.mValues = tValues.ToArray();
            this.TypeOfCompare = NWDIfType.Equal;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, float sValueMin, float sValueMax, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValueMin.ToString(), sValueMax.ToString() };
            this.TypeOfCompare = NWDIfType.Range;
            this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDFloatSliderAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public float mMin;
        public float mMax;
        //-------------------------------------------------------------------------------------------------------------
        public NWDFloatSliderAttribute(float sMin, float sMax)
        {
            this.mMin = sMin;
            this.mMax = sMax;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDIntSliderAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public int mMin;
        public int mMax;
        //-------------------------------------------------------------------------------------------------------------
        public NWDIntSliderAttribute(int sMin, int sMax)
        {
            this.mMin = sMin;
            this.mMax = sMax;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDEnumStringAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string[] mEnumString;
        //-------------------------------------------------------------------------------------------------------------
        public NWDEnumStringAttribute(string[] sEnumString)
        {
            this.mEnumString = sEnumString;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDLongStringAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDVeryLongStringAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDFlagsEnumAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDFlagsEnumAttribute()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================