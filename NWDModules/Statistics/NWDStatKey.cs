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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDStatKeyDomain : int
    {
        AccountStat,
        GameSaveStat,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDStatKeyFormat : int
    {
        None = 0,
        Integer = 1,
        DecimalTwo = 2,
        DecimalThree = 3,
        DecimalFour = 4,
        Float = 10,
        // %
        //PurcentTwo = 20,
        // for time show
        Seconds = 30,
        MinutesSeconds = 31,
        HoursMinutesSeconds = 32,
        DaysHoursMinutesSeconds = 33,
    }
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
    ///         public NWDStatKeyConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDStatKey tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDStatKeyConnection : NWDConnection<NWDStatKey>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDStatKey class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("STK")]
    [NWDClassDescriptionAttribute("Stat Key")]
    [NWDClassMenuNameAttribute("Stat Key")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDStatKey : NWDBasis<NWDStatKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your static properties
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Information")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        public NWDStatKeyDomain Domain
        {
            get; set;
        }
        public NWDStatKeyFormat Format
        {
            get; set;
        }
        public bool UnityStat
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Use parent stats")]
        public NWDReferenceType<NWDStatKey> Parent
        {
            get; set;
        }
        public NWDReferencesListType<NWDStatKey> Dependent
        {
            get; set;
        }

        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Counter format (#x# is the value)")]
        public float InitCounter
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Counter #x# time'")]
        public NWDLocalizableStringType NoCounterFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Counter #x# time'")]
        public NWDLocalizableStringType SingleCounterFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Counter #x# time'")]
        public NWDLocalizableStringType PluralCounterFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Total format (#x# is the value)")]
        public float InitTotal
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Total #x# seconds per bit'")]
        public NWDLocalizableStringType NoTotalFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Total #x# seconds per bit'")]
        public NWDLocalizableStringType SingleTotalFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Total #x# seconds per bit'")]
        public NWDLocalizableStringType PluralTotalFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Average format (#x# is the value)")]
        public float InitAverage
        {
            get; set;
        }
        public bool ShowAverageAsPurcent
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType NoAverageFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType SingleAverageFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType PluralAverageFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Average with parent format (#x# is the value)")]
        public float InitAverageWithParent
        {
            get; set;
        }
        public bool ShowAverageWithParentAsPurcent
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType NoAverageWithParentFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType SingleAverageWithParentFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType PluralAverageWithParentFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Last format (#x# is the value)")]
        public float InitLast
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Last #x# seconds per bit'")]
        public NWDLocalizableStringType NoLastFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Last #x# seconds per bit'")]
        public NWDLocalizableStringType SingleLastFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Last #x# seconds per bit'")]
        public NWDLocalizableStringType PluralLastFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Max format (#x# is the value)")]
        public float InitMax
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Max #x# seconds per bit'")]
        public NWDLocalizableStringType NoMaxFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Max #x# seconds per bit'")]
        public NWDLocalizableStringType SingleMaxFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Max #x# seconds per bit'")]
        public NWDLocalizableStringType PluralMaxFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Min format (#x# is the value)")]
        public float InitMin
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Min #x# seconds per bit'")]
        public NWDLocalizableStringType NoMinFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Min #x# seconds per bit'")]
        public NWDLocalizableStringType SingleMinFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Min #x# seconds per bit'")]
        public NWDLocalizableStringType PluralMinFormat
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDStatKey()
        {
            //Debug.Log("NWDStatKey Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDStatKey(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDStatKey Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            NoCounterFormat.AddBaseString("#x#");
            SingleCounterFormat.AddBaseString("#x#");
            PluralCounterFormat.AddBaseString("#x#");

            NoTotalFormat.AddBaseString("#x#");
            SingleTotalFormat.AddBaseString("#x#");
            PluralTotalFormat.AddBaseString("#x#");

            NoAverageFormat.AddBaseString("#x#");
            SingleAverageFormat.AddBaseString("#x#");
            PluralAverageFormat.AddBaseString("#x#");

            NoAverageWithParentFormat.AddBaseString("#x#");
            SingleAverageWithParentFormat.AddBaseString("#x#");
            PluralAverageWithParentFormat.AddBaseString("#x#");

            NoLastFormat.AddBaseString("#x#");
            SingleLastFormat.AddBaseString("#x#");
            PluralLastFormat.AddBaseString("#x#");

            NoMaxFormat.AddBaseString("#x#");
            SingleMaxFormat.AddBaseString("#x#");
            PluralMaxFormat.AddBaseString("#x#");

            NoMinFormat.AddBaseString("#x#");
            SingleMinFormat.AddBaseString("#x#");
            PluralMinFormat.AddBaseString("#x#");

            UnityStat = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void ErrorRegenerate()
        {
#if UNITY_EDITOR
            NWDError.CreateGenericError("NWDStatKey BasicError", "STKz01", "Internal error", "Internal error to test", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagInternal);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ClassInitialization() // call by invoke
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddEnter(float sValue)
        {
            switch (Domain)
            {
                case NWDStatKeyDomain.AccountStat:
                    {
                        NWDStatKeyValue.UserStatForKey(Reference).AddEnter(sValue);
                    }
                    break;
                case NWDStatKeyDomain.GameSaveStat:
                    {
                        NWDUserStatKeyValue.UserStatForKey(Reference).AddEnter(sValue);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public float AddEnterForParent(float sValue)
        {
            float rReturn = 0.0F;
            switch (Domain)
            {
                case NWDStatKeyDomain.AccountStat:
                    {
                        rReturn = NWDStatKeyValue.UserStatForKey(Reference).AddEnterForParent(sValue, this);
                    }
                    break;
                case NWDStatKeyDomain.GameSaveStat:
                    {
                        rReturn = NWDUserStatKeyValue.UserStatForKey(Reference).AddEnterForParent(sValue, this);
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float Counter()
        {
            float rReturn = 0.0F;
            switch (Domain)
            {
                case NWDStatKeyDomain.AccountStat:
                    {
                        rReturn = NWDStatKeyValue.UserStatForKey(Reference).Counter;
                    }
                    break;
                case NWDStatKeyDomain.GameSaveStat:
                    {
                        rReturn = NWDUserStatKeyValue.UserStatForKey(Reference).Counter;
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public string ReturnWithFormat(float sValue)
        {
            string rReturn = string.Empty;
            switch (Format)
            {
                case NWDStatKeyFormat.Integer:
                    {
                        rReturn = sValue.ToString("0");
                    }
                    break;
                case NWDStatKeyFormat.DecimalTwo:
                    {
                        rReturn = sValue.ToString("F2");
                    }
                    break;
                case NWDStatKeyFormat.DecimalThree:
                    {
                        rReturn = sValue.ToString("F3");
                    }
                    break;
                case NWDStatKeyFormat.DecimalFour:
                    {
                        rReturn = sValue.ToString("F4");
                    }
                    break;
                case NWDStatKeyFormat.Float:
                    {
                        rReturn = sValue.ToString("F7");
                    }
                    break;
                //case NWDStatKeyFormat.PurcentTwo:
                //{
                //    rReturn = sValue.ToString("P2");
                //}
                //break;
                case NWDStatKeyFormat.DaysHoursMinutesSeconds:
                    {
                        int tAllSeconds = (int)sValue;
                        int tDays = tAllSeconds / (3600 * 24);
                        int tHours = (tAllSeconds - tDays * 3600 * 24) / 3600;
                        int tMinutes = (tAllSeconds - tDays * 3600 * 24 - tHours * 3600) / 60;
                        int tSeconds = (tAllSeconds - tDays * 3600 * 24 - tHours * 3600 - tMinutes * 60);
                        rReturn = tDays.ToString("D") + " Days " + tHours.ToString("D2") + ":" + tMinutes.ToString("D2") + ":" + tSeconds.ToString("D2");
                    }
                    break;
                case NWDStatKeyFormat.HoursMinutesSeconds:
                    {
                        int tAllSeconds = (int)sValue;
                        int tHours = (tAllSeconds) / 3600;
                        int tMinutes = (tAllSeconds - tHours * 3600) / 60;
                        int tSeconds = (tAllSeconds - tHours * 3600 - tMinutes * 60);
                        rReturn = tHours.ToString("D2") + ":" + tMinutes.ToString("D2") + ":" + tSeconds.ToString("D2");
                    }
                    break;
                case NWDStatKeyFormat.MinutesSeconds:
                    {
                        int tAllSeconds = (int)sValue;
                        int tMinutes = (tAllSeconds) / 60;
                        int tSeconds = (tAllSeconds - tMinutes * 60);
                        rReturn = tMinutes.ToString("D2") + ":" + tSeconds.ToString("DD2");
                    }
                    break;
                case NWDStatKeyFormat.Seconds:
                    {
                        int tAllSeconds = (int)sValue;
                        rReturn = tAllSeconds.ToString("D") + "s";
                    }
                    break;
                default:
                    {
                        rReturn = sValue.ToString();
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDStatKey) };
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
        /// <summary>
        /// Addons the delete me.
        /// </summary>
        public override void AddonDeleteMe()
        {
            // do something when object will be delete from local base
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            // do something when object will be web service upgrade
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
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = false;
            // check if you found error in Data values.
            // normal way is return false!
            return rReturnErrorFound;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================