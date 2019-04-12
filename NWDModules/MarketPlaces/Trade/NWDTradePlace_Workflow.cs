// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:46
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDTradePlace : NWDBasis<NWDTradePlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDTradePlace()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTradePlace(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            RefreshDelay = 60 * 3;
            CancelDelay = 60 * 5;
            RequestLifeTime = 60 * 60 * 24;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetLifeTime()
        {
            // Return value
            string rLifeTime = "";

            // Set Trade Life Time
            DateTime tToday = DateTime.UtcNow;
            DateTime tLifeTime = tToday.AddSeconds(RequestLifeTime);
            TimeSpan tSpan = tLifeTime.Subtract(tToday);
            if (tSpan.TotalSeconds > 0)
            {
                if (tSpan.Days > 0)
                {
                    rLifeTime = tSpan.Days + " day(s)";
                }
                else
                {
                    if (tSpan.Hours > 0)
                    {
                        rLifeTime = tSpan.Hours + "h ";
                    }
                    if (tSpan.Minutes > 0)
                    {
                        rLifeTime  += tSpan.Minutes + "m ";
                    }
                    if (tSpan.Seconds > 0)
                    {
                        rLifeTime += tSpan.Seconds + "s";
                    }
                }
            }

            return rLifeTime;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================