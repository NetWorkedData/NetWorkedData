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
    public partial class NWDServerDatas : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatas()
        {
            //Debug.Log("NWDServerConfig Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatas(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDServerConfig Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
            InternalKey = "Unused config";
            Port = 22;
            //User = NWDAppEnvironment.SelectedEnvironment().SFTPUser;
            IP.SetValue("192.168.0.1");

            // no sync please
            DevSync = -1;
            PreprodSync = -1;
            ProdSync = -1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            MySQLUser = NWDToolbox.UnixCleaner(MySQLUser);
            MySQLBase = NWDToolbox.UnixCleaner(MySQLBase);
            Root_MysqlPassword.SetValue(NWDToolbox.UnixCleaner(Root_MysqlPassword.GetValue()));

            Admin_User = NWDToolbox.UnixCleaner(Admin_User);
            Admin_Password.SetValue(NWDToolbox.UnixCleaner(Admin_Password.GetValue()));

            Root_User = NWDToolbox.UnixCleaner(Root_User);
            Root_Password.SetValue(NWDToolbox.UnixCleaner(Root_Password.GetValue()));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif