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
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Reflection;
using System.IO;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeCard
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass DataObject;
        //-------------------------------------------------------------------------------------------------------------
        public Vector2 Position;
        public int Line = 0;
        public int Column = 0;
        public NWDNodeDocument ParentDocument;
        public List<NWDNodePloter> PloterList = new List<NWDNodePloter>();
        public List<Vector2> PlotsList = new List<Vector2>();
        public Texture2D ClassTexture;
        //-------------------------------------------------------------------------------------------------------------
        public Rect CardRect;
        public float TotalHeight = 0.0F;
        //public float CardTopHeight = 0;
        //public float TopHeight = 0.0F;
        //public float CardNodalHeight = 0;
        //public float MiddleHeight = 0.0F;
        //public float EditorAddOnHeight = 0.0F;
        //public float BottomHeight = 0.0F;

        public Rect TotalRect;

        public Rect HeaderRect;
        public Rect InformationsRect;
        public Rect NodalRect;
        public Rect PropertiesRect;
        public Rect AddonRect;
        public Rect ActionRect;

        //-------------------------------------------------------------------------------------------------------------
        public void SetData(NWDTypeClass sData)
        {
            if (NWDBasisHelper.FindTypeInfos(sData.GetType()).AllDatabaseIsLoaded())
            {
                DataObject = sData;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass GetData()
        {
            return DataObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Analyze(NWDNodeDocument sDocument)
        {
            //NWDBenchmark.Start();
            ParentDocument = sDocument;
            sDocument.ColumnMaxCount(Column);
            // I analyze the properties of data.
            if (DataObject != null)
            {
                DataObject.NodeCardAnalyze(this);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDNodeCard> AddPropertyResult(object[] sObjectsArray)
        {
            //NWDBenchmark.Start();
            List<NWDNodeCard> rResult = new List<NWDNodeCard>();
            foreach (NWDTypeClass tObject in sObjectsArray)
            {
                // Card exist?
                if (tObject != null)
                {
                    Type tTypeOfThis = tObject.GetType();
                if (ParentDocument.AnalyzeStyleClasses[tTypeOfThis.Name] == NWDClasseAnalyseEnum.Show ||
                    ParentDocument.AnalyzeStyleClasses[tTypeOfThis.Name] == NWDClasseAnalyseEnum.Analyze
                    )
                {
                        bool tDataAlReadyShow = false;
                        NWDNodeCard tCard = null;
                        foreach (NWDNodeCard tOldCard in ParentDocument.AllCards)
                        {
                            if (tOldCard.DataObject == tObject)
                            {
                                tDataAlReadyShow = true;
                                tCard = tOldCard;
                                break;
                            }
                        }
                        if (tDataAlReadyShow == false)
                        {
                            tCard = new NWDNodeCard();
                            tCard.Column = Column + 1;
                            tCard.Line = ParentDocument.GetNextLine(tCard);
                            tCard.SetData(tObject);
                            rResult.Add(tCard);
                            ParentDocument.AllCards.Add(tCard);
                            if (ParentDocument.MatrixCards.ContainsKey(tCard.Column) == false)
                            {
                                ParentDocument.MatrixCards.Add(tCard.Column, new List<NWDNodeCard>());
                            }
                            ParentDocument.MatrixCards[tCard.Column].Add(tCard);
                        }
                    }
                }
            }
            //NWDBenchmark.Finish();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float ReEvaluateLayout(float sY)
        {
            //NWDBenchmark.Start();
            float tX = ParentDocument.DocumentMarge + ParentDocument.CardMarge + Column * (NWDGUI.kNodeCardWidth + ParentDocument.CardMarge);
            if (ParentDocument.FixeMargePreference == true)
            {
                tX = ParentDocument.CardMarge + Column * (NWDGUI.kNodeCardWidth + ParentDocument.CardMarge);
            }
            float tY = sY;
            Position = new Vector2(tX, tY);
            PloterList.Clear();
            PlotsList.Clear();
            TotalHeight = 0;
            CardRect = new Rect(tX, tY, NWDGUI.kNodeCardWidth, DataObject.DrawEditorTotalHeight(this, NWDGUI.kNodeCardWidth));
            //NWDBenchmark.Finish();
            return CardRect.height;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawCard(Rect sVisibleRect)
        {
            //NWDBenchmark.Start();
            if (sVisibleRect.Overlaps(CardRect))
            {
                GUI.Box(NWDGUI.UnMargeAll(CardRect), " ", EditorStyles.helpBox);
                DataObject.DrawEditor(CardRect, false, this, this.ParentDocument.EditorWindow);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawConnection(List<NWDNodeCard> sAllCards)
        {
            PlotsList.Clear();
            // NWDBenchmark.Start();
            float Tangent = ParentDocument.CardMarge;
            Vector2 tOrign = new Vector2(Position.x - NWDGUI.kFieldMarge, Position.y + NWDGUI.kEditWidthMini * 3);
            Vector2 tOrignPa = new Vector2(Position.x - Tangent, Position.y);
            bool tOriginIsUse = false;
            foreach (NWDNodeCard tCard in sAllCards)
            {
                if (tCard != this)
                {
                    foreach (NWDNodePloter tPlot in tCard.PloterList)
                    {
                        if (tPlot.Reference == this.DataObject.Reference)
                        {
                            if (ParentDocument.DrawPropertiesArea == false)
                            {
                                tPlot.Point.y = -NWDGUI.kFieldMarge - NWDGUI.kEditWidthMini;
                            }
                            Vector2 tFinal = new Vector2(tCard.Position.x + NWDGUI.kFieldMarge * 1.5F + NWDGUI.kNodeCardWidth + tPlot.Point.x, tPlot.Point.y + tCard.PropertiesRect.y);

                            Vector2 tFinalPa = new Vector2(tFinal.x + Tangent, tFinal.y);
                            // Draw line
                            if (DataObject.IsEnable() == true)
                            {
                                Handles.DrawBezier(tOrign, tFinal, tOrignPa, tFinalPa, NWDGUI.kNodeLineColor, NWDGUI.kImageBezierTexture, 2.0F);
                            }
                            else
                            {
                                Handles.DrawBezier(tOrign, tFinal, tOrignPa, tFinalPa, NWDGUI.kRowColorTrash, NWDGUI.kImageBezierTexture, 2.0F);

                            }
                            PlotsList.Add(tFinal);
                            if (tOriginIsUse == false)
                            {
                                tOriginIsUse = true;
                                PlotsList.Add(tOrign);
                            }
                        }
                    }
                }
            }
            // NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawForwardPlot()
        {
            //NWDBenchmark.Start();
            foreach (Vector2 tPlot in PlotsList)
            {
                Handles.color = NWDGUI.kNodeLineColor;
                Handles.DrawSolidDisc(tPlot, Vector3.forward, NWDGUI.kFieldMarge);
                Handles.color = NWDGUI.kNodeOverLineColor;
                Handles.DrawSolidDisc(tPlot, Vector3.forward, NWDGUI.kFieldMarge - 1.0F);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
