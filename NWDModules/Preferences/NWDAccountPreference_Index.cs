//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountPreference : NWDBasis<NWDAccountPreference>
    {
        //-------------------------------------------------------------------------------------------------------------
        //static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDAccountPreference>> kIndex = new Dictionary<string, List<NWDAccountPreference>>();
        private List<NWDAccountPreference> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonIndexMe()
        {
            InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDesindexMe()
        {
            RemoveFromIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InsertInIndex()
        {
            if (PreferenceKey != null)
            {
                //BTBBenchmark.Start();
                if (PreferenceKey.GetReference() != null
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
                {
                    string tKey = PreferenceKey.GetReference();
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
                            kIndexList = new List<NWDAccountPreference>();
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
                            kIndexList = new List<NWDAccountPreference>();
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
        static public List<NWDAccountPreference> FindByIndex(NWDAccountPreference sDataKey)
        {
            //BTBBenchmark.Start();
            List<NWDAccountPreference> rReturn = null;
            if (sDataKey != null)
            {
                string tKey = sDataKey.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDAccountPreference> FindByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            List<NWDAccountPreference> rReturn = null;
            if (sDataKeyReference != null)
            {
                string tKey = sDataKeyReference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDAccountPreference FindFirstByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            NWDAccountPreference rObject = null;
            List<NWDAccountPreference> rReturn = null;
            if (sDataKeyReference != null)
            {
                string tKey = sDataKeyReference;
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
        public static NWDAccountPreference PreferencesForKey(string sDataReference)
        {
            NWDAccountPreference rReturn = FindFirstByIndex(sDataReference);
            NWDPreferenceKey tKey = NWDPreferenceKey.GetDataByReference(sDataReference);
            if (rReturn == null && tKey != null)
            {
                rReturn = NewData();
                #if UNITY_EDITOR
                rReturn.InternalKey = NWDAccountNickname.GetNickname();
                #endif
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.PreferenceKey.SetReference(sDataReference);
                rReturn.Value.SetValue(tKey.Default.Value);
                rReturn.SaveData();
            }
            
            if (rReturn == null)
            {
                rReturn = NewData();
                #if UNITY_EDITOR
                rReturn.InternalKey = "NoKeyError - " + NWDAccountNickname.GetNickname();
                #endif
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.PreferenceKey.SetReference(sDataReference);
                rReturn.SaveData();
            }
            
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================