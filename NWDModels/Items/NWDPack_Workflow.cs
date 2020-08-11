//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if NWD_ITEMPACK
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDPack :NWDBasis
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
            NWDItem tItem = ItemDescription.GetRawData();
            string tName = "[Missing Detail]";
            if (tItem != null)
            {
                tName = tItem.Name.GetLocalString();
            }
            string rText = sText.Replace("#P" + sCpt + NWEConstants.K_HASHTAG, tBstart + tName + tBend);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
		public NWDItem[] GetAllItemsInPack ()
		{
			List<NWDItem> tlist = new List<NWDItem> ();
			foreach (NWDItemPack tItemPack in ItemPackReference.GetReachableDatas ()) {
				tlist.AddRange (tItemPack.Items.GetReachableDatas ());
			}
			return tlist.ToArray ();
		}
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesQuantityType<NWDItem> GetAllItemReferenceAndQuantity()
        {
            NWDReferencesQuantityType<NWDItem> rResult = new NWDReferencesQuantityType<NWDItem>();
            Dictionary<string, int> tDico = new Dictionary<string, int>();

            foreach (KeyValuePair<NWDItemPack, int> pair in ItemPackReference.GetReachableDatasAndQuantities())
            {
                // Get Item Pack data
                NWDItemPack tItemPack = pair.Key;
                int tItemPackQte = pair.Value;

                // Init all Items in Item Pack
                Dictionary<NWDItem, int> tItems = tItemPack.Items.GetReachableDatasAndQuantities();
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
#endif