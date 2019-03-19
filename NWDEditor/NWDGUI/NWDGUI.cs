//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
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
        static Color kOldColor;
        static bool kOldColorInit;
        public static Color K_RED_BUTTON_COLOR = new Color(0.9F, 0.7F, 0.7F, 1.0F); // invert color from white to fusion over button
        //-------------------------------------------------------------------------------------------------------------
        static public GUIStyle kSeparatorStyle;
        static public GUIStyle kLineStyle;
        //-------------------------------------------------------------------------------------------------------------
        static public GUIStyle kTitleStyle;
        static public GUIStyle kSectionStyle;
        static public GUIStyle kSubSectionStyle;
        //-------------------------------------------------------------------------------------------------------------
        public static float KTableSearchLabelWidth = 80.0F;
        public static float KTableSearchWidth = 120.0F;
        public static float KTableSearchFieldWidth = 200.0F;
        public static float KTableReferenceWidth = 160.0F;
        public static float KTableRowWebModelWidth = 60.0F;
        public static float KTablePageMarge = 5.0F;
        public static float kFieldMarge = 5.0f;
        public static float KTableMinWidth = (KTableReferenceWidth + kFieldMarge) * 6.0F;

        public static GUIStyle KTableAreaColorDark;

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

        public static GUIStyle KTableHeaderSelect;
        public static GUIStyle KTableHeaderId;
        public static GUIStyle KTableHeaderPrefab;
        public static GUIStyle KTableHeaderInformations;
        public static GUIStyle KTableHeaderIcon;
        public static GUIStyle KTableHeaderStatut;
        public static GUIStyle KTableHeaderReference;

        public static GUIStyle KTableRowSelect;
        public static GUIStyle KTableRowId;
        public static GUIStyle KTableRowPrefab;
        public static GUIStyle KTableRowInformations;
        public static GUIStyle KTableRowIcon;
        public static GUIStyle KTableRowStatut;
        public static GUIStyle KTableRowReference;
        //-------------------------------------------------------------------------------------------------------------
        static private bool StyleLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadStyles()
        {
            if (StyleLoaded == false)
            {
                StyleLoaded = true;
                // Separator
                kSeparatorStyle = new GUIStyle(EditorStyles.label);
                kSeparatorStyle.fontSize = 0;
                kSeparatorStyle.normal.background = new Texture2D(1, 1);
                Color tGraykSeparatorStyle = new Color(0, 0, 0, 0.5F);
                kSeparatorStyle.normal.background.SetPixel(0, 0, tGraykSeparatorStyle);
                kSeparatorStyle.normal.background.Apply();
                kSeparatorStyle.padding = new RectOffset(2, 2, 2, 2);
                kSeparatorStyle.margin = new RectOffset(0, 0, 1, 1);
                kSeparatorStyle.fixedHeight = 1.0f;

                // Line
                kLineStyle = new GUIStyle(EditorStyles.label);
                kLineStyle.fontSize = 0;
                kLineStyle.normal.background = new Texture2D(1, 1);
                Color tGraykLineStyle = new Color(0, 0, 0, 0.5F);
                kLineStyle.normal.background.SetPixel(0, 0, tGraykLineStyle);
                kLineStyle.normal.background.Apply();
                kLineStyle.padding = new RectOffset(0, 0, 0, 0);
                kLineStyle.margin = new RectOffset(0, 0, 0, 0);
                kLineStyle.fixedHeight = 1.0f;


                // general windows design 
                kTitleStyle = new GUIStyle(EditorStyles.label);
                kTitleStyle.fontSize = 14;
                kTitleStyle.fontStyle = FontStyle.Bold;
                kTitleStyle.normal.background = new Texture2D(1, 1);
                Color tTitleStyleColor = new Color(0, 0, 0, 0.2F);
                kTitleStyle.normal.background.SetPixel(0, 0, tTitleStyleColor);
                kTitleStyle.normal.background.Apply();
                kTitleStyle.padding = new RectOffset(6, 2, 16, 2);
                kTitleStyle.margin = new RectOffset(0, 0, 1, 1);
                kTitleStyle.fixedHeight = kTitleStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                kTitleStyle.richText = true;

                kTitleStyle.normal.textColor = Color.red;

                kSectionStyle = new GUIStyle(EditorStyles.label);
                kSectionStyle.fontSize = 12;
                kSectionStyle.fontStyle = FontStyle.Italic;
                kSectionStyle.normal.background = new Texture2D(1, 1);
                Color tSectionStyleColor = new Color(0, 0, 0, 0.1F);
                kSectionStyle.normal.background.SetPixel(0, 0, tSectionStyleColor);
                kSectionStyle.normal.background.Apply();
                kSectionStyle.padding = new RectOffset(6, 2, 8, 2);
                kSectionStyle.margin = new RectOffset(0, 0, 1, 1);
                kSectionStyle.fixedHeight = kSectionStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                kSectionStyle.normal.textColor = Color.yellow;

                kSubSectionStyle = new GUIStyle(EditorStyles.label);
                kSubSectionStyle.fontSize = 12;
                kSubSectionStyle.fontStyle = FontStyle.Italic;
                kSubSectionStyle.normal.background = new Texture2D(1, 1);
                Color tSubSectionStyleColor = new Color(0, 0, 0, 0.1F);
                kSubSectionStyle.normal.background.SetPixel(0, 0, tSubSectionStyleColor);
                kSubSectionStyle.normal.background.Apply();
                kSubSectionStyle.padding = new RectOffset(6, 2, 8, 2);
                kSubSectionStyle.margin = new RectOffset(0, 0, 1, 1);
                kSubSectionStyle.fixedHeight = kSubSectionStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                kSubSectionStyle.normal.textColor = Color.blue;
                // Table design

                KTableClassToolbar = new GUIStyle(GUI.skin.button);
                KTableClassToolbar.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);
                KTableClassPopup = new GUIStyle(EditorStyles.popup);
                KTableClassPopup.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                KTableAreaColorDark = new GUIStyle(EditorStyles.label);
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

                KTableSearchIcon.alignment = TextAnchor.MiddleCenter;

                float tTableSearchHeight = KTableSearchTextfield.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

                KTableSearchTitle.fixedHeight = tTableSearchHeight;
                KTableSearchTextfield.fixedHeight = tTableSearchHeight;
                KTableSearchToggle.fixedHeight = tTableSearchHeight;
                KTableSearchEnum.fixedHeight = tTableSearchHeight;
                KTableSearchMask.fixedHeight = tTableSearchHeight;
                KTableSearchButton.fixedHeight = tTableSearchHeight;
                KTableSearchLabel.fixedHeight = tTableSearchHeight;

                if (EditorGUIUtility.isProSkin)
                {
                    KTableAreaColorDark.normal.background = NWDToolbox.TextureFromColor(new Color(0.0f, 0.0f, 0.0f, 0.35f));
                }
                else
                {
                    KTableAreaColorDark.normal.background = NWDToolbox.TextureFromColor(new Color(0.0f, 0.0f, 0.0f, 0.55f));
                }
                KTableAreaColorDark.padding = new RectOffset(0, 0, 5, 5);
                KTableAreaColorDark.margin = new RectOffset(0, 0, 0, 0);
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
            }
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
            sRect.height = kSeparatorStyle.fixedHeight;
            GUI.Label(sRect, string.Empty, kSeparatorStyle);
            sRect.y += kSeparatorStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void BeginRedArea()
        {
            if (kOldColorInit == false)
            {
                kOldColor = GUI.backgroundColor;
                kOldColorInit = true;
            }
            GUI.backgroundColor = K_RED_BUTTON_COLOR;
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
            Line(new Rect(sRect.x, sRect.y+ kTitleStyle.fixedHeight - 1, sRect.width, 1));
            sRect.height = kTitleStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Section(Rect sRect, string sTitle)
        {
            GUI.Label(new Rect(sRect.x, sRect.y + NWDConstants.kFieldMarge, sRect.width, sRect.height - NWDConstants.kFieldMarge), sTitle, NWDGUI.kSectionStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            sRect.height = kSectionStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect Section(Rect sRect, GUIContent sTitle)
        {
            GUI.Label(new Rect(sRect.x, sRect.y + NWDConstants.kFieldMarge, sRect.width, sRect.height - NWDConstants.kFieldMarge), sTitle, NWDGUI.kSectionStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            sRect.height = kSectionStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect SubTitle(Rect sRect, string sTitle)
        {
            GUI.Label(new Rect(sRect.x, sRect.y + NWDConstants.kFieldMarge, sRect.width, sRect.height - NWDConstants.kFieldMarge), sTitle, NWDGUI.kSubSectionStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            sRect.height = kSubSectionStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect SubTitle(Rect sRect, GUIContent sTitle)
        {
            GUI.Label(new Rect(sRect.x, sRect.y + NWDConstants.kFieldMarge, sRect.width, sRect.height - NWDConstants.kFieldMarge), sTitle, NWDGUI.kSubSectionStyle);
            Line(new Rect(sRect.x, sRect.y, sRect.width, 1));
            sRect.height = kSubSectionStyle.fixedHeight;
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect HelpBox(Rect sRect, string sTitle)
        {
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Info);
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect WarningBox(Rect sRect, string sTitle)
        {
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Warning);
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect ErrorBox(Rect sRect, string sTitle)
        {
            sRect.height = EditorStyles.helpBox.CalcHeight(new GUIContent(sTitle), sRect.width);
            EditorGUI.HelpBox(sRect, sTitle, MessageType.Error);
            return sRect;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
