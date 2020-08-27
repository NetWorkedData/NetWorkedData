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
                tH = AddOnNodeDrawHeight(sInRect.width - 4 * NWDGUI.kFieldMarge) + NWDGUI.kFieldMarge * 4;

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
                PropertiesRect = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, sInRect.width, tH);
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
                tH = NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 8, false) + NWDGUI.kFieldMarge;
            }
            if (sWithScrollview == true)
            {
                ActionRect = new Rect(sInRect.x, sInRect.y + sInRect.height - tH + NWDGUI.kFieldMarge, sInRect.width, tH);
            }
            else
            {
                if (AddOnRect.height > 0)
                {
                    ActionRect = new Rect(AddOnRect.x, AddOnRect.y + AddOnRect.height + NWDGUI.kFieldMarge, AddOnRect.width, tH);
                }
                else
                {
                    ActionRect = new Rect(AddOnRect.x, AddOnRect.y + NWDGUI.kFieldMarge, AddOnRect.width, tH);
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
            AddOnSize(sInRect, sWithScrollview, sNodalCard);
            ContentRect = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, PropertiesRect.width, PropertiesRect.height + AddOnRect.height + NWDGUI.kFieldMarge*2);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ScroolViewSize(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            ScrollRect = new Rect(sInRect.x, NodalRect.y + NodalRect.height + NWDGUI.kFieldMarge, sInRect.width + NWDGUI.kFieldMarge, sInRect.height +(sInRect.y- NodalRect.y) - ActionRect.height - NWDGUI.kFieldMarge);         
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
        public virtual float AddonEditorHeight(float sWidth)
        {
            return 0.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 0.0f;
            //return NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kLabelStyle.fixedHeight + (NWDGUI.kFieldMarge)*2;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif