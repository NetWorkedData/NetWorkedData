//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using BasicToolBox;
using System.Text;
using System.Reflection;
using System;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelperGroup
    {
        public NWDBasisHelperGroup FromGroup;
        public string Name = string.Empty;
        public string Tooltips = string.Empty;
        public int Indent = 0;
        public bool Open = false;
        public bool Bold = false;
        public bool Reducible = false;
        public bool Separator = true;
        public string ClassName;
        //public List<NWDBasisHelperGroup> Groups = new List<NWDBasisHelperGroup>();
        public List<NWDBasisHelperElement> Elements = new List<NWDBasisHelperElement>();
        //-------------------------------------------------------------------------------------------------------------
        private string Key()
        {
            return ClassName + Name;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsDrawable()
        {
            return GetDrawable();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDrawable(bool sOpen = true)
        {
            string tKey = Key();
            //EditorPrefs.HasKey(tKey);
            EditorPrefs.SetBool(tKey, sOpen);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetDrawable()
        {
            string tKey = Key();
            //EditorPrefs.HasKey(tKey);
            return EditorPrefs.GetBool(tKey, Open);
        }
        //-------------------------------------------------------------------------------------------------------------
        public GUIContent Content()
        {
            return new GUIContent(Name, Tooltips);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelperElement
    {
        public PropertyInfo Property;
        public NWDBasisHelperGroup Group;
        public GUIContent Information;
        public string Name;
        public int Order = 0;
        public int Indent = 0;
        public bool Enable = true;
        public bool WebModelError = false;
        public bool NotEditable = false;
        public float SpaceBefore = 0.0F;
        public bool Separator = false;
        public string Tooltips;

        //-------------------------------------------------------------------------------------------------------------
        public GUIContent Content()
        {
            return new GUIContent(Name, Tooltips);
        }
        //-------------------------------------------------------------------------------------------------------------
        public float NewDrawObjectInspectorHeight(object sObject)
        {
            float tY = SpaceBefore;
            if (Separator == true)
            {
                tY += NWDConstants.kFieldMarge * 2;
            }
            if (Property != null)
            {
                bool tDraw = true;
                bool tNotEditable = NotEditable;
                foreach (NWDIfAttribute tReference in Property.GetCustomAttributes(typeof(NWDIfAttribute), true))
                {
                    if (tReference.IsDrawable(sObject) == false)
                    {
                        if (tReference.mVisible == false)
                        {
                            tDraw = false;
                        }
                        tNotEditable = true;
                    }
                }
                if (tDraw == true)
                {
                    if (Information != null)
                    {
                        tY += 100;
                    }
                    EditorGUI.BeginDisabledGroup(tNotEditable);
                    // check this propertyheight
                    if (Property.GetCustomAttributes(typeof(NWDEnumStringAttribute), true).Length > 0)
                    {
                        tY += NWDConstants.tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                    //else if (Property.GetCustomAttributes(typeof(NWDEnumAttribute), true).Length > 0)
                    //{
                    //    tY += NWDConstants.tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                    //}
                    else
                    {
                        Type tTypeOfThis = Property.PropertyType;
                        if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                        {
                            if (Property.GetCustomAttributes(typeof(NWDLongStringAttribute), true).Length > 0)
                            {
                                tY += NWDConstants.tTextFieldStyle.fixedHeight * NWDConstants.kLongString + NWDConstants.kFieldMarge;
                            }
                            else if (Property.GetCustomAttributes(typeof(NWDVeryLongStringAttribute), true).Length > 0)
                            {
                                tY += NWDConstants.tTextFieldStyle.fixedHeight * NWDConstants.kVeryLongString + NWDConstants.kFieldMarge;
                            }
                            else
                            {
                                tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                            }
                        }
                        else if (tTypeOfThis.IsEnum)
                        {
                            tY += NWDConstants.tEnumStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            tY += NWDConstants.tToggleStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(int))
                        {
                            tY += NWDConstants.tIntFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(long))
                        {
                            tY += NWDConstants.tLongFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            tY += NWDConstants.tFloatFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            tY += NWDConstants.tDoubleFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataType tBTBDataType = (BTBDataType)tValue;
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeInt tBTBDataType = (BTBDataTypeInt)tValue;
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeFloat tBTBDataType = (BTBDataTypeFloat)tValue;
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeEnum)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeEnum tBTBDataType = (BTBDataTypeEnum)tValue;
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else
                        {
                            tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
            if (Group != null)
            {
                if (Group.Separator == true)
                {
                    tY += NWDConstants.kFieldMarge * 2;
                }
                if (Group.Reducible == true)
                {
                    if (Group.Bold == true)
                    {
                        tY += NWDConstants.tBoldFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                    else
                    {
                        tY += NWDConstants.tFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                }
                else
                {
                    if (Group.Bold == true)
                    {
                        tY += NWDConstants.tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                    else
                    {
                        tY += NWDConstants.tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                }
                tY += NWDConstants.kFieldMarge;
                if (Group.IsDrawable() == true)
                {
                    foreach (NWDBasisHelperElement tElement in Group.Elements)
                    {
                        tY += tElement.NewDrawObjectInspectorHeight(sObject);
                    }
                }
            }
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float NewDrawObjectInspector(object sObject, float sX, float sY, float sWidth)
        {
            float tWidth = sWidth;
            float tX = sX;
            float tY = sY;

            // EditorGUI.indentLevel = Indent;

            if (Separator == true)
            {
                Rect tRect = EditorGUI.IndentedRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1));
                EditorGUI.DrawRect(tRect, NWDConstants.kRowColorLine);
                tY += NWDConstants.kFieldMarge * 2;
            }

            if (Property != null)
            {
                bool tDraw = true;
                bool tNotEditable = NotEditable;
                foreach (NWDIfAttribute tReference in Property.GetCustomAttributes(typeof(NWDIfAttribute), true))
                {
                    if (tReference.IsDrawable(sObject) == false)
                    {
                        if (tReference.mVisible == false)
                        {
                            tDraw = false;
                        }
                        tNotEditable = true;
                    }
                }
                if (tDraw == true)
                {
                    //  draw informations? and add height in size operation?
                    if (Information != null)
                    {
                        float tInformationsHeight = NWDConstants.tHelpBoxStyle.CalcHeight(Information, tWidth);
                        GUI.Label(new Rect(tX, tY, tWidth, tInformationsHeight), Information, NWDConstants.tHelpBoxStyle);
                        tY += tInformationsHeight + NWDConstants.kFieldMarge;
                    }
                    // prepare positions and size
                    float tXField = sX + EditorGUIUtility.labelWidth;
                    float tWidthField = tWidth - EditorGUIUtility.labelWidth;
                    // create rects for label and field
                    float tIndent = Indent * NWDConstants.kFieldIndent;
                    Rect tEntitlementRect = new Rect(tX + tIndent, tY, EditorGUIUtility.labelWidth - tIndent, NWDConstants.kPropertyLabelStyle.fixedHeight);
                    Rect tFieldRect = new Rect(tXField, tY, tWidthField, NWDConstants.kPropertyLabelStyle.fixedHeight);

                    // begin editable field?
                    EditorGUI.BeginDisabledGroup(tNotEditable);

                    //swicth case of type
                    if (Property.GetCustomAttributes(typeof(NWDEnumStringAttribute), true).Length > 0)
                    {
                        EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);
                        tFieldRect.height = NWDConstants.tPopupdStyle.fixedHeight;
                        NWDEnumStringAttribute tInfo = Property.GetCustomAttributes(typeof(NWDEnumStringAttribute), true)[0] as NWDEnumStringAttribute;
                        string[] tV = tInfo.mEnumString;
                        string tValue = (string)Property.GetValue(sObject, null);
                        int tValueInt = Array.IndexOf<string>(tV, tValue);
                        //remove EditorGUI.indentLevel to draw next controller without indent 
                        //int tIndentLevel = EditorGUI.indentLevel;
                        EditorGUI.indentLevel = 0;
                        int tValueIntNext = EditorGUI.Popup(tFieldRect, string.Empty, tValueInt, tV, NWDConstants.tPopupdStyle);
                        tY += NWDConstants.tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                        string tValueNext = string.Empty;
                        if (tValueIntNext < tV.Length && tValueIntNext >= 0)
                        {
                            tValueNext = tV[tValueIntNext];
                        }
                        if (tValueNext != tValue)
                        {
                            Property.SetValue(sObject, tValueNext, null);
                            //rNeedBeUpdate = true;
                        }
                        //EditorGUI.indentLevel = tIndentLevel;
                    }
                    //else if (Property.GetCustomAttributes(typeof(NWDEnumAttribute), true).Length > 0)
                    //{
                    //    NWDEnumAttribute tInfo = Property.GetCustomAttributes(typeof(NWDEnumAttribute), true)[0] as NWDEnumAttribute;
                    //    string[] tV = tInfo.mEnumString;
                    //    int[] tI = tInfo.mEnumInt;
                    //    int tValue = (int)Property.GetValue(sObject, null);
                    //    int tValueInt = Array.IndexOf<int>(tI, tValue);
                    //    EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.kTextFieldStyle.fixedHeight), Content());

                    //    //remove EditorGUI.indentLevel to draw next controller without indent 
                    //    int tIndentLevel = EditorGUI.indentLevel;
                    //    EditorGUI.indentLevel = 0;

                    //    int tValueIntNext = EditorGUI.Popup(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth - EditorGUIUtility.labelWidth, NWDConstants.tPopupdStyle.fixedHeight), string.Empty, tValueInt, tV, NWDConstants.tPopupdStyle);
                    //    tY += NWDConstants.tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                    //    int tValueNext = 0;
                    //    if (tValueIntNext < tI.Length && tValueIntNext >= 0)
                    //    {
                    //        tValueNext = tI[tValueIntNext];
                    //    }
                    //    if (tValueNext != tValue)
                    //    {
                    //        Property.SetValue(sObject, tValueNext, null);
                    //        //rNeedBeUpdate = true;
                    //    }
                    //    EditorGUI.indentLevel = tIndentLevel;
                    //}
                    else
                    {
                        Type tTypeOfThis = Property.PropertyType;
                        if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                        {

                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);
                            float tH = 0;
                            if (Property.GetCustomAttributes(typeof(NWDLongStringAttribute), true).Length > 0)
                            {
                                tH += NWDConstants.tTextFieldStyle.fixedHeight * NWDConstants.kLongString + NWDConstants.kFieldMarge;
                            }
                            else if (Property.GetCustomAttributes(typeof(NWDVeryLongStringAttribute), true).Length > 0)
                            {
                                tH += NWDConstants.tTextFieldStyle.fixedHeight * NWDConstants.kVeryLongString + NWDConstants.kFieldMarge;
                            }
                            else
                            {
                                tH += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                            }
                            string tValue = Property.GetValue(sObject, null) as string;
                            if (string.IsNullOrEmpty(tValue) == false)
                            {
                                tValue = NWDToolbox.TextUnprotect(tValue);
                            }
                            tFieldRect.height = tH;
                            string tValueNext = EditorGUI.TextField(tFieldRect, tValue, NWDConstants.tTextFieldStyle);
                            tY += tH;
                            if (tValueNext != tValue)
                            {
                                tValueNext = NWDToolbox.TextProtect(tValueNext);
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                        }
                        else if (tTypeOfThis.IsEnum)
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);
                            tFieldRect.height = NWDConstants.tEnumStyle.fixedHeight;
                            Enum tValue = Property.GetValue(sObject, null) as Enum;
                            Enum tValueNext = tValue;
                            if (Property.GetCustomAttributes(typeof(NWDFlagsEnumAttribute), true).Length > 0)
                            {
                                tValueNext = EditorGUI.EnumFlagsField(tFieldRect, tValue, NWDConstants.tEnumStyle);
                            }
                            else
                            {
                                tValueNext = EditorGUI.EnumPopup(tFieldRect, tValue, NWDConstants.tEnumStyle);
                            }
                            tY += NWDConstants.tEnumStyle.fixedHeight + NWDConstants.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);
                            tFieldRect.height = NWDConstants.tToggleStyle.fixedHeight;

                            bool tValue = (bool)Property.GetValue(sObject, null);
                            bool tValueNext = EditorGUI.Toggle(tFieldRect, tValue, NWDConstants.tToggleStyle);
                            tY += NWDConstants.tToggleStyle.fixedHeight + NWDConstants.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tToggleStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(int))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);
                            tFieldRect.height = NWDConstants.tIntFieldStyle.fixedHeight;
                            int tValue = (int)Property.GetValue(sObject, null);
                            int tValueNext = tValue;
                            if (Property.GetCustomAttributes(typeof(NWDIntSliderAttribute), true).Length > 0)
                            {
                                NWDIntSliderAttribute tSlider = Property.GetCustomAttributes(typeof(NWDIntSliderAttribute), true)[0] as NWDIntSliderAttribute;
                                tValueNext = EditorGUI.IntSlider(tFieldRect, tValue, tSlider.mMin, tSlider.mMax);
                            }
                            else
                            {
                                tValueNext = EditorGUI.IntField(tFieldRect, tValue, NWDConstants.tIntFieldStyle);
                            }
                            tY += NWDConstants.tIntFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(long))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);
                            tFieldRect.height = NWDConstants.tLongFieldStyle.fixedHeight;
                            long tValue = (long)Property.GetValue(sObject, null);
                            long tValueNext = tValue;
                            tValueNext = EditorGUI.LongField(tFieldRect, tValue, NWDConstants.tLongFieldStyle);
                            tY += NWDConstants.tLongFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);
                            tFieldRect.height = NWDConstants.tFloatFieldStyle.fixedHeight;
                            float tValue = (float)Property.GetValue(sObject, null);
                            float tValueNext = tValue;
                            if (Property.GetCustomAttributes(typeof(NWDFloatSliderAttribute), true).Length > 0)
                            {
                                NWDFloatSliderAttribute tSlider = Property.GetCustomAttributes(typeof(NWDFloatSliderAttribute), true)[0] as NWDFloatSliderAttribute;
                                tValueNext = EditorGUI.Slider(tFieldRect, tValue, tSlider.mMin, tSlider.mMax);
                            }
                            else
                            {
                                tValueNext = EditorGUI.FloatField(tFieldRect, tValue, NWDConstants.tFloatFieldStyle);
                            }
                            tY += NWDConstants.tFloatFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tFloatFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);
                            tFieldRect.height = NWDConstants.tDoubleFieldStyle.fixedHeight;

                            double tValue = (double)Property.GetValue(sObject, null);
                            double tValueNext = EditorGUI.DoubleField(tFieldRect, tValue, NWDConstants.tDoubleFieldStyle);
                            tY += NWDConstants.tDoubleFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tFloatFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                        {

                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataType tBTBDataType = tValue as BTBDataType;
                            BTBDataType tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight),
                                                                                      "  ", Tooltips) as BTBDataType;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tBTBDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeInt tBTBDataType = tValue as BTBDataTypeInt;
                            BTBDataTypeInt tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDConstants.tIntFieldStyle.fixedHeight),
                                                                                     "  ", Tooltips) as BTBDataTypeInt;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tBTBDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeFloat tBTBDataType = tValue as BTBDataTypeFloat;
                            BTBDataTypeFloat tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDConstants.tFloatFieldStyle.fixedHeight),
                                                                                     "  ", Tooltips) as BTBDataTypeFloat;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tBTBDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeEnum)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeEnum tBTBDataType = tValue as BTBDataTypeEnum;
                            BTBDataTypeEnum tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDConstants.tFloatFieldStyle.fixedHeight),
                                                                                     "  ", Tooltips) as BTBDataTypeEnum;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                ((BTBDataTypeEnum)tValue).Value = tBTBDataTypeNext.Value;
                                Property.SetValue(sObject, tValue, null);
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeMask)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDConstants.kPropertyLabelStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeMask tBTBDataType = tValue as BTBDataTypeMask;
                            BTBDataTypeMask tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDConstants.tFloatFieldStyle.fixedHeight),
                                                                                     Property.Name, Tooltips) as BTBDataTypeMask;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                ((BTBDataTypeMask)tValue).Value = tBTBDataTypeNext.Value;
                                Property.SetValue(sObject, tValue, null);
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDConstants.kFieldMarge;
                        }
                        else
                        {
                            string tValue = Property.GetValue(sObject, null) as string;
                            EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight), Name, tValue, NWDConstants.tTextFieldStyle);
                            tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            if (Group != null)
            {
                if (Group.Separator == true)
                {
                    Rect tRect = EditorGUI.IndentedRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1));
                    EditorGUI.DrawRect(tRect, NWDConstants.kRowColorLine);
                    tY += 1 + NWDConstants.kFieldMarge * 2;
                }
                if (Group.Reducible == true)
                {
                    bool tActualDraw = Group.IsDrawable();
                    if (Group.Bold == true)
                    {
                        tActualDraw = EditorGUI.Foldout(new Rect(tX, tY, tWidth, NWDConstants.tBoldFoldoutStyle.fixedHeight), tActualDraw, Group.Content(), NWDConstants.tBoldFoldoutStyle);
                        tY += NWDConstants.tBoldFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                    else
                    {
                        tActualDraw = EditorGUI.Foldout(new Rect(tX, tY, tWidth, NWDConstants.tFoldoutStyle.fixedHeight), tActualDraw, Group.Content(), NWDConstants.tFoldoutStyle);
                        tY += NWDConstants.tFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                    Group.SetDrawable(tActualDraw);
                }
                else
                {
                    if (Group.Bold == true)
                    {
                        EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.tBoldLabelStyle.fixedHeight), Group.Content(), NWDConstants.tBoldLabelStyle);
                        tY += NWDConstants.tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                    else
                    {
                        EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.tLabelStyle.fixedHeight), Group.Content(), NWDConstants.tLabelStyle);
                        tY += NWDConstants.tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                    }
                }
                if (Group.IsDrawable() == true)
                {
                    foreach (NWDBasisHelperElement tElement in Group.Elements)
                    {
                        tY = tElement.NewDrawObjectInspector(sObject, sX, tY, sWidth);
                    }
                }
            }
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelperGroup InspectorHelper = new NWDBasisHelperGroup();
        public Dictionary<string, NWDBasisHelperGroup> GroupsByName = new Dictionary<string, NWDBasisHelperGroup>();
        bool LoadStyle = false;
        //-------------------------------------------------------------------------------------------------------------
        public void AnalyzeForInspector()
        {
            if (LoadStyle == false)
            {
                LoadStyle = true;
                NWDConstants.LoadStyles();
                Dictionary<NWDBasisHelperElement, string> PropertiesForGroupName = new Dictionary<NWDBasisHelperElement, string>();
                List<string> tPropertyListInWebModel = new List<string>();
                if (WebServiceWebModel.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
                {
                    if (WebModelPropertiesOrder.ContainsKey(WebServiceWebModel[NWDAppConfiguration.SharedInstance().WebBuild]))
                    {
                        tPropertyListInWebModel.AddRange(WebModelPropertiesOrder[WebServiceWebModel[NWDAppConfiguration.SharedInstance().WebBuild]]);
                    }
                }
                Type tType = ClassType;
                InspectorHelper = new NWDBasisHelperGroup();
                InspectorHelper.FromGroup = InspectorHelper;
                GroupsByName.Add("", InspectorHelper);
                NWDBasisHelperGroup tGroup = InspectorHelper;
                PropertyInfo[] tPropertiesArray = tType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo tProp in tPropertiesArray)
                {
                    if (tProp.GetCustomAttributes(typeof(NWDHidden), true).Length > 0 || tProp.GetCustomAttributes(typeof(NWDNotVisible), true).Length > 0)
                    {
                        // hidden this property
                    }
                    else
                    {
                        string tEntitled = NWDToolbox.SplitCamelCase(tProp.Name);
                        NWDBasisHelperElement tProperty = new NWDBasisHelperElement();
                        if (tProp.PropertyType.IsGenericType)
                        {
                            //Debug.Log(tProp.PropertyType.Name);
                            tProperty.Tooltips = "Property : " + tProp.Name + "\nType : " + tProp.PropertyType.Name.Replace("`1", "<" + tProp.PropertyType.GenericTypeArguments[0].Name + ">") + "\n\n" + tEntitled;
                        }
                        else
                        {
                            tProperty.Tooltips = "Property : " + tProp.Name + "\nType : " + tProp.PropertyType.Name + "\n\n" + tEntitled;
                        }
                        tProperty.Property = tProp;

                        foreach (NWDGroupResetAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDGroupResetAttribute), true))
                        {
                            tGroup.Elements.Sort((x, y) => x.Order.CompareTo(y.Order));
                            tGroup = InspectorHelper;
                        }
                        foreach (NWDGroupEndAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDGroupEndAttribute), true))
                        {
                            tGroup.Elements.Sort((x, y) => x.Order.CompareTo(y.Order));
                            tGroup = tGroup.FromGroup;
                        }
                        foreach (NWDSeparatorAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDSeparatorAttribute), true))
                        {
                            tProperty.Separator = true;
                        }

                        foreach (NWDInformationAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDInformationAttribute), true))
                        {
                            tProperty.Information = tInsideReference.Content();
                        }
                        //foreach (NWDGroupSeparatorAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDGroupSeparatorAttribute), true))
                        //{
                        //    tProperty.Separator = true;
                        //}

                        foreach (NWDGroupStartAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDGroupStartAttribute), true))
                        {
                            NWDBasisHelperElement tElement = new NWDBasisHelperElement();
                            tElement.Order = tGroup.Elements.Count();
                            tGroup.Elements.Add(tElement);
                            NWDBasisHelperGroup tSubGroup = new NWDBasisHelperGroup();
                            if (tGroup == InspectorHelper && InspectorHelper.Elements.Count == 1)
                            {
                                tSubGroup.Separator = false;
                            }
                            tElement.Group = tSubGroup;
                            tSubGroup.FromGroup = tGroup;
                            tSubGroup.Indent = tGroup.Indent + 1;
                            tSubGroup.Name = tInsideReference.mGroupName;
                            tSubGroup.Tooltips = tInsideReference.mToolsTips;
                            tSubGroup.Bold = tInsideReference.mBoldHeader;
                            tSubGroup.Open = tInsideReference.mOpen;
                            tSubGroup.Reducible = tInsideReference.mReducible;
                            if (GroupsByName.ContainsKey(tSubGroup.Name) == false)
                            {
                                GroupsByName.Add(tSubGroup.Name, tSubGroup);
                            }
                            tGroup = tSubGroup;
                        }

                        tProperty.Indent = tGroup.Indent;

                        if (tPropertyListInWebModel.Contains(tProp.Name))
                        {
                            tProperty.Enable = true;
                        }
                        else
                        {
                            tProperty.Enable = false;
                            tProperty.WebModelError = true;
                            tProperty.Name = "<color=orange>!!!</color>" + tProperty.Name + "<color=orange>!!!</color>";
                        }
                        tProperty.Order = tGroup.Elements.Count();
                        //tProperty.Name = "(" + tProperty.Order + ")" + tEntitled;

                        tProperty.Name = tEntitled;
                        foreach (NWDSpaceAttribute tReference in tProp.GetCustomAttributes(typeof(NWDSpaceAttribute), true))
                        {
                            tProperty.SpaceBefore += NWDConstants.kFieldMarge;
                        }
                        foreach (NWDTooltipsAttribute tReference in tProp.GetCustomAttributes(typeof(NWDTooltipsAttribute), true))
                        {
                            tProperty.Tooltips = tReference.ToolsTips;
                        }
                        foreach (NWDNotEditableAttribute tReference in tProp.GetCustomAttributes(typeof(NWDNotEditableAttribute), true))
                        {
                            tProperty.NotEditable = true;
                        }
                        foreach (NWDNotWorking tReference in tProp.GetCustomAttributes(typeof(NWDNotWorking), true))
                        {
                            tProperty.Name = "<color=red>[NOT WORKING]</color> " + tProperty.Name;
                        }
                        foreach (NWDInDevelopment tReference in tProp.GetCustomAttributes(typeof(NWDInDevelopment), true))
                        {
                            tProperty.Name = "<color=red>[IN DEV]</color> " + tProperty.Name;
                        }
                        foreach (NWDOrderAttribute tReference in tProp.GetCustomAttributes(typeof(NWDOrderAttribute), true))
                        {
                            tProperty.Order = tReference.Order;
                        }
                        if (tProp.GetCustomAttributes(typeof(NWDInspectorGroupOrderAttribute), true).Length > 0)
                        {
                            foreach (NWDInspectorGroupOrderAttribute tReference in tProp.GetCustomAttributes(typeof(NWDInspectorGroupOrderAttribute), true))
                            {
                                tProperty.Order = tReference.mGroupOrder;
                                if (string.IsNullOrEmpty(tReference.mEntitled) == false)
                                {
                                    tProperty.Name = tReference.mEntitled;
                                }
                                if (string.IsNullOrEmpty(tReference.mToolsTips) == false)
                                {
                                    tProperty.Tooltips = tReference.mToolsTips;
                                }
                                tProperty.Order = tReference.mGroupOrder;

                                PropertiesForGroupName.Add(tProperty, tReference.mGroupName);
                            }
                        }
                        else
                        {
                            tGroup.Elements.Add(tProperty);
                        }
                        bool tCertified = false;
                        foreach (NWDCertified tReference in tProp.GetCustomAttributes(typeof(NWDCertified), true))
                        {
                            tCertified = true;
                        }
                        if (tCertified == false)
                        {
                            tProperty.Name = "<i> <color=orange>•</color>" + tProperty.Name + "</i>";
                            tProperty.Tooltips += "\n\nNot certified by attribut";
                        }
                    }
                }
                tGroup = InspectorHelper;
                InspectorHelper.Elements.Sort((x, y) => x.Order.CompareTo(y.Order));
                foreach (KeyValuePair<NWDBasisHelperElement, string> tKeyValue in PropertiesForGroupName)
                {
                    if (GroupsByName.ContainsKey(tKeyValue.Value) == true)
                    {
                        //Debug.Log("try to add " + tKeyValue.Key.Name + " in group : " + tKeyValue.Value);
                        tKeyValue.Key.Indent = GroupsByName[tKeyValue.Value].Indent;
                        GroupsByName[tKeyValue.Value].Elements.Add(tKeyValue.Key);
                        GroupsByName[tKeyValue.Value].Elements.Sort((x, y) => x.Order.CompareTo(y.Order));
                    }
                    else
                    {
                        //Debug.Log("try to add " + tKeyValue.Key.Name + " in new group : " + tKeyValue.Value);
                        NWDBasisHelperElement tElement = new NWDBasisHelperElement();
                        tElement.Order = InspectorHelper.Elements.Count();
                        InspectorHelper.Elements.Add(tElement);
                        NWDBasisHelperGroup tSubGroup = new NWDBasisHelperGroup();
                        tElement.Group = tSubGroup;
                        tSubGroup.Name = tKeyValue.Value;
                        tSubGroup.Reducible = true;
                        tSubGroup.Open = false;
                        GroupsByName.Add(tSubGroup.Name, tSubGroup);
                        GroupsByName[tKeyValue.Value].Elements.Add(tKeyValue.Key);
                        GroupsByName[tKeyValue.Value].Elements.Sort((x, y) => x.Order.CompareTo(y.Order));
                    }
                }
                // order elements
                InspectorHelper.Open = true;
                InspectorHelper.Name = ClassNamePHP + "_hj444gf445675xhcjh444vk";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public static GUIContent GetGuiContent(string sReference)
        {
            GUIContent rReturn = null;
            if (BasisHelper().DatasByReference.ContainsKey(sReference))
            {
                NWDBasis<K> tObject = BasisHelper().DatasByReference[sReference] as K;
                if (string.IsNullOrEmpty(tObject.InternalKey))
                {
                    rReturn = new GUIContent("<i>no internal key</i> <color=#555555>[" + sReference + "]</color> ", tObject.PreviewTexture2D(), tObject.InternalDescription);
                }
                else
                {
                    rReturn = new GUIContent(tObject.InternalKey + " <color=#555555>[" + sReference + "]</color> ", tObject.PreviewTexture2D(), tObject.InternalDescription);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(sReference))
                {
                    rReturn = new GUIContent("none");
                }
                else
                {
                    rReturn = new GUIContent("<i>WARNING</i> [" + sReference + "]");
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float NewDrawObjectInspectorHeight()
        {
            float tY = 0;
            BasisHelper().AnalyzeForInspector();
            NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
            foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
            {
                tY += tElement.NewDrawObjectInspectorHeight(this);
            }
            tY += AddonEditorHeight();
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect NewDrawObjectInspector(Rect sInRect, bool sEditionEnable)
        {
            Rect tRect = Rect.zero;
            BasisHelper().AnalyzeForInspector();
            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDConstants.kFieldMarge;
            float tY = sInRect.position.y + NWDConstants.kFieldMarge;
            BasisHelper().AnalyzeForInspector();
            NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
            foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
            {
                tY = tElement.NewDrawObjectInspector(this, tX, tY, tWidth);
            }
            tRect = new Rect(sInRect.position.x, tY, sInRect.width, sInRect.height);
            tY += AddonEditor(tRect);
            return tRect;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif