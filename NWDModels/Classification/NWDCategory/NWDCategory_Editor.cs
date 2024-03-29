//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
#if NWD_CLASSIFICATION
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCategory : NWDBasis
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
            float tWidth = sRect.width;
            float tX = sRect.x;
            float tY = sRect.y;
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 20);
            tY += NWDGUI.Separator(NWDGUI.MargeLeftRight(sRect)).height;
            foreach (NWDItem tItem in NWDItem.FindByCategory(this))
            {
                GUI.Label(new Rect(tX, tY, tWidth, NWDGUI.kLabelStyle.fixedHeight), tItem.InternalKey + " " + tItem.Reference, NWDGUI.kLabelStyle);
                tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddonNodalHeight(float sCardWidth)
        {
            float tYadd = base.AddonNodalHeight(sCardWidth);
            tYadd += 130;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddonNodal(Rect sRect)
        {
            base.AddonNodal(sRect);
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
        public override void AddonInsertMe()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertedMe()
        {
            UpdateAll();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicatedMe()
        {
            UpdateAll();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            UpdateAll();
        }
        //-------------------------------------------------------------------------------------------------------------
        static void ChildrenAssembly(List<NWDCategory> sList, NWDCategory sCat)
        {
            if (sList.Contains(sCat) == false)
            {
                sList.Add(sCat);
                if (sCat.ChildrenCategoryList != null)
                {
                    foreach (NWDCategory tData in sCat.ChildrenCategoryList.GetEditorDatas())
                    {
                        if (tData != null)
                        {
                            ChildrenAssembly(sList, tData);
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static void UpdateAll()
        {
            foreach (NWDCategory tData in NWDBasisHelper.BasisHelper<NWDCategory>().Datas)
            {
                tData.PropertiesPrevent();
                if (tData.ChildrenCategoryList != null)
                {
                    tData.ChildrenCategoryList.Flush();
                }
                if (tData.CascadeCategoryList != null)
                {
                    tData.CascadeCategoryList.Flush();
                }
            }
            foreach (NWDCategory tData in NWDBasisHelper.BasisHelper<NWDCategory>().Datas)
            {
                int valueOutput;
                IOrderedEnumerable<NWDTypeClass> tDataSubs = NWDBasisHelper.BasisHelper<NWDCategory>().Datas.OrderBy(x => int.TryParse(x.InternalDescription.Split(' ')[0], out valueOutput) ? valueOutput : int.MaxValue);
                foreach (NWDCategory tDataSub in tDataSubs)
                //foreach (NWDCategory tDataSub in NWDBasisHelper.BasisHelper<NWDCategory>().Datas)
                {
                    if (tDataSub != tData)
                    {
                        if (tDataSub.ParentCategoryList != null)
                        {
                            if (tDataSub.ParentCategoryList.ConstaintsData(tData))
                            {
                                if (tData != null && tData.ChildrenCategoryList != null)
                                {
                                    if (tData.ChildrenCategoryList.ConstaintsData(tDataSub) == false)
                                    {
                                        tData.ChildrenCategoryList.AddData(tDataSub);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (NWDCategory tData in NWDBasisHelper.BasisHelper<NWDCategory>().Datas)
            {
                if (tData != null)
                {
                    List<NWDCategory> tList = new List<NWDCategory>();
                    ChildrenAssembly(tList, tData);
                    if (tData.CascadeCategoryList != null)
                    {
                        tData.CascadeCategoryList.Flush();
                        tData.CascadeCategoryList.AddData(tData);
                        tData.CascadeCategoryList.AddDatas(tList.ToArray());
                    }
                    tData.UpdateDataIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
#endif
