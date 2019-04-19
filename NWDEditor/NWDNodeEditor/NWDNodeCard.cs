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
        public List<NWDNodeConnection> ConnectionList = new List<NWDNodeConnection>();
        public Vector2 Position;
        public Vector2 CirclePosition;
        public Vector2 PositionTangent;
        public int Line = 0;
        public int Column = 0;
        public NWDNodeDocument ParentDocument;
        public List<NWDNodePloter> PloterList = new List<NWDNodePloter>();
        public List<Vector2> PlotsList = new List<Vector2>();
        //public Color InformationsColor = Color.white;

        public Texture2D ClassTexture;
        //-------------------------------------------------------------------------------------------------------------
        //float tX;
        //float tY;
        //public float Width;
        //public float Height;
        //public float InformationsHeight;
        //string Infos = string.Empty;
        //string InfosCard = string.Empty;
        //string InfosCardCustom = string.Empty;
        Rect CardRect;

        Vector2 CardTopLeft;
        Vector2 CardTopRight;
        Vector2 CardBottomLeft;
        Vector2 CardBottomRight;


        public float TotalHeight = 0.0F;
        public float TopHeight = 0.0F;
        public float MiddleHeight = 0.0F;
        public float BottomHeight = 0.0F;
        //Rect CardTypeRect;
        //Rect CardReferenceRect;
        //Rect CardInternalKeyRect;
        //Rect InfoRect;
        //Rect IconRect;
        //Rect InfoUsableRect;
        //public string TypeString;
        //public string ReferenceString;
        //public string InternalKeyString;
        //public string Informations;
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
            //Debug.Log("NWDNodeCard Analyze()");
            ParentDocument = sDocument;
            sDocument.ColumnMaxCount(Column);
            // I analyze the properties of data.
            if (DataObject != null)
            {
                DataObject.NodeCardAnalyze(this);
                //// TODO : Change to remove invoke!
                //Type tType = Data.GetType();
                ////var tMethodInfo = tType.GetMethod("NodeCardAnalyze", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                //MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicInstance(tType, NWDConstants.M_NodeCardAnalyze);

                //if (tMethodInfo != null)
                //{
                //    tMethodInfo.Invoke(Data, new object[] { this });
                //}
            }
            else
            {
                //Debug.Log("NWDNodeCard Analyze() NO DATA (null)");
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDNodeCard> AddPropertyResult(PropertyInfo sProperty, NWDNodeConnectionReferenceType sConType, object[] sObjectsArray, bool sButtonAdd)
        {
            //BTBBenchmark.Start();
            // Debug.Log("NWDNodeCard AddPropertyResult()");
            List<NWDNodeCard> rResult = new List<NWDNodeCard>();
            NWDNodeConnection tNewConnection = null;
            if (ParentDocument.UsedOnlyProperties == true && sObjectsArray.Length == 0)
            {
            }
            else
            {
                if (/*ParentDocument.ReGroupProperties == true || */(ParentDocument.UsedOnlyProperties == false && sObjectsArray.Length == 0))
                {
                    //foreach (NWDNodeConnection tConnection in ConnectionList)
                    //{
                    //    if (tConnection.PropertyName == sProperty.Name)
                    //    {
                    //        tNewConnection = tConnection;
                    //        break;
                    //    }
                    //}
                    //if (tNewConnection == null)
                    //{
                    tNewConnection = new NWDNodeConnection();
                    tNewConnection.PropertyName = sProperty.Name;
                    tNewConnection.ConType = sConType;
                    ConnectionList.Add(tNewConnection);
                    tNewConnection.Parent = this;
                    //}
                    tNewConnection.Property = sProperty;
                    if (sProperty.GetCustomAttributes(typeof(NWDNotEditable), true).Length > 0)
                    {
                        tNewConnection.AddButton = false;
                    }
                    else
                    {
                        tNewConnection.AddButton = sButtonAdd;
                    }
                    //int tLine = 0;
                }
                foreach (NWDTypeClass tObject in sObjectsArray)
                {
                    //if (ParentDocument.ReGroupProperties == false)
                    {
                        tNewConnection = new NWDNodeConnection();
                        tNewConnection.PropertyName = sProperty.Name;
                        tNewConnection.ConType = sConType;
                        tNewConnection.Parent = this;
                        tNewConnection.Property = sProperty;
                        if (sProperty.GetCustomAttributes(typeof(NWDNotEditable), true).Length > 0)
                        {
                            tNewConnection.AddButton = false;
                        }
                        else
                        {
                            tNewConnection.AddButton = sButtonAdd;
                        }
                        ConnectionList.Add(tNewConnection);
                    }
                    // Card exist?
                    if (tObject != null)
                    {
                        bool tDataAllReadyShow = false;
                        NWDNodeCard tCard = null;
                        NWDNodeConnectionLine tCardLine = new NWDNodeConnectionLine();
                        tCardLine.ConType = sConType;
                        foreach (NWDNodeCard tOldCard in ParentDocument.AllCards)
                        {
                            if (tOldCard.DataObject == tObject)
                            {
                                tDataAllReadyShow = true;
                                tCard = tOldCard;
                                if (tCard.Column < Column)
                                {
                                    tCardLine.Style = NWDNodeConnectionType.OldCard;
                                }
                                else
                                {
                                    tCardLine.Style = NWDNodeConnectionType.FuturCard;
                                }
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

                            tCardLine.Style = NWDNodeConnectionType.Valid;
                        }
                        tCardLine.Child = tCard;
                        tNewConnection.ChildrenList.Add(tCardLine);
                    }
                    else
                    {
                        // broken link?!
                        NWDNodeConnectionLine tCardLine = new NWDNodeConnectionLine();
                        tCardLine.ConType = sConType;
                        tCardLine.Style = NWDNodeConnectionType.Broken;
                        tNewConnection.ChildrenList.Add(tCardLine);
                    }
                }
                ParentDocument.PropertyCount(ConnectionList.Count);
            }
            //BTBBenchmark.Finish();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void ReEvaluateHeightWidth()
        //{
        //    //BTBBenchmark.Start();
        //    //tY = NWDGUI.kNodeCardHeight + Line * (ParentDocument.Height + NWDGUI.kNodeCardHeight);
        //    //Height = NWDGUI.kFieldMarge + (ParentDocument.HeightLabel + NWDGUI.kFieldMarge) * 3 + InformationsHeight + NWDGUI.kFieldMarge + (ParentDocument.HeightProperty + NWDGUI.kFieldMarge) * ConnectionList.Count;

        //    //Height = DataObject.New_DrawObjectInspectorHeight(ParentDocument);

        //    //BTBBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public float ReEvaluateLayout(float sY)
        {
            //BTBBenchmark.Start();
            //Infos = "";//Data.GetType().AssemblyQualifiedName;
            //InfosCard = " " + Column + " x " + Line + "\n";
            //InfosCardCustom = "";
            //Width = 250;
            //Height = 250;
            float tX = ParentDocument.DocumentMarge + ParentDocument.CardMarge + Column * (NWDGUI.kNodeCardWidth +ParentDocument.CardMarge);
            float tY = sY;

            PloterList.Clear();
            PlotsList.Clear();
            TotalHeight = 0;
            CardRect = new Rect(tX, tY, NWDGUI.kNodeCardWidth, DataObject.DrawEditorTotalHeight(this, NWDGUI.kNodeCardWidth));


            CardTopLeft = new Vector2(CardRect.xMin, CardRect.yMax);
            CardTopRight = new Vector2(CardRect.xMax, CardRect.yMax);
            CardBottomLeft = new Vector2(CardRect.xMin, CardRect.yMin);
            CardBottomRight = new Vector2(CardRect.xMax, CardRect.yMin);

            Position = new Vector2(tX, tY);
            CirclePosition = new Vector2(tX + 0, tY + NWDGUI.kIconWidth / 2.0F + NWDGUI.kFieldMarge);
            PositionTangent = new Vector2(CirclePosition.x - NWDGUI.kNodeCardHeight, CirclePosition.y);
            //sY += CardRect.height;
            int tPropertyCounter = 0;
            //foreach (NWDNodeConnection tConnection in ConnectionList)
            //{
            //    //Debug.Log("NWDNodeCard DrawCard() draw connection");
            //    //tConnection.Rectangle = new Rect(tX + NWDGUI.kFieldMarge,
            //    //                                tY + ParentDocument.HeightLabel * 3 + NWDGUI.kFieldMarge * 5 + InformationsHeight + (NWDGUI.kFieldMarge + ParentDocument.HeightProperty) * tPropertyCounter,
            //    //                                Width - NWDGUI.kFieldMarge * 2 - NWDGUI.kEditWidthMini / 2.0F,
            //    //                                ParentDocument.HeightProperty);

            //    //////GUI.Label(new Rect(tX + 2, tY + ParentDocument.HeightInformations + 1 + ParentDocument.HeightProperty * tPropertyCounter - 2, tWidth - 4, ParentDocument.HeightProperty - 2), tConnection.PropertyName);
            //    //tConnection.Position = new Vector2(tX + Width - NWDGUI.kFieldMarge,
            //    //                                  tConnection.Rectangle.y + ParentDocument.HeightProperty / 2.0F);


            //    //tConnection.CirclePosition = new Vector2(
            //    //    tX + Width,
            //    //                                  //                      tX + Width - NWDGUI.kFieldMarge - NWDGUI.kEditWidth/2.0F,
            //    //                                  tConnection.Rectangle.y + ParentDocument.HeightProperty / 2.0F);
            //    //tConnection.PositionTangent = new Vector2(tConnection.CirclePosition.x + NWDGUI.kNodeCardHeight, tConnection.CirclePosition.y);
            //    //tPropertyCounter++;
            //}
            //BTBBenchmark.Finish();
            return CardRect.height;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawCard(Rect sVisibleRect)
        {
            //BTBBenchmark.Start();
            //if (sVisibleRect.Contains(CardTopLeft) ||
            //    sVisibleRect.Contains(CardTopRight) ||
            //    sVisibleRect.Contains(CardBottomLeft) ||
            //    sVisibleRect.Contains(CardBottomRight)
            //)

                if (sVisibleRect.Overlaps(CardRect))
                {
                // Debug.Log("NWDNodeCard DrawCard() rect = " + CardRect.ToString());
                GUI.Box(CardRect, " ", EditorStyles.helpBox);
                //GUI.DrawTexture(CardRect, NWDToolbox.TextureFromColor(Color.red));




                //if (NWDDataInspector.ObjectInEdition() == DataObject)
                //{
                //    Color tOldColor = GUI.backgroundColor;
                //    GUI.backgroundColor = new Color(0.55f, 0.55f, 1.00f, 0.5f);
                //    GUI.Box(CardRect, string.Empty, EditorStyles.helpBox);
                //    GUI.backgroundColor = tOldColor;
                //}
                //if (ClassTexture != null)
                //{
                //    GUI.DrawTexture(IconRect, ClassTexture);
                //}

                //GUI.Label(CardTypeRect, TypeString, EditorStyles.boldLabel);
                //GUI.Label(CardReferenceRect, ReferenceString);
                //GUI.Label(CardInternalKeyRect, DataObject.InternalKey);

                //// Draw informations box with the color of informations
                //Color tOldBackgroundColor = GUI.backgroundColor;
                //GUI.backgroundColor = InformationsColor;
                //GUI.Box(InfoRect, " ", EditorStyles.helpBox);
                //GUI.backgroundColor = tOldBackgroundColor;
                //// add button to edit data
                //if (GUI.Button(new Rect(tX + Width - NWDGUI.kEditIconSide - NWDGUI.kFieldMarge, tY + NWDGUI.kFieldMarge, NWDGUI.kEditIconSide, NWDGUI.kEditIconSide), NWDGUI.kEditContentIcon, EditorStyles.miniButton))
                //{
                //    NWDDataInspector.InspectNetWorkedData(DataObject, true, true);
                //}
                //// add button to center node on this data
                //if (GUI.Button(new Rect(tX + Width - NWDGUI.kEditIconSide * 2 - NWDGUI.kFieldMarge * 2, tY + NWDGUI.kFieldMarge, NWDGUI.kEditIconSide, NWDGUI.kEditIconSide), NWDGUI.kNodeContentIcon, EditorStyles.miniButton))
                //{
                //    NWDDataInspector.InspectNetWorkedData(DataObject, true, true);
                //    ParentDocument.SetData(DataObject);
                //}

                //NWDBasisHelper.FindTypeInfos(Data.GetType());
                string tPrefName = "NWDEditorAnalyze_" + DataObject.GetType().Name;
                bool tAnalyze = EditorPrefs.GetBool(tPrefName, true);
                //bool tAnalyzeChange = EditorGUI.ToggleLeft(new Rect(tX + Width - NWDGUI.kEditIconSide * 3 - NWDGUI.kFieldMarge * 3, tY + NWDGUI.kFieldMarge, NWDGUI.kEditIconSide, NWDGUI.kEditIconSide), "", tAnalyze);
                //if (tAnalyzeChange != tAnalyze)
                //{
                //    EditorPrefs.SetBool(tPrefName, tAnalyzeChange);
                //    ParentDocument.LoadClasses();
                //    ParentDocument.ReAnalyze();
                //}

                //DataObject.AddOnNodeDraw(InfoUsableRect, /*ParentDocument.ReGroupProperties*/ false);

                DataObject.DrawEditorTop(CardRect, false, this);


                //foreach (NWDNodeConnection tConnection in ConnectionList)
                //{
                //    GUIStyle tBox = new GUIStyle(EditorStyles.helpBox);
                //    tBox.alignment = TextAnchor.MiddleLeft;
                //    Type tTypeProperty = tConnection.Property.GetValue(DataObject, null).GetType();


                //  //  object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
                //    var tMethodPropertyField = tTypeProperty.GetMethod("ControlField", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                //    if (tMethodPropertyField != null)
                //    {
                //        BTBDataType tOld = (BTBDataType)tConnection.Property.GetValue(DataObject, null);
                //        BTBDataType tResult = (BTBDataType)tMethodPropertyField.Invoke(tOld, new object[] { tConnection.Rectangle, tConnection.PropertyName, BTBConstants.K_EMPTY_STRING });
                //        if (tOld.Value != tResult.Value)
                //        {
                //            tConnection.Property.SetValue(DataObject, tResult);
                //            DataObject.UpdateData();
                //            ParentDocument.ReAnalyze();
                //            ParentDocument.ReEvaluateLayout();
                //        }
                //    }



                //    //if (/*ParentDocument.ReGroupProperties == false &&*/ tConnection.ChildrenList.Count > 0)
                //    //{
                //    //    NWDNodeCard tSubCard = tConnection.ChildrenList[0].Child;
                //    //    if (tSubCard != null)
                //    //    {
                //    //        // draw properties distinct
                //    //        GUI.Box(tConnection.Rectangle, string.Empty, tBox);
                //    //        // add special infos in this property draw
                //    //        NWDTypeClass tSubData = tSubCard.DataObject;
                //    //        tSubData.AddOnNodePropertyDraw(tConnection.PropertyName, new Rect(tConnection.Rectangle.x + NWDGUI.kFieldMarge, tConnection.Rectangle.y + 2, tConnection.Rectangle.width - 2 - (NWDGUI.kEditWidth + NWDGUI.kFieldMarge) * 3, tConnection.Rectangle.height));
                //    //        // Add button to edit this data
                //    //        if (GUI.Button(new Rect(tConnection.Rectangle.x + tConnection.Rectangle.width + NWDGUI.kFieldMarge - (NWDGUI.kEditIconSide + NWDGUI.kFieldMarge) * 2 - 2, tConnection.Rectangle.y + 2, NWDGUI.kEditIconSide, NWDGUI.kEditIconSide), NWDGUI.kEditContentIcon, EditorStyles.miniButton))
                //    //        {
                //    //            NWDDataInspector.InspectNetWorkedData(tSubCard.DataObject, true, true);
                //    //        }
                //    //        // add button to center node on this data
                //    //        if (GUI.Button(new Rect(tConnection.Rectangle.x + tConnection.Rectangle.width + NWDGUI.kFieldMarge - (NWDGUI.kEditIconSide + NWDGUI.kFieldMarge) * 3 - 2, tConnection.Rectangle.y + 2, NWDGUI.kEditIconSide, NWDGUI.kEditIconSide), NWDGUI.kNodeContentIcon, EditorStyles.miniButton))
                //    //        {
                //    //            NWDDataInspector.InspectNetWorkedData(tSubCard.DataObject, true, true);
                //    //            ParentDocument.SetData(tSubCard.DataObject);
                //    //        }
                //    //    }
                //    //    else
                //    //    {
                //    //        GUI.Box(tConnection.Rectangle, tConnection.PropertyName + " <BROKEN> " + tConnection.ChildrenList[0].Style.ToString(), tBox);
                //    //    }
                //    //}
                //    //else
                //    //{
                //    //    // draw properties group
                //    //    GUI.Box(tConnection.Rectangle, tConnection.PropertyName, tBox);
                //    //}

                //    //if (tConnection.AddButton == true)
                //    //{

                //    //    if (GUI.Button(new Rect(tConnection.Rectangle.x + tConnection.Rectangle.width - NWDGUI.kEditIconSide - 2, tConnection.Rectangle.y + 2, NWDGUI.kEditIconSide, NWDGUI.kEditIconSide), NWDGUI.kNewContentIcon, EditorStyles.miniButton))
                //    //    {
                //    //        // TODO : Change to remove invoke!
                //    //        //Debug.Log("ADD REFERENCE FROM NODE EDITOR");
                //    //        // call the method EditorAddNewObject();
                //    //        // TODO : Change to remove invoke!
                //    //        //Debug.Log("tTypeProperty = " + tTypeProperty.Name);
                //    //        var tMethodProperty = tTypeProperty.GetMethod("EditorAddNewObject", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                //    //        if (tMethodProperty != null)
                //    //        {
                //    //            tMethodProperty.Invoke(tConnection.Property.GetValue(DataObject, null), null);
                //    //            // Ok I update the data
                //    //            Type tDataType = DataObject.GetType();
                //    //            // TODO : Change to remove invoke!
                //    //            var tDataTypeUpdate = tDataType.GetMethod("UpdateMe", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                //    //            if (tDataTypeUpdate != null)
                //    //            {
                //    //                //Debug.Log("UpdateMe is Ok ");
                //    //                tDataTypeUpdate.Invoke(DataObject, new object[] { true });
                //    //                ParentDocument.ReAnalyze();
                //    //            }
                //    //        }
                //    //        else
                //    //        {
                //    //            //Debug.Log("NO tMethodProperty ");
                //    //        }
                //    //    }
                //    //}
                //}

            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawBackgroundLine()
        {
            //BTBBenchmark.Start();
            // Debug.Log("NWDNodeCard DrawLine()");
            foreach (NWDNodeConnection tConnection in ConnectionList)
            {
                tConnection.DrawBackgroundLine();
            }
            //BTBBenchmark.Finish();
        }


        //-------------------------------------------------------------------------------------------------------------
        public void DrawConnection( List<NWDNodeCard> sAllCards)
        {
            PlotsList.Clear();
           // BTBBenchmark.Start();

            //DataObject.DrawEditorTopHeight(this,NWDGUI.kNodeCardWidth);

            //Debug.Log("DrawConnection DrawLine()");
            float Tangent = ParentDocument.CardMarge;
            Vector2 tOrign = new Vector2(Position.x , Position.y + NWDGUI.kEditWidthMini*3);
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
                            if (ParentDocument.MiddleCard == false)
                            {
                                tPlot.Point.y = - NWDGUI.kFieldMarge - NWDGUI.kEditWidthMini;
                            }
                                Vector2 tFinal = new Vector2(tCard.Position.x + NWDGUI.kNodeCardWidth + tPlot.Point.x, tCard.Position.y + tPlot.Point.y+ tCard.TopHeight);

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
                            ////Debug.Log("DrawConnection line for" + tPlot.Reference);
                            //Vector2 tFinalPzzz = new Vector2(tFinal.x - 80, tFinal.y);
                            //Handles.color = Color.white;
                            //Handles.DrawLine(tFinalPzzz, tFinal);

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
            //Debug.Log("NWDNodeCard DrawPlot()");
            //Handles.color = NWDGUI.kNodeLineColor;
            //Handles.DrawSolidDisc(CirclePosition, Vector3.forward, NWDGUI.kEditWidthHalf);
            //Handles.color = NWDGUI.kNodeOverLineColor;
            //Handles.DrawSolidDisc(CirclePosition, Vector3.forward, NWDGUI.kEditWidthHalf - 1.0f);
            //// Draw plot of my connections 
            //foreach (NWDNodeConnection tConnection in ConnectionList)
            //{
            //    tConnection.DrawForwardPlot();
            //}


            foreach (Vector2 tPlot in PlotsList)
            {
                Handles.color = NWDGUI.kNodeLineColor;
                Handles.DrawSolidDisc(tPlot, Vector3.forward, NWDGUI.kEditWidthMiniHalf);
                Handles.color = NWDGUI.kNodeOverLineColor;
                Handles.DrawSolidDisc(tPlot, Vector3.forward, NWDGUI.kEditWidthMiniHalf - 1.0F);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
