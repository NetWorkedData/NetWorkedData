// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:41:49
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDTipKey : NWDBasis<NWDTipKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        static List<NWDTipKey> ListForRandom = new List<NWDTipKey>();
        //-------------------------------------------------------------------------------------------------------------
        public NWDTipKey()
        {
            //Debug.Log("NWDTipsAndTricks Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTipKey(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDTipsAndTricks Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            Weighting = 1;
            Title = new NWDLocalizableStringType();
            SubTitle = new NWDLocalizableStringType();
            Message = new NWDLocalizableTextType();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDTipKey> PrepareListForRandom()
        {
            ListForRandom.Clear();
            foreach (NWDTipKey tObject in NWDTipKey.GetReachableDatas())
            {
                /* I list the object compatible with request
			 	* I insert in the list  each object (Frequency) times
			 	* I return the List
				*/
                for (int i = 0; i < tObject.Weighting; i++)
                {
                    ListForRandom.Add(tObject);
                }
            }
            return ListForRandom;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDTipKey> PrepareListForRandom(NWDWorld sWorld)
        {
            ListForRandom.Clear();
            foreach (NWDTipKey tObject in NWDTipKey.GetReachableDatas())
            {
                bool tAdd = true;
                if (sWorld != null && tObject.WorldList.ContainsReference(sWorld.Reference) == false)
                {
                    tAdd = false;
                }
                if (tAdd == true)
                {
                    for (int i = 0; i < tObject.Weighting; i++)
                    {
                        ListForRandom.Add(tObject);
                    }
                }
            }
            return ListForRandom;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDTipKey> PrepareListForRandom(NWDCategory sCategory)
        {
            ListForRandom.Clear();
            foreach (NWDTipKey tObject in NWDTipKey.GetReachableDatas())
            {
                bool tAdd = true;

                if (sCategory != null)
                {
                    bool tValid = false;
                    foreach (NWDCategory tCat in tObject.CategoryList.FindDatas())
                    {
                        if (tCat.Containts(sCategory))
                        {
                            tValid = true;
                            break;
                        }
                    }
                    if (tValid == false)
                    {
                        tAdd = false;
                    }
                }
                if (tAdd == true)
                {
                    for (int i = 0; i < tObject.Weighting; i++)
                    {
                        ListForRandom.Add(tObject);
                    }
                }
            }
            return ListForRandom;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDTipKey> PrepareListForRandom(NWDWorld sWorld, NWDCategory sCategory, NWDFamily sFamily, NWDKeyword sKeyword)
        {
            ListForRandom.Clear();
            foreach (NWDTipKey tObject in NWDTipKey.GetReachableDatas())
            {
                bool tAdd = true;
                if (sWorld != null && tObject.WorldList.ContainsReference(sWorld.Reference) == false)
                {
                    tAdd = false;
                }
                if (sCategory != null)
                {
                    bool tValid = false;
                    foreach (NWDCategory tCat in tObject.CategoryList.FindDatas())
                    {
                        if (tCat.Containts(sCategory))
                        {
                            tValid = true;
                            break;
                        }
                    }
                    if (tValid == false)
                    {
                        tAdd = false;
                    }
                }
                if (sFamily != null && tObject.FamilyList.ContainsReference(sFamily.Reference) == false)
                {
                    tAdd = false;
                }
                if (sKeyword != null && tObject.KeywordList.ContainsReference(sKeyword.Reference) == false)
                {
                    tAdd = false;
                }
                if (tAdd == true)
                {
                    for (int i = 0; i < tObject.Weighting; i++)
                    {
                        ListForRandom.Add(tObject);
                    }
                }
            }
            return ListForRandom;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDTipKey SelectRandomTips(bool sAbsoluteRemove = false)
        {
            NWDTipKey rReturn = null;
            // I select the tick by random 
            int tCount = ListForRandom.Count - 1;
            int tIndex = UnityEngine.Random.Range(0, tCount);
            if (tIndex >= 0 && tIndex <= tCount)
            {
                rReturn = ListForRandom[tIndex];
                if (sAbsoluteRemove == false)
                {
                    ListForRandom.RemoveAt(tIndex);
                }
                else
                {
                    while (ListForRandom.Contains(rReturn))
                    {
                        ListForRandom.Remove(rReturn);
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Visualized()
        {
            NWDUserTip tUserTip = NWDUserTip.FindDataByTip(this);
            if (tUserTip == null)
            {
                tUserTip = NWDUserTip.NewData();
                tUserTip.Tip.SetData(this);
            }
            tUserTip.AlreadyVisualize = true;
            tUserTip.ViewingNumber++;
            tUserTip.UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================