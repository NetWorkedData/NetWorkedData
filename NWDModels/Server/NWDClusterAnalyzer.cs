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
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDClusterAnalyzer
    {
        //-------------------------------------------------------------------------------------------------------------
        private static bool kInProgress = false;
        //-------------------------------------------------------------------------------------------------------------
        static public void CheckAllCluster()
        {
            if (kInProgress == false)
            {
                //Debug.Log("NWDClusterAnalyzer start process!");
                kInProgress = true;
                //Debug.Log("NWDClusterAnalyzer load all datas for analyze!");
                NWDBasisHelper tNWDServerDomainHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServerDomain));
                if (tNWDServerDomainHelper.IsLoaded() == false)
                {
                    tNWDServerDomainHelper.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
                }
                NWDBasisHelper tNWDServerDatas = NWDBasisHelper.FindTypeInfos(typeof(NWDServerDatas));
                if (tNWDServerDatas.IsLoaded() == false)
                {
                    tNWDServerDatas.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
                }
                NWDBasisHelper tNWDServerServices = NWDBasisHelper.FindTypeInfos(typeof(NWDServerServices));
                if (tNWDServerServices.IsLoaded() == false)
                {
                    tNWDServerServices.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
                }
                NWDBasisHelper tNWDCluster = NWDBasisHelper.FindTypeInfos(typeof(NWDCluster));
                if (tNWDCluster.IsLoaded() == false)
                {
                    tNWDCluster.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
                }
                //Debug.Log("NWDClusterAnalyzer set false by default process!");
                foreach (NWDServerDomain tServerDomains in tNWDServerDomainHelper.Datas)
                {
                    tServerDomains.Dev = false;
                    tServerDomains.Preprod = false;
                    tServerDomains.Prod = false;
                }
                foreach (NWDServerDatas tServerDatas in tNWDServerDatas.Datas)
                {
                    tServerDatas.Dev = false;
                    tServerDatas.Preprod = false;
                    tServerDatas.Prod = false;
                }
                foreach (NWDServerServices tServerServices in tNWDServerServices.Datas)
                {
                    tServerServices.Dev = false;
                    tServerServices.Preprod = false;
                    tServerServices.Prod = false;
                }
                //Debug.Log("NWDClusterAnalyzer loop process!");
                // Analyze each cluster
                foreach (NWDCluster tCluster in tNWDCluster.Datas)
                {
                    tCluster.PropertiesPrevent();
                    StringBuilder tInformations = new StringBuilder();
                    if (tCluster.IsEnable())
                    {
                        //Debug.Log("<b><color=red>Analyze</color> Cluster : " + tCluster.InternalKey + "</b>");
                        tCluster.Domains.Flush();
                        foreach (NWDServerServices tServerServices in tCluster.Services.GetEditorDatas())
                        {
                            //Debug.Log("Analyze Cluster analyze tServerServices : " + tServerServices.InternalKey);
                            if (tServerServices.IsEnable())
                            {
                                if (tServerServices.ServerDomain != null)
                                {
                                    NWDServerDomain tServerDomains = tServerServices.ServerDomain.GetEditorData() as NWDServerDomain;
                                    if (tServerDomains != null)
                                    {
                                        //Debug.Log("Analyze Cluster analyze  tServerDomains : " + tServerDomains.InternalKey);
                                        if (tServerDomains.IsEnable())
                                        {
                                            //Debug.Log("Analyze Cluster add  domains : " + tServerDomains.InternalKey);
                                            tCluster.Domains.AddData(tServerDomains);
                                        }
                                        else
                                        {
                                            //Debug.Log("Analyze Cluster domains : " + tServerDomains.InternalKey + " IS DISABLE ?");
                                        }
                                    }
                                    else
                                    {
                                        //Debug.Log("Analyze Cluster tServerDomains is NULL");
                                    }
                                }
                                else
                                {
                                    //Debug.Log("Analyze Cluster tServerServices : " + tServerServices.InternalKey + " ServerDomain is NULL");
                                }
                            }
                            else
                            {
                                //Debug.Log("Analyze Cluster tServerServices : " + tServerServices.InternalKey + " IS DISABLE !?");
                            }
                        }
                        if (tCluster.Dev == true)
                        {
                            tInformations.AppendLine("• Cluster in active in Dev Environment (" + NWDAppConfiguration.SharedInstance().DevEnvironment.Environment + ") ");
                            if (tCluster.Domains.Count() > 0)
                            {
                                tInformations.AppendLine("  • DNS used are : ");
                                foreach (NWDServerDomain tServerDomains in tCluster.Domains.GetEditorDatas())
                                {
                                    if (tServerDomains != null)
                                    {
                                        if (tServerDomains.IsEnable())
                                        {
                                            tInformations.AppendLine("    • " + tServerDomains.ServerDNS + " ");
                                            tServerDomains.Dev = true;
                                            tServerDomains.DevSyncActive(true);
                                        }
                                    }
                                }
                            }
                            if (tCluster.Services.Count() > 0)
                            {
                                tInformations.AppendLine("  • Services used are : ");
                                foreach (NWDServerServices tServerServices in tCluster.Services.GetEditorDatas())
                                {
                                    if (tServerServices != null)
                                    {
                                        if (tServerServices.IsEnable())
                                        {
                                            NWDServerDomain tDomain = tServerServices.ServerDomain.GetEditorData() as NWDServerDomain;
                                            if (tDomain != null)
                                            {
                                                if (tDomain.IsEnable())
                                                {
                                                    tInformations.AppendLine("    • " + tServerServices.InternalKey + " ");
                                                    tServerServices.Dev = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (tCluster.DataBases.Count() > 0)
                            {
                                tInformations.AppendLine("  • Databases used are : ");
                                foreach (NWDServerDatas tServerDatas in tCluster.DataBases.GetEditorDatas())
                                {
                                    if (tServerDatas != null)
                                    {
                                        if (tServerDatas.IsEnable())
                                        {
                                            tInformations.AppendLine("    • " + tServerDatas.MySQLIP.ToString() + " ");
                                            tServerDatas.Dev = true;
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
                                tInformations.AppendLine("  • DNS used are : ");
                                foreach (NWDServerDomain tServerDomains in tCluster.Domains.GetEditorDatas())
                                {
                                    if (tServerDomains != null)
                                    {
                                        if (tServerDomains.IsEnable())
                                        {
                                            tInformations.AppendLine("    • " + tServerDomains.ServerDNS + " ");
                                            tServerDomains.Preprod = true;
                                            tServerDomains.PreprodSyncActive(true);
                                        }
                                    }
                                }
                            }
                            if (tCluster.Services.Count() > 0)
                            {
                                tInformations.AppendLine("  • Services used are : ");
                                foreach (NWDServerServices tServerServices in tCluster.Services.GetEditorDatas())
                                {
                                    if (tServerServices != null)
                                    {
                                        if (tServerServices.IsEnable())
                                        {
                                            NWDServerDomain tDomain = tServerServices.ServerDomain.GetEditorData() as NWDServerDomain;
                                            if (tDomain != null)
                                            {
                                                if (tDomain.IsEnable())
                                                {
                                                    tInformations.AppendLine("    • " + tServerServices.InternalKey + " ");
                                                    tServerServices.Preprod = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (tCluster.DataBases.Count() > 0)
                            {
                                tInformations.AppendLine("  • Databases used are : ");
                                foreach (NWDServerDatas tServerDatas in tCluster.DataBases.GetEditorDatas())
                                {
                                    if (tServerDatas != null)
                                    {
                                        if (tServerDatas.IsEnable())
                                        {
                                            tInformations.AppendLine("    • " + tServerDatas.MySQLIP.ToString() + " ");
                                            tServerDatas.Preprod = true;
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
                                tInformations.AppendLine("  • DNS used are : ");
                                foreach (NWDServerDomain tServerDomains in tCluster.Domains.GetEditorDatas())
                                {
                                    if (tServerDomains != null)
                                    {
                                        if (tServerDomains.IsEnable())
                                        {
                                            tInformations.AppendLine("    • " + tServerDomains.ServerDNS + " ");
                                            tServerDomains.Prod = true;
                                            tServerDomains.ProdSyncActive(true);
                                        }
                                    }
                                }
                            }
                            if (tCluster.Services.Count() > 0)
                            {
                                tInformations.AppendLine("  • Services used are : ");
                                foreach (NWDServerServices tServerServices in tCluster.Services.GetEditorDatas())
                                {
                                    if (tServerServices != null)
                                    {
                                        if (tServerServices.IsEnable())
                                        {
                                            NWDServerDomain tDomain = tServerServices.ServerDomain.GetEditorData() as NWDServerDomain;
                                            if (tDomain != null)
                                            {
                                                if (tDomain.IsEnable())
                                                {
                                                    tInformations.AppendLine("    • " + tServerServices.InternalKey + " ");
                                                    tServerServices.Prod = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (tCluster.DataBases.Count() > 0)
                            {
                                tInformations.AppendLine("  • Databases used are : ");
                                foreach (NWDServerDatas tServerDatas in tCluster.DataBases.GetEditorDatas())
                                {
                                    if (tServerDatas != null)
                                    {
                                        if (tServerDatas.IsEnable())
                                        {
                                            tInformations.AppendLine("    • " + tServerDatas.MySQLIP.ToString() + " ");
                                            tServerDatas.Prod = true;
                                        }
                                    }
                                }
                            }
                            tInformations.AppendLine("");
                        }
                    }
                    else
                    {
                        if (tCluster.IsTrashed())
                        {
                            tInformations.AppendLine("This cluster is trashed!");
                        }
                        else
                        {
                            tInformations.AppendLine("This cluster is disable!");
                        }
                    }
                    tCluster.Information = tInformations.ToString();
                }
                //Debug.Log("<b><color=red>Analyze Finish</color></b>");
                foreach (NWDServerDomain tServerDomains in tNWDServerDomainHelper.Datas)
                {
                    tServerDomains.UpdateDataIfModified();
                }
                foreach (NWDServerDatas tServerDatas in tNWDServerDatas.Datas)
                {
                    tServerDatas.UpdateDataIfModified();
                }
                foreach (NWDServerServices tServerServices in tNWDServerServices.Datas)
                {
                    tServerServices.UpdateDataIfModified();
                }
                foreach (NWDCluster tCluster in tNWDCluster.Datas)
                {
                    tCluster.UpdateDataIfModified();
                }
                NWDAppConfiguration.SharedInstance().ServerEnvironmentCheck();
                kInProgress = false;
                //Debug.Log("NWDClusterAnalyzer finished process!");
            }
            else
            {
                //Debug.Log("NWDClusterAnalyzer in progres...");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif