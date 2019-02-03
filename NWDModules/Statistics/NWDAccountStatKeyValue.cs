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
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDUserStatKeyValue class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("STC")]
    [NWDClassDescriptionAttribute("StatKeyValue")]
    [NWDClassMenuNameAttribute("StatKeyValue")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDAccountStatKeyValue : NWDBasis<NWDAccountStatKeyValue>
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
        [NWDGroupStart("Connection")]
        public NWDReferenceType<NWDAccount> Account {get; set;}
        public NWDReferenceType<NWDStatKey> StatKey
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Values")]
        public double Total
        {
            get; set;
        }
        public double Counter
        {
            get; set;
        }
        public double Average
        {
            get; set;
        }
        public double AverageWithParent
        {
            get; set;
        }
        public double Last
        {
            get; set;
        }
        public double Max
        {
            get; set;
        }
        public double Min
        {
            get; set;
        }

        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountStatKeyValue()
        {
            //Debug.Log("NWDUserStatKeyValue Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountStatKeyValue(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserStatKeyValue Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
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
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public static void MyClassMethod()
        {
            // do something with this class
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public double AddEnterForParent(float sValue, NWDStatKey sStatKey)
        {

            if (sStatKey.UnityStat == true)
            {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            //    AnalyticsEvent.Custom(sStatKey.InternalKey, new Dictionary<string, object>
            //{
            //        {"nwd_account_ref", Account.GetReference()},
            //        {"nwd_value", sValue},
            //        /*
            //    {"nwd_total", Total},
            //    {"nwd_counter", Counter},
            //    {"nwd_average", Average},
            //    {"nwd_average_parent", AverageWithParent},
            //    {"nwd_last", Last},
            //    {"nwd_max", Max},
            //    {"nwd_min", Min},
            //    */
            //});
#endif
            }
            if (Max < sValue)
            {
                Max = sValue;
            }
            if (Min > sValue)
            {
                Min = sValue;
            }
            Last = sValue;
            Counter += 1.0F;
            Total += sValue;
            if (Counter != 0)
            {
                Average = Total / Counter;
            }
            else
            {
                Average = 0.0F;
            }
            UpdateData(true, kWritingMode);
            return Counter;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Reevaluate()
        {
            NWDStatKey tStatKey = StatKey.GetObject();
            if (tStatKey.Parent != null)
            {
                NWDStatKey tStatKeyParent = tStatKey.Parent.GetObject();
                if (tStatKeyParent != null)
                {
                    //I need transfert data to parent and use data for recalulate result
                    double tCounter = tStatKeyParent.Counter();
                    if (tCounter != 0)
                    {
                        AverageWithParent = Total / tCounter;
                    }
                    else
                    {
                        AverageWithParent = 0.0F;
                    }
                }
            }
            UpdateData(true, kWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddEnter(float sValue)
        {
            //Debug.Log("NWDStatKeyValue AddEnter(" + sValue.ToString() + ")");
            NWDStatKey tStatKey = StatKey.GetObject();
            AddEnterForParent(sValue, tStatKey);
            if (tStatKey.Parent != null)
            {
                NWDStatKey tStatKeyParent = tStatKey.Parent.GetObject();
                if (tStatKeyParent != null)
                {
                    //I need transfert data to parent and use data for recalulate result
                    double tCounter = tStatKeyParent.AddEnterForParent(sValue);
                    if (tCounter != 0)
                    {
                        AverageWithParent = Total / tCounter;
                    }
                    else
                    {
                        AverageWithParent = 0.0F;
                    }
                }
            }
            foreach (NWDStatKey tStatKeyChild in tStatKey.Dependent.GetObjects())
            {
                tStatKeyChild.AddEnterForParent(sValue);
            }
            UpdateData(true, kWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string TotalStylized()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            return tStaKey.ReturnWithFormat(Total);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AverageStylized()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            return tStaKey.ReturnWithFormat(Average);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string LastStylized()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            return tStaKey.ReturnWithFormat(Last);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string MinStylized()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            return tStaKey.ReturnWithFormat(Min);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string MaxStylized()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            return tStaKey.ReturnWithFormat(Max);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string TotalDescription ()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            string tTotalFormatted = tStaKey.ReturnWithFormat(Total);
            string rReturn = string.Empty;
            if (Total == 0.0f)
            {
                rReturn = tStaKey.NoTotalFormat.GetLocalString().Replace("#x#",tTotalFormatted);
            }
            else if (Total == 1.0f)
            {
                rReturn = tStaKey.SingleTotalFormat.GetLocalString().Replace("#x#", tTotalFormatted);
            }
            else
            {
                rReturn = tStaKey.PluralTotalFormat.GetLocalString().Replace("#x#", tTotalFormatted);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AverageDescription()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            string tAverageFormatted = tStaKey.ReturnWithFormat(Average);
            if (tStaKey.ShowAverageWithParentAsPurcent == true)
            {
                tAverageFormatted = (Average * 100.0F).ToString("F2") + " %";
            }
            string rReturn = string.Empty;
            if (Average == 0.0f)
            {
                rReturn = tStaKey.NoAverageFormat.GetLocalString().Replace("#x#", tAverageFormatted);
            }
            else if (Average == 1.0f)
            {
                rReturn = tStaKey.SingleAverageFormat.GetLocalString().Replace("#x#", tAverageFormatted);
            }
            else
            {
                rReturn = tStaKey.PluralAverageFormat.GetLocalString().Replace("#x#", tAverageFormatted);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AverageWithParentDescription()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            string tAverageWithParentFormatted = tStaKey.ReturnWithFormat(AverageWithParent);
            if (tStaKey.ShowAverageWithParentAsPurcent == true)
            {
                tAverageWithParentFormatted = (AverageWithParent * 100.0F).ToString("F2") + " %";
            }
            string rReturn = string.Empty;
            if (AverageWithParent == 0.0f)
            {
                rReturn = tStaKey.NoAverageWithParentFormat.GetLocalString().Replace("#x#", tAverageWithParentFormatted);
            }
            else if (AverageWithParent == 1.0f)
            {
                rReturn = tStaKey.SingleAverageWithParentFormat.GetLocalString().Replace("#x#", tAverageWithParentFormatted);
            }
            else
            {
                rReturn = tStaKey.PluralAverageWithParentFormat.GetLocalString().Replace("#x#", tAverageWithParentFormatted);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string LastDescription()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            string tLastFormatted = tStaKey.ReturnWithFormat(Last);
            string rReturn = string.Empty;
            if (Last == 0.0f)
            {
                rReturn = tStaKey.NoLastFormat.GetLocalString().Replace("#x#", tLastFormatted);
            }
            else if (Last == 1.0f)
            {
                rReturn = tStaKey.SingleLastFormat.GetLocalString().Replace("#x#", tLastFormatted);
            }
            else
            {
                rReturn = tStaKey.PluralLastFormat.GetLocalString().Replace("#x#", tLastFormatted);
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public string MinDescription()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            string tMinFormatted = tStaKey.ReturnWithFormat(Min);
            string rReturn = string.Empty;
            if (Min == 0.0f)
            {
                rReturn = tStaKey.NoMinFormat.GetLocalString().Replace("#x#", tMinFormatted);
            }
            else if (Min == 1.0f)
            {
                rReturn = tStaKey.SingleMinFormat.GetLocalString().Replace("#x#", tMinFormatted);
            }
            else
            {
                rReturn = tStaKey.PluralMinFormat.GetLocalString().Replace("#x#", tMinFormatted);
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public string MaxDescription()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            string tMaxFormatted = tStaKey.ReturnWithFormat(Max);
            string rReturn = string.Empty;
            if (Max == 0.0f)
            {
                rReturn = tStaKey.NoMaxFormat.GetLocalString().Replace("#x#", tMaxFormatted);
            }
            else if (Max == 1.0f)
            {
                rReturn = tStaKey.SingleMaxFormat.GetLocalString().Replace("#x#", tMaxFormatted);
            }
            else
            {
                rReturn = tStaKey.PluralMaxFormat.GetLocalString().Replace("#x#", tMaxFormatted);
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public string CounterDescription()
        {
            NWDStatKey tStaKey = StatKey.GetObject();
            string tCounterFormatted = ((int)Counter).ToString();
            string rReturn = string.Empty;
            if (Counter == 0.0f)
            {
                rReturn = tStaKey.NoCounterFormat.GetLocalString().Replace("#x#", tCounterFormatted);
            }
            else if (Counter == 1.0f)
            {
                rReturn = tStaKey.SingleCounterFormat.GetLocalString().Replace("#x#", tCounterFormatted);
            }
            else
            {
                rReturn = tStaKey.PluralCounterFormat.GetLocalString().Replace("#x#", tCounterFormatted);
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
            return new List<Type> { typeof(NWDUserStatKeyValue) };
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            InsertInIndex();
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
            InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        //public override void AddonUpdatedMeFromWeb()
        //{
        //    // do something when object finish to be updated from CSV from WebService response
        //    // TODO verif if method is call in good place in good timing
        //    InsertInIndex();
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
            InsertInIndex();
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
            RemoveFromIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        //{
        //    // do something when object will be web service upgrade
        //    // TODO verif if method is call in good place in good timing
        //}
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