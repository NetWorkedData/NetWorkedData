// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:21:1
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using BasicToolBox;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public override float DrawEditorTotalHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            //BTBBenchmark.Start();
            //if (RecalulateHeight == true)
            {
                TotalHeight = DrawEditorTopHeight(sNodalCard, sWidth) + DrawEditorMiddleHeight(sNodalCard, sWidth) + DrawEditorBottomHeight(sNodalCard, sWidth);
                if (sNodalCard != null)
                {
                    sNodalCard.TotalHeight += TotalHeight;
                }
                // RecalulateHeight = false;
            }
            //BTBBenchmark.Finish();
            return TotalHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float DrawEditorTopHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            // BTBBenchmark.Start();
            TopHeight = 0;
            // Top inpector
            float tY = 0;
            tY += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
            tY += NWDGUI.kFieldMarge;
            if (BasisHelper().kSyncAndMoreInformations)
            {
                EditorGUI.EndDisabledGroup();

                tY += (NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge) * 14;
            }
            tY += NWDGUI.kObjectFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            bool tInternalKeyEditable = true;
            if (GetType().GetCustomAttributes(typeof(NWDInternalKeyNotEditableAttribute), true).Length > 0)
            {
                tInternalKeyEditable = false;
            }
            if (tInternalKeyEditable == true)
            {
                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            }
            else
            {
                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            }
            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            // Tag management
            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (BasisHelper().kAccountDependent == false)
            {
                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            }
            // Toogle Dev Prepprod Prod and operation associated
            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            TopHeight += tY;
            if (sNodalCard != null)
            {
                if (sNodalCard.ParentDocument.TopCard == false)
                {
                    TopHeight = 0;
                }
            }

            // Top inpector Infos (it's constance)
            TopHeight += NWDGUI.kInspectorInternalTitle.fixedHeight;
            TopHeight += NWDGUI.kInspectorReferenceCenter.fixedHeight;
            TopHeight += NWDGUI.kIconClassWidth + NWDGUI.kFieldMarge;

            if (sNodalCard != null)
            {
                TopHeight += NWDGUI.kFieldMarge;
                TopHeight += NWDGUI.kPopupStyle.fixedHeight;
            }

            if (BasisHelper().WebModelChanged == true)
            {
                TopHeight += NWDGUI.WarningBoxHeight(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL) + NWDGUI.kFieldMarge;
            }
            if (BasisHelper().WebModelDegraded == true)
            {
                TopHeight += NWDGUI.WarningBoxHeight(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED) + NWDGUI.kFieldMarge;
            }

            if (XX > 0 || IsEnable() == false || WebserviceVersionIsValid() == false)
            {
                TopHeight += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                TopHeight += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;
                TopHeight += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                TopHeight += NWDGUI.kFieldMarge;
            }


            // add nodal area
            if (sNodalCard != null)
            {
                // add nodal area
                TopHeight += AddOnNodeDrawHeight(sWidth - NWDGUI.kFieldMarge * 2) + NWDGUI.kFieldMarge * 2;
                // reccord nodal top height
                sNodalCard.TopHeight = TopHeight;

                //Debug.Log("sNodalCard "+ sNodalCard.DataObject.Reference+ " TopHeight = " + sNodalCard.TopHeight);
            }


            // BTBBenchmark.Finish();
            return TopHeight;
            ;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float DrawEditorMiddleHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            //BTBBenchmark.Start();
            MiddleHeight = DrawInspectorHeight(sNodalCard, sWidth);
            if (sNodalCard != null)
            {
                if (sNodalCard.ParentDocument.MiddleCard == false)
                {
                    MiddleHeight = 0;
                }
                sNodalCard.MiddleHeight = MiddleHeight;
            }
            //BTBBenchmark.Finish();
            return MiddleHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float DrawEditorBottomHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            //BTBBenchmark.Start();
            BottomHeight = NWDGUI.kFieldMarge +
                NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge +
                NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge +
                NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge +
                NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            if (sNodalCard != null)
            {
                if (sNodalCard.ParentDocument.BottomCard == false)
                {
                    BottomHeight = 0;
                }
                sNodalCard.BottomHeight = BottomHeight;
            }
            //BTBBenchmark.Finish();
            return BottomHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float DrawInspectorHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            //BTBBenchmark.Start();
            //if (RecalulateHeight == true)
            {
                InspectorHeight = 0;
                BasisHelper().AnalyzeForInspector();
                NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
                foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
                {
                    InspectorHeight += tElement.NewDrawObjectInspectorHeight(this, sNodalCard, sWidth);
                }
                if (sNodalCard == null)
                {
                    InspectorHeight += AddonEditorHeight();
                }
                InspectorHeight += NWDGUI.kFieldMarge;
                //RecalulateHeight = false;
            }
            //BTBBenchmark.Finish();
            return InspectorHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddonEditorHeight()
        {
            return 0.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddOnNodeDrawHeight(float sCardWidth)
        {
            float tHeight = NWDGUI.kHelpBoxStyle.CalcHeight(new GUIContent(InternalDescription), sCardWidth);
            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif