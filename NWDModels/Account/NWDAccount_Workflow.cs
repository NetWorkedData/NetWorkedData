//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:15
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDAccountEnvironment : int
    {
        InGame = 0, // player state (Prod)
        Dev = 1,    // dev state
        Preprod = 2, // preprod state
        Prod = 3, //NEVER COPY ACCOUNT IN PROD !!!!
        None = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccount : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount() {}
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) {}
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            UseInEnvironment = NWDAccountEnvironment.InGame;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AccountCanSignOut()
        {
            bool rReturn = true;
            foreach (NWDAccountSign tSign in NWDAccountSign.GetReachableDatasAssociated())
            {
                if (tSign.SignHash == NWDAppEnvironment.SelectedEnvironment().SecretKeyDevice())
                {
                    rReturn = false;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
