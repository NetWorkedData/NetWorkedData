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
    public enum NWDNodeConnectionType : byte
    {
        None,
        Valid,
        Broken,
        OldCard,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeConnectionLine
    {
        public NWDNodeConnectionType Style = NWDNodeConnectionType.Valid;
        public NWDNodeCard Child;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeConnection
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDNodeCard Parent;
        public string PropertyName;
        public PropertyInfo Property;
        public List<NWDNodeConnectionLine> ChildrenList = new List<NWDNodeConnectionLine>();
        public Vector2 Position;
        public Vector2 CirclePosition;
        public Vector2 PositionTangent;
        public Rect Rectangle;
        public bool AddButton = true;
        //public bool ConnectionToPreviewCard = false;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawBackgroundLine()
        {
            //Debug.Log("NWDNodeConnection DrawLine()");
            foreach (NWDNodeConnectionLine tCardLine in ChildrenList)
            {
                NWDNodeCard tCard = tCardLine.Child;
                if (tCard != null)
                {
                    switch (tCardLine.Style)
                    {
                        case NWDNodeConnectionType.Valid:
                            {
                                //Handles.color = Color.red;
                                //Handles.DrawLine(Position, tCard.Position);
                                Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, NWDConstants.kNodeLineColor, NWDConstants.kImageBezierTexture , 2.0F);
                                //Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, NWDConstants.kNodeLineColor, null, 2.0F);
                                //Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, Color.black, null, 2.0F);
                                //Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, Color.black, null, 2.0F);
                            }
                            break;
                        case NWDNodeConnectionType.Broken:
                            {
                                Handles.color = Color.red;
                                Handles.DrawLine(Position, tCard.Position);
                            }
                            break;
                        case NWDNodeConnectionType.OldCard:
                            {
                                Handles.DrawBezier(Position, tCard.CirclePosition, PositionTangent, tCard.PositionTangent, NWDConstants.kNodeLineColor, NWDConstants.kImageBezierTexture, 2.0F);
                            }
                            break;
                        case NWDNodeConnectionType.None:
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
            // Debug.Log("NWDNodeConnection DrawPlot()");
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
