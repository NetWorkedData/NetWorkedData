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
    public partial class NWDUserPreferences : NWDBasis<NWDUserPreferences>
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
        static Dictionary<string, List<NWDUserPreferences>> kIndex = new Dictionary<string, List<NWDUserPreferences>>();
        private List<NWDUserPreferences> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        private void InsertInIndex()
        {
            //Debug.Log("InsertInIndex reference =" + Reference);
            //BTBBenchmark.Start();
            if (PreferencesKey.GetReference() != null
                && GameSave != null
                && GameSave.GetReference() != null  // permet aussi d'avoir indirectement l'account
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = PreferencesKey.GetReference() + "*" + GameSave.GetReference();
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
                        kIndexList = new List<NWDUserPreferences>();
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
                        kIndexList = new List<NWDUserPreferences>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
            }
            else
            {
                RemoveFromIndex();
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
        static public List<NWDUserPreferences> FindByIndex(NWDPreferencesKey sDataKey, NWDGameSave sGameSave)
        {
            //BTBBenchmark.Start();
            List<NWDUserPreferences> rReturn = null;
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
        static public List<NWDUserPreferences> FindByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            List<NWDUserPreferences> rReturn = null;
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
        static public NWDUserPreferences FindFirstByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            NWDUserPreferences rObject = null;
            List<NWDUserPreferences> rReturn = null;
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
        public static NWDUserPreferences PreferencesForKey(string sDataReference)
        {
            //BTBBenchmark.Start();
            NWDUserPreferences rReturn = FindFirstByIndex(sDataReference);
            NWDPreferencesKey tKey = NWDPreferencesKey.GetDataByReference(sDataReference);
            if (rReturn == null && tKey!=null)
            {
                rReturn = NewData(kWritingMode);
                rReturn.PreferencesKey.SetReference(sDataReference);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                // init the stat with default value
                rReturn.Value.SetString(tKey.Default);
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