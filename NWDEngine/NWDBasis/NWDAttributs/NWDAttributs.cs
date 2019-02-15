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
    /// <summary>
    /// NWDNotVersionnableAttribute excluded the NWDBasis<K> from the version systeme restriction. Never use in custom Class!
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDFlagsEnumAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDNotVersionnableAttribute excluded the NWDBasis<K> from the version systeme restriction. Never use in custom Class!
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class NWDNotVersionnableAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDInternalKeyNotEditableAttribute forbidden the edition of the InternalKey in the editor.
    /// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class NWDInternalKeyNotEditableAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDInternalKeyNotEditableAttribute forbidden the edition of the InternalKey in the editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class NWDForceSecureDataAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDEntitledAttribute custom toolstip and entitlement for property. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDNotWorking : Attribute
    {
        public string ToolsTips = string.Empty;
        public NWDNotWorking()
        {
            this.ToolsTips = "[NOT WORKING]";
        }
        public NWDNotWorking(string sToolsTips = "[NOT WORKING]")
        {
            this.ToolsTips = sToolsTips;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NWDAliasMethod : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Alias = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        private static Dictionary<Type, Dictionary<string, MethodInfo>> kCache = new Dictionary<Type, Dictionary<string, MethodInfo>>();
        //-------------------------------------------------------------------------------------------------------------
        public NWDAliasMethod(string sAlias)
        {
            this.Alias = sAlias;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static MethodInfo GetMethod(Type sType, string sAlias, BindingFlags sFlags)
        {
            MethodInfo rReturn = null;
            bool tAllreadyCache = false;
            if (kCache.ContainsKey(sType))
            {
                if (kCache[sType].ContainsKey(sAlias))
                {
                    tAllreadyCache = true;
                    rReturn = kCache[sType][sAlias];
                }
            }
            if (tAllreadyCache == false)
            {
                foreach (MethodInfo tProp in sType.GetMethods(sFlags))
                {
                    foreach (NWDAliasMethod tReference in tProp.GetCustomAttributes(typeof(NWDAliasMethod), true))
                    {
                        if (tReference.Alias == sAlias)
                        {
                            rReturn = tProp;
                        }
                    }
                }
                if (kCache.ContainsKey(sType) ==false)
                {
                    kCache.Add(sType, new Dictionary<string, MethodInfo>());
                }
                if (kCache[sType].ContainsKey(sAlias) == false)
                {
                    kCache[sType].Add(sAlias, rReturn);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static MethodInfo GetMethodPublicInstance(Type sType, string sAlias)
        {
            return GetMethod(sType, sAlias,BindingFlags.Public | BindingFlags.Instance);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static MethodInfo GetMethodPublicStaticFlattenHierarchy(Type sType, string sAlias)
        {
            return GetMethod(sType, sAlias, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool InvokeClassMethod(Type sType, string sAlias, object sSender = null, object[] sParameter = null)
        {
            bool rReturn = true;
            MethodInfo tMethodInfo = GetMethod(sType, sAlias, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(sSender, sParameter);
            }
            else
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDAlias : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string FindAliasName(Type sType, string sAlias)
        {
            string rReturn = sAlias;
            foreach (PropertyInfo tProp in sType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (NWDAlias tReference in tProp.GetCustomAttributes(typeof(NWDAlias), true))
                {
                    if (tReference.Alias == sAlias)
                    {
                        rReturn = tProp.Name;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // use in NWDBasis with FindAliasName();
        public string Alias = string.Empty;
        public NWDAlias(string sAlias)
        {
            this.Alias = sAlias;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDEntitledAttribute custom toolstip and entitlement for property. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInDevelopment : Attribute
    {
        public string ToolsTips = string.Empty;
        public NWDInDevelopment()
        {
            this.ToolsTips = "[IN DEV]";
        }
        public NWDInDevelopment(string sToolsTips = "[IN DEV]")
        {
            this.ToolsTips = sToolsTips;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDEntitledAttribute custom toolstip and entitlement for property. 
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDEntitledAttribute : Attribute
    {
        public string Entitled = string.Empty;
        public string ToolsTips = string.Empty;

        public NWDEntitledAttribute(string sEntitled)
        {
            this.Entitled = sEntitled;
        }

        public NWDEntitledAttribute(string sEntitled, string sToolsTips)
        {
            this.Entitled = sEntitled;
            this.ToolsTips = sToolsTips;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDTooltipsAttribute custom toolstip and entitlement. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDTooltipsAttribute : Attribute
    {
        public string ToolsTips = string.Empty;
        public NWDTooltipsAttribute(string sToolsTips)
        {
            this.ToolsTips = sToolsTips;
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDOrderAttribute : Attribute
    {
        public int Order = 0;
        public NWDOrderAttribute(int sOrder)
        {
            this.Order = sOrder;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDHeaderAttribute add an header in inspector before this property.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDHeaderAttribute : Attribute
    {
        public string mHeader;

        public NWDHeaderAttribute(string sHeader)
        {
            this.mHeader = sHeader;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDSpaceAttribute add a space before this property.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDSpaceAttribute : Attribute
    {
        public float mSize;

        public NWDSpaceAttribute()
        {
            this.mSize = 10.0f;
        }

        public NWDSpaceAttribute(float sSize)
        {
            this.mSize = sSize;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGroupResetAttribute create a group befaore this property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDGroupResetAttribute : Attribute
    {
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGroupStartAttribute create a group befaore this property.
    /// </summary>
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //public class NWDInspectorOrderAttribute : Attribute
    //{
    //    public string mToolsTips = string.Empty;
    //    public string mGroupOrder = string.Empty;
    //    public string mOrder = string.Empty;
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDInspectorGroupOrderAttribute : Attribute
    {
        public string mGroupName = string.Empty;
        public int mGroupOrder = 0;
        public string mEntitled = string.Empty;
        public string mToolsTips = string.Empty; 
        public NWDInspectorGroupOrderAttribute(string sGroupName, int sGroupOrder = 0, string sToolsTips = "", string sEntitled = "")
        {
            this.mGroupName = sGroupName;
            this.mGroupOrder = sGroupOrder;
            this.mToolsTips = sToolsTips;
            this.mEntitled = sEntitled;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGroupStartAttribute create a group befaore this property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDGroupStartAttribute : Attribute
    {
        public string mGroupName = string.Empty;
        public string mToolsTips = string.Empty;
        public bool mBoldHeader;
        public bool mReducible;
        public bool mOpen;
        public NWDGroupStartAttribute Parent;
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
        public NWDGroupStartAttribute(string sGroupName, bool sBoldHeader = false, bool sReducible = true, bool sOpen = true)
        {
            this.mGroupName = sGroupName;
            this.mBoldHeader = sBoldHeader;
            this.mOpen = sOpen;
            this.mReducible = sReducible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDGroupStartAttribute(string sGroupName, string sToolsTips, bool sBoldHeader = false, bool sReducible = true, bool sOpen = true)
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
    /// <summary>
    /// NWDIfType is used only in NWDIfAttribute.
    /// </summary>
    public enum NWDIfType
    {
        Equal,
        Range,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDIfAttribute can hidde the next property if specific condition is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDNotVisible : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDIfAttribute can hidde the next property if specific condition is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDHidden : Attribute
    {
        // = NWDNotVisible
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDIfAttribute can hidde the next property if specific condition is true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDIfAttribute : Attribute
    {
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
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDIfAttribute(string sPropertyName, Enum sValue, bool sVisible = true)
        //{
        //    this.mPropertyName = sPropertyName;
        //    this.mValues = new string[] { sValue.ToString() };
        //    this.TypeOfCompare = NWDIfType.Equal;
        //    this.mVisible = sVisible;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDIfAttribute(string sPropertyName, Enum[] sValues, bool sVisible = true)
        //{
        //    this.mPropertyName = sPropertyName;
        //    List<string> tValues = new List<string>();
        //    foreach (Enum ti in sValues)
        //    {
        //        tValues.Add(ti.ToString());
        //    }
        //    this.mValues = tValues.ToArray();
        //    this.TypeOfCompare = NWDIfType.Equal;
        //    this.mVisible = sVisible;
        //}
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
    /// <summary>
    /// NWDGroupEndAttribute close the NWDGroupStartAttribute befaore the next property.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDGroupEndAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGroupSeparatorAttribute must be use after NWDGroupEndAttribute. It draw separator line.
    /// </summary>
 //   [Obsolete]
	//[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    //public class NWDGroupSeparatorAttribute : Attribute
    //{
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDSeparatorAttribute draw separator line before property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDSeparatorAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDNotEditableAttribute disable property edition in editor.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDNotEditableAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDFloatSliderAttribute draw slider for float property.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDFloatSliderAttribute : Attribute
    {
        public float mMin;
        public float mMax;

        public NWDFloatSliderAttribute(float sMin, float sMax)
        {
            this.mMin = sMin;
            this.mMax = sMax;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDIntSliderAttribute draw slider for int property.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDIntSliderAttribute : Attribute
    {
        public int mMin;
        public int mMax;

        public NWDIntSliderAttribute(int sMin, int sMax)
        {
            this.mMin = sMin;
            this.mMax = sMax;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDEnumAttribute draw popmenu for int property. It will be obsolete and replace by enum type!
    /// </summary>
    //[Obsolete("Use an enum")]
    [Obsolete]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDEnumAttribute : Attribute
    {
        public int[] mEnumInt;
        public string[] mEnumString;

        public NWDEnumAttribute(int[] sEnumInt, string[] sEnumString)
        {
            this.mEnumInt = sEnumInt;
            this.mEnumString = sEnumString;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDEnumAttribute draw popmenu for string property. It will be obsolete and replace by enum type!
    /// </summary>
    [Obsolete("Use an enum")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDEnumStringAttribute : Attribute
    {
        public string[] mEnumString;

        public NWDEnumStringAttribute(string[] sEnumString)
        {
            this.mEnumString = sEnumString;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDLongStringAttribute draw a bigger textfield in the editor for this property
    /// </summary>
    //[Obsolete("Use Long string type")]
    [Obsolete]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDLongStringAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDVeryLongStringAttribute draw a bigger textfield in the editor for this property
    /// </summary>
    //[Obsolete("Use Long string type")]
    [Obsolete]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDVeryLongStringAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAddonAttribute : Attribute
    {
        //public string AddonGroup;
        public NWDAddonAttribute()
        {
            //this.AddonGroup = "";
        }
        //public NWDAddonAttribute(string sAddonGroup = "")
        //{
        //    this.AddonGroup = sAddonGroup;
        //}
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================