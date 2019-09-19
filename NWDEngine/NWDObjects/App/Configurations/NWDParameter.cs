//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:41
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BasicToolBox;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("PRM")]
    [NWDClassDescriptionAttribute("Parameters of game. You can use this class to create Parameters of your game. \n" +
                                   "Parameters are set for all user. Use InternalKey to find them and use them. \n" +
                                   "")]
    [NWDClassMenuNameAttribute("Parameters")]
    public partial class NWDParameter : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations", true, true, true)]
        public NWDLocalizableTextType Name
        {
            get; set;
        }
        public NWDReferencesListType<NWDCategory> CategoryList
        {
            get; set;
        }
        [NWDTooltips("The description item. Usable to be ownershipped")]
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        public NWDMultiType Value
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================