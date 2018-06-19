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
    /// AIRSurfaceConnection can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
    /// Use like :
    /// public class MyScriptInGame : MonoBehaviour
    /// { 
    /// [NWDConnectionAttribut (true, true, true, true)] // optional
    /// public AIRSurfaceConnection MyNetWorkedData;
    /// }
    /// </summary>
    [Serializable]
    public class AIRSurfaceConnection : NWDConnection<AIRSurface>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AIRS")]
    [NWDClassDescriptionAttribute("AIR Surface")]
    [NWDClassMenuNameAttribute("AIR Surface")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDInternalKeyNotEditableAttribute]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD example class. This class is use for (complete description here)
    /// </summary>
    public partial class AIRSurface : NWDBasis<AIRSurface>
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
        [NWDGroupStart("Options")]

        public NWDColorType IntoleranceColor
        {
            get; set;
        }
        public NWDColorType IndifferenceColor
        {
            get; set;
        }
        public NWDColorType FriendshipColor
        {
            get; set;
        }
        public NWDColorType AffinityColor
        {
            get; set;
        }
        public NWDColorType AdmirationColor
        {
            get; set;
        }
        public NWDColorType JealousyColor
        {
            get; set;
        }

        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Surface")]
        public AIRReferencesAverageType<AIRDimension> DimensionArea
        {
            get; set;
        }
        //public int Resulting {get; set;}


        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public AIRSurface()
        {
            //Debug.Log("AIRSurface Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public AIRSurface(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("AIRSurface Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
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
            NWDError.CreateGenericError("AIRSurface BasicError", "AIRSz01", "Internal error", "Internal error to test", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagInternal);
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
                Dictionary<AIRDimension, AIRAverage> tDimensionAverageOld = DimensionArea.GetObjectAndAverage();
                // order
                List<AIRDimension> tKeys = tDimensionAverageOld.Keys.ToList();
                tKeys.Sort((x, y) => x.Order.CompareTo(y.Order));
                Dictionary<AIRDimension, AIRAverage> tDimensionAverage = new Dictionary<AIRDimension, AIRAverage>();
                foreach (AIRDimension tD in tKeys)
                {
                    tDimensionAverage.Add(tD, tDimensionAverageOld[tD]);
                }
                // draw
                int tDimNumber = tDimensionAverage.Count;
                float tAngleIncrement = Mathf.PI * 2 / tDimNumber;
                float tAngleUsed = -Mathf.PI / 2.0f;
                //Debug.Log("tDimNumber = " + tDimNumber.ToString());
                Vector2[] tZeroArray = new Vector2[tDimNumber];
                Vector2[] tIntoleranceArray = new Vector2[tDimNumber];
                Vector2[] tFriendshipArray = new Vector2[tDimNumber];
                Vector2[] tAffinityArray = new Vector2[tDimNumber];
                Vector2[] tAdmirationArray = new Vector2[tDimNumber];
                Vector2[] tJealousyArray = new Vector2[tDimNumber];
                Vector2[] tOneArray = new Vector2[tDimNumber];
                int tCounter = 0;
                foreach (KeyValuePair<AIRDimension, AIRAverage> tDimension in tDimensionAverage)
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
                    tIntoleranceArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * tDimension.Value.Intolerance * sRect.width / 2.0F),
                                                              (float)(sRect.center.y + Math.Sin(tAngleUsed) * tDimension.Value.Intolerance * sRect.width / 2.0F));
                    tFriendshipArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * tDimension.Value.Friendship * sRect.width / 2.0F),
                                                            (float)(sRect.center.y + Math.Sin(tAngleUsed) * tDimension.Value.Friendship * sRect.width / 2.0F));
                    tAffinityArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * tDimension.Value.Affinity * sRect.width / 2.0F),
                                                          (float)(sRect.center.y + Math.Sin(tAngleUsed) * tDimension.Value.Affinity * sRect.width / 2.0F));
                    tAdmirationArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * tDimension.Value.Admiration * sRect.width / 2.0F),
                                                            (float)(sRect.center.y + Math.Sin(tAngleUsed) * tDimension.Value.Admiration * sRect.width / 2.0F));
                    tJealousyArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * tDimension.Value.Jealousy * sRect.width / 2.0F),
                                                           (float)(sRect.center.y + Math.Sin(tAngleUsed) * tDimension.Value.Jealousy * sRect.width / 2.0F));
                    tOneArray[tCounter] = new Vector2((float)(sRect.center.x + Math.Cos(tAngleUsed) * sRect.width / 2.0F),
                                                          (float)(sRect.center.y + Math.Sin(tAngleUsed) * sRect.width / 2.0F));

                    tCounter++;
                    tAngleUsed += tAngleIncrement;
                }
                AirDraw.DrawPeri(tIntoleranceArray, Color.red, 1.0F, true);
                AirDraw.DrawPeri(tFriendshipArray, Color.red, 1.0F, true);
                AirDraw.DrawPeri(tAffinityArray, Color.red, 1.0F, true);
                AirDraw.DrawPeri(tAdmirationArray, Color.red, 1.0F, true);
                AirDraw.DrawPeri(tJealousyArray, Color.red, 1.0F, true);

                AirDraw.DrawPeriArea(tZeroArray, tIntoleranceArray, IntoleranceColor.GetColor());
                AirDraw.DrawPeriArea(tIntoleranceArray, tFriendshipArray, IndifferenceColor.GetColor());
                AirDraw.DrawPeriArea(tFriendshipArray, tAffinityArray, FriendshipColor.GetColor());
                AirDraw.DrawPeriArea(tAffinityArray, tAdmirationArray, AffinityColor.GetColor());
                AirDraw.DrawPeriArea(tAdmirationArray, tJealousyArray, AdmirationColor.GetColor());
                AirDraw.DrawPeriArea(tJealousyArray, tOneArray, JealousyColor.GetColor());


                //OVERRIDE THE LINE 
                tAngleUsed = -Mathf.PI / 2.0f;
                tCounter = 0;
                foreach (KeyValuePair<AIRDimension, AIRAverage> tDimension in tDimensionAverage)
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
            return new List<Type> { typeof(AIRSurface)/*, typeof(NWDUserNickname), etc*/ };
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
            float tYadd = 250.0F + NWDConstants.kFieldMarge;
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
            return 300.0f;
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