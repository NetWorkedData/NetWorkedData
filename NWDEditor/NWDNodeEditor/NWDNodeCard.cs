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
        public List<NWDNodeConnexion> ConnexionList = new List<NWDNodeConnexion>();
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
        public List<NWDNodeCard> AddPropertyResult(PropertyInfo sProperty, object[] sObjectsArray, bool sButtonAdd)
        {
            // Debug.Log("NWDNodeCard AddPropertyResult()");
            List<NWDNodeCard> rResult = new List<NWDNodeCard>();
            NWDNodeConnexion tNewConnexion = null;
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                if (tConnexion.PropertyName == sProperty.Name)
                {
                    tNewConnexion = tConnexion;
                    break;
                }
            }
            if (tNewConnexion == null)
            {
                tNewConnexion = new NWDNodeConnexion();
                tNewConnexion.PropertyName = sProperty.Name;
                ConnexionList.Add(tNewConnexion);
                tNewConnexion.Parent = this;
            }
            tNewConnexion.Property = sProperty;
            tNewConnexion.AddButton = sButtonAdd;
            //int tLine = 0;
            foreach (NWDTypeClass tObject in sObjectsArray)
            {
                // Card exist?

                if (tObject != null)
                {
                    bool tDataAllReadyShow = false;
                    NWDNodeCard tCard = null;
                    NWDNodeConnexionLine tCardLine = new NWDNodeConnexionLine();
                    foreach (NWDNodeCard tOldCard in ParentDocument.AllCards)
                    {
                        if (tOldCard.Data == tObject)
                        {
                            tDataAllReadyShow = true;
                            tCard = tOldCard;
                            if (tCard.Column < Column)
                            {
                                tCardLine.Style = NWDNodeConnexionType.OldCard;
                            }
                            else
                            {
                                tCardLine.Style = NWDNodeConnexionType.Valid;
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
                    }
                    tCardLine.Child = tCard;
                    tNewConnexion.ChildrenList.Add(tCardLine);
                }
            }
            ParentDocument.PropertyCount(ConnexionList.Count);
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReEvaluateHeightWidth()
        {
            Margin = ParentDocument.Margin;
            tX = ParentDocument.MargeWidth + Margin + Column * (ParentDocument.GetWidth() + Margin);
            tY = Margin + Line * (ParentDocument.Height + Margin);
            Height = NWDConstants.kFieldMarge + (ParentDocument.HeightLabel + NWDConstants.kFieldMarge) * 3 + InformationsHeight + NWDConstants.kFieldMarge + (ParentDocument.HeightProperty + NWDConstants.kFieldMarge) * ConnexionList.Count;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReEvaluateLayout()
        {
            //Infos = "";//Data.GetType().AssemblyQualifiedName;
            //InfosCard = " " + Column + " x " + Line + "\n";
            //InfosCardCustom = "";

            CardRect = new Rect(tX, tY, Width, Height);

            IconRect = new Rect(CardRect.x + NWDConstants.kFieldMarge + NWDConstants.kEditWidthHalf , CardRect.y + NWDConstants.kFieldMarge, NWDConstants.kIconWidth, NWDConstants.kIconWidth);

            CardTypeRect = new Rect(tX + NWDConstants.kIconWidth + NWDConstants.kEditWidthHalf+ NWDConstants.kFieldMarge * 2, tY + NWDConstants.kFieldMarge, Width - NWDConstants.kEditWidth * 2 - NWDConstants.kFieldMarge * 4, ParentDocument.HeightLabel);
            CardReferenceRect = new Rect(tX + NWDConstants.kIconWidth + NWDConstants.kEditWidthHalf+ NWDConstants.kFieldMarge * 2, tY + ParentDocument.HeightLabel + NWDConstants.kFieldMarge * 2, Width - NWDConstants.kEditWidth * 2, ParentDocument.HeightLabel);
            CardInternalKeyRect = new Rect(tX + NWDConstants.kIconWidth + NWDConstants.kEditWidthHalf+ NWDConstants.kFieldMarge * 2, tY + ParentDocument.HeightLabel * 2 + NWDConstants.kFieldMarge * 3, Width - NWDConstants.kEditWidth * 2, ParentDocument.HeightLabel);

            InfoRect = new Rect(tX + NWDConstants.kFieldMarge, tY + ParentDocument.HeightLabel * 3 + NWDConstants.kFieldMarge * 4, Width - +NWDConstants.kFieldMarge * 2, InformationsHeight);
            InfoUsableRect = new Rect(InfoRect.x + NWDConstants.kFieldMarge, InfoRect.y + NWDConstants.kFieldMarge, InfoRect.width - NWDConstants.kFieldMarge * 2, InfoRect.height - NWDConstants.kFieldMarge * 2);

            Position = new Vector2(tX, tY);
            CirclePosition = new Vector2(tX + 0, tY + NWDConstants.kIconWidth/2.0F+ NWDConstants.kFieldMarge);
            PositionTangent = new Vector2(CirclePosition.x - ParentDocument.Margin, CirclePosition.y);

            int tPropertyCounter = 0;
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                //Debug.Log("NWDNodeCard DrawCard() draw connexion");
                tConnexion.Rectangle = new Rect(tX + NWDConstants.kFieldMarge,
                                                tY + ParentDocument.HeightLabel * 3 + NWDConstants.kFieldMarge * 5 + InformationsHeight + (NWDConstants.kFieldMarge + ParentDocument.HeightProperty) * tPropertyCounter,
                                                Width - NWDConstants.kFieldMarge * 2 - NWDConstants.kEditWidthMini/2.0F,
                                                ParentDocument.HeightProperty);

                ////GUI.Label(new Rect(tX + 2, tY + ParentDocument.HeightInformations + 1 + ParentDocument.HeightProperty * tPropertyCounter - 2, tWidth - 4, ParentDocument.HeightProperty - 2), tConnexion.PropertyName);
                tConnexion.Position = new Vector2(tX + Width - NWDConstants.kFieldMarge,
                                                  tConnexion.Rectangle.y + ParentDocument.HeightProperty / 2.0F);


                tConnexion.CirclePosition = new Vector2(
                    tX + Width,
                              //                      tX + Width - NWDConstants.kFieldMarge - NWDConstants.kEditWidth/2.0F,
                                                  tConnexion.Rectangle.y + ParentDocument.HeightProperty / 2.0F);
                tConnexion.PositionTangent = new Vector2(tConnexion.CirclePosition.x + ParentDocument.Margin, tConnexion.CirclePosition.y);
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
                tMethodInfo.Invoke(Data, new object[] { InfoUsableRect });
            }
            // add connexion
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                GUI.Box(tConnexion.Rectangle, tConnexion.PropertyName);
                Type tTypeProperty = tConnexion.Property.GetValue(Data, null).GetType();
                GUIContent tNewContent = new GUIContent(NWDConstants.kImageNew, "New");
                if (tConnexion.AddButton == true)
                {
                    if (GUI.Button(new Rect(tConnexion.Rectangle.x + tConnexion.Rectangle.width - NWDConstants.kEditWidth - 2, tConnexion.Rectangle.y + 2, NWDConstants.kEditWidth, NWDConstants.kEditWidth), tNewContent, NWDConstants.StyleMiniButton))
                    {
                        Debug.Log("ADD REFERENCE FROM NODE EDITOR");
                        // call the method EditorAddNewObject();

                        Debug.Log("tTypeProperty = " + tTypeProperty.Name);
                        var tMethodProperty = tTypeProperty.GetMethod("EditorAddNewObject", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tMethodProperty != null)
                        {
                            tMethodProperty.Invoke(tConnexion.Property.GetValue(Data, null), null);
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
        public void DrawBackgroundPlot()
        {
            // Debug.Log("NWDNodeCard DrawLine()");
            Handles.color = NWDConstants.kNodeLineColor;
            Handles.DrawSolidDisc(CirclePosition, Vector3.forward, NWDConstants.kEditWidthHalf);
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                tConnexion.DrawBackgroundPlot();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawForwardPlot()
        {
            //Debug.Log("NWDNodeCard DrawPlot()");
            Handles.color = NWDConstants.kNodeOverLineColor;
            Handles.DrawSolidDisc(CirclePosition, Vector3.forward, NWDConstants.kEditWidthHalf - 1.0f);
            // Draw plot of my connexions 
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                tConnexion.DrawForwardPlot();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
