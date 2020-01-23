//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:13
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
	{
		//-------------------------------------------------------------------------------------------------------------
		public void SavePreferences (NWDAppEnvironment sEnvironment)
        {
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.SavePreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.SavePreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.SavePreferences();
#else
            sEnvironment.SavePreferences ();
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		public void LoadPreferences (NWDAppEnvironment sEnvironment)
		{
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.LoadPreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.LoadPreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.LoadPreferences();
#else
            sEnvironment.LoadPreferences ();
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		public void ResetPreferences (NWDAppEnvironment sEnvironment)
        {
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.ResetPreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.ResetPreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.ResetPreferences();
#else
            sEnvironment.ResetPreferences ();
#endif
		}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================