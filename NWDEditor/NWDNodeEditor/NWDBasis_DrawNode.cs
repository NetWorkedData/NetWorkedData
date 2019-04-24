// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:55
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using BasicToolBox;
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
        public override void NodeCardAnalyze(NWDNodeCard sCard)
        {
            //BTBBenchmark.Start();
            sCard.ClassTexture = BasisHelper().TextureOfClass();
            bool tDataAllReadyAnalyze = false;
            foreach (NWDNodeCard tCard in sCard.ParentDocument.AllCardsAnalyzed)
            {
                if (tCard.DataObject == sCard.DataObject)
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
                if (sCard.ParentDocument.AnalyzeStyleClasses[tType.Name] == NWDClasseAnalyseEnum.Analyze ||
                    sCard.ParentDocument.AnalyzeStyleClasses[tType.Name] == NWDClasseAnalyseEnum.Both)
                {
                    foreach (PropertyInfo tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        Type tTypeOfThis = tProp.PropertyType;

                        if (tProp.GetCustomAttributes(typeof(NWDHidden), true).Length > 0
                         || tProp.GetCustomAttributes(typeof(NWDNotVisible), true).Length > 0
                            )
                        {
                            // hidden this property
                        }
                        else
                        {
                            if (tTypeOfThis != null)
                            {
                                if (tTypeOfThis.IsGenericType)
                                {
                                    if (
                                tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)) ||
                                tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple))
                                       )
                                    {
                                        Type tSubType = tTypeOfThis.GetGenericArguments()[0];

                                        if (sCard.ParentDocument.AnalyzeStyleClasses[tType.Name] == NWDClasseAnalyseEnum.Show ||
                    sCard.ParentDocument.AnalyzeStyleClasses[tType.Name] == NWDClasseAnalyseEnum.Both)
                                        {
                                            var tVar = tProp.GetValue(this, null);
                                            if (tVar != null)
                                            {
                                                //object[] tObjects = tMethodInfo.Invoke(tVar, null) as object[];

                                                object[] tObjects = new object[] { null };
                                                if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)))
                                                {
                                                    NWDReferenceSimple tTTVar = tVar as NWDReferenceSimple;
                                                    tObjects = tTTVar.GetEditorDatas();
                                                }
                                                if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                                                {
                                                    NWDReferenceMultiple tTTVar = tVar as NWDReferenceMultiple;
                                                    tObjects = tTTVar.GetEditorDatas();
                                                }
                                                List<NWDNodeCard> tNewCards = sCard.AddPropertyResult(tObjects);
                                                foreach (NWDNodeCard tNewCard in tNewCards)
                                                {
                                                    tNewCard.Analyze(sCard.ParentDocument);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif