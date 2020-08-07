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
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    /// <summary>
    /// NWD geoloc type.
    /// </summary>
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDGeolocType : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="NetWorkedData.NWDGeolocType"/> class.
        /// </summary>
        public NWDGeolocType()
        {
            Value = NWDToolbox.FloatToString(0.0F) + NWDConstants.kFieldSeparatorA + NWDToolbox.FloatToString(0.0F);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="NetWorkedData.NWDGeolocType"/> class.
        /// </summary>
        /// <param name="sValue">S value.</param>
        public NWDGeolocType(Vector2 sVector)
        {
            Value = NWDToolbox.FloatToString(sVector.x) + NWDConstants.kFieldSeparatorA + NWDToolbox.FloatToString(sVector.y);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.FloatToString(0.0F) + NWDConstants.kFieldSeparatorA + NWDToolbox.FloatToString(0.0F);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void BaseVerif()
        {
            // Need to check with a new dictionary each time
            if (string.IsNullOrEmpty(Value))
            {
                Default();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Latitudes the longitude.
        /// </summary>
        /// <returns>The longitude.</returns>
        public void SetLatitudeLongitude(Vector2 sVector)
        {
            Value = NWDToolbox.FloatToString(sVector.x) + NWDConstants.kFieldSeparatorA + NWDToolbox.FloatToString(sVector.y);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Latitudes the longitude.
        /// </summary>
        /// <returns>The longitude.</returns>
        public Vector2 GetLatitudeLongitude()
        {
            string[] tVect = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tLatitude = 0;
            float tLongitude = 0;
            if (tVect.Length == 2)
            {
                tLatitude = NWDToolbox.FloatFromString(tVect[0]);
                tLongitude = NWDToolbox.FloatFromString(tVect[1]);
            }
            return new Vector2(tLatitude, tLongitude);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Containtses the point in radius.
        /// </summary>
        /// <returns><c>true</c>, if point in radius was containtsed, <c>false</c> otherwise.</returns>
        /// <param name="sPoint">S point.</param>
        /// <param name="sRadius">S radius.</param>
        public bool ContaintsPointInRadius(Vector2 sPoint, float sRadius)
        {
            bool rReturn = false;
            Vector2 tLatitudeLongitude = GetLatitudeLongitude();
            if (Mathf.Abs(Vector2.Distance(sPoint, tLatitudeLongitude)) < Mathf.Abs(sRadius))
            {
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
        public override float ControlFieldHeight()
        {
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100.0f);
            return tHeight * 4;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The field to edit value in editor.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="sPosition">S position.</param>
        /// <param name="sEntitled">S entitled.</param>
        /// <param name="sPosition">S position.</param>
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDGeolocType tTemporary = new NWDGeolocType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100.0f);
            float tX = sPosition.x + EditorGUIUtility.labelWidth;
            EditorGUI.LabelField(new Rect(sPosition.x, sPosition.y, sPosition.width, sPosition.height), tContent);
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            Vector2 tLatitudeLongitude = GetLatitudeLongitude();
            GUI.Label(new Rect(tX, sPosition.y, sPosition.width - EditorGUIUtility.labelWidth, sPosition.height), "Lattitude");
            float tLatitude = EditorGUI.FloatField(new Rect(tX, sPosition.y + tHeight, sPosition.width - EditorGUIUtility.labelWidth, sPosition.height), tLatitudeLongitude.x);
            GUI.Label(new Rect(tX, sPosition.y + tHeight * 2, sPosition.width - EditorGUIUtility.labelWidth, sPosition.height), "Longitude");
            float tLongitude = EditorGUI.FloatField(new Rect(tX, sPosition.y + tHeight * 3, sPosition.width - EditorGUIUtility.labelWidth, sPosition.height), tLatitudeLongitude.y);
            tTemporary.SetLatitudeLongitude(new Vector2(tLatitude, tLongitude));
            EditorGUI.indentLevel = tIndentLevel;
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================