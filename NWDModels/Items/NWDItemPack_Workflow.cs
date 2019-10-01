//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:21
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDItemPack :NWDBasis
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDItemPack()
        {
            //Debug.Log("NWDItemPack Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemPack(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDItemPack Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
            string rText = sText.Replace("#G" + sCpt + NWEConstants.K_HASHTAG, tBstart + tName + tBend);

            return rText;
        }
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================