//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:11
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServer : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDServer()
        {
            //Debug.Log("NWDServerConfig Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServer(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDServerConfig Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
            InternalKey = "Server";
            Port = 22;
            FuturPort = 22;
            Admin_User = "admin" + NWDToolbox.RandomStringAlpha(3).ToLower();
            Root_User = "root";
            IP.SetValue("192.168.0.1");
            // no sync please
            DevSync = -1;
            PreprodSync = -1;
            ProdSync = -1;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
