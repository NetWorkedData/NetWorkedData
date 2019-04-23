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
            TotalSize(new Rect(sNodalCard.Position.x, sNodalCard.Position.y, NWDGUI.kNodeCardWidth,0), false, sNodalCard);
            return TotalRect.height;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TotalSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            //BTBBenchmark.Start();
            HeaderSize(sInRect, sWithScrollview, sNodalCard);
            InformationsSize(sInRect, sWithScrollview, sNodalCard);
            NodalSize(sInRect, sWithScrollview, sNodalCard);
            ContentSize(sInRect, sWithScrollview, sNodalCard);
            ActionSize(sInRect, sWithScrollview, sNodalCard);

            if (sWithScrollview == true)
            {
                ScroolViewSize(sInRect, sWithScrollview, sNodalCard);
            }
            TotalRect = NWDGUI.AssemblyArea(HeaderRect, ActionRect);
            if (sNodalCard != null)
            {
                sNodalCard.TotalRect = TotalRect;
            }
            //if (sNodalCard != null)
            //{
            //    sNodalCard.CardRect.height = TotalRect.height;
            //}
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void HeaderSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            float tH = 0;

            if (sNodalCard != null)
            {
                tH += NWDGUI.kFieldMarge*2 + NWDGUI.kPopupStyle.fixedHeight;
            }
            tH += NWDGUI.kInspectorInternalTitle.fixedHeight;
            tH += NWDGUI.kInspectorReferenceCenter.fixedHeight;
            tH += NWDGUI.kIconClassWidth;
            if (BasisHelper().WebModelChanged == true)
            {
                tH += NWDGUI.WarningBoxHeight(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL) + NWDGUI.kFieldMarge *2;
            }
            if (BasisHelper().WebModelDegraded == true)
            {
                tH += NWDGUI.WarningBoxHeight(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED) + NWDGUI.kFieldMarge *2;
            }
            HeaderRect = new Rect(sInRect.x, sInRect.y, sInRect.width, tH);
            if (sNodalCard != null)
            {
                sNodalCard.HeaderRect = HeaderRect;
            }

            // EditorGUI.DrawRect(HeaderRect, Color.red);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InformationsSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            bool tDraw = true;
            float tH = 0;
            if (sNodalCard != null)
            {
                tDraw = sNodalCard.ParentDocument.DrawInformationsArea;
            }
            if (tDraw == true)
            {
                tH = 0;
                // Top inpector
                tH += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                if (BasisHelper().kSyncAndMoreInformations)
                {
                    tH += (NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge) * 14;
                }
                tH += NWDGUI.kObjectFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                bool tInternalKeyEditable = true;
                if (GetType().GetCustomAttributes(typeof(NWDInternalKeyNotEditableAttribute), true).Length > 0)
                {
                    tInternalKeyEditable = false;
                }
                if (tInternalKeyEditable == true)
                {
                    tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                }
                else
                {
                    tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                }
                tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                // Tag management
                tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                if (BasisHelper().kAccountDependent == false)
                {
                    tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                }
                // Toogle Dev Prepprod Prod and operation associated
                tH += NWDGUI.kTextFieldStyle.fixedHeight;

                if (XX > 0 || IsEnable() == false || WebserviceVersionIsValid() == false)
                {
                    tH += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    tH += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;
                    tH += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    tH += NWDGUI.kFieldMarge;
                }
            }
            InformationsRect = new Rect(sInRect.x, HeaderRect.y + HeaderRect.height + NWDGUI.kFieldMarge, sInRect.width, tH);

            if (sNodalCard != null)
            {
                sNodalCard.InformationsRect = InformationsRect;
            }
            // EditorGUI.DrawRect(InformationsRect, Color.magenta);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void NodalSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            float tH = 0;
            if (sNodalCard != null)
            {
                tH = AddOnNodeDrawHeight(sInRect.width - 4 * NWDGUI.kFieldMarge) + NWDGUI.kFieldMarge * 3;
                if (InformationsRect.height > 0)
                {
                    NodalRect = new Rect(sInRect.x, InformationsRect.y + InformationsRect.height + NWDGUI.kFieldMarge, sInRect.width, tH);
                }
                else
                {
                    NodalRect = new Rect(sInRect.x, InformationsRect.y, sInRect.width, tH);
                }
            }
            else
            {
                NodalRect = new Rect(sInRect.x, InformationsRect.y + InformationsRect.height, sInRect.width, 0);
            }
            if (sNodalCard != null)
            {
                sNodalCard.NodalRect = NodalRect;
            }
            // EditorGUI.DrawRect(NodalRect, Color.yellow);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PropertiesSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            bool tDraw = true;
            float tH = 0;
            if (sNodalCard != null)
            {
                tDraw = sNodalCard.ParentDocument.DrawPropertiesArea;
            }
                BasisHelper().AnalyzeForInspector();
                NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
                foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
                {
                    tH += tElement.NewDrawObjectInspectorHeight(this, sNodalCard, sInRect.width);
                }
                tH += NWDGUI.kFieldMarge;
            if (tDraw == false)
            {
                tH = 0;
            }
            if (sWithScrollview == true)
            {
                PropertiesRect = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, sInRect.width, tH);
            }
            else
            {
                PropertiesRect = new Rect(NodalRect.x, NodalRect.y + NodalRect.height + NWDGUI.kFieldMarge*2, sInRect.width, tH);
                //EditorGUI.DrawRect(PropertiesRect, Color.green);
            }
            if (sNodalCard != null)
            {
                sNodalCard.PropertiesRect = PropertiesRect;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddOnSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            bool tDraw = true;
            float tH = 0;
            if (sNodalCard != null)
            {
                tDraw = sNodalCard.ParentDocument.DrawAddOnArea;
            }
            if (tDraw == true)
            {
                tH = AddonEditorHeight(sInRect.width);
            }
            if (PropertiesRect.height > 0)
            {
                AddOnRect = new Rect(PropertiesRect.x, PropertiesRect.y + PropertiesRect.height + NWDGUI.kFieldMarge, sInRect.width, tH);
            }
            else
            {
                AddOnRect = new Rect(PropertiesRect.x, PropertiesRect.y , sInRect.width, tH);
            }
            if (sNodalCard != null)
            {
                sNodalCard.AddOnRect = AddOnRect;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ActionSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            bool tDraw = true;
            float tH = 0;
            if (sNodalCard != null)
            {
                sWithScrollview = false;
                tDraw = sNodalCard.ParentDocument.DrawActionArea;
            }
            if (tDraw == true)
            {
                tH = NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 4, false) + NWDGUI.kFieldMarge;
            }
            if (sWithScrollview == true)
            {
                ActionRect = new Rect(sInRect.x, sInRect.y + sInRect.height - tH, sInRect.width, tH);
            }
            else
            {
                if (AddOnRect.height > 0)
                {
                    ActionRect = new Rect(AddOnRect.x, AddOnRect.y + AddOnRect.height + NWDGUI.kFieldMarge, AddOnRect.width, tH);
                }
                else
                {
                    ActionRect = new Rect(AddOnRect.x, AddOnRect.y, AddOnRect.width, tH);
                }
            }
            if (sNodalCard != null)
            {
                sNodalCard.ActionRect = ActionRect;
            }
            // EditorGUI.DrawRect(ActionRect, Color.blue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ContentSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {

            PropertiesSize(sInRect, sWithScrollview, sNodalCard);
            AddOnSize(sInRect, sWithScrollview, sNodalCard);

            ContentRect = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, PropertiesRect.width, PropertiesRect.height + AddOnRect.height + NWDGUI.kFieldMarge*2);

        }
        //-------------------------------------------------------------------------------------------------------------
        public void ScroolViewSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            ScrollRect = new Rect(sInRect.x, NodalRect.y + NodalRect.height + NWDGUI.kFieldMarge, sInRect.width + NWDGUI.kFieldMarge, sInRect.height +(sInRect.y- NodalRect.y) - ActionRect.height - NWDGUI.kFieldMarge);

            // IF SPACE IS NOT ENOUGH : Add scrollbar           
            if (ScrollRect.height < ContentRect.height)
            {
                if (ScrollRect.width < ContentRect.width)
                {
                }
                else
                {
                    sInRect.width -= NWDGUI.kScrollbar;
                    ContentSize(sInRect, sWithScrollview, sNodalCard);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public float DrawCardTopHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    //BTBBenchmark.Start();
        //    float rCardTopHeight = 0;
        //    if (sNodalCard != null)
        //    {
        //        rCardTopHeight = NWDGUI.kEditButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
        //        sNodalCard.CardTopHeight = rCardTopHeight;
        //    }
        //    //BTBBenchmark.Finish();
        //    return rCardTopHeight;
        //}

        ////-------------------------------------------------------------------------------------------------------------
        //public override float DrawEditorTopHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    // BTBBenchmark.Start();
        //    TopHeight = 0;
        //    // Top inpector
        //    float tY = 0;
        //    tY += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    tY += NWDGUI.kFieldMarge;
        //    if (BasisHelper().kSyncAndMoreInformations)
        //    {
        //        EditorGUI.EndDisabledGroup();

        //        tY += (NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge) * 14;
        //    }
        //    tY += NWDGUI.kObjectFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    bool tInternalKeyEditable = true;
        //    if (GetType().GetCustomAttributes(typeof(NWDInternalKeyNotEditableAttribute), true).Length > 0)
        //    {
        //        tInternalKeyEditable = false;
        //    }
        //    if (tInternalKeyEditable == true)
        //    {
        //        tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    }
        //    else
        //    {
        //        tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    }
        //    tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    // Tag management
        //    tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    if (BasisHelper().kAccountDependent == false)
        //    {
        //        tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    }
        //    // Toogle Dev Prepprod Prod and operation associated
        //    tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

        //    TopHeight += tY;
        //    if (sNodalCard != null)
        //    {
        //        if (sNodalCard.ParentDocument.DrawInformationsArea == false)
        //        {
        //            TopHeight = 0;
        //        }
        //    }

        //    // Top inpector Infos (it's constance)
        //    TopHeight += NWDGUI.kInspectorInternalTitle.fixedHeight;
        //    TopHeight += NWDGUI.kInspectorReferenceCenter.fixedHeight;
        //    TopHeight += NWDGUI.kIconClassWidth + NWDGUI.kFieldMarge;

        //    if (sNodalCard != null)
        //    {
        //        TopHeight += NWDGUI.kFieldMarge;
        //        TopHeight += NWDGUI.kPopupStyle.fixedHeight;
        //    }

        //    if (BasisHelper().WebModelChanged == true)
        //    {
        //        TopHeight += NWDGUI.WarningBoxHeight(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL) + NWDGUI.kFieldMarge;
        //    }
        //    if (BasisHelper().WebModelDegraded == true)
        //    {
        //        TopHeight += NWDGUI.WarningBoxHeight(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED) + NWDGUI.kFieldMarge;
        //    }

        //    if (XX > 0 || IsEnable() == false || WebserviceVersionIsValid() == false)
        //    {
        //        TopHeight += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
        //        TopHeight += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;
        //        TopHeight += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
        //        TopHeight += NWDGUI.kFieldMarge;
        //    }


        //    // add nodal area
        //    if (sNodalCard != null)
        //    {
        //        // add nodal area
        //        AddonNodalHeightResult = AddOnNodeDrawHeight(sWidth - NWDGUI.kFieldMarge * 2);
        //        if (AddonNodalHeightResult < 0)
        //        {
        //            AddonNodalHeightResult = 20;
        //        }
        //        TopHeight += AddonNodalHeightResult + NWDGUI.kFieldMarge * 2;
        //        // reccord nodal top height
        //        sNodalCard.TopHeight = TopHeight;

        //        //Debug.Log("sNodalCard "+ sNodalCard.DataObject.Reference+ " TopHeight = " + sNodalCard.TopHeight);
        //    }


        //    // BTBBenchmark.Finish();
        //    return TopHeight;
        //    ;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override float DrawEditorMiddleHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    //BTBBenchmark.Start();
        //    MiddleHeight = DrawInspectorHeight(sNodalCard, sWidth);

        //    MiddleHeight += DrawEditorAddonHeight(sNodalCard, sWidth);
        //    if (sNodalCard != null)
        //    {
        //        if (sNodalCard.ParentDocument.DrawPropertiesArea == false)
        //        {
        //            MiddleHeight = 0;
        //        }
        //        sNodalCard.MiddleHeight = MiddleHeight;
        //    }
        //    //BTBBenchmark.Finish();
        //    return MiddleHeight;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public float DrawEditorAddonHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    //BTBBenchmark.Start();
        //    EditorHeight = 0;
        //    bool tAddonEditor = true;
        //    if (sNodalCard != null)
        //    {
        //        tAddonEditor = sNodalCard.ParentDocument.DrawAddOnArea;
        //    }
        //    if (tAddonEditor == true)
        //    {
        //        AddonEditorHeightResult = AddonEditorHeight();
        //        if (AddonEditorHeightResult > 0)
        //        {
        //            EditorHeight += AddonEditorHeightResult + NWDGUI.kFieldMarge * 2;
        //        }
        //    }
        //    //BTBBenchmark.Finish();
        //    return EditorHeight;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override float DrawEditorBottomHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    //BTBBenchmark.Start();
        //    BottomHeight = NWDGUI.kFieldMarge +
        //        NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge +
        //        NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge +
        //        NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge +
        //        NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

        //    if (sNodalCard != null)
        //    {
        //        if (sNodalCard.ParentDocument.DrawActionArea == false)
        //        {
        //            BottomHeight = 0;
        //        }
        //        sNodalCard.BottomHeight = BottomHeight;
        //    }
        //    //BTBBenchmark.Finish();
        //    return BottomHeight;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override float DrawInspectorHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    //BTBBenchmark.Start();
        //    //if (RecalulateHeight == true)
        //    {
        //        InspectorHeight = 0;
        //        BasisHelper().AnalyzeForInspector();
        //        NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
        //        foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
        //        {
        //            InspectorHeight += tElement.NewDrawObjectInspectorHeight(this, sNodalCard, sWidth);
        //        }

        //        InspectorHeight += NWDGUI.kFieldMarge;
        //        //RecalulateHeight = false;
        //    }
        //    //BTBBenchmark.Finish();
        //    return InspectorHeight;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddonEditorHeight(float sWidth) // add width!!!
        {
            return 0.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 0.0f;
            //float tHeight = NWDGUI.kHelpBoxStyle.CalcHeight(new GUIContent(InternalDescription), sCardWidth);
            //return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif