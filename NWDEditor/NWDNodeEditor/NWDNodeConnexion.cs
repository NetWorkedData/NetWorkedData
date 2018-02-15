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
    public class NWDNodeConnexion {
        //-------------------------------------------------------------------------------------------------------------
        public NWDNodeCard Parent;
        public string PropertyName;
        public List<NWDNodeCard> ChildrenList = new List<NWDNodeCard>();
        public Vector2 Position;
        public Vector2 PositionTangent;
        //public bool ConnexionToPreviewCard = false;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawLine()
        {
            //Debug.Log("NWDNodeConnexion DrawLine()");
            foreach (NWDNodeCard tCard in ChildrenList)
            {
                if (tCard != null)
                {
                    //Handles.color = Color.red;
                    //Handles.DrawLine(Position, tCard.Position);
                    Handles.DrawBezier(Position, tCard.Position,PositionTangent,tCard.PositionTangent,Color.black,null, 4.0f);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPlot()
        {
           // Debug.Log("NWDNodeConnexion DrawPlot()");
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
