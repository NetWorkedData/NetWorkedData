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
    public class AIRUserRelationshipConnection : NWDConnection<AIRUserRelationship>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AIRR")]
    [NWDClassDescriptionAttribute("AIRUserRelationship")]
    [NWDClassMenuNameAttribute("AIRUserRelationship")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference")]
    public partial class AIRUserRelationship : NWDBasis<AIRUserRelationship>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Account")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Score by CharacterSurface")]
        public NWDReferenceType<AIRCharacterSurface> CharacterSurface
        {
            get; set;
        }
        public NWDReferencesAverageType<AIRDimension> DimensionAverage
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Options")]

        public NWDColorType BadColor
        {
            get; set;
        }
        public NWDColorType GoodColor
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public AIRUserRelationship()
        {
            //Debug.Log("AIRRelationship Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public AIRUserRelationship(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("AIRRelationship Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
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
            NWDError.CreateGenericError("AIRRelationship BasicError", "AIRRz01", "Internal error", "Internal error to test", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagInternal);
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
        public void CheckDimensions()
        {
            Debug.Log("AIRRelationship CheckDimensions()");
            AIRCharacterSurface tSurface = CharacterSurface.GetObject();
            if (tSurface != null)
            {
                string[] tDimensionsNeed = tSurface.DimensionArea.GetReferences();
                string[] tDimensionsUsed = DimensionAverage.GetReferences();
                Debug.Log("AIRRelationship CheckDimensions() test "+tDimensionsNeed.Length+" need dimension with "+tDimensionsUsed.Length+" used dimensions ");
                foreach (string tStr in tDimensionsNeed)
                {
                    if (tDimensionsUsed.Contains(tStr) == false)
                    {
                        AIRDimension tDimension = AIRDimension.GetObjectByReference(tStr);
                        if (tDimension != null)
                        {
                            DimensionAverage.AddObjectValue(tDimension, tDimension.DefaultValue);
                        }
                    }
                }
                foreach (string tStr in tDimensionsUsed)
                {
                    if (tDimensionsNeed.Contains(tStr) == false)
                    {
                        AIRDimension tDimension = AIRDimension.GetObjectByReference(tStr);
                        if (tDimension != null)
                        {
                            Debug.Log("AIRRelationship CheckDimensions() Remove dimension " + tDimension.InternalKey);
                            DimensionAverage.RemoveObject(tDimension);
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawAreaInRect(Rect sRect, bool sEditorMode = false)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
                AirDraw.DrawRect(sRect, Color.white);

                AIRCharacterSurface tSurface = CharacterSurface.GetObject();
                if (tSurface != null)
                {
                    tSurface.DrawAreaInRect(sRect, sEditorMode);
                }

                // get value
                Dictionary<AIRDimension, NWDAverage> tDimensionAverageOld = DimensionAverage.GetObjectAndAverage();
                // order
                List<AIRDimension> tKeys = tDimensionAverageOld.Keys.ToList();
                tKeys.Sort((x, y) => x.Order.CompareTo(y.Order));
                Dictionary<AIRDimension, NWDAverage> tDimensionAverage = new Dictionary<AIRDimension, NWDAverage>();
                foreach (AIRDimension tD in tKeys)
                {
                    NWDItem[] tItemsToShow = tD.ItemToShow.GetObjects();
                    bool iSVisible = sEditorMode;
                    if (tD.IsVisible == false)
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
                Vector2[] tLimitArray = new Vector2[tDimNumber];
                Vector2[] tOneArray = new Vector2[tDimNumber];
                int tCounter = 0;
                foreach (KeyValuePair<AIRDimension, NWDAverage> tDimension in tDimensionAverage)
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
                    tLimitArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * tDimension.Value.Average * sRect.width / 2.0F),
                                                              (float)(sRect.center.y + Math.Sin(tAngleUsed) * tDimension.Value.Average * sRect.width / 2.0F));
                    tOneArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * sRect.width / 2.0F),
                                                          (float)(sRect.center.y + Math.Sin(tAngleUsed) * sRect.width / 2.0F));

                    tCounter++;
                    tAngleUsed += tAngleIncrement;
                }
                AirDraw.DrawPeri(tLimitArray, Color.red, 1.0F, true);

                AirDraw.DrawPeriArea(tZeroArray, tLimitArray, BadColor.GetColor());
                AirDraw.DrawPeriArea(tLimitArray, tOneArray, GoodColor.GetColor());


                //OVERRIDE THE LINE 
                tAngleUsed = -Mathf.PI / 2.0f;
                tCounter = 0;
                foreach (KeyValuePair<AIRDimension, NWDAverage> tDimension in tDimensionAverage)
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
            return new List<Type> { typeof(AIRUserRelationship)/*, typeof(NWDUserNickname), etc*/ };
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
            CheckDimensions();
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
            return 300.0f;
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