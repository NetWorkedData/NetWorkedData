// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:47
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System;
using SQLite.Attribute;
using UnityEngine;

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
        public NWDTypeClassReference(bool sFromDatabase)
        {
            // come from table select
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
        protected bool InDatabase = false;
        protected bool FromDatabase = false;
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart(NWD.K_INSPECTOR_BASIS)]
        [PrimaryKey, AutoIncrement, NWDNotEditable]
        [NWDCertified]
        public int ID
        {
            get; set;
        }
        [Indexed(NWD.K_BASIS_INDEX, 0)]
        [NWDNotEditable]
        [NWDCertified]
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
        [Indexed(NWD.K_BASIS_INDEX, 4)]
        public int WebModel
        {
            get; set;
        }
        [Indexed(NWD.K_INTERNAL_INDEX, 0)]
        [NWDNotEditable]
        [NWDCertified]
        public string InternalKey
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(NWD.K_INTERNAL_INDEX, 0)]
        public string InternalDescription
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public string Preview
        {
            get; set;
        }
        [Indexed(NWD.K_EDITOR_INDEX, 0)]
        [NWDNotEditable]
        [NWDCertified]
        public bool AC
        {
            get; set;
        }
        [Indexed(NWD.K_EDITOR_INDEX, 1)]
        [NWDNotEditable]
        [NWDCertified]
        public int DC
        {
            get; set;
        }
        [Indexed(NWD.K_EDITOR_INDEX, 2)]
        [NWDNotEditable]
        [NWDCertified]
        public int DM
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public int DD
        {
            get; set;
        }
        [Indexed(NWD.K_EDITOR_INDEX, 3)]
        [NWDNotEditable]
        [NWDCertified]
        public int XX
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public string Integrity
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public int DS
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(NWD.K_BASIS_INDEX, 3)]
        public int DevSync
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(NWD.K_BASIS_INDEX, 2)]
        public int PreprodSync
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(NWD.K_BASIS_INDEX, 1)]
        public int ProdSync
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public NWDBasisTag Tag
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public string ServerHash
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public string ServerLog
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
            //BTBBenchmark.Start();
            WritingLocksCounter++;
            if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
            {
                NWDDataManager.SharedInstance().kDataInWriting.Add(this);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Writing lock open once. If lock =0 then the object can change writing mode.
        /// </summary>
        public void WritingLockRemove()
        {
            //BTBBenchmark.Start();
            WritingLocksCounter--;
            if (WritingLocksCounter == 0)
            {
                WritingState = NWDWritingState.Free;
                if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
                {
                    NWDDataManager.SharedInstance().kDataInWriting.Remove(this);
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InstanceInit()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void Initialization()
        {
            // for developper
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string NewReference()
        {
            return "ERROR";
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void WebserviceVersionCheckMe()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool WebserviceVersionIsValid()
        {
            return true;
        }
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
        public virtual void LoadedFromDatabase()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void Index()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ReIndex()
        {
            Desindex();
            Index();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void Desindex()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ChangeAssetPathMe(string sOldPath, string sNewPath)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        //public virtual string ClassNameUsedValue()
        //{
        //    return string.Empty;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool DataIntegrityState()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool TestIntegrity()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsEnable()
        {
            return AC;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ReOrderLocalizationsValues(string[] sLanguageArray)
        {
        }
        public virtual string ExportCSV(string[] sLanguageArray)
        {
            return string.Empty;
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
        public virtual Texture2D PreviewTexture2D()
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void EnableData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void RowAnalyze()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual NWDTypeClass Base_DuplicateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DisableData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //public virtual float DrawInspectorHeight(NWDNodeCard sNodalCard, float sWidth)
        //{
        //    return 0;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DrawEditor(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
        }
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
#endif
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
        public virtual void TrashData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UnTrashData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool IsReacheableByGameSave(NWDGameSave sGameSave)
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool VisibleByGameSave(string sGameSaveReference)
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool VisibleByAccountByEqual(string sAccountReference)
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool VisibleByAccount(string sAccountReference)
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool IsReacheableByAccount(string sAccountReference = null)
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
            public bool IsUsable()
        {
            if (AC == true && XX <= 0 && TestIntegrity() == true)
            {
                return true;
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public virtual bool TrashState()
        //{
        //    return false;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual void TrashAction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        //public virtual void UpdateIntegrityAction()
        //{
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateIntegrity()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread, bool sWebServiceUpgrade = true, bool sWithCallBack = true)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ErrorCheck()
        {
        }
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
            public string DatasMenu()
        {
            string rReturn = InternalKey + " <" + Reference + ">";
            rReturn = rReturn.Replace("/", " ");
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataProceedWithTransaction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataProceed()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataFinish()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataProceedWithTransaction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataProceed()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataFinish()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataProceedWithTransaction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataProceed()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataFinish()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AnalyzeData()
        {

        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NodeCardAnalyze(NWDNodeCard sCard)
        {

        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ChangeReferenceForAnother(string sOldReference, string sNewReference)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ChangeUser(string sOldUser, string sNewUser)
        {
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

        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void FillDataFromWeb(NWDAppEnvironment sEnvironment, string[] sDataArray)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Delete()
        {
            //this = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDTypeClass()
        {
            //Debug.Log("Destructor : " + Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
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
    public class NWDClassServerSynchronizeAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool ServerSynchronize;
        //-------------------------------------------------------------------------------------------------------------
        public NWDClassServerSynchronizeAttribute(bool sServerSynchronize)
        {
            this.ServerSynchronize = sServerSynchronize;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
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