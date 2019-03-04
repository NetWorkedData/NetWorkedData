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
    ///         public NWDExampleConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDExample tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
	[Serializable]
    public class NWDItemGroupConnection : NWDConnection<NWDItemGroup>
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool Contains(NWDItem sItem, bool sNotConnectedResult = false)
        {
            bool rReturn = sNotConnectedResult;
            NWDItemGroup tItemGroup = GetObject();
            if (tItemGroup != null)
            {
                rReturn = tItemGroup.ItemList.ContainsObject(sItem);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ITG")]
    [NWDClassDescriptionAttribute("Item Group descriptions Class")]
    [NWDClassMenuNameAttribute("Item Group")]
    public partial class NWDItemGroup : NWDBasis<NWDItemGroup>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        [NWDGroupStartAttribute("Description Item", true, true, true)] // ok
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Item(s) in this group", true, true, true)] // ok
        public NWDReferencesListType<NWDItem> ItemList
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemGroup()
        {
            //Debug.Log("NWDItemGroup Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemGroup(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDItemGroup Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void MyClassMethod()
        {
            // do something with this class
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            CheckMyItems();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            ItemList = new NWDReferencesListType<NWDItem>();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDeleteMe()
        {
            // do something when object will be delete from local base
            //Debug.Log("AddonDeleteMe()");
            ItemList = new NWDReferencesListType<NWDItem>();
            CheckMyItems();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckMyItems()
        {
            List<NWDItem> tActualItems = ItemList.GetObjectsList();
            foreach (NWDItem tItem in NWDItem.BasisHelper().Datas)
            {
                if (tActualItems.Contains(tItem))
                {
                    if (tItem.ItemGroupList.GetObjectsList().Contains(this) == true)
                    {
                        // ok It's contains me
                    }
                    else
                    {
                        // oh item group not contains me! WHYYYYYYYY
                        tItem.ItemGroupList.AddObject(this);
                        tItem.UpdateData();
                        foreach (NWDCraftBook tCraftbook in NWDCraftBook.BasisHelper().Datas)
                            {
                            if (tCraftbook.ItemGroupIngredient.ContainsObject(this))
                            {
                                tCraftbook.RecalculMe();
                                tCraftbook.UpdateDataIfModified();
                            }
                        }
                    }
                }
                else
                {
                    if (tItem.ItemGroupList.GetObjectsList().Contains(this))
                    {
                        // Oh This ItemGroup contains me but I not refere it ... remove me from it
                        tItem.ItemGroupList.RemoveObjects(new NWDItemGroup[] { this });
                        tItem.UpdateData();
                        foreach (NWDCraftBook tCraftbook in NWDCraftBook.BasisHelper().Datas)
                        {
                            if (tCraftbook.ItemGroupIngredient.ContainsObject(this))
                            {
                                tCraftbook.RecalculMe();
                                tCraftbook.UpdateDataIfModified();
                            }
                        }
                    }
                    else
                    {
                        // ok i'ts not contains me!
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------

        //		public static List<NWDItemGroup> GetItemGroupForItem (NWDItem sItem)
        //		{
        //			List<NWDItemGroup> rReturn = new List<NWDItemGroup> ();
        //			foreach (NWDItemGroup tGroup in GetAllObjects()) {
        //				if (tGroup.ItemList.ContainsObject (sItem)) {
        //					rReturn.Add (tGroup);
        //				}
        //			}
        //			return rReturn;
        //		}

        //-------------------------------------------------------------------------------------------------------------

    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================