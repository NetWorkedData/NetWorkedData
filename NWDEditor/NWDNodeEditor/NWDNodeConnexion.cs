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
        public PropertyInfo Property;
        public List<NWDNodeCard> ChildrenList;
        public Vector2 Position;
        public bool ConnexionToPreviewCard = false;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawLine()
        {
            
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPlot()
        {
            
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
