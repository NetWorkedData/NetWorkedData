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

using BasicToolBox;
using System.Timers;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDExampleConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDExample tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDUserNetWorkingConnection : NWDConnection<NWDUserNetWorking>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDUserNetWorkingState : int
    {
        Unknow = -1,
        OffLine = 0,
        OnLine = 1,

        NotDisturbe = 2,
        Masked = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UNW")]
    [NWDClassDescriptionAttribute("User statut on Network")]
    [NWDClassMenuNameAttribute("User Net Working")]
    public partial class NWDUserNetWorking : NWDBasis<NWDUserNetWorking>
    {
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        #region Class Properties
        // Your static properties
        #endregion
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        #region Instance Properties
        public NWDReferencesListType<NWDAccount> Account
        {
            get; set;
        }
        public NWDDateTimeType NextUpdate
        {
            get; set;
        }
        public bool NotDisturbe
        {
            get; set;
        }
        public bool Masked
        {
            get; set;
        }
        // perhaps add some stats 
        public int TotalPlay
        {
            get; set;
        }
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNetWorking()
        {
            //Debug.Log("NWDUserNetWorking Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNetWorking(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserNetWorking Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        private static int UpdateDelayInSeconds = 600;
        static bool Started = false;
        static List<Type> OtherData = new List<Type>();
        //-------------------------------------------------------------------------------------------------------------
        public static void ClassInitialization() // call by invoke
        {
            //Debug.Log("NWDUserNetWorking Static ClassInitialization()");
            //Only on player
            //if (Application.isPlaying == true)
            //{
            //    StartUpdate(60, null);
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        //private static Timer NWDUserNetWorkingTimer;
        //-------------------------------------------------------------------------------------------------------------
        //public static void StartUpdate(int sUpdateDelayInSeconds, List<Type> sOtherData)
        //{
        //    Debug.Log("NWDUserNetWorking Static StartUpdate()");
        //    if (Started == false)
        //    {
        //        Started = true;
        //        UpdateDelayInSeconds = sUpdateDelayInSeconds;
        //        if (UpdateDelayInSeconds < 60)
        //        {
        //            UpdateDelayInSeconds = 60;
        //        }
        //        if (sOtherData != null)
        //        {
        //            OtherData = sOtherData;
        //        }
        //        if (OtherData.Contains(typeof(NWDUserNetWorking)) == false)
        //        {
        //            OtherData.Add(typeof(NWDUserNetWorking));
        //        }
        //        // do something with this class
        //        NWDUserNetWorking tUserNetWorking = NWDUserNetWorking.GetFirstObject();
        //        if (tUserNetWorking == null)
        //        {
        //            tUserNetWorking = NWDUserNetWorking.NewObject();
        //            tUserNetWorking.InsertMe();
        //        }
        //        NWDUserNetWorkingTimer = new System.Timers.Timer(UpdateDelayInSeconds * 1000);
        //        NWDUserNetWorkingTimer.Elapsed += new System.Timers.ElapsedEventHandler(NetworkingUpdate);
        //        NWDUserNetWorkingTimer.Interval = UpdateDelayInSeconds * 1000;
        //        NWDUserNetWorkingTimer.Enabled = true;
        //    }
        //}
        public static float DelayInSeconds()
        {
            return (float)UpdateDelayInSeconds;
        }

        public static void PrepareUpdate(int sUpdateDelayInSeconds, List<Type> sOtherData)
        {
            //Debug.Log("NWDUserNetWorking Static StartUpdate()");
            if (Started == false)
            {
                Started = true;
                if (sUpdateDelayInSeconds >= 60)
                {
                    UpdateDelayInSeconds = sUpdateDelayInSeconds;
                }
                if (sOtherData != null)
                {
                    OtherData = sOtherData;
                }
                if (OtherData.Contains(typeof(NWDUserNetWorking)) == false)
                {
                    OtherData.Add(typeof(NWDUserNetWorking));
                }
                // do something with this class
                NWDUserNetWorking tUserNetWorking = NWDUserNetWorking.GetFirstData();
                if (tUserNetWorking == null)
                {
                    tUserNetWorking = NWDUserNetWorking.NewData();
                    tUserNetWorking.InsertData();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
     
        //-------------------------------------------------------------------------------------------------------------
        //private static void NetworkingUpdate(object source, ElapsedEventArgs e)
        //{
        public static void NetworkingUpdate()
            {
                //Debug.Log("NWDUserNetWorking Static NetworkingUpdate()");
                Started = true;
            NWDUserNetWorking tUserNetWorking = NWDUserNetWorking.GetFirstData();
                if (tUserNetWorking != null)
                {
                    DateTime tDateTime = DateTime.Now;
                    int tTimestampA = (int)BTBDateHelper.ConvertToTimestamp(tDateTime);
                    int tTimestampB = (int)BTBDateHelper.ConvertToTimestamp(tUserNetWorking.NextUpdate.ToDateTime());
                    tUserNetWorking.TotalPlay += UpdateDelayInSeconds - tTimestampB + tTimestampA;
                    tDateTime = tDateTime.AddSeconds(UpdateDelayInSeconds);
                    tUserNetWorking.NextUpdate.SetDateTime(tDateTime);
                    tUserNetWorking.UpdateData();
                    NWDDataManager.SharedInstance().AddWebRequestSynchronization(OtherData, true);
                // use AddWebRequestSynchronizationWithBlock?
                }
        }

        //-------------------------------------------------------------------------------------------------------------
        public static void NetworkingOffline()
        {
            //Debug.Log("NWDUserNetWorking Static NetworkingOffline()");
            if (Started == true)
            {
                Started = false;
                NWDUserNetWorking tUserNetWorking = NWDUserNetWorking.GetFirstData();
                if (tUserNetWorking != null)
                {
                    tUserNetWorking.Offline();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void Offline()
        {
            DateTime tDateTime = DateTime.Now;
            int tTimestampA = (int)BTBDateHelper.ConvertToTimestamp(tDateTime);
            int tTimestampB = (int)BTBDateHelper.ConvertToTimestamp(NextUpdate.ToDateTime());
            TotalPlay += UpdateDelayInSeconds - tTimestampB + tTimestampA;
            NextUpdate.SetDateTime(tDateTime);
            UpdateData();
            //NWDDataManager.SharedInstance().AddWebRequestSynchronization(OtherData, true);
            NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDUserNetWorking) }, true);
            // use AddWebRequestSynchronizationWithBlock?
        }
        //-------------------------------------------------------------------------------------------------------------
        //~NWDUserNetWorking()
        //{
        //    Offline();
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public NWDUserNetWorkingState Statut()
        {
            NWDUserNetWorkingState rReturn = NWDUserNetWorkingState.Unknow;
            if (Masked == true)
            {
                rReturn = NWDUserNetWorkingState.Masked;
            }
            else
            {
                if (NextUpdate.ToDateTime() > DateTime.Now)
                {
                    if (NotDisturbe == true)
                    {
                        rReturn = NWDUserNetWorkingState.NotDisturbe;
                    }
                    else
                    {
                        rReturn = NWDUserNetWorkingState.OnLine;
                    }
                }
                else
                {
                    rReturn = NWDUserNetWorkingState.OffLine;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserNetWorking)/*, typeof(NWDUserNickname), etc*/ };
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        public override void AddonUpdatedMeFromWeb()
        {
            // do something when object finish to be updated from CSV from WebService response
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Editor
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the width of node draw.
        /// </summary>
        /// <returns>The on node draw width.</returns>
        /// <param name="sDocumentWidth">S document width.</param>
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds color on node.
        /// </summary>
        /// <returns>The on node color.</returns>
        public override Color AddOnNodeColor()
        {
            return Color.gray;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================