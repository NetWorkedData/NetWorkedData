//=====================================================================================================================
//
//  ideMobi 2020©
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
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelperElement
    {
        //-------------------------------------------------------------------------------------------------------------
        public void NewDrawObjectInspectorHeightInvisible(object sObject, NWDNodeCard sNodalCard)
        {
            //NWEBenchmark.Start();
            if (Property != null)
            {
                Type tTypeOfThis = Property.PropertyType;
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                {

                    var tValue = Property.GetValue(sObject, null);
                    if (tValue == null)
                    {
                        tValue = Activator.CreateInstance(tTypeOfThis);
                    }
                    NWEDataType tNWEDataType = (NWEDataType)tValue;

                    if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)))
                    {
                        IsReconnection = true;
                        IsReconnectionMultiple = false;
                        if (sNodalCard != null)
                        {
                            NWDReferenceSimple tV = (NWDReferenceSimple)tNWEDataType;
                            tV.CreatePlotersInvisible(sNodalCard, sNodalCard.TotalHeight);
                        }
                    }
                    if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                    {
                        IsReconnection = true;
                        IsReconnectionMultiple = true;
                        if (sNodalCard != null)
                        {
                            NWDReferenceMultiple tV = (NWDReferenceMultiple)tNWEDataType;
                            tV.CreatePlotersInvisible(sNodalCard, sNodalCard.TotalHeight);
                        }
                    }
                }
            }
            if (Group != null)
            {
                if (Group.Visible == true)
                {
                    foreach (NWDBasisHelperElement tElement in Group.Elements)
                    {
                        tElement.NewDrawObjectInspectorHeightInvisible(sObject, sNodalCard);
                    }
                }
            }
            //NWEBenchmark.Finish();
        }


        //-------------------------------------------------------------------------------------------------------------
        public float NewDrawObjectInspectorHeight(object sObject, NWDNodeCard sNodalCard, float sWidth)
        {
            //NWEBenchmark.Start();
            float tY = SpaceBefore;
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
                        tY += NWDGUI.SeparatorHeight();
                    }
                    if (Information != null)
                    {
                        float tInformationsHeight = NWDGUI.kHelpBoxStyle.CalcHeight(Information, sWidth - tIndent);
                        tY += tInformationsHeight;
                    }
                    //EditorGUI.BeginDisabledGroup(tNotEditable);
                    // check this propertyheight
                    if (Property.GetCustomAttributes(typeof(NWDEnumString), true).Length > 0)
                    {
                        tY += NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge;
                    }
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
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                        {

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataType tNWEDataType = (NWEDataType)tValue;

                            if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)))
                            {
                                IsReconnection = true;
                                IsReconnectionMultiple = false;
                                if (sNodalCard != null)
                                {
                                    NWDReferenceSimple tV = (NWDReferenceSimple)tNWEDataType;
                                    tV.CreatePloters(sNodalCard, sNodalCard.TotalHeight + tY);
                                }
                            }
                            if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                            {
                                IsReconnection = true;
                                IsReconnectionMultiple = true;
                                if (sNodalCard != null)
                                {
                                    NWDReferenceMultiple tV = (NWDReferenceMultiple)tNWEDataType;
                                    tV.CreatePloters(sNodalCard, sNodalCard.TotalHeight + tY);
                                }
                            }

                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataTypeInt tNWEDataType = (NWEDataTypeInt)tValue;
                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataTypeFloat tNWEDataType = (NWEDataTypeFloat)tValue;
                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataTypeEnum tNWEDataType = (NWEDataTypeEnum)tValue;
                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                        {
                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            NWEDataTypeMask tNWEDataType = (NWEDataTypeMask)tValue;
                            float tHeight = tNWEDataType.ControlFieldHeight();
                            tY += tHeight + NWDGUI.kFieldMarge;
                        }
                        else
                        {
                            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                        }

                        if (sNodalCard != null)
                        {
                            sNodalCard.TotalHeight += tY;
                        }

                    }
                    //EditorGUI.EndDisabledGroup();
                }
            }
            if (Group != null)
            {
                //float tIndentGroup = (Group.Indent - 1) * NWDGUI.kFieldIndent;
                if (Group.Visible == true)
                {
                    if (Group.Separator == true)
                    {
                        tY += NWDGUI.SeparatorHeight();
                    }
                    if (Group.Reducible == true)
                    {
                        if (Group.Bold == true)
                        {
                            tY += NWDGUI.kBoldFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (sNodalCard != null)
                            {
                                sNodalCard.TotalHeight += tY;
                            }
                        }
                        else
                        {
                            tY += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (sNodalCard != null)
                            {
                                sNodalCard.TotalHeight += tY;
                            }
                        }
                    }
                    else
                    {
                        if (Group.Bold == true)
                        {
                            tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (sNodalCard != null)
                            {
                                sNodalCard.TotalHeight += tY;
                            }
                        }
                        else
                        {
                            tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                            if (sNodalCard != null)
                            {
                                sNodalCard.TotalHeight += tY;
                            }
                        }
                    }
                    if (Group.IsDrawable() == true)
                    {
                        foreach (NWDBasisHelperElement tElement in Group.Elements)
                        {
                            tY += tElement.NewDrawObjectInspectorHeight(sObject, sNodalCard, sWidth);
                        }
                    }
                    else
                    {

                        float tAdjust = 0;

                        if (sNodalCard != null)
                        {
                            if (Separator == true)
                            {
                                tAdjust += NWDGUI.SeparatorHeight();
                            }
                            tAdjust += NWDGUI.kBoldFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                            sNodalCard.TotalHeight -= tAdjust;
                        }
                        foreach (NWDBasisHelperElement tElement in Group.Elements)
                        {
                            tElement.NewDrawObjectInspectorHeightInvisible(sObject, sNodalCard);
                        }
                        if (sNodalCard != null)
                        {
                            sNodalCard.TotalHeight += tAdjust;
                        }
                    }
                }
            }
            return tY;
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif