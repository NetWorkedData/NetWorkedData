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
            //BTBBenchmark.Start();
            TopHeight = 0;

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

            // TODO : RealCalcule!!!!
            TopHeight += 272;

            // add nodal area
            if (sNodalCard != null)
            {
                // add nodal area
                TopHeight += AddOnNodeDrawHeight(NWDGUI.kNodeCardWidth) + NWDGUI.kFieldMarge *2;
                // reccord nodal top height
                sNodalCard.TopHeight = TopHeight;
            }
            //BTBBenchmark.Finish();
            return TopHeight;;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float DrawEditorMiddleHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            //BTBBenchmark.Start();
            MiddleHeight = DrawInspectorHeight(sNodalCard, sWidth);
            if (sNodalCard != null)
            {
                sNodalCard.MiddleHeight = MiddleHeight;
            }
            //BTBBenchmark.Finish();
            return MiddleHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float DrawEditorBottomHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            //BTBBenchmark.Start();
            BottomHeight = NWDGUI.kFieldMarge+
                NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge +
                NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge +
                NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge +
                NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            if (sNodalCard != null)
            {
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
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif