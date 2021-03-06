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
using UnityEngine;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        public override float DrawEditorTotalHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            if (sNodalCard != null)
            {
                TotalSize(new Rect(sNodalCard.Position.x, sNodalCard.Position.y, NWDGUI.kNodeCardWidth,0), false, sNodalCard);
            }
            else
            {
                TotalSize(new Rect(0, 0, sWidth, 0), false, sNodalCard);
            }
            return TotalRect.height;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TotalSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            //NWDBenchmark.Trace();
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
            if (BasisHelper().TablePrefix != BasisHelper().TablePrefixOld)
            {
                tH += NWDGUI.WarningBoxHeight(new Rect(0, 0, NWDGUI.kNodeCardWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_PREFIXE) + NWDGUI.kFieldMarge * 2;
            }
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
                //tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                // Tag management
                tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                //if (BasisHelper().kAccountDependent == false)
                    if (BasisHelper().TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                    {
                    tH += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                }
                // Toogle Dev Prepprod Prod and operation associated
                tH += NWDGUI.kTextFieldStyle.fixedHeight;

                if (XX > 0 || IsEnable() == false || WebserviceVersionIsValid() == false || IntegrityIsValid() == false)
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public void NodalSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            float tH = 0;
            if (sNodalCard != null)
            {
                tH = AddonNodalHeight(sInRect.width - 4 * NWDGUI.kFieldMarge) + NWDGUI.kFieldMarge * 4;

                //tH += NWDGUI.kBoldLabelStyle.CalcHeight(new GUIContent(InternalKey), sInRect.width);
                //tH += NWDGUI.kBoldLabelStyle.CalcHeight(new GUIContent(InternalDescription), sInRect.width);

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
                PropertiesRect = new Rect(0, NWDGUI.kFieldMarge, sInRect.width, tH);
            }
            else
            {
                PropertiesRect = new Rect(NodalRect.x, NodalRect.y + NodalRect.height + NWDGUI.kFieldMarge*2, sInRect.width, tH);
            }
            if (sNodalCard != null)
            {
                sNodalCard.PropertiesRect = PropertiesRect;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddonSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            bool tDraw = true;
            float tH = 0;
            if (sNodalCard != null)
            {
                tDraw = sNodalCard.ParentDocument.DrawAddonArea;
            }
            if (tDraw == true)
            {
                tH = AddonEditorHeight(sInRect.width);
            }
            if (PropertiesRect.height > 0)
            {
                AddonRect = new Rect(PropertiesRect.x, PropertiesRect.y + PropertiesRect.height + NWDGUI.kFieldMarge, sInRect.width, tH);
            }
            else
            {
                AddonRect = new Rect(PropertiesRect.x, PropertiesRect.y , sInRect.width, tH);
            }

            //AddOnRect.x -= NWDGUI.kFieldMarge;
            //AddOnRect.width += NWDGUI.kFieldMarge*2;

            if (sNodalCard != null)
            {
                sNodalCard.AddonRect = AddonRect;
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
                tH = NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 8, false) + NWDGUI.kFieldMarge;
            }
            if (sWithScrollview == true)
            {
                ActionRect = new Rect(sInRect.x, sInRect.y + sInRect.height - tH + NWDGUI.kFieldMarge, sInRect.width, tH);
            }
            else
            {
                if (AddonRect.height > 0)
                {
                    ActionRect = new Rect(AddonRect.x, AddonRect.y + AddonRect.height + NWDGUI.kFieldMarge, AddonRect.width, tH);
                }
                else
                {
                    ActionRect = new Rect(AddonRect.x, AddonRect.y + NWDGUI.kFieldMarge, AddonRect.width, tH);
                }
            }
            if (sNodalCard != null)
            {
                sNodalCard.ActionRect = ActionRect;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ContentSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            PropertiesSize(sInRect, sWithScrollview, sNodalCard);
            AddonSize(sInRect, sWithScrollview, sNodalCard);
            //ContentRect = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, PropertiesRect.width, PropertiesRect.height + AddOnRect.height + NWDGUI.kFieldMarge*2);
            ContentRect = new Rect(-NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, PropertiesRect.width, PropertiesRect.height + AddonRect.height + NWDGUI.kFieldMarge * 2);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ScroolViewSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            ScrollRect = new Rect(sInRect.x- NWDGUI.kFieldMarge, NodalRect.y + NodalRect.height + NWDGUI.kFieldMarge, sInRect.width + 2*NWDGUI.kFieldMarge, sInRect.height +(sInRect.y- NodalRect.y) - ActionRect.height);         
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
        protected Rect[,] EditorMatrix;
        protected int EditorMatrixIndex = 0;
        protected int EditorMatrixLine = 2;
        protected int EditorMatrixColunm = 1;
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddonEditorHeight(float sWidth)
        {
            return LayoutEditorHeight;
            //return NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, EditorMatrixLine);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected Rect[,] EditorNodalMatrix;
        protected int EditorNodalMatrixIndex = 0;
        protected int EditorNodalMatrixLine = 2;
        protected int EditorNodalMatrixColunm = 1;
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddonNodalHeight(float sCardWidth)
        {
            return LayoutNodalHeight;
            //return NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, EditorNodalMatrixLine);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
