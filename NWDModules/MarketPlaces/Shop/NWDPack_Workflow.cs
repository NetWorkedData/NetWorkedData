//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDPack :NWDBasis <NWDPack>
	{
		//-------------------------------------------------------------------------------------------------------------
        public NWDPack()
        {
            //Debug.Log("NWDPack Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDPack(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDPack Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Enrichment(string sText, int sCpt = 0, string sLanguage = null, bool sBold = true)
        {
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }

            // Replace Tag by Item Name
            NWDItem tItem = ItemDescription.GetObject();
            string tName = "[Missing Detail]";
            if (tItem != null)
            {
                tName = tItem.Name.GetLocalString();
            }
            string rText = sText.Replace("#P" + sCpt + BTBConstants.K_HASHTAG, tBstart + tName + tBend);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetIAPKey()
        {
            NWDInAppPack tInAppPack = InAppReference.GetObject();
            if (tInAppPack != null)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    return tInAppPack.GoogleID;
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                   return tInAppPack.AppleID;
                }
            }

            return "NoSupportedKeyFound";
        }
        //-------------------------------------------------------------------------------------------------------------
		public NWDItem[] GetAllItemsInPack ()
		{
			List<NWDItem> tlist = new List<NWDItem> ();
			foreach (NWDItemPack tItemPack in ItemPackReference.GetObjects ()) {
				tlist.AddRange (tItemPack.Items.GetObjects ());
			}
			return tlist.ToArray ();
		}
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesQuantityType<NWDItem> GetAllItemReferenceAndQuantity()
        {
            NWDReferencesQuantityType<NWDItem> rResult = new NWDReferencesQuantityType<NWDItem>();
            Dictionary<string, int> tDico = new Dictionary<string, int>();

            foreach (KeyValuePair<NWDItemPack, int> pair in ItemPackReference.GetObjectAndQuantity())
            {
                // Get Item Pack data
                NWDItemPack tItemPack = pair.Key;
                int tItemPackQte = pair.Value;

                // Init all Items in Item Pack
                Dictionary<NWDItem, int> tItems = tItemPack.Items.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItem, int> p in tItems)
                {
                    // Get Item data
                    NWDItem tNWDItem = p.Key;
                    int tItemQte = p.Value;

                    if(tDico.ContainsKey(tNWDItem.Reference))
                    {
                        tDico[tNWDItem.Reference] += tItemQte;
                    }
                    else
                    {
                        tDico.Add(tNWDItem.Reference, tItemQte);
                    }
                }
            }

            rResult.SetReferenceAndQuantity(tDico);

            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================