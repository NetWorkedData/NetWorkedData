

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
	// Custom comparer for the NWEDataTypeFloat class
	//class NWEDataTypeFloatComparer : IEqualityComparer<NWEDataTypeFloat>
	//{
	//	//-------------------------------------------------------------------------------------------------------------
	//	// Products are equal if their names and product numbers are equal.
	//	public bool Equals(NWEDataTypeFloat x, NWEDataTypeFloat y)
	//	{
	//		//Check whether the compared objects reference the same data.
	//		if (ReferenceEquals (x, y)) {
	//			return true;
	//		}
	//		//Check whether any of the compared objects is null.
	//		if (ReferenceEquals (x, null) || ReferenceEquals (y, null)) {
	//			return false;
	//		}
	//		//Check whether the products' properties are equal.
	//		return x.Value == y.Value;
	//	}
	//	//-------------------------------------------------------------------------------------------------------------
	//	// If Equals() returns true for a pair of objects 
	//	// then GetHashCode() must return the same value for these objects
	//	public int GetHashCode(NWEDataTypeFloat product)
	//	{
	//		//Check whether the object is null
	//		if (ReferenceEquals (product, null)) {
	//			return 0;
	//		}
	//		//Get hash code for the Name field if it is not null.
	//		int hashProductName = product.Value.GetHashCode();
	//		//Get hash code for the Code field.
	//		int hashProductCode = product.Value.GetHashCode();
	//		//Calculate the hash code for the product.
	//		return hashProductName ^ hashProductCode;
	//	}
	//	//-------------------------------------------------------------------------------------------------------------
	//}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[SerializeField]
	
	/// <summary>
	/// NWE data type. Use in SQLite insertion for special data (like color, json, etc.)
	/// Must be herited by another DataType class (for example class ColorDatatype : NWEDataTypeFloat)
	/// </summary>
	public class NWEDataTypeFloat
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The value as string
		/// </summary>
		public double Value;
		/// <summary>
		/// The type of the SQL column : var char because it'use string convertion as value 
		/// </summary>
		public const string SQLType = "double";
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicToolBox.NWEDataTypeFloat"/> class.
		/// </summary>
		public NWEDataTypeFloat ()
		{
            Value = 0;
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicToolBox.NWEDataTypeFloat"/> class.
		/// </summary>
		/// <param name="sValue">S value.</param>
		public NWEDataTypeFloat (double sValue = 0)
		{
			Value = sValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="BasicToolBox.NWEDataTypeFloat"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="BasicToolBox.NWEDataTypeFloat"/>.</returns>
        public override string ToString ()
		{
            return Value.ToString(CultureInfo.InvariantCulture);
        }
        //-------------------------------------------------------------------------------------------------------------
        public double ToDouble()
        {
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the value as string.
        /// </summary>
        /// <returns>The string.</returns>
        public double GetDouble ()
		{
			return Value;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetDouble (double sDouble)
		{
				Value = sDouble;
		}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public bool InError = false;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The height of the field. Add a simular method like ControlFieldHeight in your code 
        /// </summary>
        /// <returns>The field height.</returns>
        public virtual float ControlFieldHeight () {
			//FAKE
			return 100.0f;
		}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The field to edit value in editor.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="sPosition">S position.</param>
        /// <param name="sEntitled">S entitled.</param>
        public virtual object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "") {
			NWEDataTypeFloat tTemporary = new NWEDataTypeFloat ();
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
            Value = 0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Flush()
        {
            Value = 0f;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================