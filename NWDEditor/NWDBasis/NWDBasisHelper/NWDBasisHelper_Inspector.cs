// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:36
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
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
                tY += NWDGUI.kFieldMarge * 2;
            }
            if (Property != null)
            {
                bool tDraw = true;
                bool tNotEditable = NotEditable;
                foreach (NWDIf tReference in Property.GetCustomAttributes(typeof(NWDIf), true))
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
                    if (Property.GetCustomAttributes(typeof(NWDEnumString), true).Length > 0)
                    {
                        tY += NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
                    //else if (Property.GetCustomAttributes(typeof(NWDEnumAttribute), true).Length > 0)
                    //{
                    //    tY += NWDGUI.tPopupdStyle.fixedHeight + NWDGUI.kFieldMarge;
                    //}
                    else
                    {
                        Type tTypeOfThis = Property.PropertyType;
                        if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                        {
                            if (Property.GetCustomAttributes(typeof(NWDLongString), true).Length > 0)
                            {
                                tY += NWDGUI.kTextFieldStyle.fixedHeight * NWDGUI.kLongString + NWDGUI.kFieldMarge;
                            }
                            else if (Property.GetCustomAttributes(typeof(NWDVeryLongString), true).Length > 0)
                            {
                                tY += NWDGUI.kTextFieldStyle.fixedHeight * NWDGUI.kVeryLongString + NWDGUI.kFieldMarge;
                            }
                            else
                            {
                                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                            }
                        }
                        else if (tTypeOfThis.IsEnum)
                        {
                            tY += NWDGUI.kEnumStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            tY += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(int))
                        {
                            tY += NWDGUI.kIntFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(long))
                        {
                            tY += NWDGUI.kLongFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            tY += NWDGUI.kFloatFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            tY += NWDGUI.kDoubleFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
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
                            tY += tHeight + NWDGUI.kFieldMarge;
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
                            tY += tHeight + NWDGUI.kFieldMarge;
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
                            tY += tHeight + NWDGUI.kFieldMarge;
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
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeMask)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeMask tBTBDataType = (BTBDataTypeMask)tValue;
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else
                        {
                            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
            if (Group != null)
            {
                if (Group.Separator == true)
                {
                    tY += NWDGUI.kFieldMarge * 2;
                }
                if (Group.Reducible == true)
                {
                    if (Group.Bold == true)
                    {
                        tY += NWDGUI.kBoldFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
                    else
                    {
                        tY += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
                }
                else
                {
                    if (Group.Bold == true)
                    {
                        tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
                    else
                    {
                        tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
                }
                tY += NWDGUI.kFieldMarge;
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
                tY += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;
            }

            if (Property != null)
            {
                bool tDraw = true;
                bool tNotEditable = NotEditable;
                foreach (NWDIf tReference in Property.GetCustomAttributes(typeof(NWDIf), true))
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
                        float tInformationsHeight = NWDGUI.kHelpBoxStyle.CalcHeight(Information, tWidth);
                        GUI.Label(new Rect(tX, tY, tWidth, tInformationsHeight), Information, NWDGUI.kHelpBoxStyle);
                        tY += tInformationsHeight + NWDGUI.kFieldMarge;
                    }
                    // prepare positions and size
                    float tXField = sX + EditorGUIUtility.labelWidth;
                    float tWidthField = tWidth - EditorGUIUtility.labelWidth;
                    // create rects for label and field
                    float tIndent = Indent * NWDGUI.kFieldIndent;
                    Rect tEntitlementRect = new Rect(tX + tIndent, tY, EditorGUIUtility.labelWidth - tIndent, NWDGUI.kPropertyEntitlementStyle.fixedHeight);
                    Rect tFieldRect = new Rect(tXField, tY, tWidthField, NWDGUI.kPropertyEntitlementStyle.fixedHeight);

                    // begin editable field?
                    EditorGUI.BeginDisabledGroup(tNotEditable);

                    //swicth case of type
                    if (Property.GetCustomAttributes(typeof(NWDEnumString), true).Length > 0)
                    {
                        EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);
                        tFieldRect.height = NWDGUI.kPopupStyle.fixedHeight;
                        NWDEnumString tInfo = Property.GetCustomAttributes(typeof(NWDEnumString), true)[0] as NWDEnumString;
                        string[] tV = tInfo.mEnumString;
                        string tValue = (string)Property.GetValue(sObject, null);
                        int tValueInt = Array.IndexOf<string>(tV, tValue);
                        //remove EditorGUI.indentLevel to draw next controller without indent 
                        //int tIndentLevel = EditorGUI.indentLevel;
                        EditorGUI.indentLevel = 0;
                        int tValueIntNext = EditorGUI.Popup(tFieldRect, string.Empty, tValueInt, tV, NWDGUI.kPopupStyle);
                        tY += NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge;
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
                    //    EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDGUI.tTextFieldStyle.fixedHeight), Content());

                    //    //remove EditorGUI.indentLevel to draw next controller without indent 
                    //    int tIndentLevel = EditorGUI.indentLevel;
                    //    EditorGUI.indentLevel = 0;

                    //    int tValueIntNext = EditorGUI.Popup(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth - EditorGUIUtility.labelWidth, NWDGUI.tPopupdStyle.fixedHeight), string.Empty, tValueInt, tV, NWDGUI.tPopupdStyle);
                    //    tY += NWDGUI.tPopupdStyle.fixedHeight + NWDGUI.kFieldMarge;
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

                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);
                            float tH = 0;
                            if (Property.GetCustomAttributes(typeof(NWDLongString), true).Length > 0)
                            {
                                tH += NWDGUI.kTextFieldStyle.fixedHeight * NWDGUI.kLongString + NWDGUI.kFieldMarge;
                            }
                            else if (Property.GetCustomAttributes(typeof(NWDVeryLongString), true).Length > 0)
                            {
                                tH += NWDGUI.kTextFieldStyle.fixedHeight * NWDGUI.kVeryLongString + NWDGUI.kFieldMarge;
                            }
                            else
                            {
                                tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                            }
                            string tValue = Property.GetValue(sObject, null) as string;
                            if (string.IsNullOrEmpty(tValue) == false)
                            {
                                tValue = NWDToolbox.TextUnprotect(tValue);
                            }
                            tFieldRect.height = tH;
                            string tValueNext = EditorGUI.TextField(tFieldRect, tValue, NWDGUI.kTextFieldStyle);
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
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);
                            tFieldRect.height = NWDGUI.kEnumStyle.fixedHeight;
                            Enum tValue = Property.GetValue(sObject, null) as Enum;
                            Enum tValueNext = tValue;
                            if (Property.GetCustomAttributes(typeof(NWDFlagsEnum), true).Length > 0)
                            {
                                tValueNext = EditorGUI.EnumFlagsField(tFieldRect, tValue, NWDGUI.kEnumStyle);
                            }
                            else
                            {
                                tValueNext = EditorGUI.EnumPopup(tFieldRect, tValue, NWDGUI.kEnumStyle);
                            }
                            tY += NWDGUI.kEnumStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tPopupdStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);
                            tFieldRect.height = NWDGUI.kToggleStyle.fixedHeight;

                            bool tValue = (bool)Property.GetValue(sObject, null);
                            bool tValueNext = EditorGUI.Toggle(tFieldRect, tValue, NWDGUI.kToggleStyle);
                            tY += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tToggleStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(int))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);
                            tFieldRect.height = NWDGUI.kIntFieldStyle.fixedHeight;
                            int tValue = (int)Property.GetValue(sObject, null);
                            int tValueNext = tValue;
                            if (Property.GetCustomAttributes(typeof(NWDIntSlider), true).Length > 0)
                            {
                                NWDIntSlider tSlider = Property.GetCustomAttributes(typeof(NWDIntSlider), true)[0] as NWDIntSlider;
                                tValueNext = EditorGUI.IntSlider(tFieldRect, tValue, tSlider.mMin, tSlider.mMax);
                            }
                            else
                            {
                                tValueNext = EditorGUI.IntField(tFieldRect, tValue, NWDGUI.kIntFieldStyle);
                            }
                            tY += NWDGUI.kIntFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(long))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);
                            tFieldRect.height = NWDGUI.kLongFieldStyle.fixedHeight;
                            long tValue = (long)Property.GetValue(sObject, null);
                            long tValueNext = tValue;
                            tValueNext = EditorGUI.LongField(tFieldRect, tValue, NWDGUI.kLongFieldStyle);
                            tY += NWDGUI.kLongFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);
                            tFieldRect.height = NWDGUI.kFloatFieldStyle.fixedHeight;
                            float tValue = (float)Property.GetValue(sObject, null);
                            float tValueNext = tValue;
                            if (Property.GetCustomAttributes(typeof(NWDFloatSlider), true).Length > 0)
                            {
                                NWDFloatSlider tSlider = Property.GetCustomAttributes(typeof(NWDFloatSlider), true)[0] as NWDFloatSlider;
                                tValueNext = EditorGUI.Slider(tFieldRect, tValue, tSlider.mMin, tSlider.mMax);
                            }
                            else
                            {
                                tValueNext = EditorGUI.FloatField(tFieldRect, tValue, NWDGUI.kFloatFieldStyle);
                            }
                            tY += NWDGUI.kFloatFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tFloatFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);
                            tFieldRect.height = NWDGUI.kDoubleFieldStyle.fixedHeight;

                            double tValue = (double)Property.GetValue(sObject, null);
                            double tValueNext = EditorGUI.DoubleField(tFieldRect, tValue, NWDGUI.kDoubleFieldStyle);
                            tY += NWDGUI.kDoubleFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (tValueNext != tValue)
                            {
                                Property.SetValue(sObject, tValueNext, null);
                                //rNeedBeUpdate = true;
                            }
                            //tY += NWDBasisHelper.tFloatFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                        {

                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataType tBTBDataType = tValue as BTBDataType;
                            BTBDataType tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight),
                                                                                      "  ", Tooltips) as BTBDataType;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tBTBDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeInt tBTBDataType = tValue as BTBDataTypeInt;
                            BTBDataTypeInt tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kIntFieldStyle.fixedHeight),
                                                                                     "  ", Tooltips) as BTBDataTypeInt;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tBTBDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeFloat tBTBDataType = tValue as BTBDataTypeFloat;
                            BTBDataTypeFloat tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kFloatFieldStyle.fixedHeight),
                                                                                     "  ", Tooltips) as BTBDataTypeFloat;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tBTBDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeEnum)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeEnum tBTBDataType = tValue as BTBDataTypeEnum;
                            BTBDataTypeEnum tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kFloatFieldStyle.fixedHeight),
                                                                                     "  ", Tooltips) as BTBDataTypeEnum;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                ((BTBDataTypeEnum)tValue).Value = tBTBDataTypeNext.Value;
                                Property.SetValue(sObject, tValue, null);
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeMask)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataTypeMask tBTBDataType = tValue as BTBDataTypeMask;
                            BTBDataTypeMask tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kFloatFieldStyle.fixedHeight),
                                                                                     " ", Tooltips) as BTBDataTypeMask;

                            if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                            {
                                ((BTBDataTypeMask)tValue).Value = tBTBDataTypeNext.Value;
                                Property.SetValue(sObject, tValue, null);
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tBTBDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else
                        {
                            string tValue = Property.GetValue(sObject, null) as string;
                            EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), Name, tValue, NWDGUI.kTextFieldStyle);
                            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            if (Group != null)
            {
                if (Group.Separator == true)
                {
                    tY += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;
                }
                if (Group.Reducible == true)
                {
                    bool tActualDraw = Group.IsDrawable();
                    if (Group.Bold == true)
                    {
                        tActualDraw = EditorGUI.Foldout(new Rect(tX, tY, tWidth, NWDGUI.kBoldFoldoutStyle.fixedHeight), tActualDraw, Group.Content(), NWDGUI.kBoldFoldoutStyle);
                        tY += NWDGUI.kBoldFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
                    else
                    {
                        tActualDraw = EditorGUI.Foldout(new Rect(tX, tY, tWidth, NWDGUI.kFoldoutStyle.fixedHeight), tActualDraw, Group.Content(), NWDGUI.kFoldoutStyle);
                        tY += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
                    Group.SetDrawable(tActualDraw);
                }
                else
                {
                    if (Group.Bold == true)
                    {
                        EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), Group.Content(), NWDGUI.kBoldLabelStyle);
                        tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
                    else
                    {
                        EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDGUI.kLabelStyle.fixedHeight), Group.Content(), NWDGUI.kLabelStyle);
                        tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
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
                NWDGUI.LoadStyles();
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
                            tProperty.Tooltips = "Property : " + tProp.Name + "\nType : " + tProp.PropertyType.Name.Replace("`1", "<" + tProp.PropertyType.GenericTypeArguments[0].Name + ">") + "\n\n";
                        }
                        else
                        {
                            tProperty.Tooltips = "Property : " + tProp.Name + "\nType : " + tProp.PropertyType.Name + "\n\n";
                        }
                        tProperty.Property = tProp;

                        foreach (NWDInspectorGroupReset tInsideReference in tProp.GetCustomAttributes(typeof(NWDInspectorGroupReset), true))
                        {
                            tGroup.Elements.Sort((x, y) => x.Order.CompareTo(y.Order));
                            tGroup = InspectorHelper;
                        }
                        foreach (NWDInspectorGroupEnd tInsideReference in tProp.GetCustomAttributes(typeof(NWDInspectorGroupEnd), true))
                        {
                            tGroup.Elements.Sort((x, y) => x.Order.CompareTo(y.Order));
                            tGroup = tGroup.FromGroup;
                        }
                        foreach (NWDInspectorSeparator tInsideReference in tProp.GetCustomAttributes(typeof(NWDInspectorSeparator), true))
                        {
                            tProperty.Separator = true;
                        }

                        foreach (NWDInformation tInsideReference in tProp.GetCustomAttributes(typeof(NWDInformation), true))
                        {
                            tProperty.Information = tInsideReference.Content();
                        }
                        //foreach (NWDGroupSeparatorAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDGroupSeparatorAttribute), true))
                        //{
                        //    tProperty.Separator = true;
                        //}

                        foreach (NWDInspectorGroupStart tInsideReference in tProp.GetCustomAttributes(typeof(NWDInspectorGroupStart), true))
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
                        foreach (NWDSpace tReference in tProp.GetCustomAttributes(typeof(NWDSpace), true))
                        {
                            tProperty.SpaceBefore += NWDGUI.kFieldMarge;
                        }
                        foreach (NWDTooltips tReference in tProp.GetCustomAttributes(typeof(NWDTooltips), true))
                        {
                            tProperty.Tooltips+= tReference.ToolsTips;
                        }
                        foreach (NWDNotEditable tReference in tProp.GetCustomAttributes(typeof(NWDNotEditable), true))
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
                        foreach (NWDOrder tReference in tProp.GetCustomAttributes(typeof(NWDOrder), true))
                        {
                            tProperty.Order = tReference.Order;
                        }
                        if (tProp.GetCustomAttributes(typeof(NWDInspectorGroupOrder), true).Length > 0)
                        {
                            foreach (NWDInspectorGroupOrder tReference in tProp.GetCustomAttributes(typeof(NWDInspectorGroupOrder), true))
                            {
                                tProperty.Order = tReference.mGroupOrder;
                                if (string.IsNullOrEmpty(tReference.mEntitled) == false)
                                {
                                    tProperty.Name = tReference.mEntitled;
                                }
                                if (string.IsNullOrEmpty(tReference.mToolsTips) == false)
                                {
                                    tProperty.Tooltips+= tReference.mToolsTips;
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
        public GUIContent New_GetGuiContent(string sReference)
        {
            GUIContent rReturn = null;
            if (DatasByReference.ContainsKey(sReference))
            {
                NWDTypeClass tObject = DatasByReference[sReference] as NWDTypeClass;
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        //public static GUIContent GetGuiContent(string sReference)
        //{
        //    GUIContent rReturn = null;
        //    if (BasisHelper().DatasByReference.ContainsKey(sReference))
        //    {
        //        NWDBasis<K> tObject = BasisHelper().DatasByReference[sReference] as K;
        //        if (string.IsNullOrEmpty(tObject.InternalKey))
        //        {
        //            rReturn = new GUIContent("<i>no internal key</i> <color=#555555>[" + sReference + "]</color> ", tObject.PreviewTexture2D(), tObject.InternalDescription);
        //        }
        //        else
        //        {
        //            rReturn = new GUIContent(tObject.InternalKey + " <color=#555555>[" + sReference + "]</color> ", tObject.PreviewTexture2D(), tObject.InternalDescription);
        //        }
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(sReference))
        //        {
        //            rReturn = new GUIContent("none");
        //        }
        //        else
        //        {
        //            rReturn = new GUIContent("<i>WARNING</i> [" + sReference + "]");
        //        }
        //    }
        //    return rReturn;
        //}
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
            float tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDGUI.kFieldMarge;
            float tY = sInRect.position.y + NWDGUI.kFieldMarge;
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