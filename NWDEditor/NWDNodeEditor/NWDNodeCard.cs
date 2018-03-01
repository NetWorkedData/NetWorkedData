using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using System;
using System.Reflection;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeCard
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass Data;
        public List<NWDNodeConnection> ConnectionList = new List<NWDNodeConnection>();
        public Vector2 Position;
        public Vector2 CirclePosition;
        public Vector2 PositionTangent;
        public int Line = 0;
        public int Column = 0;
        public NWDNodeDocument ParentDocument;
        public Color InformationsColor = Color.white;

        public Texture2D ClassTexture;
        //-------------------------------------------------------------------------------------------------------------
        float tX;
        float tY;
        float Margin;
        public float Width;
        public float Height;
        public float InformationsHeight;
        //string Infos = "";
        //string InfosCard = "";
        //string InfosCardCustom;
        Rect CardRect;
        Rect CardTypeRect;
        Rect CardReferenceRect;
        Rect CardInternalKeyRect;
        Rect InfoRect;
        Rect IconRect;
        Rect InfoUsableRect;
        public string TypeString;
        public string ReferenceString;
        public string InternalKeyString;
        //public string Informations;
        //-------------------------------------------------------------------------------------------------------------
        public void Analyze(NWDNodeDocument sDocument)
        {
            //Debug.Log("NWDNodeCard Analyze()");
            ParentDocument = sDocument;
            sDocument.ColumnMaxCount(Column);
            // I analyze the properties of data.
            if (Data != null)
            {
                Type tType = Data.GetType();
                var tMethodInfo = tType.GetMethod("NodeCardAnalyze", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(Data, new object[] { this });
                }
            }
            else
            {
                //Debug.Log("NWDNodeCard Analyze() NO DATA (null)");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDNodeCard> AddPropertyResult(PropertyInfo sProperty, NWDNodeConnectionReferenceType sConType, object[] sObjectsArray, bool sButtonAdd)
        {
            // Debug.Log("NWDNodeCard AddPropertyResult()");
            List<NWDNodeCard> rResult = new List<NWDNodeCard>();
            NWDNodeConnection tNewConnection = null;
            if (ParentDocument.UsedOnlyProperties == true && sObjectsArray.Length == 0)
            {
            }
            else
            {
                if (ParentDocument.ReGroupProperties == true || (ParentDocument.UsedOnlyProperties == false && sObjectsArray.Length == 0))
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
                    tNewConnection.AddButton = sButtonAdd;
                    //int tLine = 0;
                }
                foreach (NWDTypeClass tObject in sObjectsArray)
                {
                    if (ParentDocument.ReGroupProperties == false)
                    {
                        tNewConnection = new NWDNodeConnection();
                        tNewConnection.PropertyName = sProperty.Name;
                        tNewConnection.ConType = sConType;
                        tNewConnection.Parent = this;
                        tNewConnection.Property = sProperty;
                        tNewConnection.AddButton = sButtonAdd;
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
                            if (tOldCard.Data == tObject)
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
                            tCard.Data = tObject;
                            rResult.Add(tCard);
                            ParentDocument.AllCards.Add(tCard);
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
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReEvaluateHeightWidth()
        {
            Margin = ParentDocument.Margin;
            tX = ParentDocument.MargeWidth + Margin + Column * (ParentDocument.GetWidth() + Margin);
            tY = Margin + Line * (ParentDocument.Height + Margin);
            Height = NWDConstants.kFieldMarge + (ParentDocument.HeightLabel + NWDConstants.kFieldMarge) * 3 + InformationsHeight + NWDConstants.kFieldMarge + (ParentDocument.HeightProperty + NWDConstants.kFieldMarge) * ConnectionList.Count;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReEvaluateLayout()
        {
            //Infos = "";//Data.GetType().AssemblyQualifiedName;
            //InfosCard = " " + Column + " x " + Line + "\n";
            //InfosCardCustom = "";

            CardRect = new Rect(tX, tY, Width, Height);

            IconRect = new Rect(CardRect.x + NWDConstants.kFieldMarge + NWDConstants.kEditWidthHalf, CardRect.y + NWDConstants.kFieldMarge, NWDConstants.kIconWidth, NWDConstants.kIconWidth);

            CardTypeRect = new Rect(tX + NWDConstants.kIconWidth + NWDConstants.kEditWidthHalf + NWDConstants.kFieldMarge * 2, tY + NWDConstants.kFieldMarge, Width - NWDConstants.kEditWidth * 2 - NWDConstants.kFieldMarge * 4, ParentDocument.HeightLabel);
            CardReferenceRect = new Rect(tX + NWDConstants.kIconWidth + NWDConstants.kEditWidthHalf + NWDConstants.kFieldMarge * 2, tY + ParentDocument.HeightLabel + NWDConstants.kFieldMarge * 2, Width - NWDConstants.kEditWidth * 2, ParentDocument.HeightLabel);
            CardInternalKeyRect = new Rect(tX + NWDConstants.kIconWidth + NWDConstants.kEditWidthHalf + NWDConstants.kFieldMarge * 2, tY + ParentDocument.HeightLabel * 2 + NWDConstants.kFieldMarge * 3, Width - NWDConstants.kEditWidth * 2, ParentDocument.HeightLabel);

            InfoRect = new Rect(tX + NWDConstants.kFieldMarge, tY + ParentDocument.HeightLabel * 3 + NWDConstants.kFieldMarge * 4, Width - +NWDConstants.kFieldMarge * 2, InformationsHeight);
            InfoUsableRect = new Rect(InfoRect.x + NWDConstants.kFieldMarge, InfoRect.y + NWDConstants.kFieldMarge, InfoRect.width - NWDConstants.kFieldMarge * 2, InfoRect.height - NWDConstants.kFieldMarge * 2);

            Position = new Vector2(tX, tY);
            CirclePosition = new Vector2(tX + 0, tY + NWDConstants.kIconWidth / 2.0F + NWDConstants.kFieldMarge);
            PositionTangent = new Vector2(CirclePosition.x - ParentDocument.Margin, CirclePosition.y);

            int tPropertyCounter = 0;
            foreach (NWDNodeConnection tConnection in ConnectionList)
            {
                //Debug.Log("NWDNodeCard DrawCard() draw connection");
                tConnection.Rectangle = new Rect(tX + NWDConstants.kFieldMarge,
                                                tY + ParentDocument.HeightLabel * 3 + NWDConstants.kFieldMarge * 5 + InformationsHeight + (NWDConstants.kFieldMarge + ParentDocument.HeightProperty) * tPropertyCounter,
                                                Width - NWDConstants.kFieldMarge * 2 - NWDConstants.kEditWidthMini / 2.0F,
                                                ParentDocument.HeightProperty);

                ////GUI.Label(new Rect(tX + 2, tY + ParentDocument.HeightInformations + 1 + ParentDocument.HeightProperty * tPropertyCounter - 2, tWidth - 4, ParentDocument.HeightProperty - 2), tConnection.PropertyName);
                tConnection.Position = new Vector2(tX + Width - NWDConstants.kFieldMarge,
                                                  tConnection.Rectangle.y + ParentDocument.HeightProperty / 2.0F);


                tConnection.CirclePosition = new Vector2(
                    tX + Width,
                                                  //                      tX + Width - NWDConstants.kFieldMarge - NWDConstants.kEditWidth/2.0F,
                                                  tConnection.Rectangle.y + ParentDocument.HeightProperty / 2.0F);
                tConnection.PositionTangent = new Vector2(tConnection.CirclePosition.x + ParentDocument.Margin, tConnection.CirclePosition.y);
                tPropertyCounter++;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawCard()
        {
            // Debug.Log("NWDNodeCard DrawCard()");

            GUI.Box(CardRect, " ", EditorStyles.helpBox);
            //GUI.DrawTexture(CardRect, NWDConstants.kImageNodalCard);

            /// if selected redraw twice or three time this card background
            if (NWDDataInspector.ObjectInEdition() == Data)
            {
                //var tTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                //tTexture.SetPixel(0, 0, new Color(1.0F, 1.0F, 1.0F, 0.5F));
                //GUIStyle tStyle = new GUIStyle(EditorStyles.helpBox);
                //tStyle.normal.background = tTexture;

                Color tOldColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.55f, 0.55f, 1.00f, 0.5f);
                GUI.Box(CardRect, "", EditorStyles.helpBox);
                GUI.backgroundColor = tOldColor;
            }
            //else
            //{
            //    // draw background
            //    GUI.Box(CardRect, " ", EditorStyles.helpBox);
            //}

            //EditorGUI.DrawRect(new Rect(CardRect.x + 1, CardRect.y + 1, CardRect.width-2, CardRect.height-2), Color.gray);
            if (ClassTexture != null)
            {
                //EditorGUI.DrawPreviewTexture(IconRect, ClassTexture);
                GUI.DrawTexture(IconRect, ClassTexture);
            }

            GUI.Label(CardTypeRect, TypeString, EditorStyles.boldLabel);
            GUI.Label(CardReferenceRect, ReferenceString);
            //GUI.Label(CardInternalKeyRect, InternalKeyString);
            GUI.Label(CardInternalKeyRect, Data.InternalKeyValue());

            // Draw informations box with the color of informations
            Color tOldBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = InformationsColor;
            GUI.Box(InfoRect, " ", EditorStyles.helpBox);
            GUI.backgroundColor = tOldBackgroundColor;
            // add button to edit data
            GUIContent tButtonContent = new GUIContent(NWDConstants.kImageTabReduce, "edit");
            if (GUI.Button(new Rect(tX + Width - NWDConstants.kEditWidth - NWDConstants.kFieldMarge, tY + NWDConstants.kFieldMarge, NWDConstants.kEditWidth, NWDConstants.kEditWidth), tButtonContent, NWDConstants.StyleMiniButton))
            {
                NWDDataInspector.InspectNetWorkedData(Data, true, true);
            }
            // add button to center node on this data
            GUIContent tNodeContent = new GUIContent(NWDConstants.kImageSelectionUpdate, "node");
            if (GUI.Button(new Rect(tX + Width - NWDConstants.kEditWidth * 2 - NWDConstants.kFieldMarge * 2, tY + NWDConstants.kFieldMarge, NWDConstants.kEditWidth, NWDConstants.kEditWidth), tNodeContent, NWDConstants.StyleMiniButton))
            {
                NWDDataInspector.InspectNetWorkedData(Data, true, true);
                ParentDocument.SetData(Data);
            }
            // add custom draw in information rect
            Type tType = Data.GetType();
            var tMethodInfo = tType.GetMethod("AddOnNodeDraw", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(Data, new object[] { InfoUsableRect, ParentDocument.ReGroupProperties });
            }
            // add connection
            foreach (NWDNodeConnection tConnection in ConnectionList)
            {
                GUIStyle tBox = new GUIStyle(EditorStyles.helpBox);
                tBox.alignment = TextAnchor.MiddleLeft;
                Type tTypeProperty = tConnection.Property.GetValue(Data, null).GetType();
                GUIContent tNewContent = new GUIContent(NWDConstants.kImageNew, "New");

                if (ParentDocument.ReGroupProperties == false && tConnection.ChildrenList.Count > 0)
                {
                    NWDNodeCard tSubCard = tConnection.ChildrenList[0].Child;
                    if (tSubCard != null)
                    {
                        // draw properties distinct
                        GUI.Box(tConnection.Rectangle, "", tBox);
                        // add special infos in this property draw
                        NWDTypeClass tSubData = tSubCard.Data;
                        Type tSubType = tSubData.GetType();
                        var tSubMethodInfo = tSubType.GetMethod("AddOnNodePropertyDraw", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tSubMethodInfo != null)
                        {
                            tSubMethodInfo.Invoke(tSubData, new object[] { tConnection.PropertyName, new Rect(tConnection.Rectangle.x, tConnection.Rectangle.y + 2, tConnection.Rectangle.width - 2 - (NWDConstants.kEditWidth + NWDConstants.kFieldMarge) * 3, tConnection.Rectangle.height) });
                        }
                        // Add button to edit this data
                        if (GUI.Button(new Rect(tConnection.Rectangle.x + tConnection.Rectangle.width + NWDConstants.kFieldMarge - (NWDConstants.kEditWidth + NWDConstants.kFieldMarge) * 2 - 2, tConnection.Rectangle.y + 2, NWDConstants.kEditWidth, NWDConstants.kEditWidth), tButtonContent, NWDConstants.StyleMiniButton))
                        {
                            NWDDataInspector.InspectNetWorkedData(tSubCard.Data, true, true);
                        }
                        // add button to center node on this data
                        if (GUI.Button(new Rect(tConnection.Rectangle.x + tConnection.Rectangle.width + NWDConstants.kFieldMarge - (NWDConstants.kEditWidth + NWDConstants.kFieldMarge) * 3 - 2, tConnection.Rectangle.y + 2, NWDConstants.kEditWidth, NWDConstants.kEditWidth), tNodeContent, NWDConstants.StyleMiniButton))
                        {
                            NWDDataInspector.InspectNetWorkedData(tSubCard.Data, true, true);
                            ParentDocument.SetData(tSubCard.Data);
                        }
                    }
                    else
                    {
                        GUI.Box(tConnection.Rectangle, tConnection.PropertyName + " <BROKEN> " + tConnection.ChildrenList[0].Style.ToString(), tBox);
                    }
                }
                else
                {
                    // draw properties group
                    GUI.Box(tConnection.Rectangle, tConnection.PropertyName, tBox);
                }

                if (tConnection.AddButton == true)
                {

                    if (GUI.Button(new Rect(tConnection.Rectangle.x + tConnection.Rectangle.width - NWDConstants.kEditWidth - 2, tConnection.Rectangle.y + 2, NWDConstants.kEditWidth, NWDConstants.kEditWidth), tNewContent, NWDConstants.StyleMiniButton))
                    {
                        Debug.Log("ADD REFERENCE FROM NODE EDITOR");
                        // call the method EditorAddNewObject();

                        Debug.Log("tTypeProperty = " + tTypeProperty.Name);
                        var tMethodProperty = tTypeProperty.GetMethod("EditorAddNewObject", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tMethodProperty != null)
                        {
                            tMethodProperty.Invoke(tConnection.Property.GetValue(Data, null), null);
                            // Ok I update the data
                            Type tDataType = Data.GetType();
                            var tDataTypeUpdate = tDataType.GetMethod("UpdateMe", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                            if (tDataTypeUpdate != null)
                            {
                                Debug.Log("UpdateMe is Ok ");
                                tDataTypeUpdate.Invoke(Data, new object[] { true });
                                ParentDocument.ReAnalyze();
                            }
                        }
                        else
                        {
                            Debug.Log("NO tMethodProperty ");
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawBackgroundLine()
        {
            // Debug.Log("NWDNodeCard DrawLine()");
            foreach (NWDNodeConnection tConnection in ConnectionList)
            {
                tConnection.DrawBackgroundLine();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawForwardPlot()
        {
            //Debug.Log("NWDNodeCard DrawPlot()");
            Handles.color = NWDConstants.kNodeLineColor;
            Handles.DrawSolidDisc(CirclePosition, Vector3.forward, NWDConstants.kEditWidthHalf);
            Handles.color = NWDConstants.kNodeOverLineColor;
            Handles.DrawSolidDisc(CirclePosition, Vector3.forward, NWDConstants.kEditWidthHalf - 1.0f);
            // Draw plot of my connections 
            foreach (NWDNodeConnection tConnection in ConnectionList)
            {
                tConnection.DrawForwardPlot();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
