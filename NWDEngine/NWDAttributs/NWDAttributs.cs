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
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDNotVersionnableAttribute : Attribute
    {
    }
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Class, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDInternalKeyNotEditableAttribute : Attribute
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
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
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDTooltipsAttribute : Attribute
    {
        public string ToolsTips = "";
        public NWDTooltipsAttribute(string sToolsTips)
        {
            this.ToolsTips = sToolsTips;
        }
    }
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDHeaderAttribute : Attribute
	{
		public string mHeader;

		public NWDHeaderAttribute (string sHeader)
		{
			this.mHeader = sHeader;
		}
	}
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
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
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDGroupStartAttribute : Attribute
	{
		public string mGroupName;
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
		public NWDGroupStartAttribute (string sGroupName, bool sBoldHeader = false, bool sReducible = true, bool sOpen = true)
		{
			this.mGroupName = sGroupName;
			this.mBoldHeader = sBoldHeader;
			this.mOpen = sOpen;
			this.mReducible = sReducible;
		}
	}
    //-------------------------------------------------------------------------------------------------------------
    public enum NWDIfType
    {
        Equal,
        Range,
    }
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
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
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDGroupEndAttribute : Attribute
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDSeparatorAttribute : Attribute
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDNotEditableAttribute : Attribute
	{
	}
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
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
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
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
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
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
	//-------------------------------------------------------------------------------------------------------------
	[AttributeUsage (AttributeTargets.Property, AllowMultiple = true)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDEnumStringAttribute : Attribute
	{
		public string[] mEnumString;

		public NWDEnumStringAttribute (string[] sEnumString)
		{
			this.mEnumString = sEnumString;
		}
    }
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDLongStringAttribute : Attribute
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDVeryLongStringAttribute : Attribute
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    //public class NWDUniqueFromPropertyAttribute : Attribute
    //{
    //    public string mProperty;
    //    public NWDUniqueFromPropertyAttribute(string sOriginalProperty)
    //    {
    //        this.mProperty = sOriginalProperty;
    //    }
    //}
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================