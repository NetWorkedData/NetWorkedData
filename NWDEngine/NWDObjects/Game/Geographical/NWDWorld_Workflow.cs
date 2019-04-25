// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:41:39
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
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDWorld : NWDBasis<NWDWorld>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDWorld()
        {
            //Debug.Log("NWDWorld Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDWorld(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDWorld Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            base.AddonUpdatedMe();
            NWDWorld tParent = this.WorldParent.GetData();
            foreach (NWDWorld tWorld in BasisHelper().Datas)
            {
                if (tWorld != tParent)
                {
                    if (tWorld.WorldChildren != null)
                    {
                        if (tWorld.WorldChildren.ConstaintsData(this) == true)
                        {
                            tWorld.WorldChildren.RemoveDatas(new NWDWorld[] { this });
                            tWorld.UpdateData();
                        }
                    }
                }
            }
            if (tParent != null)
            {
                if (tParent.WorldChildren == null)
                {
                    tParent.WorldChildren = new NWDReferencesListType<NWDWorld>();
                }
                    if (tParent.WorldChildren.ConstaintsData(this) == false)
                {
                    tParent.WorldChildren.AddData(this);
                    tParent.UpdateDataIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
