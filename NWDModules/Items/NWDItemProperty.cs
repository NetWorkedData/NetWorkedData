// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:29
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

////=====================================================================================================================
////
//// ideMobi copyright 2017 
//// All rights reserved by ideMobi
////
////=====================================================================================================================

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;

//using UnityEngine;

//using SQLite4Unity3d;

//using BasicToolBox;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

////=====================================================================================================================
//namespace NetWorkedData
//{
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//    /// <summary>
//    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
//    /// <para>The GameObject can use the object referenced by binding in game. </para>
//    /// <example>
//    /// Example :
//    /// <code>
//    /// public class MyScriptInGame : MonoBehaviour<br/>
//    ///     {
//    ///         NWDConnectionAttribut (true, true, true, true)] // optional
//    ///         public NWDExampleConnection MyNetWorkedData;
//    ///         public void UseData()
//    ///             {
//    ///                 NWDExample tObject = MyNetWorkedData.GetObject();
//    ///                 // Use tObject
//    ///             }
//    ///     }
//    /// </code>
//    /// </example>
//    /// </summary>
//	[Serializable]
//    public class NWDConnection : NWDConnection<NWDItemProperty>
//    {
//    }
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//    [NWDClassServerSynchronizeAttribute(true)]
//    [NWDClassTrigrammeAttribute("ITX")]
//    [NWDClassDescriptionAttribute("Item Properties descriptions Class")]
//    [NWDClassMenuNameAttribute("Item Properties")]
//    public partial class NWDItemProperty : NWDBasis<NWDItemProperty>
//    {
//        //-------------------------------------------------------------------------------------------------------------
//        #region Properties
//        //-------------------------------------------------------------------------------------------------------------
//        [NWDGroupStartAttribute("Informations", true, true, true)] // ok
//        public string Values
//        {
//            get; set;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//        #region Constructors
//        //-------------------------------------------------------------------------------------------------------------
//        public NWDItemProperty()
//        {
//            //Debug.Log("NWDItemProperty Constructor");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public NWDItemProperty(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
//        {
//            //Debug.Log("NWDItemProperty Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//        #region Class methods
//        //-------------------------------------------------------------------------------------------------------------
//        public static void MyClassMethod()
//        {
//            // do something with this class
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//        #region Instance methods
//        //-------------------------------------------------------------------------------------------------------------
//        public override void Initialization()
//        {
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public void MyInstanceMethod()
//        {
//            // do something with this object
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        #region NetWorkedData addons methods
//        //-------------------------------------------------------------------------------------------------------------
//        public override void AddonInsertMe()
//        {
//            // do something when object will be inserted
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void AddonUpdateMe()
//        {
//            // do something when object will be updated
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void AddonUpdatedMe()
//        {
//            // do something when object finish to be updated
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void AddonDuplicateMe()
//        {
//            // do something when object will be dupplicate
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void AddonEnableMe()
//        {
//            // do something when object will be enabled
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void AddonDisableMe()
//        {
//            // do something when object will be disabled
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void AddonTrashMe()
//        {
//            // do something when object will be put in trash
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void AddonUnTrashMe()
//        {
//            // do something when object will be remove from trash
//        }
//        //-------------------------------------------------------------------------------------------------------------
//#if UNITY_EDITOR
//        //-------------------------------------------------------------------------------------------------------------
//        //Addons for Edition
//        //-------------------------------------------------------------------------------------------------------------
//        public override bool AddonEdited(bool sNeedBeUpdate)
//        {
//            if (sNeedBeUpdate == true)
//            {
//                // do something
//            }
//            return sNeedBeUpdate;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override float AddonEditor(Rect sInRect)
//        {
//            // Draw the interface addon for editor
//            float tYadd = 0.0f;
//            return tYadd;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override float AddonEditorHeight()
//        {
//            // Height calculate for the interface addon for editor
//            float tYadd = 0.0f;
//            return tYadd;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//#endif
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//    }
//    //-------------------------------------------------------------------------------------------------------------
//}
////=====================================================================================================================