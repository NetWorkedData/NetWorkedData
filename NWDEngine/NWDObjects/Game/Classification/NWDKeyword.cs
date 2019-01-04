//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDKeywordConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDKeyword tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
	[Serializable]
    public class NWDKeywordConnection : NWDConnection<NWDKeyword>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDKeyword class. This class is used to reccord the keyword available in the game. 
    /// </summary>
	[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("KWD")]
    [NWDClassDescriptionAttribute("This class is used to reccord the keyword available in the game")]
    [NWDClassMenuNameAttribute("Keyword")]
    public partial class NWDKeyword : NWDBasis<NWDKeyword>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Informations", true, true, true)]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDKeyword> KeywordList
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDKeyword()
        {
            //Debug.Log("NWDKeyword Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDKeyword(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDKeyword Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        //#region Class methods
        ////-------------------------------------------------------------------------------------------------------------
        //public static void MyClassMethod ()
        //{
        //	// do something with this class
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //#endregion
        ////-------------------------------------------------------------------------------------------------------------
        //      #region Instance methods
        ////-------------------------------------------------------------------------------------------------------------
        //public void MyInstanceMethod ()
        //{
        //	// do something with this object
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      #endregion
        ////-------------------------------------------------------------------------------------------------------------
        //#region override of NetWorkedData addons methods
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonInsertMe ()
        //{
        //	// do something when object will be inserted
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonUpdateMe ()
        //{
        //	// do something when object will be updated
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonUpdatedMe ()
        //{
        //	// do something when object finish to be updated
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonDuplicateMe ()
        //{
        //	// do something when object will be dupplicate
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonEnableMe ()
        //{
        //	// do something when object will be enabled
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonDisableMe ()
        //{
        //	// do something when object will be disabled
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonTrashMe ()
        //{
        //	// do something when object will be put in trash
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonUnTrashMe ()
        //{
        //	// do something when object will be remove from trash
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      #endregion
        ////-------------------------------------------------------------------------------------------------------------
        //#if UNITY_EDITOR
        ////-------------------------------------------------------------------------------------------------------------
        ////Addons for Edition
        ////-------------------------------------------------------------------------------------------------------------
        //public override bool AddonEdited( bool sNeedBeUpdate)
        //{
        //	if (sNeedBeUpdate == true) 
        //	{
        //		// do something
        //	}
        //	return sNeedBeUpdate;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override float AddonEditor (Rect sInRect)
        //{
        //	// Draw the interface addon for editor
        //	float tYadd = 0.0f;
        //	return tYadd;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override float AddonEditorHeight ()
        //{
        //	// Height calculate for the interface addon for editor
        //	float tYadd = 0.0f;
        //	return tYadd;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
