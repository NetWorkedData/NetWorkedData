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
    public partial class NWDUserPreference : NWDBasis<NWDUserPreference>
    {
        //-------------------------------------------------------------------------------------------------------------
        // Create an Index
        // must ADD InsertInIndex(); in : 
        // AddonLoadedMe()
        // AddonUpdatedMeFromWeb()
        // AddonUpdatedMe()
        // AddonDuplicateMe()
        // must ADD RemoveFromIndex(); in : 
        // AddonDeleteMe()
        static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDUserPreference>> kIndex = new Dictionary<string, List<NWDUserPreference>>();
        private List<NWDUserPreference> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        private void InsertInIndex()
        {
            //Debug.Log("InsertInIndex reference =" + Reference);
            //BTBBenchmark.Start();
            if (PreferenceKey != null)
            {
                if (PreferenceKey.GetReference() != null
                    && GameSave != null
                    && GameSave.GetReference() != null  // permet aussi d'avoir indirectement l'account
                    && IsEnable() == true
                    && IsTrashed() == false
                    && TestIntegrity() == true)
                {
                    string tKey = PreferenceKey.GetReference() + "*" + GameSave.GetReference();
                    if (kIndexList != null)
                    {
                        // I have allready index
                        if (kIndex.ContainsKey(tKey))
                        {
                            if (kIndex[tKey] == kIndexList)
                            {
                                // I am in the good index ... do nothing
                            }
                            else
                            {
                                // I Changed index! during update ?!!
                                kIndexList.Remove(this);
                                kIndexList = null;
                                kIndexList = kIndex[tKey];
                                kIndexList.Add(this);
                            }
                        }
                        else
                        {
                            kIndexList.Remove(this);
                            kIndexList = null;
                            kIndexList = new List<NWDUserPreference>();
                            kIndex.Add(tKey, kIndexList);
                            kIndexList.Add(this);
                        }
                    }
                    else
                    {
                        // I need add in index!
                        if (kIndex.ContainsKey(tKey))
                        {
                            // index exists
                            kIndexList = kIndex[tKey];
                            kIndexList.Add(this);
                        }
                        else
                        {
                            // index must be create
                            kIndexList = new List<NWDUserPreference>();
                            kIndex.Add(tKey, kIndexList);
                            kIndexList.Add(this);
                        }
                    }
                }
                else
                {
                    RemoveFromIndex();
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RemoveFromIndex()
        {
            if (kIndexList != null)
            {
                kIndexList.Contains(this);
                {
                    kIndexList.Remove(this);
                }
                kIndexList = null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserPreference> FindByIndex(NWDPreferenceKey sDataKey, NWDGameSave sGameSave)
        {
            //BTBBenchmark.Start();
            List<NWDUserPreference> rReturn = null;
            if (sDataKey != null && sGameSave != null)
            {
                string tKey = sDataKey.Reference + "*" + sGameSave.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserPreference> FindByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            List<NWDUserPreference> rReturn = null;
            if (sDataKeyReference != null)
            {
                string tKey = sDataKeyReference + "*" + NWDGameSave.Current().Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDUserPreference FindFirstByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            NWDUserPreference rObject = null;
            List<NWDUserPreference> rReturn = null;
            if (sDataKeyReference != null)
            {
                string tKey = sDataKeyReference + "*" + NWDGameSave.Current().Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            if (rReturn != null)
            {
                if (rReturn.Count > 0)
                {
                    rObject = rReturn[0];
                }
            }
            //BTBBenchmark.Finish();
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserPreference PreferencesForKey(string sDataReference)
        {
            //BTBBenchmark.Start();
            NWDUserPreference rReturn = FindFirstByIndex(sDataReference);
            NWDPreferenceKey tKey = NWDPreferenceKey.GetDataByReference(sDataReference);
            if (rReturn == null && tKey!=null)
            {
                rReturn = NewData(kWritingMode);
                rReturn.PreferenceKey.SetReference(sDataReference);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                // init the stat with default value
                rReturn.Value.SetValue(tKey.Default.GetValue());
                // update in writing mode (default is poolThread for stats)
                rReturn.UpdateData(true, kWritingMode);
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================