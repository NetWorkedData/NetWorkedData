using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	/// <summary>
	/// NWD geoloc type.
	/// </summary>
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDGeolocType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDGeolocType"/> class.
		/// </summary>
		public NWDGeolocType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDGeolocType"/> class.
		/// </summary>
		/// <param name="sValue">S value.</param>
		public NWDGeolocType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Latitudes the longitude.
		/// </summary>
		/// <returns>The longitude.</returns>
		public Vector2 LatitudeLongitude ()
		{
			string[] tVect = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);

			float tLatitude = 0;
			float tLongitude = 0;
			if (tVect.Length == 2) {
				float.TryParse (tVect [0], out tLatitude);
				float.TryParse (tVect [1], out tLongitude);
			}
			return new Vector2 (tLatitude, tLongitude);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Containtses the point in radius.
		/// </summary>
		/// <returns><c>true</c>, if point in radius was containtsed, <c>false</c> otherwise.</returns>
		/// <param name="sPoint">S point.</param>
		/// <param name="sRadius">S radius.</param>
		public bool ContaintsPointInRadius (Vector2 sPoint, float sRadius)
		{
			bool rReturn = false;
			Vector2 tLatitudeLongitude = LatitudeLongitude ();
			if (Mathf.Abs (Vector2.Distance (sPoint, tLatitudeLongitude)) < Mathf.Abs (sRadius)) {
				rReturn = true;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The height of the field. Add a simular method like ControlFieldHeight in your code
		/// </summary>
		/// <returns>The field height.</returns>
		public override float ControlFieldHeight ()
		{
			GUIStyle tTextFieldStyle = new GUIStyle (EditorStyles.textField);
			float tHeight = tTextFieldStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			return tHeight * 4;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The field to edit value in editor.
		/// </summary>
		/// <returns>The field.</returns>
		/// <param name="sPosition">S position.</param>
		/// <param name="sEntitled">S entitled.</param>
		/// <param name="sPos">S position.</param>
		public override object ControlField (Rect sPos, string sEntitled)
		{
			NWDGeolocType tTemporary = new NWDGeolocType ();
			GUIStyle tTextFieldStyle = new GUIStyle (EditorStyles.textField);
			float tHeight = tTextFieldStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			float tX = sPos.x + EditorGUIUtility.labelWidth;

			GUI.Label (new Rect (sPos.x, sPos.y, sPos.width, sPos.height), sEntitled);

			Vector2 tLatitudeLongitude = LatitudeLongitude ();

			GUI.Label (new Rect (tX, sPos.y, sPos.width - EditorGUIUtility.labelWidth, sPos.height), "Lattitude");
			float tLatitude = EditorGUI.FloatField (new Rect (tX, sPos.y + tHeight, sPos.width - EditorGUIUtility.labelWidth, sPos.height), tLatitudeLongitude.x);
		
			GUI.Label (new Rect (tX, sPos.y + tHeight * 2, sPos.width - EditorGUIUtility.labelWidth, sPos.height), "Longitude");
			float tLongitude = EditorGUI.FloatField (new Rect (tX, sPos.y + tHeight * 3, sPos.width - EditorGUIUtility.labelWidth, sPos.height), tLatitudeLongitude.y);

			tTemporary.Value = tLatitude + NWDConstants.kFieldSeparatorA + tLongitude;
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Return if the asset path is used in this DataType.
		/// </summary>
		/// <returns><c>true</c>, if asset path was changed, <c>false</c> otherwise.</returns>
		/// <param name="sOldPath">S old path.</param>
		/// <param name="sNewPath">S new path.</param>
		public override bool ChangeAssetPath (string sOldPath, string sNewPath)
		{
			return false;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================