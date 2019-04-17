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
        public float DrawObjectInspectorHeight(NWDNodeCard sNodalCard)
        {
            //BTBBenchmark.Start();
            float tY = 0;
            BasisHelper().AnalyzeForInspector();
            NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
            foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
            {
                tY += tElement.NewDrawObjectInspectorHeight(this, sNodalCard);
            }
            tY += AddonEditorHeight();
            //BTBBenchmark.Finish();
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float New_DrawObjectEditorHeight(NWDNodeCard sNodalCard)
        {
            float tY = 0;
            //BTBBenchmark.Start();
            //Todo Calculate the real height
            float tHeightTop = 312;

            if (BasisHelper().WebModelChanged == true)
            {
                tHeightTop += NWDGUI.WarningBox(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL).height + NWDGUI.kFieldMarge;
            }

            if (BasisHelper().WebModelDegraded == true)
            {
                tHeightTop += NWDGUI.WarningBox(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED).height + NWDGUI.kFieldMarge;
            }

            /*if (tTestIntegrity == false)
            {
                tHeightTop += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                tHeightTop += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;
                tHeightTop += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                tHeightTop += NWDGUI.kFieldMarge;

            }
            else */if (XX > 0 || IsEnable() == false || WebserviceVersionIsValid()==false)
            {
                tHeightTop += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                tHeightTop += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;
                tHeightTop += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                tHeightTop += NWDGUI.kFieldMarge;
            }

            if (sNodalCard != null)
            {
                sNodalCard.TotalHeightDynamique += tHeightTop;
            }

            tY += tHeightTop;
            tY += New_DrawObjectInspectorHeight(sNodalCard);

            //float tHeightBottom = 60;
            //tY += tHeightBottom;
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float New_DrawObjectInspectorHeight(NWDNodeCard sNodalCard)
        {
            //BTBBenchmark.Start();
            float tY = 0;
            if (sNodalCard != null)
            {

            }
            NWDGUI.LoadStyles();
            Type tType = ClassType();
            tY += DrawObjectInspectorHeight(sNodalCard);
            tY += NWDGUI.kFieldMarge;
            tY += AddonEditorHeight();
            //BTBBenchmark.Finish();
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddonEditorHeight()
        {
            return 00.0f;
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