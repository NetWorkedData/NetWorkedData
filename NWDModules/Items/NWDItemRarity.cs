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
    ///         public NWDItemRarityConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDItemRarity tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDItemRarityConnection : NWDConnection<NWDItemRarity>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDItemRarity class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("IRY")]
    [NWDClassDescriptionAttribute("Rarity of Item")]
    [NWDClassMenuNameAttribute("Item Rarity")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDItemRarity : NWDBasis<NWDItemRarity>
    {
        #region Class Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your static properties
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        //Example
        //[NWDGroupStart("Account")]
        //public NWDReferenceType<NWDAccount> Account { get; set; }
        //[NWDGroupEnd()]
        //[NWDGroupSeparator()]
        //[NWDGroupStart("Other")]
        //public int Other { get; set; }

        //PROPERTIES
        [NWDTooltips("The item to check")]
        public NWDReferenceType<NWDItem> ItemReference
        {
            get; set;
        }
        [NWDTooltips("The total of this item in all game save")]
        public long ItemTotal
        {
            get; set;
        }
        [NWDTooltips("The total of user own this item in all game save")]
        public long OwnerUserTotal
        {
            get; set;
        }
        [NWDTooltips("The total of user in all game save")]
        public long UserTotal
        {
            get; set;
        }
        [NWDTooltips("The maximum item in all game save for one game save")]
        public long Maximum
        {
            get; set;
        }
        [NWDTooltips("The minimum item in all game save for one game save")]
        public long Minimum
        {
            get; set;
        }
        [NWDTooltips("The average in all game save")]
        public double Average
        {
            get; set;
        }
        [NWDTooltips("The frequency in all game save (OwnerUserTotal/UserTotal)/Average")]
        public double Frequency
        {
            get; set;
        }
        [NWDTooltips("The rarity in all game save (1/Frequency)")]
        public double Rarity
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemRarity()
        {
            //Debug.Log("NWDItemRarity Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemRarity(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDItemRarity Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
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
        public static void ErrorRegenerate()
        {
#if UNITY_EDITOR
            NWDError.CreateGenericError("NWDItemRarity BasicError", "IRYz01", "Internal error", "Internal error to test", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagInternal);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod("ClassInitialization")]
        public static void ClassInitialization() // call by invoke
        {
        }
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
            return new List<Type> { typeof(NWDItemRarity)/*, typeof(NWDUserNickname), etc*/ };
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
        public static string AddonPhpPreCalculate()
        {
            return "// write your php script here to update $tReference before sync on server\n";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpPostCalculate()
        {
            return "// write your php script here to update afetr sync on server\n";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpSpecialCalculate()
        {
            return "\n" +
        "\t\tglobal $NWD_FLOAT_FORMAT;\n" +
        "\t\t// Count user by gamesave\n" +
        "\t\t$tUserCount = 0;\n" +
        "\t\t$tQuery = 'SELECT COUNT(Reference) as TotalUser FROM `'.$ENV.'_NWDGameSave`';\n" +
        "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
        "\t\tif (!$tResult)\n" +
        "\t\t\t{\n" +
        "\t\t\t\terror('IRYx33');\n" +
        "\t\t\t}\n" +
        "\t\telse\n" +
        "\t\t\t{\n" +
        "\t\t\t\twhile($tRow = $tResult->fetch_assoc())\n" +
        "\t\t\t\t\t{\n" +
        "\t\t\t\t\t\t$tUserCount = $tRow['TotalUser'];\n" +
        "\t\t\t\t\t}\n" +
        "\t\t\t\tmysqli_free_result($tResult);\n" +
        "\t\t\t}\n" +
        "\t\t// calculate rarity and fill the datas\n" +
        "\t\t$tQuery = 'SELECT t2.Reference, t2.ItemReference,';\n" +
        "\t\t$tQuery.= ' SUM(t1.Quantity) as Total ,';\n" +
        "\t\t$tQuery.= ' COUNT(t1.Reference) as OwnerTotal,';\n" +
        "\t\t$tQuery.= ' MAX(t1.Quantity) as ItemMax,';\n" +
        "\t\t$tQuery.= ' MIN(t1.Quantity) as ItemMin,';\n" +
        "\t\t$tQuery.= ' AVG(t1.Quantity) as ItemAvg';\n" +
        "\t\t$tQuery.= ' FROM `'.$ENV.'_NWDUserOwnership` t1 INNER JOIN `'.$ENV.'_NWDItemRarity` t2 ON t1.Item = t2.ItemReference GROUP BY t2.ItemReference;';\n" +
        "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
        //"\t\tmyLog('mysqli request : ('. $tQuery.')', __FILE__, __FUNCTION__, __LINE__);\n" +
        "\t\tif (!$tResult)\n" +
        "\t\t\t{\n" +
        "\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
        "\t\t\t\terror('IRYx33');\n" +
        "\t\t\t}\n" +
        "\t\telse\n" +
        "\t\t\t{\n" +
        "\t\t\t\twhile($tRow = $tResult->fetch_assoc())\n" +
        "\t\t\t\t\t{\n" +
        "\t\t\t\t\t\t$tUpdate = 'UPDATE `'.$ENV.'_NWDItemRarity` SET `DM` = \\''.$TIME_SYNC.'\\', `DS` = \\''.$TIME_SYNC.'\\', `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Maximum` = \\''.$tRow['ItemMax'].'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Minimum` = \\''.$tRow['ItemMin'].'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `OwnerUserTotal` = \\''.$tRow['OwnerTotal'].'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `UserTotal` = \\''.$tUserCount.'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Average` = \\''.number_format ($tRow['ItemAvg'], $NWD_FLOAT_FORMAT ,'.','').'\\',';\n" +
        "\t\t\t\t\t\tif ($tRow['ItemAvg']!=0 && $tUserCount!=0)\n" +
        "\t\t\t\t\t\t\t{\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Frequency` = \\''.number_format (($tRow['OwnerTotal']/$tUserCount)/$tRow['ItemAvg'], $NWD_FLOAT_FORMAT,'.','').'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Rarity` = \\''.number_format (1.0/(($tRow['OwnerTotal']/$tUserCount)/$tRow['ItemAvg']), $NWD_FLOAT_FORMAT,'.','').'\\',';\n" +
        "\t\t\t\t\t\t\t}\n" +
        "\t\t\t\t\t\t$tUpdate.=' `ItemTotal` = \\''.$tRow['Total'].'\\'';\n" +
        "\t\t\t\t\t\t$tUpdate.=' WHERE `Reference` = \\''.$tRow['Reference'].'\\' AND `ItemReference` = \\''.$tRow['ItemReference'].'\\';';\n" +
        "\t\t\t\t\t\t$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
        //"\t\t\t\t\t\t$REP['spc'][$tRow['Reference']] = $tUpdate;\n" +
        "\t\t\t\t\t\tif (!$tUpdateResult)\n" +
        "\t\t\t\t\t\t{\n" +
        "\t\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);\n" +
        "\t\t\t\t\t\t\terror('IRYx38');\n" +
        "\t\t\t\t\t\t}\n" +
        "\t\t\t\t\t\telse\n" +
        "\t\t\t\t\t\t{\n" +
        "\t\t\t\t\t\tIntegrityNWDItemRarityReevalue($tRow['Reference']);\n" +
        "\t\t\t\t\t\t}\n" +
        "\t\t\t\t\t}\n" +
        "\t\t\t\tmysqli_free_result($tResult);\n" +
        "\t\t\t\t$REP['special'] ='success!';\n" +
        "\t\t\t}" +
        "";
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================