// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:2
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
using System.Collections.Generic;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDConsent : NWDBasis<NWDConsent>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDConsent[] SelectCurrentDatas()
        {
            List<NWDConsent> rList = new List<NWDConsent>();
            Dictionary<string, NWDConsent> tDico = new Dictionary<string, NWDConsent>();
            NWDConsent[] tConsentList = NWDConsent.GetDatas();
            foreach (NWDConsent tConsent in tConsentList)
            {
                if (tDico.ContainsKey(tConsent.KeyOfConsent) == false)
                {
                    tDico.Add(tConsent.KeyOfConsent, tConsent);
                }
                else
                {
                    if (tDico[tConsent.KeyOfConsent].Version.ToInt() < tConsent.Version.ToInt())
                    {
                        tDico[tConsent.KeyOfConsent] = tConsent;
                    }
                }
            }
            foreach (KeyValuePair<string, NWDConsent> tConsentKeyValue in tDico)
            {
                rList.Add(tConsentKeyValue.Value);
            }
            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================