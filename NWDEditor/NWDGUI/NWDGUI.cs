// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:51
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDGUI
    {
        //-------------------------------------------------------------------------------------------------------------
        public static float kFieldMarge = 5.0f;
        //public static float kTopAdjustMarge = 2.0f;
        public static float kFieldIndent = 15.0f;
        public static float kScrollbar = 20f;
        public static GUIStyle kScrollviewFullWidth;
        public static float KTAB_BAR_HEIGHT = 40.0F;
        //-------------------------------------------------------------------------------------------------------------
        // window top tabbar 
        public static Color KTAB_BAR_BACK_COLOR;
        public static Color KTAB_BAR_LINE_COLOR;
        public static Color KTAB_BAR_HIGHLIGHT_COLOR;
        //-------------------------------------------------------------------------------------------------------------
        public static Texture2D kImageSelectionUpdate = NWDFindPackage.PackageEditorTexture("NWDInterfaceNode.png");  // node
        public static Texture2D kImageUp = NWDFindPackage.PackageEditorTexture("NWDInterfaceUp.png");
        public static Texture2D kImageDown = NWDFindPackage.PackageEditorTexture("NWDInterfaceDown.png");
        public static Texture2D kImageLeft = NWDFindPackage.PackageEditorTexture("NWDInterfaceLeft.png");
        public static Texture2D kImageRight = NWDFindPackage.PackageEditorTexture("NWDInterfaceRight.png");
        public static Texture2D kImageAction = NWDFindPackage.PackageEditorTexture("NWDInterfaceAction.png"); // Clean
        public static Texture2D kImageNew = NWDFindPackage.PackageEditorTexture("NWDInterfaceNew.png");  // new
        public static Texture2D kImageTabReduce = NWDFindPackage.PackageEditorTexture("NWDInterfaceEdit.png"); // edit
        public static Texture2D kImageRed = NWDFindPackage.PackageEditorTexture("NWDRed.png");
        public static Texture2D kImageGreen = NWDFindPackage.PackageEditorTexture("NWDGreen.png");
        public static Texture2D kImageForbidden = NWDFindPackage.PackageEditorTexture("NWDForbidden.png");
        public static Texture2D kImageEmpty = NWDFindPackage.PackageEditorTexture("NWDEmpty.png");
        public static Texture2D kImageWaiting = NWDFindPackage.PackageEditorTexture("NWDWaiting.png");
        public static Texture2D kImageDiskUnknow = NWDFindPackage.PackageEditorTexture("NWDDiskUnknow.png");
        public static Texture2D kImageDiskUpdate = NWDFindPackage.PackageEditorTexture("NWDDiskUpdate.png");
        public static Texture2D kImageDiskInsert = NWDFindPackage.PackageEditorTexture("NWDDiskInsert.png");
        public static Texture2D kImageDiskDelete = NWDFindPackage.PackageEditorTexture("NWDDiskDelete.png");
        public static Texture2D kImageDiskDatabase = NWDFindPackage.PackageEditorTexture("NWDDiskDatabase.png");
        public static Texture2D kImageCheckValid = NWDFindPackage.PackageEditorTexture("NWDCheckValid.png");
        public static Texture2D kImageCheckWorkInProgress = NWDFindPackage.PackageEditorTexture("NWDCheckWorkInProgress.png");
        public static Texture2D kImageCheckWarning = NWDFindPackage.PackageEditorTexture("NWDCheckWarning.png");
        public static Texture2D kImageSyncGeneralForbidden = NWDFindPackage.PackageEditorTexture("NWDSyncGeneralForbidden.png");
        public static Texture2D kImageSyncGeneralForward = NWDFindPackage.PackageEditorTexture("NWDSyncGeneralForward.png");
        public static Texture2D kImageSyncGeneralSuccessed = NWDFindPackage.PackageEditorTexture("NWDSyncGeneralSuccessed.png");
        public static Texture2D kImageSyncGeneralWaiting = NWDFindPackage.PackageEditorTexture("NWDSyncGeneralWaiting.png");
        public static Texture2D kImageSyncDanger = NWDFindPackage.PackageEditorTexture("NWDSyncDanger.png");
        public static Texture2D kImageSyncForbidden = NWDFindPackage.PackageEditorTexture("NWDSyncForbidden.png");
        public static Texture2D kImageSyncForward = NWDFindPackage.PackageEditorTexture("NWDSyncForward.png");
        public static Texture2D kImageSyncRequired = NWDFindPackage.PackageEditorTexture("NWDSyncRequired.png");
        public static Texture2D kImageSyncSuccessed = NWDFindPackage.PackageEditorTexture("NWDSyncSuccessed.png");
        public static Texture2D kImageSyncWaiting = NWDFindPackage.PackageEditorTexture("NWDSyncWaiting.png");
        public static Texture2D kImageSyncProceed = NWDFindPackage.PackageEditorTexture("NWDProceed.png");

        public static Texture2D kImageBezierTexture = NWDFindPackage.PackageEditorTexture("NWDBezierTexture.psd"); //for nodal line?
        public static Texture2D kImageDefaultIcon = NWDFindPackage.PackageEditorTexture("NWDExample.psd");
        //-------------------------------------------------------------------------------------------------------------
        // change color of background interface element
        static Color kOldColor;
        static bool kOldColorInit;
        public static Color kRedElementColor; // invert color from white to fusion over button
        public static Color kGreenElementColor; // invert color from white to fusion over button
        public static Color kYellowElementColor; // invert color from white to fusion over button
        public static Color kBlueElementColor; // invert color from white to fusion over button
        public static Color kOrangeElementColor; // invert color from white to fusion over button
        public static Color kGrayElementColor; // invert color from white to fusion over button
        public static float WarningMinHeight = 48.0F;
        public static float ErrorMinHeight = 48.0F;
        //-------------------------------------------------------------------------------------------------------------
        public static GUIStyle kSeparatorStyle;
        static Texture2D kSeparatorTexture;
        static Color kSeparatorColor;
        public static float kSeparatorHeight = 1.0F;
        //-------------------------------------------------------------------------------------------------------------
        public static GUIStyle kLineStyle;
        static Texture2D kLineTexture;
        static Color kLineColor;
        public static float kLineHeight = 1.0F;
        //-------------------------------------------------------------------------------------------------------------
        public static GUIStyle kTitleStyle;
        static Texture2D kTitleTexture;
        static Color kTitleColor;
        //-----------------------------------------------------------k-----------------k-------------------------------
        public static GUIStyle kSectionStyle;
        static Texture2D kSectionTexture;
        static Color kSectionColor;
        //-------------------------------------------------------------------------------------------------------------
        public static GUIStyle kSubSectionStyle;
        static Texture2D kSubSectionTexture;
        static Color kSubSectionColor;
        //-------------------------------------------------------------------------------------------------------------
        public static float KTableSearchLabelWidth = 80.0F;
        public static float KTableSearchWidth = 120.0F;
        public static float KTableSearchFieldWidth = 200.0F;
        public static float KTableReferenceWidth = 160.0F;
        public static float KTableRowWebModelWidth = 70.0F;
        public static float KTablePageMarge = 5.0F;
        public static float KTableMinWidth = (KTableReferenceWidth + kFieldMarge) * 6.0F;

        public static Color kRowColorSelected;
        public static Color kRowColorSelectedDark;
        public static Color kRowColorError;
        public static Color kRowColorWarning;
        public static Color kRowColorTrash;
        public static Color kRowColorDisactive;

        public static GUIStyle KTableToolbar;
        public static GUIStyle KTableSearchTitle;
        public static GUIStyle KTableSearchMask;
        public static GUIStyle KTableSearchButton;
        public static GUIStyle KTableSearchLabel;
        public static GUIStyle KTableSearchToggle;
        public static GUIStyle KTableSearchEnum;
        public static GUIStyle KTableSearchDescription;
        public static GUIStyle KTableSearchClassIcon;
        public static GUIStyle KTableSearchIcon;
        public static GUIStyle KTableSearchTextfield;

        public static GUIStyle KTableClassToolbar;
        public static GUIStyle KTableClassPopup;

        public static float kTablePrefabWidth = 40.0F;
        public static float kTableSelectWidth = 20.0f;
        public static float kTableIDWidth = 45.0f;
        public static float kTableIconWidth = 44.0f;

        public static float kTableHeaderHeight = 30.0f;
        public static Color kTableHeaderColor;
        public static GUIStyle KTableHeaderSelect;
        public static GUIStyle KTableHeaderId;
        public static GUIStyle KTableHeaderPrefab;
        public static GUIStyle KTableHeaderInformations;
        public static GUIStyle KTableHeaderIcon;
        public static GUIStyle KTableHeaderStatut;
        public static GUIStyle KTableHeaderReference;

        public static float kTableRowHeight = 40.0f;
        public static Color kRowColorLineWhite;
        public static Color kRowColorLineBlack;
        public static GUIStyle KTableRowSelect;
        public static GUIStyle KTableRowId;
        public static GUIStyle KTableRowPrefab;
        public static GUIStyle KTableRowInformations;
        public static GUIStyle KTableRowIcon;
        public static GUIStyle KTableRowStatut;
        public static GUIStyle KTableRowReference;

        //-------------------------------------------------------------------------------------------------------------
        // Datas Selector
        //public static Color kSelectorTileSelected;
        //public static GUIStyle kSelectorTileDarkStyle;
        //public static Color kSelectorRowSelected;
        //public static GUIStyle kSelectorRowDarkStyle;
        ////public static GUIStyle kDatasSelectorRowStyle;
        //public static GUIStyle kDatasSelectorRowErrorStyle;

        public static GUIStyle kDataSelectorFieldStyle;
        public static GUIStyle kDataSelectorTileStyle;
        public static GUIStyle kDataSelectorRowStyle;
        public static float kDatasSelectorYOffset;
        //-------------------------------------------------------------------------------------------------------------
        // Data inspector properties
        // TODO : all rename!! with right name!

        public static Color kIdentityColor; // inspector identity color area
        public static Color kPropertyColor; // inspector identity color area

        public static int kLongString = 5;
        public static int kVeryLongString = 15;
        public static float kPrefabSize = 80.0f;
        public static float kIntWidth = 36.0f;
        public static float kConditionalWidth = 42.0f;
        public static float kEditWidth = 36.0f;
        public static float kLangWidth = 50.0f;
        public static float kEnumWidth = 70.0f;
        public static float kConnectionIndent = 10.0f;
        public static float kUpDownWidth = 18.0f;

        public static GUIStyle kPropertyEntitlementStyle;

        public static GUIStyle kInspectorInternalTitle;
        public static GUIStyle kInspectorReferenceCenter;


        public static GUIStyle kLabelStyle;
        public static GUIStyle kBoldLabelStyle;

        public static GUIStyle kHelpBoxStyle;
        public static GUIStyle kMiniButtonStyle;
        public static GUIStyle kObjectFieldStyle;
        public static GUIStyle kTextFieldStyle;
        public static GUIStyle kFloatFieldStyle;
        public static GUIStyle kIntFieldStyle;
        public static GUIStyle kDoubleFieldStyle;
        public static GUIStyle kLongFieldStyle;
        public static GUIStyle kFoldoutStyle;
        public static GUIStyle kBoldFoldoutStyle;
        public static GUIStyle kColorFieldStyle;
        public static GUIStyle kPopupStyle;
        public static GUIStyle kEnumStyle;
        public static GUIStyle kToggleStyle;

        public static GUIStyle kTextAreaStyle;
        public static GUIStyle kRedLabelStyle;
        public static GUIStyle kGrayLabelStyle;
        public static GUIStyle kLabelRightStyle;
        public static GUIStyle kMiniLabelStyle;

        public static GUIStyle kIconButtonStyle;
        public static GUIStyle kEditButtonStyle;

        //-------------------------------------------------------------------------------------------------------------
        public static GUIContent kNodeContentIcon;

        public static GUIContent kEditContentIcon;
        public static GUIContent kNewContentIcon;
        public static GUIContent kCleanContentIcon;
        public static GUIContent kUpContentIcon;
        public static GUIContent kDownContentIcon;
        public static GUIContent kLeftContentIcon;
        public static GUIContent kRightContentIcon;

        //-------------------------------------------------------------------------------------------------------------
        // Nodal Document
        // TODO : all rename!! with right name!

        public static float kNodeCardWidth = 360.0F;
        public static float kNodeCardHeight = 360.0F;
        //public static float kNodeCardMarging = 50.0F;

        public static float kNodeCanvasFraction = 20;
        public static Color kNodeCanvasMajor;
        public static Color kNodeCanvasMinor;
        public static Color kNodeCanvasMargeBlack;

        public static Color kNodeLineColor;
        public static Color kNodeOverLineColor;
        public static float kIconWidth = 36.0f;
        public static float kEditIconSide = 16.0f;
        public static float kEditWidthHalf = 8.0f;
        public static float kEditWidthMini = 12.0f;
        public static float kEditWidthMiniHalf = 6.0f;
        //-------------------------------------------------------------------------------------------------------------
        static private bool StyleLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        static NWDGUI()
        {
            StyleLoaded = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadStylesReforce()
        {
            StyleLoaded = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadStyles()
        {
            //BTBBenchmark.Start();
            if (StyleLoaded == false)
            {
                //Debug.Log("LoadStyles()");
                StyleLoaded = true;

                // color force 

                KTAB_BAR_BACK_COLOR = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                KTAB_BAR_LINE_COLOR = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                KTAB_BAR_HIGHLIGHT_COLOR = new Color(0.5f, 0.5f, 0.5f, 1.0f);

                kRedElementColor = new Color(0.9F, 0.7F, 0.7F, 1.0F); // invert color from white to fusion over button
                kGreenElementColor = new Color(0.7F, 0.9F, 0.7F, 1.0F); // invert color from white to fusion over button
                kYellowElementColor = new Color(0.1F, 0.9F, 0.9F, 1.0F); // invert color from white to fusion over button
                kBlueElementColor = new Color(0.7F, 0.7F, 0.9F, 1.0F); // invert color from white to fusion over button
                kOrangeElementColor = new Color(0.9F, 0.8F, 0.7F, 1.0F); // invert color from white to fusion over button
                kGrayElementColor = new Color(0.7F, 0.7F, 0.7F, 1.0F); // invert color from white to fusion over button

                kSeparatorColor = new Color(0, 0, 0, 0.5F);

                kLineColor = new Color(0, 0, 0, 0.5F);

                kTitleColor = new Color(0, 0, 0, 0.3F);

                kSectionColor = new Color(0, 0, 0, 0.2F);

                kSubSectionColor = new Color(0, 0, 0, 0.1F);

                kRowColorSelected = new Color(0.55f, 0.55f, 1.00f, 0.25f);
                kRowColorSelectedDark = new Color(0.55f, 0.55f, 1.00f, 0.75f);
                kRowColorError = new Color(1.00f, 0.00f, 0.00f, 0.55f);
                kRowColorWarning = new Color(1.00f, 0.50f, 0.00f, 0.55f);
                kRowColorTrash = new Color(0.00f, 0.00f, 0.00f, 0.45f);
                kRowColorDisactive = new Color(0.00f, 0.00f, 0.00f, 0.35f);


                kTableHeaderColor = new Color(0.0f, 0.0f, 0.0f, 0.35f);

                kRowColorLineWhite = new Color(1.0f, 1.0f, 1.0f, 0.25f);
                kRowColorLineBlack = new Color(0.0f, 0.0f, 0.0f, 0.25f);


                //kSelectorTileSelected = new Color(0.55f, 0.55f, 1.00f, 0.75f);

                //kSelectorRowSelected = new Color(0.55f, 0.55f, 1.00f, 0.75f);

                kNodeCanvasMajor = new Color(1.0F, 1.0F, 1.0F, 0.20F);
                kNodeCanvasMinor = new Color(1.0F, 1.0F, 1.0F, 0.10F);

                kNodeCanvasMargeBlack = new Color(0.1F, 0.1F, 0.1F, 1.0F);

                kNodeLineColor = new Color(1.0F, 1.0F, 1.0F, 0.40F);
                kNodeOverLineColor = new Color(0.50F, 0.50F, 0.50F, 0.70F);

                // texture apply

                kSeparatorTexture = new Texture2D(1, 1);
                kSeparatorTexture.SetPixel(0, 0, kSeparatorColor);
                kSeparatorTexture.Apply();

                kLineTexture = new Texture2D(1, 1);
                kLineTexture.SetPixel(0, 0, kLineColor);
                kLineTexture.Apply();

                kTitleTexture = new Texture2D(1, 1);
                kTitleTexture.SetPixel(0, 0, kTitleColor);
                kTitleTexture.Apply();

                kSectionTexture = new Texture2D(1, 1);
                kSectionTexture.SetPixel(0, 0, kSectionColor);
                kSectionTexture.Apply();

                kSubSectionTexture = new Texture2D(1, 1);
                kSubSectionTexture.SetPixel(0, 0, kSubSectionColor);
                kSubSectionTexture.Apply();


                // Scrollview full marging

                kScrollviewFullWidth = new GUIStyle(EditorStyles.inspectorFullWidthMargins);
                kScrollviewFullWidth.padding = new RectOffset(0, 0, 0, 0);
                kScrollviewFullWidth.margin = new RectOffset(0, 0, 0, 0);

                // Separator

                kSeparatorStyle = new GUIStyle(EditorStyles.label);
                kSeparatorStyle.fontSize = 0;
                kSeparatorStyle.normal.background = kSeparatorTexture;
                kSeparatorStyle.padding = new RectOffset(2, 2, 2, 2);
                kSeparatorStyle.margin = new RectOffset(0, 0, 1, 1);
                kSeparatorStyle.fixedHeight = kSeparatorHeight;

                // Line

                kLineStyle = new GUIStyle(EditorStyles.label);
                kLineStyle.fontSize = 0;
                kLineStyle.normal.background = kLineTexture;
                kLineStyle.padding = new RectOffset(0, 0, 0, 0);
                kLineStyle.margin = new RectOffset(0, 0, 0, 0);
                kLineStyle.fixedHeight = kLineHeight;

                // general windows design 

                kTitleStyle = new GUIStyle(EditorStyles.label);
                kTitleStyle.fontSize = 14;
                //kTitleStyle.fontStyle = FontStyle.Bold;
                kTitleStyle.normal.background = kTitleTexture;
                kTitleStyle.padding = new RectOffset(6, 2, 16, 2);
                kTitleStyle.margin = new RectOffset(0, 0, 1, 1);
                kTitleStyle.fixedHeight = kTitleStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                kTitleStyle.richText = true;
                //kTitleStyle.normal.textColor = Color.red;

                kSectionStyle = new GUIStyle(EditorStyles.label);
                kSectionStyle.fontSize = 12;
                kSectionStyle.fontStyle = FontStyle.Italic;
                kSectionStyle.normal.background = kSectionTexture;
                kSectionStyle.padding = new RectOffset(6, 2, 8, 2);
                kSectionStyle.margin = new RectOffset(0, 0, 1, 1);
                kSectionStyle.fixedHeight = kSectionStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                //kSectionStyle.normal.textColor = Color.yellow;

                kSubSectionStyle = new GUIStyle(EditorStyles.label);
                kSubSectionStyle.fontSize = 12;
                kSubSectionStyle.fontStyle = FontStyle.Italic;
                kSubSectionStyle.normal.background = kSubSectionTexture;
                kSubSectionStyle.padding = new RectOffset(6, 2, 8, 2);
                kSubSectionStyle.margin = new RectOffset(0, 0, 1, 1);
                kSubSectionStyle.fixedHeight = kSubSectionStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                //kSubSectionStyle.normal.textColor = Color.blue;

                // Table design

                KTableClassToolbar = new GUIStyle(GUI.skin.button);
                KTableClassToolbar.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                KTableClassPopup = new GUIStyle(EditorStyles.popup);
                KTableClassPopup.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                KTableToolbar = new GUIStyle(EditorStyles.toolbar);
                KTableSearchIcon = new GUIStyle(EditorStyles.label);
                KTableSearchTitle = new GUIStyle(EditorStyles.helpBox);
                KTableSearchDescription = new GUIStyle(EditorStyles.helpBox);
                KTableSearchClassIcon = new GUIStyle(EditorStyles.helpBox);
                KTableSearchTextfield = new GUIStyle(EditorStyles.textField);
                KTableSearchToggle = new GUIStyle(EditorStyles.toggle);
                KTableSearchEnum = new GUIStyle(EditorStyles.popup);
                KTableSearchMask = new GUIStyle(EditorStyles.layerMaskField);
                KTableSearchButton = new GUIStyle(EditorStyles.miniButton);
                KTableSearchLabel = new GUIStyle(EditorStyles.label);

                KTableToolbar.fixedHeight = KTableToolbar.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                KTableSearchIcon.alignment = TextAnchor.MiddleCenter;

                float tTableSearchHeight = KTableSearchTextfield.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                KTableSearchTitle.fixedHeight = tTableSearchHeight;
                KTableSearchTextfield.fixedHeight = tTableSearchHeight;
                KTableSearchToggle.fixedHeight = tTableSearchHeight;
                KTableSearchEnum.fixedHeight = tTableSearchHeight;
                KTableSearchMask.fixedHeight = tTableSearchHeight;
                KTableSearchButton.fixedHeight = tTableSearchHeight;
                KTableSearchLabel.fixedHeight = tTableSearchHeight;

                KTableSearchTitle.alignment = TextAnchor.MiddleCenter;
                KTableSearchIcon.alignment = TextAnchor.MiddleCenter;

                KTableHeaderSelect = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderPrefab = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderId = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderIcon = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderInformations = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderStatut = new GUIStyle(EditorStyles.helpBox);
                KTableHeaderReference = new GUIStyle(EditorStyles.helpBox);

                KTableHeaderSelect.alignment = TextAnchor.MiddleCenter;
                KTableHeaderPrefab.alignment = TextAnchor.MiddleCenter;
                KTableHeaderId.alignment = TextAnchor.MiddleCenter;
                KTableHeaderIcon.alignment = TextAnchor.MiddleCenter;
                KTableHeaderInformations.alignment = TextAnchor.MiddleCenter;
                KTableHeaderStatut.alignment = TextAnchor.MiddleCenter;
                KTableHeaderReference.alignment = TextAnchor.MiddleCenter;

                KTableRowSelect = new GUIStyle(EditorStyles.label);
                KTableRowPrefab = new GUIStyle(EditorStyles.label);
                KTableRowId = new GUIStyle(EditorStyles.label);
                KTableRowIcon = new GUIStyle(EditorStyles.label);
                KTableRowInformations = new GUIStyle(EditorStyles.label);
                KTableRowStatut = new GUIStyle(EditorStyles.label);
                KTableRowReference = new GUIStyle(EditorStyles.label);

                KTableRowSelect.fixedHeight = KTableRowSelect.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                KTableRowSelect.alignment = TextAnchor.MiddleCenter;
                KTableRowPrefab.alignment = TextAnchor.MiddleCenter;
                KTableRowId.alignment = TextAnchor.MiddleRight;
                KTableRowIcon.alignment = TextAnchor.MiddleCenter;
                KTableRowInformations.alignment = TextAnchor.MiddleLeft;
                KTableRowStatut.alignment = TextAnchor.MiddleCenter;
                KTableRowReference.alignment = TextAnchor.MiddleRight;

                KTableRowIcon.richText = true;
                KTableRowInformations.richText = true;
                KTableRowStatut.richText = true;

                KTableRowIcon.padding = new RectOffset(10, 10, 10, 10);
                KTableRowInformations.wordWrap = true;

                // Inspector 

                kIdentityColor = new Color(0.7f, 0.7f, 0.7f, 0.4f);
                kPropertyColor = new Color(0.8f, 0.8f, 0.8f, 0.3f);

                if (EditorGUIUtility.isProSkin)
                {
                    kIdentityColor = new Color(0.3f, 0.3f, 0.3f, 0.4f);
                    kPropertyColor = new Color(0.2f, 0.2f, 0.2f, 0.2f);
                }

                kInspectorInternalTitle = new GUIStyle(EditorStyles.boldLabel);
                kInspectorInternalTitle.alignment = TextAnchor.MiddleCenter;
                kInspectorInternalTitle.fontSize = 14;
                kInspectorInternalTitle.fixedHeight = kInspectorInternalTitle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);
                kInspectorInternalTitle.richText = true;

                kInspectorReferenceCenter = new GUIStyle(EditorStyles.miniLabel);
                kInspectorReferenceCenter.fixedHeight = kInspectorReferenceCenter.CalcHeight(new GUIContent(BTBConstants.K_A), 100);
                kInspectorReferenceCenter.alignment = TextAnchor.MiddleCenter;


                kPropertyEntitlementStyle = new GUIStyle(EditorStyles.label);
                kPropertyEntitlementStyle.fixedHeight = kPropertyEntitlementStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                kPropertyEntitlementStyle.richText = true;

                kLabelStyle = new GUIStyle(EditorStyles.label);
                kLabelStyle.fixedHeight = kLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
                kMiniLabelStyle.fixedHeight = kMiniLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
                kBoldLabelStyle.fixedHeight = kBoldLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                kHelpBoxStyle.fixedHeight = kHelpBoxStyle.CalcHeight(new GUIContent("A\nA\nA"), 100);

                kMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kMiniButtonStyle.fixedHeight = kMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
                kObjectFieldStyle.fixedHeight = kObjectFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kTextFieldStyle = new GUIStyle(EditorStyles.textField);
                kTextFieldStyle.fixedHeight = kTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);
                kTextFieldStyle.richText = true;

                kFloatFieldStyle = new GUIStyle(EditorStyles.numberField);
                kFloatFieldStyle.fixedHeight = kFloatFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kIntFieldStyle = new GUIStyle(EditorStyles.numberField);
                kIntFieldStyle.fixedHeight = kIntFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kLongFieldStyle = new GUIStyle(EditorStyles.numberField);
                kLongFieldStyle.fixedHeight = kLongFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kDoubleFieldStyle = new GUIStyle(EditorStyles.numberField);
                kDoubleFieldStyle.fixedHeight = kDoubleFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kFoldoutStyle = new GUIStyle(EditorStyles.foldout);
                kFoldoutStyle.fixedHeight = kFoldoutStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kColorFieldStyle = new GUIStyle(EditorStyles.colorField);
                kColorFieldStyle.fixedHeight = kColorFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kPopupStyle = new GUIStyle(EditorStyles.popup);
                kPopupStyle.fixedHeight = kPopupStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kEnumStyle = new GUIStyle(EditorStyles.popup);
                kEnumStyle.fixedHeight = kEnumStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kToggleStyle = new GUIStyle(EditorStyles.toggle);
                kToggleStyle.fixedHeight = kToggleStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kBoldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
                kBoldFoldoutStyle.fontStyle = FontStyle.Bold;
                kBoldFoldoutStyle.fixedHeight = kBoldFoldoutStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

                kLabelRightStyle = new GUIStyle(EditorStyles.label);
                kLabelRightStyle.alignment = TextAnchor.MiddleRight;
                kLabelRightStyle.fixedHeight = kLabelRightStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);


                kTextAreaStyle = new GUIStyle(EditorStyles.textField);
                kTextAreaStyle.wordWrap = true;


                kRedLabelStyle = new GUIStyle(EditorStyles.label);
                kRedLabelStyle.fixedHeight = kRedLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                kRedLabelStyle.normal.textColor = Color.red;

                kMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kMiniButtonStyle.fixedHeight = kMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);


                // Data Selector design

                kDataSelectorFieldStyle = new GUIStyle(EditorStyles.helpBox);
                kDataSelectorFieldStyle.richText = true;
                kDataSelectorFieldStyle.fontSize = 12;
                kDataSelectorFieldStyle.wordWrap = false;
                kDataSelectorFieldStyle.alignment = TextAnchor.MiddleLeft;
                kDataSelectorFieldStyle.imagePosition = ImagePosition.ImageLeft;
                kDataSelectorFieldStyle.border = new RectOffset(2, 2, 2, 2);
                kDataSelectorFieldStyle.fixedHeight = kDataSelectorFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                //kDatasSelectorRowErrorStyle = new GUIStyle(kDataSelectorFieldStyle);
                //kDatasSelectorRowErrorStyle.normal.textColor = Color.red;

                // Selector design

                kDataSelectorTileStyle = new GUIStyle(EditorStyles.helpBox);
                kDataSelectorTileStyle.richText = true;
                kDataSelectorTileStyle.fontSize = 10;
                kDataSelectorTileStyle.imagePosition = ImagePosition.ImageAbove;
                kDataSelectorTileStyle.border = new RectOffset(2, 2, 2, 4);
                kDataSelectorTileStyle.alignment = TextAnchor.LowerCenter;
                kDataSelectorTileStyle.fixedHeight = kDataSelectorTileStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                //kSelectorTileDarkStyle = new GUIStyle(EditorStyles.helpBox);
                //kSelectorTileDarkStyle.richText = true;
                //kSelectorTileDarkStyle.fontSize = 14;
                //kSelectorTileDarkStyle.imagePosition = ImagePosition.ImageAbove;
                //kSelectorTileDarkStyle.border = new RectOffset(2, 2, 2, 4);
                //kSelectorTileDarkStyle.alignment = TextAnchor.LowerCenter;
                //kSelectorTileDarkStyle.fixedHeight = kSelectorTileDarkStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                kDataSelectorRowStyle = new GUIStyle(EditorStyles.helpBox);
                kDataSelectorRowStyle.richText = true;
                kDataSelectorRowStyle.fontSize = 14;
                kDataSelectorRowStyle.imagePosition = ImagePosition.ImageLeft;
                kDataSelectorRowStyle.border = new RectOffset(2, 4, 2, 2);
                kDataSelectorRowStyle.alignment = TextAnchor.MiddleLeft;
                kDataSelectorRowStyle.fixedHeight = kDataSelectorRowStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                //kSelectorRowDarkStyle = new GUIStyle(EditorStyles.helpBox);
                //kSelectorRowDarkStyle.richText = true;
                //kSelectorRowDarkStyle.fontSize = 14;
                //kSelectorRowDarkStyle.imagePosition = ImagePosition.ImageLeft;
                //kSelectorRowDarkStyle.border = new RectOffset(2, 4, 2, 2);
                //kSelectorRowDarkStyle.alignment = TextAnchor.MiddleLeft;
                //kSelectorRowDarkStyle.fixedHeight = kSelectorRowDarkStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);


                // References content

                kIconButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kIconButtonStyle.fixedHeight = kIconButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                kIconButtonStyle.padding = new RectOffset(2, 2, 2, 2);
                kUpDownWidth = kIconButtonStyle.fixedHeight;

                kEditButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kEditButtonStyle.fixedHeight = kEditButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                kEditButtonStyle.padding = new RectOffset(2, 2, 2, 2);

                kNodeContentIcon = new GUIContent(/*kImageSelectionUpdate,*/ "node");

                kEditContentIcon = new GUIContent(/*kImageTabReduce,*/ "edit");
                kNewContentIcon = new GUIContent(/*kImageNew, */"new");
                kCleanContentIcon = new GUIContent(/*kImageAction, */"clean");

                kUpContentIcon = new GUIContent(kImageUp, "up");
                kDownContentIcon = new GUIContent(kImageDown, "down");
                kLeftContentIcon = new GUIContent(kImageLeft, "<");
                kRightContentIcon = new GUIContent(kImageRight, ">");

                kDatasSelectorYOffset = 2;

                //kNodeContentIcon = new GUIContent("node");
                //kEditContentIcon = new GUIContent("edit");
                //kNewContentIcon = new GUIContent("new");
                //kCleanContentIcon = new GUIContent("clean");
                //kUpContentIcon = new GUIContent("up");
                //kDownContentIcon = new GUIContent("down");
                //kLeftContentIcon = new GUIContent("<");
                //kRightContentIcon = new GUIContent(">");
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect MargeLeftRight(Rect sRect)
        {
            return new Rect(sRect.x + kFieldMarge, sRect.y, sRect.width - kFieldMarge * 2, sRect.height);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect MargeTopBottom(Rect sRect)
        {
            return new Rect(sRect.x, sRect.y + kFieldMarge, sRect.width, sRect.height - kFieldMarge * 2);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect MargeAll(Rect sRect)
        {
            return new Rect(sRect.x + kFieldMarge, sRect.y + kFieldMarge, sRect.width - kFieldMarge * 2, sRect.height - kFieldMarge * 2);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect UnMargeAll(Rect sRect)
        {
            return new Rect(sRect.x - kFieldMarge, sRect.y - kFieldMarge, sRect.width + kFieldMarge * 2, sRect.height + kFieldMarge * 2);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Line(Rect sRect)
        {
            sRect.height = kLineStyle.fixedHeight;
            GUI.Label(sRect, string.Empty, kLineStyle);
            sRect.y += kLineStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Separator(Rect sRect)
        {
            sRect.y += kFieldMarge;
            sRect.height = kSeparatorStyle.fixedHeight;
            GUI.Label(sRect, string.Empty, kSeparatorStyle);
            sRect.y += kSeparatorStyle.fixedHeight + kFieldMarge;
            sRect.height = kSeparatorStyle.fixedHeight + kFieldMarge * 2;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static float SeparatorHeight()
        {
            return kSeparatorStyle.fixedHeight + kFieldMarge * 2;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void BeginRedArea()
        {
            if (kOldColorInit == false)
            {
                kOldColor = GUI.backgroundColor;
                kOldColorInit = true;
            }
            GUI.backgroundColor = kRedElementColor;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void EndRedArea()
        {
            if (kOldColorInit == true)
            {
                GUI.backgroundColor = kOldColor;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Color DefaultColorArea()
        {
            if (kOldColorInit == false)
            {
                kOldColor = GUI.backgroundColor;
                kOldColorInit = true;
            }
            return kOldColor;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void BeginColorArea(Color sColor)
        {
            if (kOldColorInit == false)
            {
                kOldColor = GUI.backgroundColor;
                kOldColorInit = true;
            }
            GUI.backgroundColor = sColor;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void EndColorArea()
        {
            if (kOldColorInit == true)
            {
                GUI.backgroundColor = kOldColor;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Title(Rect sRect, string sTitle)
        {
            GUI.Label(sRect, sTitle, NWDGUI.kTitleStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            Line(new Rect(sRect.x, sRect.y + kTitleStyle.fixedHeight - 1, sRect.width, 1));
            sRect.height = kTitleStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Title(Rect sRect, GUIContent sTitle)
        {
            GUI.Label(sRect, sTitle, NWDGUI.kTitleStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            Line(new Rect(sRect.x, sRect.y + kTitleStyle.fixedHeight - 1, sRect.width, 1));
            sRect.height = kTitleStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Section(Rect sRect, string sTitle)
        {
            GUI.Label(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, sRect.height - NWDGUI.kFieldMarge), sTitle, NWDGUI.kSectionStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            sRect.height = kSectionStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Section(Rect sRect, GUIContent sTitle)
        {
            GUI.Label(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, sRect.height - NWDGUI.kFieldMarge), sTitle, NWDGUI.kSectionStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            sRect.height = kSectionStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect SubSection(Rect sRect, string sTitle)
        {
            GUI.Label(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, sRect.height - NWDGUI.kFieldMarge), sTitle, NWDGUI.kSubSectionStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            sRect.height = kSubSectionStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect SubSection(Rect sRect, GUIContent sTitle)
        {
            GUI.Label(new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, sRect.width, sRect.height - NWDGUI.kFieldMarge), sTitle, NWDGUI.kSubSectionStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            sRect.height = kSubSectionStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Informations(Rect sRect, string sTitle)
        {
            //sRect.y += kFieldMarge;
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            EditorGUI.HelpBox(sRect, sTitle, MessageType.None);
            //sRect.height += kFieldMarge * 2;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect HelpBox(Rect sRect, string sTitle)
        {
            //sRect.y += kFieldMarge;
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Info);
            //sRect.height += kFieldMarge * 2;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect WarningBox(Rect sRect, string sTitle)
        {
            BeginColorArea(kYellowElementColor);
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            if (sRect.height < WarningMinHeight)
            {
                sRect.height = WarningMinHeight;
            }
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Warning);
            EndColorArea();
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static float WarningBoxHeight(Rect sRect, string sTitle)
        {
            float tHeight = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            if (tHeight < WarningMinHeight)
            {
                tHeight = WarningMinHeight;
            }
            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect ErrorBox(Rect sRect, string sTitle)
        {
            //sRect.y += kFieldMarge;
            BeginColorArea(kRedElementColor);
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            if (sRect.height < ErrorMinHeight)
            {
                sRect.height = ErrorMinHeight;
            }
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Error);
            EndColorArea();
            //sRect.height += kFieldMarge*2;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
