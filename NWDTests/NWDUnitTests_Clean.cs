//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:53
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using NetWorkedData;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Clean all local datas and put synchronized data in trash state.
        /// </summary>
        /// <param name="sLocalDataOnly"></param>
        public static void CleanUnitTests(bool sLocalDataOnly = true)
        {
            List<NWDTypeClass> tToDelete = new List<NWDTypeClass>();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                foreach (NWDTypeClass tObject in tHelper.Datas)
                {
                    if (tObject.Tag == NWDBasisTag.UnitTestToDelete)
                    {
                        //if (tObject.DevSync < 0 && tObject.PreprodSync < 0 && tObject.ProdSync < 0)
                        if (tObject.DevSync < 1 && tObject.PreprodSync < 1 && tObject.ProdSync < 1)
                            {
                            // local ... delete now
                            tToDelete.Add(tObject);
                        }
                        else
                        {
                            // not local data ... put in trash and Sync on server
                            if (sLocalDataOnly == false)
                            {
                                tObject.TrashData();
                            }
                        }
                    }
                }
            }
            foreach (NWDTypeClass tObject in tToDelete)
            {
                tObject.DeleteData(NWDWritingMode.MainThread);
            }
            if (sLocalDataOnly == false)
            {
                // sync with server now?
                // TODO : force sync on server? And Trash on server?
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================