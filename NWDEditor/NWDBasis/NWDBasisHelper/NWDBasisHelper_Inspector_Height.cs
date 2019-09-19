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
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelperElement
    {
        //-------------------------------------------------------------------------------------------------------------
        public void NewDrawObjectInspectorHeightInvisible(object sObject, NWDNodeCard sNodalCard)
        {
            //BTBBenchmark.Start();
            if (Property != null)
            {
                Type tTypeOfThis = Property.PropertyType;
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                {

                    var tValue = Property.GetValue(sObject, null);
                    if (tValue == null)
                    {
                        tValue = Activator.CreateInstance(tTypeOfThis);
                    }
                    BTBDataType tBTBDataType = (BTBDataType)tValue;

                    if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)))
                    {
                        IsReconnection = true;
                        IsReconnectionMultiple = false;
                        if (sNodalCard != null)
                        {
                            NWDReferenceSimple tV = (NWDReferenceSimple)tBTBDataType;
                            tV.CreatePlotersInvisible(sNodalCard, sNodalCard.TotalHeight);
                        }
                    }
                    if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                    {
                        IsReconnection = true;
                        IsReconnectionMultiple = true;
                        if (sNodalCard != null)
                        {
                            NWDReferenceMultiple tV = (NWDReferenceMultiple)tBTBDataType;
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
            //BTBBenchmark.Finish();
        }


        //-------------------------------------------------------------------------------------------------------------
        public float NewDrawObjectInspectorHeight(object sObject, NWDNodeCard sNodalCard, float sWidth)
        {
            //BTBBenchmark.Start();
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
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                        {

                            var tValue = Property.GetValue(sObject, null);
                            if (tValue == null)
                            {
                                tValue = Activator.CreateInstance(tTypeOfThis);
                            }
                            BTBDataType tBTBDataType = (BTBDataType)tValue;

                            if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)))
                            {
                                IsReconnection = true;
                                IsReconnectionMultiple = false;
                                if (sNodalCard != null)
                                {
                                    NWDReferenceSimple tV = (NWDReferenceSimple)tBTBDataType;
                                    tV.CreatePloters(sNodalCard, sNodalCard.TotalHeight + tY);
                                }
                            }
                            if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                            {
                                IsReconnection = true;
                                IsReconnectionMultiple = true;
                                if (sNodalCard != null)
                                {
                                    NWDReferenceMultiple tV = (NWDReferenceMultiple)tBTBDataType;
                                    tV.CreatePloters(sNodalCard, sNodalCard.TotalHeight + tY);
                                }
                            }

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
            //BTBBenchmark.Finish();
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