//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    ///// <summary>
    ///// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    ///// <para>The GameObject can use the object referenced by binding in game. </para>
    ///// <example>
    ///// Example :
    ///// <code>
    ///// public class MyScriptInGame : MonoBehaviour<br/>
    /////     {
    /////         NWDConnectionAttribut (true, true, true, true)] // optional
    /////         public NWDExampleConnection MyNetWorkedData;
    /////         public void UseData()
    /////             {
    /////                 NWDExample tObject = MyNetWorkedData.GetObject();
    /////                 // Use tObject
    /////             }
    /////     }
    ///// </code>
    ///// </example>
    ///// </summary>
    //[Serializable]
    //public class NWDSchemeActionConnection : NWDConnection<NWDSchemeAction>
    //{
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("URI")]
    [NWDClassDescriptionAttribute("Action by Scheme URI")]
    [NWDClassMenuNameAttribute("Scheme Action")]
    public partial class NWDSchemeAction : NWDBasis<NWDSchemeAction>
    {
        //-------------------------------------------------------------------------------------------------------------
        //      [NWDTooltips("The name of message post to all observer objects. Example 'Raining', 'Start Quest Music', etc.")]
        //public string ActionName {get; set;}
        [NWDGroupStart("Optional", true, true, false)]
        [NWDTooltips("An additional message, it's optional and not used in standard process.")]
        public string Message
        {
            get; set;
        }
        [NWDTooltips("An additional param as string, it's optional and not used in standard process.")]
        public string Parameter
        {
            get; set;
        }
        [NWDTooltips("An additional Action, it's optional but used in standard process.")]
        public NWDReferenceType<NWDAction> Action
        {
            get; set;
        }
        [NWDGroupEnd]
        [NWDGroupSeparator]
        [NWDGroupStart("Scene", true, true, false)]
        [NWDTooltips("An additional scene to use, it's optional and not used in standard process.")]
        public NWDSceneType UseScene
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
//        public NWDSchemeAction()
//        {
//            //Debug.Log("NWDSchemeAction Constructor");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public NWDSchemeAction(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
//        {
//            //Debug.Log("NWDSchemeAction Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
//        {
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//        #region Class methods
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Exampel of implement for class method.
//        /// </summary>
//        public static void MyClassMethod()
//        {
//            // do something with this class
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public Texture2D FlashMyApp(string sProto, bool sRedirection, int sDimension)
//        {
//            Texture2D rTexture = new Texture2D(sDimension, sDimension);
//            var color32 = Encode(URISchemePath(sProto, string.Empty), rTexture.width, rTexture.height);
//            rTexture.SetPixels32(color32);
//            rTexture.Apply();
//            return rTexture;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private static Color32[] Encode(string textForEncoding, int width, int height)
//        {
//            var writer = new BarcodeWriter
//            {
//                Format = BarcodeFormat.QR_CODE,
//                Options = new QrCodeEncodingOptions
//                {
//                    Height = height,
//                    Width = width
//                }
//            };
//            return writer.Write(textForEncoding);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public string URISchemePath(string sProtocol, string sAdditional)
//        {
//            string tText = sProtocol;
//            tText = tText.Replace(":", string.Empty);
//            tText = tText.Replace("/", string.Empty);
//            tText += "://do?A=" + UnityWebRequest.EscapeURL(Reference);
//            if (string.IsNullOrEmpty(Message) == false)
//            {
//                tText += "&M=" + UnityWebRequest.EscapeURL(Message);
//            }
//            if (string.IsNullOrEmpty(Parameter) == false)
//            {
//                tText += "&P=" + UnityWebRequest.EscapeURL(Parameter);
//            }
//            if (string.IsNullOrEmpty(sAdditional) == false)
//            {
//                tText += "&A=" + UnityWebRequest.EscapeURL(sAdditional);
//            }
//            return tText;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//        #region Instance methods
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Exampel of implement for instance method.
//        /// </summary>
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//        #region NetWorkedData addons methods
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just after loaded from database.
//        /// </summary>
//        public override void AddonLoadedMe()
//        {
//            // do something when object was loaded
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just before unload from memory.
//        /// </summary>
//        public override void AddonUnloadMe()
//        {
//            // do something when object will be unload
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just before insert.
//        /// </summary>
//        public override void AddonInsertMe()
//        {
//            // do something when object will be inserted
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just before update.
//        /// </summary>
//        public override void AddonUpdateMe()
//        {
//            // do something when object will be updated
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method when updated.
//        /// </summary>
//        public override void AddonUpdatedMe()
//        {
//            // do something when object finish to be updated
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method when updated me from Web.
//        /// </summary>
//        //public override void AddonUpdatedMeFromWeb()
//        //{
//        //    // do something when object finish to be updated from CSV from WebService response
//        //    // TODO verif if method is call in good place in good timing
//        //}
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just before dupplicate.
//        /// </summary>
//        public override void AddonDuplicateMe()
//        {
//            // do something when object will be dupplicate
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just before enable.
//        /// </summary>
//        public override void AddonEnableMe()
//        {
//            // do something when object will be enabled
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just before disable.
//        /// </summary>
//        public override void AddonDisableMe()
//        {
//            // do something when object will be disabled
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just before put in trash.
//        /// </summary>
//        public override void AddonTrashMe()
//        {
//            // do something when object will be put in trash
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addon method just before remove from trash.
//        /// </summary>
//        public override void AddonUnTrashMe()
//        {
//            // do something when object will be remove from trash
//            // TODO verif if method is call in good place in good timing
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        #endregion
//        //-------------------------------------------------------------------------------------------------------------
//        #region Editor
//#if UNITY_EDITOR
//        //-------------------------------------------------------------------------------------------------------------
//        //Addons for Edition
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addons in edition state of object.
//        /// </summary>
//        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
//        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
//        public override bool AddonEdited(bool sNeedBeUpdate)
//        {
//            if (sNeedBeUpdate == true)
//            {
//                // do something
//            }
//            return sNeedBeUpdate;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addons editor interface.
//        /// </summary>
//        /// <returns>The editor height addon.</returns>
//        /// <param name="sInRect">S in rect.</param>
//        public override float AddonEditor(Rect sInRect)
//        {

//            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
//            float tX = sInRect.position.x + NWDConstants.kFieldMarge;
//            float tY = sInRect.position.y + NWDConstants.kFieldMarge;

//            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
//            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

//            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
//            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

//            float tYadd = 0.0f;

//            tYadd += NWDConstants.kFieldMarge;
//            // Draw line 
//            EditorGUI.DrawRect(new Rect(tX, tY + tYadd, tWidth, 1), NWDConstants.kRowColorLine);
//            tYadd += NWDConstants.kFieldMarge;
//            // draw Flash My App

//            foreach (string tProtocol in NWDAppEnvironment.SelectedEnvironment().AppProtocol.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
//            {
//                string tProto = tProtocol.Replace("://", string.Empty);
//                EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI Scheme Action", URISchemePath(tProto, string.Empty));
//                tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
//                if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI Scheme Action", tMiniButtonStyle))
//                {
//                    Application.OpenURL(URISchemePath(tProto, string.Empty));
//                }
//                tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

//                // Draw QRCode texture
//                Texture2D tTexture = FlashMyApp(tProto,false, 256);
//                EditorGUI.DrawPreviewTexture(new Rect(tX, tY + tYadd, NWDConstants.kPrefabSize * 2, NWDConstants.kPrefabSize * 2),
//                                             tTexture);
//                tYadd += NWDConstants.kPrefabSize * 2 + NWDConstants.kFieldMarge;
//            }
//            return tYadd;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Addons editor intreface expected height.
//        /// </summary>
//        /// <returns>The editor expected height.</returns>
//        public override float AddonEditorHeight()
//        {
//            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
//            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);
//            // Height calculate for the interface addon for editor
//            float tYadd = 0.0F;
//            foreach (string tProtocol in NWDAppEnvironment.SelectedEnvironment().AppProtocol.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
//            {
//                tYadd = NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
//                tYadd += NWDConstants.kFieldMarge;
//                tYadd += NWDConstants.kFieldMarge;
//                tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
//                tYadd += NWDConstants.kPrefabSize * 2 + NWDConstants.kFieldMarge;
//            }
//            return tYadd;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Adds the width of node draw.
//        /// </summary>
//        /// <returns>The on node draw width.</returns>
//        /// <param name="sDocumentWidth">S document width.</param>
//        public override float AddOnNodeDrawWidth(float sDocumentWidth)
//        {
//            return 250.0f;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Adds the height of node draw.
//        /// </summary>
//        /// <returns>The on node draw height.</returns>
//        public override float AddOnNodeDrawHeight(float sCardWidth)
//        {
//            return 40.0f;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Adds node draw.
//        /// </summary>
//        /// <param name="sRect">S rect.</param>
//        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
//        {

//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Adds color on node.
//        /// </summary>
//        /// <returns>The on node color.</returns>
//        public override Color AddOnNodeColor()
//        {
//            return Color.gray;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//#endif
        //#endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================