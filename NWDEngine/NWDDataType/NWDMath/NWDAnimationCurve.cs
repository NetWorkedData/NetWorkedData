//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:2
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

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
    //TODO: FINISH THIS CLASS NWDAnimationCurve
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDAnimationCurve : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAnimationCurve()
        {
            Value = NWDToolbox.AnimationCurveZero();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDAnimationCurve(string sValue)
        //{
        //    if (sValue == null)
        //    {
        //        Value = NWDToolbox.AnimationCurveZero();
        //    }
        //    else
        //    {
        //        Value = sValue;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDAnimationCurve(AnimationCurve sCurve)
        {
            Value = NWDToolbox.AnimationCurveToString(sCurve);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.AnimationCurveZero();
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
        public void SetAnimationCurve(AnimationCurve sCurve)
        {
            Value = NWDToolbox.AnimationCurveToString(sCurve);
            //List<string> tList = new List<string>();
            //foreach (Keyframe tF in sCurve.keys)
            //{
            //    string tV = tF.time + NWDConstants.kFieldSeparatorB +
            //                  tF.value + NWDConstants.kFieldSeparatorB +
            //                  tF.inTangent + NWDConstants.kFieldSeparatorB +
            //                  tF.outTangent;
            //    tList.Add(tV);
            //}
            //Value = string.Join(NWDConstants.kFieldSeparatorA, tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public AnimationCurve GetAnimationCurve()
        {
            return NWDToolbox.AnimationCurveFromString(Value);
            //List<Keyframe> tList = new List<Keyframe>();
            //AnimationCurve rCurve = new AnimationCurve();
            //string[] tKeyFrames = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (string tV in tKeyFrames)
            //{
            //    string[] tFloats = tV.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
            //    float tX = 0.0F;
            //    float tY = 0.0F;
            //    float tZ = 0.0F;
            //    float tW = 0.0F;
            //    if (tFloats.Count() == 4)
            //    {
            //        float.TryParse(tFloats[0], out tX);
            //        float.TryParse(tFloats[1], out tY);
            //        float.TryParse(tFloats[2], out tZ);
            //        float.TryParse(tFloats[3], out tW);
            //        Keyframe tKeyframe = new Keyframe(tX, tY, tZ, tW);
            //        tList.Add(tKeyframe);
            //    }
            //}
            //rCurve.keys = tList.ToArray();
            //return rCurve;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0f);
            return tHeight*2;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPos, string sEntitled, bool sDisabled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDAnimationCurve tTemporary = new NWDAnimationCurve();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            AnimationCurve tCurve = GetAnimationCurve();
            AnimationCurve tNextCurve = EditorGUI.CurveField(new Rect(sPos.x, sPos.y, sPos.width, NWDGUI.kLabelStyle.fixedHeight),
                                                             tContent,tCurve);
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.indentLevel = tIndentLevel;

            tTemporary.SetAnimationCurve(tNextCurve);
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================