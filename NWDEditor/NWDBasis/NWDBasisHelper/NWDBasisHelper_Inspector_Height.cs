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
#if UNITY_EDITOR
using System;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
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
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }


        //-------------------------------------------------------------------------------------------------------------
        public float NewDrawObjectInspectorHeight(object sObject, NWDNodeCard sNodalCard, float sWidth)
        {
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
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