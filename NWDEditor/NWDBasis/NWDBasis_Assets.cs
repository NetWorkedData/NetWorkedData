//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void ChangeAssetPathMe(string sOldPath, string sNewPath)
        {
            //NWEBenchmark.Start();
            //Debug.Log("OVERRIDE Reference = " + Reference + " sOldPath = " + sOldPath + " to sNewPath " + sNewPath);
            if (IntegrityIsValid() == true)
            {
                bool tUpdate = false;
                if (Preview != null)
                {
                    if (Preview.Contains(sOldPath))
                    {
                        Preview = Preview.Replace(sOldPath, sNewPath);
                        tUpdate = true;
                    }
                }
                foreach (var tProp in PropertiesAssetDependent())
                {
                    Type tTypeOfThis = tProp.PropertyType;
                    NWDAssetType tValueStruct = (NWDAssetType)tProp.GetValue(this, null);
                    if (tValueStruct != null)
                    {
                        if (tValueStruct.ChangeAssetPath(sOldPath, sNewPath))
                        {
                            tUpdate = true;
                        }
                    }
                }
                if (tUpdate == true)
                {
                    UpdateData(true, NWDWritingMode.ByDefaultLocal);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif