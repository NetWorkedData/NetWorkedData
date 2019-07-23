//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:35
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR

using System;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTransactionHelper : NWDHelper<NWDUserTransaction>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("// JE DOIS VERIFIER AVEC LES ERVEUR APPLE OU GOOGLE DE LA VALIDITEE DE LA TRANSACTION ET METTRE InAppApprouved EN Approuved OU Refused!");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

#endif