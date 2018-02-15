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
        public int Line=0;
        public int Column=0;
        public NWDNodeDocument ParentDocument;
        //-------------------------------------------------------------------------------------------------------------
        public void Analyze (NWDNodeDocument sDocument)
        {
            Debug.Log("NWDNodeCard Analyze()");
            ParentDocument = sDocument;
            sDocument.ColumnMaxCount(Column);
            // I analyze the properties of data.
            if (Data != null)
            {
                Type tType = Data.GetType();
                var tMethodInfo = tType.GetMethod("NodeCardAnalyze", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(Data, new object[] { this});
                }
            }
            else
            {
                Debug.Log("NWDNodeCard Analyze() NO DATA (null)");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDNodeCard> AddPropertyResult(PropertyInfo sProperty , object[] sObjectsArray)
        {
            Debug.Log("NWDNodeCard AddPropertyResult()");
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
            int tLine = 0;
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
                            tCardLine.Style = NWDNodeConnexionType.OldCard;
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
        public void DrawLine()
        {
           // Debug.Log("NWDNodeCard DrawLine()");
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                tConnexion.DrawLine();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawCard()
        {
            // Debug.Log("NWDNodeCard DrawCard()");
            string tInfos = "";//Data.GetType().AssemblyQualifiedName;
            string tInfosCard = " " + Column + " x " + Line +"\n";
            string tInfosCardCustom = "";
            if (Data != null)
            {
                Type tType = Data.GetType();
                var tMethodInfo = tType.GetMethod("NodeDescription", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tInfosCard+= tMethodInfo.Invoke(Data, null);
                }
                var tMethodDescription = tType.GetMethod("AddOnNodeDescription", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (tMethodDescription != null)
                {
                    tInfosCardCustom += tMethodDescription.Invoke(Data, null);
                }
            }
            float tWidth = ParentDocument.Width;
            float tHeight = ParentDocument.Height;
            float tMargin = ParentDocument.Margin;
            float tX = tMargin + Column * (tWidth + tMargin);
            float tY = tMargin + Line * (tHeight + tMargin);
            GUI.Box(new Rect(tX,tY, tWidth, tHeight ),tInfos+" "+tInfosCard+" "+ tInfosCardCustom);
            if (GUI.Button(new Rect(tX + tWidth - 24, tY + 4, 20 , 20) , "Edit"))
            {
                NWDDataInspector.InspectNetWorkedData(Data, true, true);
            }
            if (GUI.Button(new Rect(tX + tWidth - 48, tY + 4, 20, 20), "jump"))
            {
                NWDDataInspector.InspectNetWorkedData(Data, true, true);
                ParentDocument.SetData(Data);
            }

            Position = new Vector2(tX, tY);
            CirclePosition = new Vector2(tX + 10, tY + 10);
            PositionTangent = new Vector2(Position.x+10 - ParentDocument.Margin, Position.y);
            int tPropertyCounter = 0;
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                //Debug.Log("NWDNodeCard DrawCard() draw connexion");
                GUI.Box(new Rect(tX +2, tY+ParentDocument.HeightInformations+1 + ParentDocument.HeightProperty*tPropertyCounter-2, tWidth-4, ParentDocument.HeightProperty-2),
                        tConnexion.PropertyName);
                tConnexion.Position = new Vector2(tX - 10 + tWidth, tY + ParentDocument.HeightInformations + ParentDocument.HeightProperty * (tPropertyCounter + 0.5F));
                tConnexion.CirclePosition = new Vector2(tConnexion.Position.x, tConnexion.Position.y);
                tConnexion.PositionTangent = new Vector2(tConnexion.Position.x+ParentDocument.Margin, tConnexion.Position.y);
                tPropertyCounter++;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPlot()
        {
            //Debug.Log("NWDNodeCard DrawPlot()");
            Handles.color = Color.black;
            Handles.DrawSolidDisc(CirclePosition,Vector3.forward,8.0f);
            Handles.color = Color.gray;
            Handles.DrawSolidDisc(CirclePosition, Vector3.forward, 6.0f);
            // Draw plot of my connexions 
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                tConnexion.DrawPlot();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
