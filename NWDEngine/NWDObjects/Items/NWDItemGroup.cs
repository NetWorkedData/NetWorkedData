//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:28
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
    public partial class NWDItem : NWDBasis
    {
        [NWDNotEditable]
        public NWDReferencesListType<NWDItemGroup> ItemGroupList
        {
            get; set;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDItemGroupConnection : NWDConnection<NWDItemGroup>
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool Contains(NWDItem sItem, bool sNotConnectedResult = false)
        {
            bool rReturn = sNotConnectedResult;
            NWDItemGroup tItemGroup = GetData();
            if (tItemGroup != null)
            {
                rReturn = tItemGroup.ItemList.ConstaintsData(sItem);
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
    public partial class NWDItemGroup : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        [NWDInspectorGroupStart("Description Item", true, true, true)] // ok
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Item(s) in this group", true, true, true)] // ok
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
        public List<NWDItem> OwnershipIntersection(int sQuantity = 1)
        {
            List<NWDItem> rReturn = new List<NWDItem>();
            foreach (NWDItem tItem in ItemList.GetReachableDatas())
            {
                if (tItem != null)
                {
                    if (NWDUserOwnership.ContainsItem(tItem, sQuantity))
                    {
                        rReturn.Add(tItem);
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckMyItems()
        {
            List<NWDItem> tActualItems = ItemList.GetReachableDatasList();
            foreach (NWDItem tItem in NWDBasisHelper.BasisHelper<NWDItem>().Datas)
            {
                if (tActualItems.Contains(tItem))
                {
                    if (tItem.ItemGroupList.GetReachableDatasList().Contains(this) == true)
                    {
                        // ok It's contains me
                    }
                    else
                    {
                        // oh item group not contains me! WHYYYYYYYY
                        tItem.ItemGroupList.AddData(this);
                        tItem.UpdateData();
                        foreach (NWDCraftBook tCraftbook in NWDBasisHelper.BasisHelper<NWDCraftBook>().Datas)
                            {
                            if (tCraftbook.ItemGroupIngredient.ContainsData(this))
                            {
                                tCraftbook.RecalculMe();
                                tCraftbook.UpdateDataIfModified();
                            }
                        }
                    }
                }
                else
                {
                    if (tItem.ItemGroupList.GetReachableDatasList().Contains(this))
                    {
                        // Oh This ItemGroup contains me but I not refere it ... remove me from it
                        tItem.ItemGroupList.RemoveDatas(new NWDItemGroup[] { this });
                        tItem.UpdateData();
                        foreach (NWDCraftBook tCraftbook in NWDBasisHelper.BasisHelper<NWDCraftBook>().Datas)
                        {
                            if (tCraftbook.ItemGroupIngredient.ContainsData(this))
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
        public override void AddonEditor(Rect sRect)
        {
            // Draw the interface addon for editor
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
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