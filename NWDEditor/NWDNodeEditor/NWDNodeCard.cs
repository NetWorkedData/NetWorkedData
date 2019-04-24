// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:56
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Reflection;
using System.IO;
using UnityEditor;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
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
        public Rect AddOnRect;
        public Rect ActionRect;

        //-------------------------------------------------------------------------------------------------------------
        public void SetData(NWDTypeClass sData)
        {
            if (NWDBasisHelper.FindTypeInfos(sData.GetType()).DatabaseIsLoaded())
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
            //BTBBenchmark.Start();
            ParentDocument = sDocument;
            sDocument.ColumnMaxCount(Column);
            // I analyze the properties of data.
            if (DataObject != null)
            {
                DataObject.NodeCardAnalyze(this);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDNodeCard> AddPropertyResult(object[] sObjectsArray)
        {
            //BTBBenchmark.Start();
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
                        bool tDataAllReadyShow = false;
                        NWDNodeCard tCard = null;
                        foreach (NWDNodeCard tOldCard in ParentDocument.AllCards)
                        {
                            if (tOldCard.DataObject == tObject)
                            {
                                tDataAllReadyShow = true;
                                tCard = tOldCard;
                                break;
                            }
                        }
                        if (tDataAllReadyShow == false)
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
            //BTBBenchmark.Finish();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float ReEvaluateLayout(float sY)
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
            return CardRect.height;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawCard(Rect sVisibleRect)
        {
            //BTBBenchmark.Start();
            if (sVisibleRect.Overlaps(CardRect))
            {
                GUI.Box(NWDGUI.UnMargeAll(CardRect), " ", EditorStyles.helpBox);
                DataObject.DrawEditor(CardRect, false, this);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawConnection(List<NWDNodeCard> sAllCards)
        {
            PlotsList.Clear();
            // BTBBenchmark.Start();
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
            // BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawForwardPlot()
        {
            //BTBBenchmark.Start();
            foreach (Vector2 tPlot in PlotsList)
            {
                Handles.color = NWDGUI.kNodeLineColor;
                Handles.DrawSolidDisc(tPlot, Vector3.forward, NWDGUI.kFieldMarge);
                Handles.color = NWDGUI.kNodeOverLineColor;
                Handles.DrawSolidDisc(tPlot, Vector3.forward, NWDGUI.kFieldMarge - 1.0F);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
