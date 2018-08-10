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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NWDFlagsEnumAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDNotVersionnableAttribute excluded the NWDBasis<K> from the version systeme restriction. Never use in custom Class!
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NWDNotVersionnableAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDInternalKeyNotEditableAttribute forbidden the edition of the InternalKey in the editor.
    /// </summary>
	[AttributeUsage (AttributeTargets.Class, AllowMultiple = true)]
	public class NWDInternalKeyNotEditableAttribute : Attribute
	{
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDEntitledAttribute custom toolstip and entitlement for property. 
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDEntitledAttribute : Attribute
	{
		public string Entitled = "";
		public string ToolsTips = "";

		public NWDEntitledAttribute (string sEntitled)
		{
			this.Entitled = sEntitled;
		}

		public NWDEntitledAttribute (string sEntitled, string sToolsTips)
		{
			this.Entitled = sEntitled;
			this.ToolsTips = sToolsTips;
		}
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDTooltipsAttribute custom toolstip and entitlement. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NWDTooltipsAttribute : Attribute
    {
        public string ToolsTips = "";
        public NWDTooltipsAttribute(string sToolsTips)
        {
            this.ToolsTips = sToolsTips;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDHeaderAttribute add an header in inspector before this property.
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDHeaderAttribute : Attribute
	{
		public string mHeader;

		public NWDHeaderAttribute (string sHeader)
		{
			this.mHeader = sHeader;
		}
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDSpaceAttribute add a space before this property.
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDSpaceAttribute : Attribute
	{
		public float mSize;

		public NWDSpaceAttribute ()
		{
			this.mSize = 10.0f;
		}

		public NWDSpaceAttribute (float sSize)
		{
			this.mSize = sSize;
		}
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGroupResetAttribute create a group befaore this property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NWDGroupResetAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGroupStartAttribute create a group befaore this property.
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDGroupStartAttribute : Attribute
	{
        public string mGroupName = "";
        public string mToolsTips = "";
		public bool mBoldHeader;
		public bool mReducible;
        public bool mOpen;
		public NWDGroupStartAttribute Parent;
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		private string Key (String sClassName)
		{
			return sClassName + mGroupName;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool IsDrawable (String sClassName)
		{
			bool rReturn = GetDrawable (sClassName);
			if (Parent != null) {
				rReturn = IsDrawable (sClassName);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetDrawable (String sClassName, bool sBool)
		{
			string tKey = Key (sClassName);
			EditorPrefs.HasKey (tKey);
			EditorPrefs.SetBool (tKey, sBool);
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool GetDrawable (String sClassName)
		{
			string tKey = Key (sClassName);
			EditorPrefs.HasKey (tKey);
			return EditorPrefs.GetBool (tKey);
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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class NWDIfAttribute : Attribute
	{
		public string mPropertyName;
		public string[] mValues;
        public NWDIfType TypeOfCompare = NWDIfType.Equal;
        //		public bool mVisible;
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
                    List<string> tList = new List<string>(mValues);
                    object tObject = tInfo.GetValue(sObject, null);
                    string tV = tObject.ToString();
                    if (tObject.GetType().IsEnum)
                    {
                        int tvv = (int)tObject;
                        tV = tvv.ToString();
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
                    object tObject = tInfo.GetValue(sObject, null);

                    float tValue = 0;
                    float.TryParse(tObject.ToString(),out tValue);

                    float tValueMin = 0;
                    float.TryParse(mValues[0], out tValueMin);

                    float tValueMax = 0;
                    float.TryParse(mValues[1], out tValueMax);

                    if (tValue<tValueMin || tValue>tValueMax)
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
        public NWDIfAttribute (string sPropertyName, string sValue)//, bool sVisible = true)
		{
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValue };
            this.TypeOfCompare = NWDIfType.Equal;
            //          this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, string[] sValues)//, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = sValues;
            this.TypeOfCompare = NWDIfType.Equal;
            //          this.mVisible = sVisible;
        }
		//-------------------------------------------------------------------------------------------------------------
		public NWDIfAttribute (string sPropertyName, bool sValue)//, bool sVisible = true)
		{
			this.mPropertyName = sPropertyName;
            this.mValues = new string[]{sValue.ToString()};
            this.TypeOfCompare = NWDIfType.Equal;
			//			this.mVisible = sVisible;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDIfAttribute (string sPropertyName, int sValue)//, bool sVisible = true)
		{
			this.mPropertyName = sPropertyName;
            this.mValues = new string[]{sValue.ToString()};
            this.TypeOfCompare = NWDIfType.Equal;
			//			this.mVisible = sVisible;
		}
        //-------------------------------------------------------------------------------------------------------------
		public NWDIfAttribute (string sPropertyName, int[] sValues)//, bool sVisible = true)
		{
			this.mPropertyName = sPropertyName;
			List<string> tValues = new List<string> ();
			foreach (int ti in sValues) {
				tValues.Add (ti.ToString ());
			}
            this.mValues = tValues.ToArray ();
            this.TypeOfCompare = NWDIfType.Equal;
			//			this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, int sValueMin, int sValueMax)//, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValueMin.ToString(), sValueMax.ToString()};
            this.TypeOfCompare = NWDIfType.Range;
            //          this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, float sValue)//, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValue.ToString() };
            this.TypeOfCompare = NWDIfType.Equal;
            //          this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, float[] sValues)//, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            List<string> tValues = new List<string>();
            foreach (int ti in sValues)
            {
                tValues.Add(ti.ToString());
            }
            this.mValues = tValues.ToArray();
            this.TypeOfCompare = NWDIfType.Equal;
            //          this.mVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIfAttribute(string sPropertyName, float sValueMin, float sValueMax)//, bool sVisible = true)
        {
            this.mPropertyName = sPropertyName;
            this.mValues = new string[] { sValueMin.ToString(), sValueMax.ToString() };
            this.TypeOfCompare = NWDIfType.Range;
            //          this.mVisible = sVisible;
        }
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGroupEndAttribute close the NWDGroupStartAttribute befaore the next property.
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDGroupEndAttribute : Attribute
	{
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGroupSeparatorAttribute must be use after NWDGroupEndAttribute. It draw separator line.
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDGroupSeparatorAttribute : Attribute
	{
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDSeparatorAttribute draw separator line before property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NWDSeparatorAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDNotEditableAttribute disable property edition in editor.
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDNotEditableAttribute : Attribute
	{
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDFloatSliderAttribute draw slider for float property.
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDFloatSliderAttribute : Attribute
	{
		public float mMin;
		public float mMax;

		public NWDFloatSliderAttribute (float sMin, float sMax)
		{
			this.mMin = sMin;
			this.mMax = sMax;
		}
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDIntSliderAttribute draw slider for int property.
    /// </summary>
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDIntSliderAttribute : Attribute
	{
		public int mMin;
		public int mMax;

		public NWDIntSliderAttribute (int sMin, int sMax)
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
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDEnumAttribute : Attribute
	{
		public int[] mEnumInt;
		public string[] mEnumString;

		public NWDEnumAttribute (int[] sEnumInt, string[] sEnumString)
		{
			this.mEnumInt = sEnumInt;
			this.mEnumString = sEnumString;
		}
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDEnumAttribute draw popmenu for string property. It will be obsolete and replace by enum type!
    /// </summary>
    //[Obsolete("Use an enum")]
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	public class NWDEnumStringAttribute : Attribute
	{
		public string[] mEnumString;

		public NWDEnumStringAttribute (string[] sEnumString)
		{
			this.mEnumString = sEnumString;
		}
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDLongStringAttribute draw a bigger textfield in the editor for this property
    /// </summary>
    //[Obsolete("Use Long string type")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NWDLongStringAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDVeryLongStringAttribute draw a bigger textfield in the editor for this property
    /// </summary>
    //[Obsolete("Use Long string type")]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NWDVeryLongStringAttribute : Attribute
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================