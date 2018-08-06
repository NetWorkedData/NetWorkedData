//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
using SQLite4Unity3d;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public void NodeCardAnalyze(NWDNodeCard sCard)
        {
            //Debug.Log("NWDBasis<K> NodeCardAnalyze() Ananlyze type " + ClassNamePHP());
            // insert informations
            sCard.ClassTexture = Datas().TextureOfClass();
            sCard.ReferenceString = Reference;
            sCard.TypeString = Datas().ClassNamePHP;
            sCard.InternalKeyString = InternalKey;
            sCard.Width = AddOnNodeDrawWidth(sCard.ParentDocument.GetWidth());
            sCard.ParentDocument.SetWidth(AddOnNodeDrawWidth(sCard.ParentDocument.GetWidth()));
            sCard.InformationsHeight = AddOnNodeDrawHeight(sCard.Width);
            sCard.ParentDocument.SetInformationsHeight(sCard.InformationsHeight);
            sCard.InformationsColor = AddOnNodeColor();
            // data must be analyzed
            // data is in a preview card?
            //bool tDataAllReadyShow = false;
            //foreach (NWDNodeCard tCard in sCard.ParentDocument.AllCards)
            //{
            //    if (tCard.Data == sCard.Data)
            //    {
            //        tDataAllReadyShow = true;
            //        break;
            //    }
            //}
            bool tDataAllReadyAnalyze = false;
            foreach (NWDNodeCard tCard in sCard.ParentDocument.AllCardsAnalyzed)
            {
                if (tCard.Data == sCard.Data)
                {
                    tDataAllReadyAnalyze = true;
                    break;
                }
            }
            // data is not in a preview card
            if (tDataAllReadyAnalyze == false)
            {
                // I add this card
                sCard.ParentDocument.AllCardsAnalyzed.Add(sCard);
                // I analyze this card and its properties (only the reference properties)
                Type tType = ClassType();


                if (sCard.ParentDocument.AnalyzeTheseClasses[tType.Name] == true)
                {
                    foreach (PropertyInfo tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        Type tTypeOfThis = tProp.PropertyType;
                        if (tTypeOfThis != null)
                        {
                            if (tTypeOfThis.IsGenericType)
                            {
                                if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>)
                                    || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesListType<>)
                                    || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesQuantityType<>)
                                    || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesArrayType<>)


                                    || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesAverageType<>)
                                    || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesAmountType<>)
                                    || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesRangeType<>)
                                    || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceConditionalType<>)

                                    //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(AIRReferencesAverageType<>)
                                    //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(AIRReferencesRangeType<>)

                                   )
                                {




                                    Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                                    if (sCard.ParentDocument.ShowTheseClasses[tSubType.Name] == true)
                                    {

                                        var tMethodInfo = tTypeOfThis.GetMethod("EditorGetObjects", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                                        if (tMethodInfo != null)
                                        {
                                            var tVar = tProp.GetValue(this, null);
                                            if (tVar != null)
                                            {
                                                object[] tObjects = tMethodInfo.Invoke(tVar, null) as object[];
                                                bool tButtonAdd = true;
                                                int tObjectCounter = 0;
                                                foreach (object tObj in tObjects)
                                                {
                                                    if (tObj != null)
                                                    {
                                                        tObjectCounter++;
                                                    }
                                                }

                                                NWDNodeConnectionReferenceType tConType = NWDNodeConnectionReferenceType.None;

                                                if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>) && tObjectCounter > 0)
                                                {
                                                    tButtonAdd = false;
                                                }

                                                if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
                                                {
                                                    tConType = NWDNodeConnectionReferenceType.ReferenceType;
                                                }
                                                else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesListType<>))
                                                {
                                                    tConType = NWDNodeConnectionReferenceType.ReferencesListType;
                                                }
                                                else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesQuantityType<>))
                                                {
                                                    tConType = NWDNodeConnectionReferenceType.ReferenceQuantityType;
                                                }
                                                else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesArrayType<>))
                                                {
                                                    tConType = NWDNodeConnectionReferenceType.ReferencesArrayType;
                                                }
                                                else
                                                {
                                                    tConType = NWDNodeConnectionReferenceType.ReferencesArrayType;
                                                }

                                                List<NWDNodeCard> tNewCards = sCard.AddPropertyResult(tProp, tConType, tObjects, tButtonAdd);
                                                foreach (NWDNodeCard tNewCard in tNewCards)
                                                {
                                                    tNewCard.Analyze(sCard.ParentDocument);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // don't analyze this classes NWDBasis ...
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
            //return sDocumentWidth;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            GUI.Label(sRect, InternalDescription, EditorStyles.wordWrappedLabel);
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddOnNodePropertyDraw(string sPpropertyName, Rect sRect)
        {
            GUIStyle tBox = new GUIStyle(EditorStyles.helpBox);
            tBox.alignment = TextAnchor.MiddleLeft;
            GUI.Label(sRect, sPpropertyName + " : " + InternalKey, EditorStyles.miniLabel);

            //GUI.Label(sRect, sPpropertyName+ "<"+ClassNamePHP() + "> "+InternalKey, EditorStyles.wordWrappedLabel);
            //GUI.Box(sRect, sPpropertyName + "<" + ClassNamePHP() + "> " + InternalKey, tBox);

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual Color AddOnNodeColor()
        {
            return Color.white;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif