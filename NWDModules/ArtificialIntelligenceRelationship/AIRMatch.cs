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
    /// AIRMatchConnection can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
    /// Use like :
    /// public class MyScriptInGame : MonoBehaviour
    /// { 
    /// [NWDConnectionAttribut (true, true, true, true)] // optional
    /// public AIRMatchConnection MyNetWorkedData;
    /// }
    /// </summary>
    [Serializable]
    public class AIRMatchConnection : NWDConnection<AIRMatch>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AIRM")]
    [NWDClassDescriptionAttribute("AIRMatch for quest or action")]
    [NWDClassMenuNameAttribute("AIRMatch")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDInternalKeyNotEditableAttribute]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD example class. This class is use for (complete description here)
    /// </summary>
    public partial class AIRMatch : NWDBasis<AIRMatch>
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
        [NWDGroupStart("Informations")]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Surface macth")]
        public AIRReferencesRangeType<AIRDimension> DimensionMatch
        {
            get; set;
        }

        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Options")]

        public NWDColorType MinOutsideColor
        {
            get; set;
        }
        public NWDColorType InsideColor
        {
            get; set;
        }
        public NWDColorType MaxOutsideColor
        {
            get; set;
        }

        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public AIRMatch()
        {
            //Debug.Log("AIRMatch Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public AIRMatch(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("AIRMatch Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
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
            NWDError.CreateGenericError("AIRMatch BasicError", "AIRMz01", "Internal error", "Internal error to test", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagInternal);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
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
        public void DrawAreaInRect(Rect sRect, bool sEditorMode = false)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
                AirDraw.DrawRect(sRect, Color.white);
                // get value
                Dictionary<AIRDimension, AIRRange> tDimensionAverageOld = DimensionMatch.GetObjectAndRange();
                // order
                List<AIRDimension> tKeys = tDimensionAverageOld.Keys.ToList();
                tKeys.Sort((x, y) => x.Order.CompareTo(y.Order));
                Dictionary<AIRDimension, AIRRange> tDimensionAverage = new Dictionary<AIRDimension, AIRRange>();
                foreach (AIRDimension tD in tKeys)
                {
                    NWDItem[] tItemsToShow = tD.ItemToShow.GetObjects();
                    bool iSVisible = sEditorMode;
                    if (tItemsToShow.Length > 0)
                    {
                        foreach (NWDItem tItem in tItemsToShow)
                        {
                            if (NWDOwnership.QuantityForItem(tItem.Reference) > 0)
                            {
                                iSVisible = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        iSVisible = true;
                    }
                    if (iSVisible == true)
                    {
                        tDimensionAverage.Add(tD, tDimensionAverageOld[tD]);
                    }
                }
                // draw
                int tDimNumber = tDimensionAverage.Count;
                float tAngleIncrement = Mathf.PI * 2 / tDimNumber;
                float tAngleUsed = -Mathf.PI / 2.0f;
                //Debug.Log("tDimNumber = " + tDimNumber.ToString());
                Vector2[] tZeroArray = new Vector2[tDimNumber];
                Vector2[] tMinArray = new Vector2[tDimNumber];
                Vector2[] tMaxArray = new Vector2[tDimNumber];
                Vector2[] tOneArray = new Vector2[tDimNumber];
                int tCounter = 0;
                foreach (KeyValuePair<AIRDimension, AIRRange> tDimension in tDimensionAverage)
                {
                    //Debug.Log("tAngleUsed = " + tAngleUsed.ToString());
                    AirDraw.DrawLine(sRect.center,
                    new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * sRect.width / 2.0F), (float)(sRect.center.y + Math.Sin(tAngleUsed) * sRect.width / 2.0F)),
                    //new Vector2(sRect.x + sRect.width, sRect.y),
                    Color.black,
                    1.0F,
                             true);
                    tZeroArray[tCounter] = new Vector2((float)(sRect.center.x),
                                                              (float)(sRect.center.y));
                    tMinArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * (float)tDimension.Value.Min * 0.20F * sRect.width / 2.0F),
                                                      (float)(sRect.center.y + Math.Sin(tAngleUsed) * (float)tDimension.Value.Min * 0.20F * sRect.width / 2.0F));
                    tMaxArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * (float)tDimension.Value.Max * 0.20F * sRect.width / 2.0F),
                                                      (float)(sRect.center.y + Math.Sin(tAngleUsed) * (float)tDimension.Value.Max * 0.20F * sRect.width / 2.0F));
                    tOneArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * sRect.width / 2.0F),
                                                          (float)(sRect.center.y + Math.Sin(tAngleUsed) * sRect.width / 2.0F));

                    tCounter++;
                    tAngleUsed += tAngleIncrement;
                }
                AirDraw.DrawPeri(tMinArray, Color.red, 1.0F, true);
                AirDraw.DrawPeri(tMaxArray, Color.red, 1.0F, true);

                AirDraw.DrawPeriArea(tZeroArray, tMinArray, MinOutsideColor.GetColor());
                AirDraw.DrawPeriArea(tMinArray, tMaxArray, InsideColor.GetColor());
                AirDraw.DrawPeriArea(tMaxArray, tOneArray, MaxOutsideColor.GetColor());


                //OVERRIDE THE LINE 
                tAngleUsed = -Mathf.PI / 2.0f;
                tCounter = 0;
                foreach (KeyValuePair<AIRDimension, AIRRange> tDimension in tDimensionAverage)
                {
                    //Debug.Log("tAngleUsed = " + tAngleUsed.ToString());
                    AirDraw.DrawLine(sRect.center,
                    new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * sRect.width / 2.0F), (float)(sRect.center.y + Math.Sin(tAngleUsed) * sRect.width / 2.0F)),
                    //new Vector2(sRect.x + sRect.width, sRect.y),
                    Color.black,
                    1.0F,
                             true);
                    tCounter++;
                    tAngleUsed += tAngleIncrement;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(AIRMatch)/*, typeof(NWDUserNickname), etc*/ };
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
            float tYadd = 250.0F;
            DrawAreaInRect(new Rect(sInRect.x, sInRect.y, 250.0F, 250.0F), true);
            DrawAreaInRect(new Rect(sInRect.x, sInRect.y + 250.0F + NWDConstants.kFieldMarge, 250.0F, 250.0F), false);
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
            float tYadd = 250.0F + NWDConstants.kFieldMarge + 250.0F;
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
            return 300.0F;

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 350.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            DrawAreaInRect(sRect, true);

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