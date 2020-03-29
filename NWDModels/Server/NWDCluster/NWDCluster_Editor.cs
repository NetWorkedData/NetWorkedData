//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:12
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCluster : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            // do base
            bool tNeedBeUpdate =  base.AddonEdited(sNeedBeUpdate);
            if (tNeedBeUpdate == true)
            {
                // do something
            }
            return tNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight(float sWidth)
        {
            // Height calculate for the interface addon for editor
            float tYadd = base.AddonEditorHeight(sWidth);
            tYadd += NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 20);
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sRect">S in rect.</param>
        public override void AddonEditor(Rect sRect)
        {
            base.AddonEditor(sRect);
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 20);
            // Draw the interface addon for editor
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            float tYadd = base.AddOnNodeDrawHeight(sCardWidth);
            tYadd += 130;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect)
        {
            base.AddOnNodeDraw(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = base.AddonErrorFound();
            // check if you found error in Data values.
            // normal way is return false!
            return rReturnErrorFound;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif