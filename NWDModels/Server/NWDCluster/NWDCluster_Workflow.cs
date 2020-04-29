//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCluster : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        // Declare your static properties and private here
        //-------------------------------------------------------------------------------------------------------------
        static public void CheckAllCluster()
        {
            foreach (NWDServerDomain tServerDomains in NWDBasisHelper.FindTypeInfos(typeof(NWDServerDomain)).Datas)
            {
                tServerDomains.Dev = false;
                tServerDomains.Preprod = false;
                tServerDomains.Prod = false;
            }
            foreach (NWDServerDatas tServerDatas in NWDBasisHelper.FindTypeInfos(typeof(NWDServerDatas)).Datas)
            {
                tServerDatas.Dev = false;
                tServerDatas.Preprod = false;
                tServerDatas.Prod = false;
            }
            foreach (NWDServerServices tServerServices in NWDBasisHelper.FindTypeInfos(typeof(NWDServerServices)).Datas)
            {
                tServerServices.Dev = false;
                tServerServices.Preprod = false;
                tServerServices.Prod = false;
            }
            foreach (NWDCluster tCluster in NWDBasisHelper.FindTypeInfos(typeof(NWDCluster)).Datas)
            {
                tCluster.Domains.Flush();
                StringBuilder tInformations = new StringBuilder();
                foreach (NWDServerServices tServerServices in tCluster.Services.GetRawDatas())
                {
                    if (tServerServices.ServerDomain!=null)
                    {
                    NWDServerDomain tServerDomains = tServerServices.ServerDomain.GetRawData();
                    tCluster.Domains.AddData(tServerDomains);
                    }
                }
                if (tCluster.Dev == true)
                {
                    tInformations.AppendLine("• Cluster in active in Dev Environment (" + NWDAppConfiguration.SharedInstance().DevEnvironment.Environment + ") ");
                    if (tCluster.Domains.Count() > 0)
                    {
                        tInformations.AppendLine("      DNS used are : ");
                        foreach (NWDServerDomain tServerDomains in tCluster.Domains.GetRawDatas())
                        {
                            if (tServerDomains.IsEnable())
                            {
                                tInformations.AppendLine("      - " + tServerDomains.ServerDNS + " ");
                                tServerDomains.Dev = true;
                            }
                        }
                    }
                    if (tCluster.DataBases.Count() > 0)
                    {
                        tInformations.AppendLine("      Database used are : ");
                        foreach (NWDServerDatas tServerDatas in tCluster.DataBases.GetRawDatas())
                        {
                            if (tServerDatas.IsEnable())
                            {
                                tInformations.AppendLine("      - " + tServerDatas.MySQLIP.ToString() + " ");
                                tServerDatas.Dev = true;
                            }
                        }
                    }
                    if (tCluster.DataBases.Count() > 0)
                    {
                        tInformations.AppendLine("      Service used are : ");
                        foreach (NWDServerServices tServerServices in tCluster.Services.GetRawDatas())
                        {
                            if (tServerServices.IsEnable())
                            {
                                if (tServerServices.ServerDomain.GetRawData() != null)
                                {
                                    if (tServerServices.ServerDomain.GetRawData().IsEnable())
                                    {
                                        tInformations.AppendLine("      - " + tServerServices.InternalKey + " ");
                                        tServerServices.Dev = true;
                                    }
                                }
                            }
                        }
                    }
                    tInformations.AppendLine("");
                }
                if (tCluster.Preprod == true)
                {
                    tInformations.AppendLine("• Cluster in active in Preprod Environment (" + NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment + ") ");
                    if (tCluster.Domains.Count() > 0)
                    {
                        tInformations.AppendLine("      DNS used are : ");
                        foreach (NWDServerDomain tServerDomains in tCluster.Domains.GetRawDatas())
                        {
                            if (tServerDomains.IsEnable())
                            {
                                tInformations.AppendLine("      - " + tServerDomains.ServerDNS + " ");
                                tServerDomains.Preprod = true;
                            }
                        }
                    }
                    if (tCluster.DataBases.Count() > 0)
                    {
                        tInformations.AppendLine("      Database used are : ");
                        foreach (NWDServerDatas tServerDatas in tCluster.DataBases.GetRawDatas())
                        {
                            if (tServerDatas.IsEnable())
                            {
                                tInformations.AppendLine("      - " + tServerDatas.MySQLIP.ToString() + " ");
                                tServerDatas.Preprod = true;
                            }
                        }
                    }
                    if (tCluster.DataBases.Count() > 0)
                    {
                        tInformations.AppendLine("      Service used are : ");
                        foreach (NWDServerServices tServerServices in tCluster.Services.GetRawDatas())
                        {
                            if (tServerServices.IsEnable())
                            {
                                if (tServerServices.ServerDomain.GetRawData() != null)
                                {
                                    if (tServerServices.ServerDomain.GetRawData().IsEnable())
                                    {
                                        tInformations.AppendLine("      - " + tServerServices.InternalKey + " ");
                                        tServerServices.Preprod = true;
                                    }
                                }
                            }
                        }
                    }
                    tInformations.AppendLine("");
                }
                if (tCluster.Prod == true)
                {
                    tInformations.AppendLine("• Cluster in active in Prod Environment (" + NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment + ") ");
                    if (tCluster.Domains.Count() > 0)
                    {
                        tInformations.AppendLine("      DNS used are : ");
                        foreach (NWDServerDomain tServerDomains in tCluster.Domains.GetRawDatas())
                        {
                            if (tServerDomains.IsEnable())
                            {
                                tInformations.AppendLine("      - " + tServerDomains.ServerDNS + " ");
                                tServerDomains.Prod = true;
                            }
                        }
                    }
                    if (tCluster.DataBases.Count() > 0)
                    {
                        tInformations.AppendLine("      Database used are : ");
                        foreach (NWDServerDatas tServerDatas in tCluster.DataBases.GetRawDatas())
                        {
                            if (tServerDatas.IsEnable())
                            {
                                tInformations.AppendLine("      - " + tServerDatas.MySQLIP.ToString() + " ");
                                tServerDatas.Prod = true;
                            }
                        }
                    }
                    if (tCluster.DataBases.Count() > 0)
                    {
                        tInformations.AppendLine("      Service used are : ");
                        foreach (NWDServerServices tServerServices in tCluster.Services.GetRawDatas())
                        {
                            if (tServerServices.IsEnable())
                            {
                                if (tServerServices.ServerDomain.GetRawData() != null)
                                {
                                    if (tServerServices.ServerDomain.GetRawData().IsEnable())
                                    {
                                        tInformations.AppendLine("      - " + tServerServices.InternalKey + " ");
                                        tServerServices.Prod = true;
                                    }
                                }
                            }
                        }
                    }
                    tInformations.AppendLine("");
                }
                tCluster.Information = tInformations.ToString();
                tCluster.UpdateDataIfModified();
            }
            foreach (NWDServerDomain tServerDomains in NWDBasisHelper.FindTypeInfos(typeof(NWDServerDomain)).Datas)
            {
                tServerDomains.UpdateDataIfModified();
            }
            foreach (NWDServerDatas tServerDatas in NWDBasisHelper.FindTypeInfos(typeof(NWDServerDatas)).Datas)
            {
                tServerDatas.UpdateDataIfModified();
            }
            foreach (NWDServerServices tServerServices in NWDBasisHelper.FindTypeInfos(typeof(NWDServerServices)).Datas)
            {
                tServerServices.UpdateDataIfModified();
            }

            NWDAppConfiguration.SharedInstance().ServerEnvironmentCheck();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InstanceMethodExample()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------------------------------------------------------
        #region Instance Initialization
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================