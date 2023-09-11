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
using System;
using System.Collections.Generic;
using UnityEngine;
using NetWorkedData.NWDORM;
//=====================================================================================================================
#if UNITY_EDITOR
using NetWorkedData.NWDEditor;
using UnityEditor;
using Newtonsoft.Json;
using NWEMiniJSON;
using System.Linq;
using Newtonsoft.Json.Linq;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDTypeClassReference
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Reference
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClassReference(bool sFromDatabase) { }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        protected bool InDatabase = false;
        protected bool FromDatabase = false;
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart(NWD.InspectorBasisHeader)]
        [NWDNotEditable]
        [NWDCertified]
        //[NWDHidden]
        public int RangeAccess
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        [NWDInspectorGroupStart(NWD.K_INSPECTOR_BASIS)]
        [NWDNotEditable]
        [NWDCertified]
        public long ID
        {
            get; set;
        }
        [NWDIndexedAttribut(NWD.K_BASIS_INDEX)]
        //[NWDIndexedAttribut(NWD.K_REFERENCE_INDEX)]
        [NWDNotEditable]
        [NWDCertified]
        [NWDNotEditable]
        [NWDVarChar(256)]
        public string Reference
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDFlagsEnum]
        [NWDCertified]
        public NWDBasisCheckList CheckList
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDIndexedAttribut(NWD.K_BASIS_INDEX)]
        public int WebModel
        {
            get; set;
        }
        //[NWDIndexedAttribut(NWD.K_INTERNAL_INDEX)]
        [NWDNotEditable]
        [NWDCertified]
        [NWDVarChar(256)]
        public string InternalKey
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDVarChar(256)]
        //[NWDIndexedAttribut(NWD.K_INTERNAL_INDEX)]
        public string InternalDescription
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDVarChar(256)]
        public string Preview
        {
            get; set;
        }
        //[NWDIndexedAttribut(NWD.K_REFERENCE_INDEX)]
        //[NWDIndexedAttribut(NWD.K_EDITOR_INDEX)]
        [NWDNotEditable]
        [NWDCertified]
        public bool AC // actif ?
        {
            get; set;
        }
        //[NWDIndexedAttribut(NWD.K_EDITOR_INDEX)]
        [NWDNotEditable]
        [NWDCertified]
        public int DC // date creation
        {
            get; set;
        }
        //[NWDIndexedAttribut(NWD.K_EDITOR_INDEX)]
        [NWDNotEditable]
        [NWDCertified]
        public int DM // date modification
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public int DD // desactivation
        {
            get; set;
        }
        //[NWDIndexedAttribut(NWD.K_EDITOR_INDEX)]
        [NWDNotEditable]
        [NWDCertified]
        public int XX // trash
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDVarChar(256)]
        public string Integrity 
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public int DS //date synchro
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDIndexedAttribut(NWD.K_BASIS_INDEX)]
        public int DevSync //date synchro dev
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDIndexedAttribut(NWD.K_BASIS_INDEX)]
        public int PreprodSync //date synchro preprod
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDIndexedAttribut(NWD.K_BASIS_INDEX)]
        public int ProdSync //date synchro prod
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public NWDBasisTag Tag  // to delete
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDVarChar(256)]
        public string ServerHash  // to delete
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDVarChar(256)]
        public string ServerLog  // to delete
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public bool InError
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The current state of the writing for this object.
        /// </summary>
        public NWDWritingState WritingState = NWDWritingState.Free;
        /// <summary>
        /// The writing lock counter. If lock is close the number is the number of lock!
        /// </summary>
        private int WritingLocksCounter = 0;
        /// <summary>
        /// The writing pending.
        /// </summary>
        public NWDWritingPending WritingPending = NWDWritingPending.Unknow;
        //=============================================================================================================
        // PRIVATE METHOD
        //-------------------------------------------------------------------------------------------------------------
        ~NWDTypeClass() { }
        //=============================================================================================================
        // PUBLIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        public NWDWritingPending DatabasePending()
        {
            return WritingPending;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Writing lock close once more.
        /// </summary>
        public void WritingLockAdd()
        {
            //NWDBenchmark.Start();
            WritingLocksCounter++;
            if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
            {
                NWDDataManager.SharedInstance().kDataInWriting.Add(this);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Writing lock open once. If lock =0 then the object can change writing mode.
        /// </summary>
        public void WritingLockRemove()
        {
            //NWDBenchmark.Start();
            WritingLocksCounter--;
            if (WritingLocksCounter == 0)
            {
                WritingState = NWDWritingState.Free;
                if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
                {
                    NWDDataManager.SharedInstance().kDataInWriting.Remove(this);
                }
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsEnable()
        {
            return AC;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsTrashed()
        {
            if (XX > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsUsable()
        {
            if (AC == true && XX <= 0 && IntegrityIsValid() == true)
            {
                return true;
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DatasMenu()
        {
            string rReturn = InternalKey + " <" + Reference + ">";
            rReturn = rReturn.Replace("/", " ");
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void Delete() {}
        //=============================================================================================================
        // PUBLIC VIRTUAL METHOD
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InstanceInit() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void Initialization() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string NewReference()
        {
            return "ERROR";
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void WebserviceVersionCheckMe() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool WebserviceVersionIsValid()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadedFromDatabase()
        {
            //ReIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public virtual void IndexUpdate() {}
        //-------------------------------------------------------------------------------------------------------------
        public virtual void IndexInBase() { }
        public virtual void DeindexInBase() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void IndexInMemory() { }
        public virtual void DeindexInMemory() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ChangeAssetPathMe(string sOldPath, string sNewPath) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool IntegrityIsValid()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ReOrderLocalizationsValues(string[] sLanguageArray) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string ExportCSV(string[] sLanguageArray)
        {
            return string.Empty;
        }
        public virtual List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            string tReturn = "{"+
                "\"Reference\":"+NWDToolbox.NumericCleaner(Reference)+"" +
                "}";
            return new List<NWDExportObject>()
            {
                new NWDExportObject(sProjectHub, sProjectId, Reference, InternalKey, InternalDescription, tReturn, nameof(NWDTypeClass), false)
            };
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual Texture2D PreviewTexture2D()
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void EnableData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void RowAnalyze() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual NWDTypeClass Base_DuplicateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DisableData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool TrashData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWDBenchmark.Start();
            bool rReturn = false;
            if (XX == 0)
            {
                rReturn = true;
                int tTimestamp = NWDToolbox.Timestamp();
                this.DD = tTimestamp;
                this.XX = 1;
                this.AC = false;
                this.UpdateData(true, sWritingMode);
            }
            return rReturn;
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool UnTrashData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWDBenchmark.Start();
            bool rReturn = false;
            if (XX <= 1)
            {
                rReturn = true;
                this.XX = 0;
                this.AC = false;
                this.UpdateData(true, sWritingMode);
            }
            return rReturn;
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UnTrashable() { return XX <= 1; }

        //-------------------------------------------------------------------------------------------------------------
        public virtual bool IsReacheableBy(string sGameSaveReference, string sAccountReference = null)
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool IsWritableBy(string sGameSaveReference, string sAccountReference = null)
        {
            return false;
        }

        //-------------------------------------------------------------------------------------------------------------
        protected virtual string GetEventualAccount()
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        protected virtual string GetEventualGameSave()
        {
            return string.Empty;
        }


        ////-------------------------------------------------------------------------------------------------------------
        //public bool IsReacheableByGameSave(NWDGameSave sGameSave)
        //{
        //    bool rReturn = false;
        //    if (sGameSave!=null)
        //    {
        //        rReturn = IsReacheableByGamesave(sGameSave.Reference);
        //    }
        //    return true;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual bool IsReacheableByGamesaveAndAccount(NWDGameSave sGameSave)
        //{
        //    bool rReturn = false;
        //    if (sGameSave!=null)
        //    {
        //        rReturn = IsReacheableByGamesaveAndAccount(sGameSave.Reference, sGameSave.Account.GetReference());
        //    }
        //    return true;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual bool IsReacheableByGamesave(string sGameSaveReference)
        //{
        //    return true;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual bool IsReacheableByAccount(string sAccountReference = null)
        //{
        //    return true;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual bool IsReacheableByGamesaveAndAccount(string sGameSaveReference, string sAccountReference = null)
        //{
        //    return IsReacheableByGamesave(sGameSaveReference) && IsReacheableByAccount(sAccountReference);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual bool IsWritableByAccount(string sAccountReference = null)
        //{
        //    return true;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual bool VisibleByGameSave(string sGameSaveReference)
        //{
        //    return true;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual bool VisibleByAccountByEqual(string sAccountReference)
        //{
        //    return true;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual bool VisibleByAccount(string sAccountReference)
        //{
        //    return true;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual void TrashAction() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateIntegrity() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataEditor() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread, bool sWebServiceUpgrade = true, bool sWithCallBack = true) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ErrorCheck() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool IsSynchronized()
        {
            int tD = 0;
            if (NWDAppConfiguration.SharedInstance().IsDevEnvironement())
            {
                tD = DevSync;
            }
            else if (NWDAppConfiguration.SharedInstance().IsPreprodEnvironement())
            {
                tD = PreprodSync;
            }
            else if (NWDAppConfiguration.SharedInstance().IsProdEnvironement())
            {
                tD = ProdSync;
            }

            if (tD > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual int WebModelToUse()
        {
            return 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataProceedWithTransaction() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataProceed(ITransaction sDeviceTransaction, ITransaction sEditorTransaction) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataFinish() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataProceedWithTransaction() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataProceed(ITransaction sDeviceTransaction, ITransaction sEditorTransaction) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataFinish() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataProceedWithTransaction() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataProceed(ITransaction sDeviceTransaction, ITransaction sEditorTransaction) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataFinish() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AnalyzeData() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ChangeReferenceForAnother(string sOldReference, string sNewReference) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ChangeUser(string sOldUser, string sNewUser) {

            Debug.Log("ChangeUser(string sOldUser, string sNewUser) VIRTUAL DO NOTHING");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool IsLockedObject()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string CSVAssembly()
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataFromWeb(NWDAppEnvironment sEnvironment,
                                      string[] sDataArray,
                                      NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void FillDataFromWeb(NWDAppEnvironment sEnvironment, string[] sDataArray) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string DGPRLinearization(string sTypeName, bool sAsssemblyAsCSV = true)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void PropertiesAutofill() { }
        public virtual void PropertiesPrevent() { }
        public virtual void PropertiesMinimal() { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool InsertData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void CopyData(NWDTypeClass sOriginal) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataOperation(bool sAutoDate = true, bool sWebServiceUpgrade = true) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddonDuplicateMe() { }
        public virtual void AddonDuplicatedMe() { }


        //-------------------------------------------------------------------------------------------------------------
        //public virtual bool IsReacheableByAccount(string sAccount)
        //{
        //    bool rReturn = true;
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual void DrawEditor(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        //{
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual void DrawEditorMiddle(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard, bool sEditionEnable)
        //{
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual void DrawEditorBottom(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard, bool sEditionEnable)
        //{
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual float DrawEditorTopHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    return 0;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual float DrawEditorMiddleHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    return DrawInspectorHeight(sNodalCard, sWidth);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual float DrawEditorBottomHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    return 0;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual string InternalKeyValue()
        //{
        //    return string.Empty;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual string InternalDescriptionValue()
        //{
        //    return string.Empty;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual string ReferenceValue()
        //{
        //    return string.Empty;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual string ClassNameUsedValue()
        //{
        //    return string.Empty;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual bool TestIntegrity()
        //{
        //    return true;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual bool TrashState()
        //{
        //    return false;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual void UpdateIntegrityAction()
        //{
        //}
        //------------------------------------------------------------------------------------------------------------- 
        //public virtual bool EnableState()
        //{
        //    return true;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual bool ReachableState()
        //{
        //    return true;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual bool InGameSaveState()
        //{
        //    return true;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual void SetCurrentGameSave()
        //{
        //}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //public virtual float DrawInspectorHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    return 0;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DrawEditor(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard, EditorWindow sWindow) { }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float DrawEditorTotalHeight(NWDNodeCard sNodalCard, float sWidth)
        {
            return 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual Rect DrawRowInEditor(Vector2 sMouseClickPosition, Rect sRectRow, bool sSelectAndClick, int sRow, float sZoom)
        {
            return Rect.zero;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NodeCardAnalyze(NWDNodeCard sCard) { }
        //-------------------------------------------------------------------------------------------------------------
        
        public string GetJSonBaseParameters()
        {
            var tExport = new {
                RangeAccess = RangeAccess,
                ID = ID,
                Reference = Reference,
                CheckList = CheckList, //NWDBasisCheckList
                WebModel = WebModel,
                InternalKey = InternalKey,
                InternalDescription = InternalDescription,
                Preview = Preview,
                AC = AC,
                DC = DC,
                DM = DM,
                DD = DD,
                XX = XX,
                Integrity = Integrity,
                DS = DS,
                DevSync = DevSync,
                PreprodSync = PreprodSync,
                ProdSync = ProdSync,
                Tag = Tag, //NWDBasisTag
                ServerHash = ServerHash,
                ServerLog = ServerLog,
                InError = InError
            };

            string tJson = JsonConvert.SerializeObject(tExport);

            return tJson;
        }

        public string GetJSonMergeWithBase(string sJson)
        {
            JObject rJson = JObject.Parse(sJson);
            JObject tBaseJson = JObject.Parse(GetJSonBaseParameters());

            rJson.Merge(tBaseJson, new JsonMergeSettings {MergeArrayHandling = MergeArrayHandling.Union});

            return rJson.ToString();
        }        
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NWDClassTrigrammeAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Trigramme;
        //-------------------------------------------------------------------------------------------------------------
        public NWDClassTrigrammeAttribute(string sTrigramme)
        {
            this.Trigramme = sTrigramme;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NWDClassMacroAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Macro;
        //-------------------------------------------------------------------------------------------------------------
        public NWDClassMacroAttribute(string sMacro)
        {
            this.Macro = sMacro;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //public class NWDClassUnityEditorOnlyAttribute : Attribute
    //{
    //}
#if UNITY_EDITOR
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NWDWindowOwnerAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public Type WindowType;
        //-------------------------------------------------------------------------------------------------------------
        public NWDWindowOwnerAttribute(Type sWindowType)
        {
            if (sWindowType.IsSubclassOf(typeof(NWDTypeWindow)))
            {
                this.WindowType = sWindowType;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
#endif
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //public class NWDClassServerSynchronizeAttribute : Attribute
    //{
    //    //-------------------------------------------------------------------------------------------------------------
    //    public bool ServerSynchronize;
    //    //-------------------------------------------------------------------------------------------------------------
    //    public NWDClassServerSynchronizeAttribute(bool sServerSynchronize)
    //    {
    //        this.ServerSynchronize = sServerSynchronize;
    //    }
    //    //-------------------------------------------------------------------------------------------------------------
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NWDClassDescriptionAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Description;
        //-------------------------------------------------------------------------------------------------------------
        public NWDClassDescriptionAttribute(string sDescription)
        {
            this.Description = sDescription;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NWDClassMenuNameAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string MenuName;
        //-------------------------------------------------------------------------------------------------------------
        public NWDClassMenuNameAttribute(string sMenuName)
        {
            this.MenuName = sMenuName;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    public class NWDLockUpdateForAccountAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDLockUpdateForAccountAttribute()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    public class NWDNeedAccountAvatarAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PHPstring(string sPropertyName)
        {
            return "if ($uuid!= $tRow['" + sPropertyName + "']) {$ACC_NEEDED['NWDAccountAvatar'][$tRow['" + sPropertyName + "']]= true;}\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    public class NWDNeedUserAvatarAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PHPstring(string sPropertyName)
        {
            return "if ($uuid!= $tRow['" + sPropertyName + "']) {$ACC_NEEDED['NWDUserAvatar'][$tRow['" + sPropertyName + "']]= true;}\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    public class NWDNeedAccountNicknameAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PHPstring(string sPropertyName)
        {
            return "if ($uuid!= $tRow['" + sPropertyName + "']) {$ACC_NEEDED['NWDAccountNickname'][$tRow['" + sPropertyName + "']]= true;}\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    public class NWDNeedUserNicknameAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PHPstring(string sPropertyName)
        {
            return "if ($uuid!= $tRow['" + sPropertyName + "']) {$ACC_NEEDED['NWDUserNickname'][$tRow['" + sPropertyName + "']]= true;}\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    public class NWDNeedReferenceAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string ClassName;
        public Type ClassType;
        //-------------------------------------------------------------------------------------------------------------
        public NWDNeedReferenceAttribute(Type sClassType)
        {
            this.ClassName = sClassType.Name;
            this.ClassType = sClassType;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PHPstring(string sPropertyName)
        {
            return "$REF_NEEDED['" + this.ClassName + "'][$tRow['" + sPropertyName + "']]= true;\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
