//=====================================================================================================================
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
//=====================================================================================================================
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
//using BasicToolBox;
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
        public bool Visible = true;
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
            //NWEBenchmark.Start();
            string tKey = Key();
            //EditorPrefs.HasKey(tKey);
            EditorPrefs.SetBool(tKey, sOpen);
            //NWEBenchmark.Finish();
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
            return new GUIContent(Name /*+ "(" + Indent + ")"*/, Tooltips);
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
        public bool IsReconnection = false;
        public bool IsReconnectionMultiple = false;
        //-------------------------------------------------------------------------------------------------------------
        public GUIContent Content()
        {
            return new GUIContent(Name, Tooltips);
        }
        //-------------------------------------------------------------------------------------------------------------
        public float NewDrawObjectInspector(object sObject, NWDNodeCard sNodalCard, float sX, float sY, float sWidth)
        {
            //NWEBenchmark.Start();
            float tWidth = sWidth;
            float tX = sX;
            float tY = sY;

            // EditorGUI.indentLevel = Indent;

            float tIndent = Indent * NWDGUI.kFieldIndent;
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

                    if (Separator == true)
                    {
                        tY += NWDGUI.Separator(new Rect(tX + tIndent, tY, tWidth, 0)).height;
                    }
                    //  draw informations? and add height in size operation?
                    if (Information != null)
                    {
                        float tInformationsHeight = NWDGUI.kHelpBoxStyle.CalcHeight(Information, tWidth - tIndent);
                        GUI.Label(new Rect(tX + tIndent, tY, tWidth - tIndent, tInformationsHeight), Information, NWDGUI.kHelpBoxStyle);
                        tY += tInformationsHeight + NWDGUI.kFieldMarge;
                    }
                    // prepare positions and size
                    float tXField = sX + EditorGUIUtility.labelWidth;
                    float tWidthField = tWidth - EditorGUIUtility.labelWidth;
                    // create rects for label and field
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
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                        {

                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataType tNWEDataType = tValue as NWEDataType;
                            NWEDataType tNWEDataTypeNext = tNWEDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), "  ", tNotEditable, Tooltips) as NWEDataType;

                            if (tNWEDataTypeNext.Value != tNWEDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tNWEDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataTypeInt tNWEDataType = tValue as NWEDataTypeInt;
                            NWEDataTypeInt tNWEDataTypeNext = tNWEDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kIntFieldStyle.fixedHeight),
                                                                                     "  ", tNotEditable, Tooltips) as NWEDataTypeInt;

                            if (tNWEDataTypeNext.Value != tNWEDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tNWEDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataTypeFloat tNWEDataType = tValue as NWEDataTypeFloat;
                            NWEDataTypeFloat tNWEDataTypeNext = tNWEDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kFloatFieldStyle.fixedHeight),
                                                                                     "  ", tNotEditable, Tooltips) as NWEDataTypeFloat;

                            if (tNWEDataTypeNext.Value != tNWEDataType.Value)
                            {
                                //Debug.Log("change in "+tTypeOfThis.Name);
                                Property.SetValue(sObject, tNWEDataTypeNext, null);
                                //rNeedBeUpdate = true;
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataTypeEnum tNWEDataType = tValue as NWEDataTypeEnum;
                            NWEDataTypeEnum tNWEDataTypeNext = tNWEDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kFloatFieldStyle.fixedHeight),
                                                                                     "  ", tNotEditable, Tooltips) as NWEDataTypeEnum;

                            if (tNWEDataTypeNext.Value != tNWEDataType.Value)
                            {
                                ((NWEDataTypeEnum)tValue).Value = tNWEDataTypeNext.Value;
                                Property.SetValue(sObject, tValue, null);
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                        {
                            EditorGUI.LabelField(tEntitlementRect, Content(), NWDGUI.kPropertyEntitlementStyle);

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataTypeMask tNWEDataType = tValue as NWEDataTypeMask;
                            NWEDataTypeMask tNWEDataTypeNext = tNWEDataType.ControlField(new Rect(tX, tY, tWidth, NWDGUI.kFloatFieldStyle.fixedHeight),
                                                                                     " ", tNotEditable, Tooltips) as NWEDataTypeMask;

                            if (tNWEDataTypeNext.Value != tNWEDataType.Value)
                            {
                                ((NWEDataTypeMask)tValue).Value = tNWEDataTypeNext.Value;
                                Property.SetValue(sObject, tValue, null);
                                NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                            }
                            float tHeight = tNWEDataType.ControlFieldHeight();
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
                float tIndentGroup = (Group.Indent - 1) * NWDGUI.kFieldIndent;
                if (Group.Visible == true)
                {
                    if (Group.Separator == true)
                    {
                        tY += NWDGUI.Separator(new Rect(tX + tIndentGroup, tY, tWidth, 0)).height;
                    }
                    if (Group.Reducible == true)
                    {
                        bool tActualDraw = Group.IsDrawable();
                        if (Group.Bold == true)
                        {
                            tActualDraw = EditorGUI.Foldout(new Rect(tX + tIndentGroup, tY, tWidth, NWDGUI.kBoldFoldoutStyle.fixedHeight), tActualDraw, Group.Content(), NWDGUI.kBoldFoldoutStyle);
                            tY += NWDGUI.kBoldFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else
                        {
                            tActualDraw = EditorGUI.Foldout(new Rect(tX + tIndentGroup, tY, tWidth, NWDGUI.kFoldoutStyle.fixedHeight), tActualDraw, Group.Content(), NWDGUI.kFoldoutStyle);
                            tY += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        if (tActualDraw != Group.IsDrawable())
                        {
                            Group.SetDrawable(tActualDraw);
                            NWDNodeEditor.ReAnalyzeIfNecessary(sObject);
                        }
                    }
                    else
                    {
                        if (Group.Bold == true)
                        {
                            EditorGUI.LabelField(new Rect(tX + tIndentGroup, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), Group.Content(), NWDGUI.kBoldLabelStyle);
                            tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                        else
                        {
                            EditorGUI.LabelField(new Rect(tX + tIndentGroup, tY, tWidth, NWDGUI.kLabelStyle.fixedHeight), Group.Content(), NWDGUI.kLabelStyle);
                            tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }
                    }
                    if (Group.IsDrawable() == true)
                    {
                        foreach (NWDBasisHelperElement tElement in Group.Elements)
                        {
                            tY = tElement.NewDrawObjectInspector(sObject, sNodalCard, sX, tY, sWidth);
                        }
                    }
                }
            }
            //NWEBenchmark.Finish();
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
            //NWEBenchmark.Start();
            if (LoadStyle == false)
            {
                LoadStyle = true;
                NWDGUI.LoadStyles();
               string[] PropertiesInWebModel = PropertiesOrderArray(LastWebBuild).ToArray();
                //Debug.Log(string.Join(", ", PropertiesInWebModel));
                //List<string> tListB = new List<string>(WebModelSQLOrder[LastWebBuild].Split(new char[] { ',' }));

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

                        foreach (NWDInspectorGroupStart tInsideReference in tProp.GetCustomAttributes(typeof(NWDInspectorGroupStart), true))
                        {
                            NWDBasisHelperElement tElement = new NWDBasisHelperElement();
                            tElement.Order = tGroup.Elements.Count();
                            tGroup.Elements.Add(tElement);
                            NWDBasisHelperGroup tSubGroup = new NWDBasisHelperGroup();
                            if (tInsideReference.mGroupName == NWD.K_INSPECTOR_BASIS)
                            {
                                tSubGroup.Visible = false;
                            }
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
                        //foreach (NWDInspectorRename tReference in tProp.GetCustomAttributes(typeof(NWDInspectorRename), true))
                        //{
                        //    if (tReference.Entity == tProp.Name)
                        //    {
                        //        tProperty.Name = "dddd"+tReference.NewName;
                        //    }
                        //}

                        foreach (NWDSpace tReference in tProp.GetCustomAttributes(typeof(NWDSpace), true))
                        {
                            tProperty.SpaceBefore += NWDGUI.kFieldMarge;
                        }
                        foreach (NWDTooltips tReference in tProp.GetCustomAttributes(typeof(NWDTooltips), true))
                        {
                            tProperty.Tooltips += tReference.ToolsTips;
                        }
                        foreach (NWDEntitled tReference in tProp.GetCustomAttributes(typeof(NWDEntitled), true))
                        {
                            tProperty.Name = tReference.Entitled;
                        }

                        foreach (NWDPropertyRename tReference in ClassType.GetCustomAttributes(typeof(NWDPropertyRename), true))
                        {
                            if (tReference.Entity == tProp.Name)
                            {
                                tProperty.Name = "" + tReference.NewName + " <color=red>*</color>";
                                if (string.IsNullOrEmpty(tReference.ToolsTips) == false)
                                {
                                    tProperty.Tooltips += "\n----\n" + tReference.ToolsTips;
                                }
                            }
                        }
                        foreach (MethodInfo tMethod in ClassType.GetMethods())
                        {
                            foreach (NWDInspectorInformations tReference in tMethod.GetCustomAttributes(typeof(NWDInspectorInformations), true))
                            {
                                //Debug.Log("tReference.Entity " + tReference.Entity + " " + tMethod.Name + "() check with "+tProp.Name+"");
                                if (tReference.Entity == tProp.Name)
                                {
                                    //Debug.Log("tReference.Entity " + tReference.Entity + " OKAY");
                                    tProperty.Name = "" + tProperty.Name + tReference.NewName + " <color=red>*</color>";
                                    tProperty.Tooltips += "\n----\n - use " + tMethod.Name + "() \n" + tReference.ToolsTips;
                                }
                            }
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
                                    tProperty.Tooltips += tReference.mToolsTips;
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
                            tProperty.Tooltips += "\n - Not certified by attribut";
                        }

                        if (PropertiesInWebModel.Contains(tProp.Name) == false)
                        //if (tListB.Contains(tProp.Name) == false)
                        {
                            tProperty.Name = "!!! " + tProperty.Name + "";
                            tProperty.NotEditable = true;
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
                        tKeyValue.Key.Indent = 1;
                        NWDBasisHelperElement tElement = new NWDBasisHelperElement();
                        tElement.Order = InspectorHelper.Elements.Count();
                        InspectorHelper.Elements.Add(tElement);
                        NWDBasisHelperGroup tSubGroup = new NWDBasisHelperGroup();
                        tElement.Indent = 1;
                        tElement.Group = tSubGroup;
                        tSubGroup.Indent = 1;
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public GUIContent GetGUIContent(string sReference)
        {
            //NWEBenchmark.Start();
            GUIContent rReturn = null;
            if (DatasByReference.ContainsKey(sReference))
            {
                NWDTypeClass tObject = DatasByReference[sReference] as NWDTypeClass;
                if (string.IsNullOrEmpty(tObject.InternalKey))
                {
                    //rReturn = new GUIContent("<i>no internal key</i> <color=#555555>[" + sReference + "]</color> ", tObject.PreviewTexture2D(), tObject.InternalDescription);
                    rReturn = new GUIContent("<i>no internal key</i> <color=#555555>[" + sReference + "]</color> ", tObject.InternalDescription);
                }
                else
                {
                    //rReturn = new GUIContent(tObject.InternalKey + " <color=#555555>[" + sReference + "]</color> ", tObject.PreviewTexture2D(), tObject.InternalDescription);
                    rReturn = new GUIContent(tObject.InternalKey + " <color=#555555>[" + sReference + "]</color> ", tObject.InternalDescription);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(sReference))
                {
                    rReturn = new GUIContent(NWDConstants.kFieldNone);
                }
                else
                {
                    rReturn = new GUIContent("<i>WARNING</i> [" + sReference + "]");
                }
            }
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public GUIContent GetGUIPreview(string sReference)
        {
            GUIContent rReturn = null;
            if (DatasByReference.ContainsKey(sReference))
            {
                NWDTypeClass tObject = DatasByReference[sReference] as NWDTypeClass;
                rReturn = new GUIContent(tObject.PreviewTexture2D());
            }
            else
            {
                rReturn = new GUIContent("");
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool HasPreview(string sReference)
        {
            bool rReturn = false;
            if (sReference != null)
            {
                if (DatasByReference.ContainsKey(sReference))
                {
                    NWDTypeClass tObject = DatasByReference[sReference] as NWDTypeClass;
                    rReturn = tObject.PreviewTexture2D() != null;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif