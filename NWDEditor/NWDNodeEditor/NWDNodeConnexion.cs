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
    public enum NWDNodeConnexionType : byte
    {
        None,
        Valid,
        Broken,
        OldCard,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeConnexionLine
    {
        public NWDNodeConnexionType Style = NWDNodeConnexionType.Valid;
        public NWDNodeCard Child;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeConnexion
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDNodeCard Parent;
        public string PropertyName;
        public PropertyInfo Property;
        public List<NWDNodeConnexionLine> ChildrenList = new List<NWDNodeConnexionLine>();
        public Vector2 Position;
        public Vector2 CirclePosition;
        public Vector2 PositionTangent;
        public Rect Rectangle;
        public bool AddButton = true;
        //public bool ConnexionToPreviewCard = false;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawBackgroundLine()
        {
            //Debug.Log("NWDNodeConnexion DrawLine()");
            foreach (NWDNodeConnexionLine tCardLine in ChildrenList)
            {
                NWDNodeCard tCard = tCardLine.Child;
                if (tCard != null)
                {
                    switch (tCardLine.Style)
                    {
                        case NWDNodeConnexionType.Valid:
                            {
                                //Handles.color = Color.red;
                                //Handles.DrawLine(Position, tCard.Position);
                                Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, NWDConstants.kNodeLineColor, NWDConstants.kImageBezierTexture , 2.0F);
                                //Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, NWDConstants.kNodeLineColor, null, 2.0F);
                                //Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, Color.black, null, 2.0F);
                                //Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, Color.black, null, 2.0F);
                            }
                            break;
                        case NWDNodeConnexionType.Broken:
                            {
                                Handles.color = Color.red;
                                Handles.DrawLine(Position, tCard.Position);
                            }
                            break;
                        case NWDNodeConnexionType.OldCard:
                            {
                                Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, NWDConstants.kNodeLineColor, NWDConstants.kImageBezierTexture, 2.0F);
                            }
                            break;
                        case NWDNodeConnexionType.None:
                            {
                            }
                            break;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawForwardPlot()
        {
            // Debug.Log("NWDNodeConnexion DrawPlot()");
            if (ChildrenList.Count > 0)
            {
                Handles.color = NWDConstants.kNodeLineColor;
                Handles.DrawSolidDisc(CirclePosition, Vector3.forward, NWDConstants.kEditWidthMiniHalf);
                Handles.color = NWDConstants.kNodeOverLineColor;
                Handles.DrawSolidDisc(CirclePosition, Vector3.forward, NWDConstants.kEditWidthMiniHalf-1.0F);
            }
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
