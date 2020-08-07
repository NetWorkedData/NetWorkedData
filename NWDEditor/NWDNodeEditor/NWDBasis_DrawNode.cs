//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void NodeCardAnalyze(NWDNodeCard sCard)
        {
            //NWDBenchmark.Start();
            sCard.ClassTexture = BasisHelper().TextureOfClass();
            bool tDataAlReadyAnalyze = false;
            foreach (NWDNodeCard tCard in sCard.ParentDocument.AllCardsAnalyzed)
            {
                if (tCard.DataObject == sCard.DataObject)
                {
                    tDataAlReadyAnalyze = true;
                    break;
                }
            }
            // data is not in a preview card
            if (tDataAlReadyAnalyze == false)
            {
                // I add this card
                sCard.ParentDocument.AllCardsAnalyzed.Add(sCard);
                // I analyze this card and its properties (only the reference properties)
                Type tType = ClassType();
                if (sCard.ParentDocument.AnalyzeStyleClasses[tType.Name] == NWDClasseAnalyseEnum.Analyze)
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

                                        sCard.ParentDocument.ClassesUsed.Add(tSubType.Name);

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
                   //                         if (sCard.ParentDocument.AnalyzeStyleClasses[tSubType.Name] == NWDClasseAnalyseEnum.Show ||
                   //sCard.ParentDocument.AnalyzeStyleClasses[tSubType.Name] == NWDClasseAnalyseEnum.Analyze)
                                            {
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif