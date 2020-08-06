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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	// Custom comparer for the NWEDataType class
	//class NWEDataTypeComparer : IEqualityComparer<NWEDataType>
	//{
	//	//-------------------------------------------------------------------------------------------------------------
	//	// Products are equal if their names and product numbers are equal.
	//	public bool Equals(NWEDataType x, NWEDataType y)
	//	{
	//		//Check whether the compared objects reference the same data.
	//		if (System.Object.ReferenceEquals (x, y)) {
	//			return true;
	//		}
	//		//Check whether any of the compared objects is null.
	//		if (System.Object.ReferenceEquals (x, null) || System.Object.ReferenceEquals (y, null)) {
	//			return false;
	//		}
	//		//Check whether the products' properties are equal.
	//		return x.Value == y.Value ;
	//	}
	//	//-------------------------------------------------------------------------------------------------------------
	//	// If Equals() returns true for a pair of objects 
	//	// then GetHashCode() must return the same value for these objects
	//	public int GetHashCode(NWEDataType product)
	//	{
	//		//Check whether the object is null
	//		if (System.Object.ReferenceEquals (product, null)) {
	//			return 0;
	//		}
	//		//Get hash code for the Name field if it is not null.
	//		int hashProductName = product.Value == null ? 0 : product.Value.GetHashCode();
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
	/// Must be herited by another DataType class (for example class ColorDatatype : NWEDataType)
	/// </summary>
	public class NWEDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The value as string
		/// </summary>
		public string Value;
		/// <summary>
		/// The type of the SQL column : var char because it'use string convertion as value 
		/// </summary>
		public const string SQLType = "varchar";
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicToolBox.NWEDataType"/> class.
		/// </summary>
		public NWEDataType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="BasicToolBox.NWEDataType"/> class.
		/// </summary>
		/// <param name="sValue">S value.</param>
		public NWEDataType (string sValue = "")
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="BasicToolBox.NWEDataType"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="BasicToolBox.NWEDataType"/>.</returns>
		public override string ToString ()
		{
			if (Value == null) {
				return string.Empty;
			}
			return Value;
        }
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Gets the value as string.
        ///// </summary>
        ///// <returns>The string.</returns>
        //public string GetString()
        //{
        //    if (Value == null)
        //    {
        //        return string.Empty;
        //    }
        //    return Value;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Sets the string as value.
        ///// </summary>
        ///// <param name="sString">S string.</param>
        //public void SetString(string sString)
        //{
        //    if (sString == null)
        //    {
        //        Value = string.Empty;
        //    }
        //    else
        //    {
        //        Value = sString;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the value as string.
        /// </summary>
        /// <returns>The string.</returns>
        public virtual string GetValue ()
		{
			if (Value == null) {
				return string.Empty;
			}
			return Value;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the string as value.
		/// </summary>
		/// <param name="sString">S string.</param>
		public virtual void SetValue (string sString)
		{
			if (sString == null) {
				Value = string.Empty;
			} else {
				Value = sString;
			}
		}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns>a boolean</returns>
        public bool ValueIsNullOrEmpty()
        {
            return string.IsNullOrEmpty(Value);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Flush()
        {
            Value = string.Empty;
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
        public virtual object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "", object sAdditionnal = null) {
			NWEDataType tTemporary = new NWEDataType ();
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
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void BaseVerif()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================