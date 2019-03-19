//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDItemPack :NWDBasis <NWDItemPack>
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
            NWDItem tItem = DescriptionItem.GetObject();
            string tName = "[Missing Detail]";
            if (tItem != null)
            {
                tName = tItem.Name.GetLocalString();
            }
            string rText = sText.Replace("#G" + sCpt + BTBConstants.K_HASHTAG, tBstart + tName + tBend);

            return rText;
        }
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================