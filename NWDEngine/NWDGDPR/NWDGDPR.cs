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
#if NWD_RGPD
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// DGPR Linearization of user data in database.
        /// </summary>
        /// <returns>The inearization.</returns>
        /// <param name="sAsssemblyAsCSV">If set to <c>true</c> s asssembly as csv.</param>
        public override string DGPRLinearization(string sTypeName, bool sAsssemblyAsCSV = true)
        {
            //Debug.Log("DGPRLinearization()");
            string rReturn = string.Empty;
            Type tType = ClassType();
            List<string> tPropertiesList = BasisHelper().PropertiesOrderArray();
            NWDExample sExample = NWDExample.Fictive();
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.Integrity));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.Reference));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.ID));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.DM));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.DS));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.ServerHash));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.ServerLog));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.DevSync));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.PreprodSync));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.ProdSync));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.ProdSync));
            tPropertiesList.Remove(NWDToolbox.PropertyName(() => sExample.InError));

            // todo get the good version of assembly 
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            int tLastWebService = -1;
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in tApp.kWebBuildkDataAssemblyPropertiesList)
            {
                if (tKeyValue.Key <= WebModel && tKeyValue.Key > tLastWebService)
                {
                    if (tKeyValue.Value.ContainsKey(sTypeName))
                    {
                        tPropertiesList = tKeyValue.Value[sTypeName];
                    }
                }
            }
            //Debug.Log("DATA ASSEMBLY  initial count =  " + tPropertiesList.Count.ToString());
            foreach (string tPropertieName in tPropertiesList)
            {
                PropertyInfo tProp = tType.GetProperty(tPropertieName);
                if (tProp != null)
                {
                    Type tTypeOfThis = tProp.PropertyType;

                    // Debug.Log("this prop "+tProp.Name+" is type : " + tTypeOfThis.Name );

                    string tValueString = string.Empty;

                    object tValue = tProp.GetValue(this, null);
                    if (tValue == null)
                    {
                        tValue = string.Empty;
                    }
                    tValueString = tValue.ToString();
                    if (tTypeOfThis.IsEnum)
                    {
                        //Debug.Log("this prop  " + tTypeOfThis.Name + " is an enum");
                        int tInt = (int)tValue;
                        tValueString = tInt.ToString();
                    }
                    if (tTypeOfThis == typeof(bool))
                    {
                        //Debug.Log ("REFERENCE " + Reference + " AC + " + AC + " : " + tValueString);
                        if (tValueString == "False")
                        {
                            tValueString = "0";
                        }
                        else
                        {
                            tValueString = "1";
                        }
                    }
                    if (sAsssemblyAsCSV == true)
                    {
                        rReturn += NWDToolbox.TextCSVProtect(tValueString) + NWDConstants.kStandardSeparator;
                    }
                    else
                    {
                        rReturn += NWDToolbox.TextCSVProtect(tValueString);
                    }
                }
            }
            if (sAsssemblyAsCSV == true)
            {
                rReturn = Reference + NWDConstants.kStandardSeparator +
                DM + NWDConstants.kStandardSeparator +
                DS + NWDConstants.kStandardSeparator +
                //DevSync + NWDConstants.kStandardSeparator +
                //PreprodSync + NWDConstants.kStandardSeparator +
                //ProdSync + NWDConstants.kStandardSeparator +
                // Todo Add WebServiceVersion ?
                //WebServiceVersion + NWDConstants.kStandardSeparator +
                rReturn
                                                  // + Integrity
                                                  ;
                //Debug.Log("DATA ASSEMBLY  CSV count =  " + (tPropertiesList.Count+7).ToString());
            }
            else
            {
                rReturn = Reference +
                DM +
                rReturn;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// DGPR extraction in string.
        /// </summary>
        /// <returns>The xtract.</returns>
        //public static string DGPRExtract()
        //{
        //    Debug.Log("NWDBasis DGPRExtract()");
        //    string rExtract = "{\"" + BasisHelper().ClassNamePHP + "\"" + " : [\n\r";
        //    List<string> tList = new List<string>();
        //    foreach (K tObject in GetReachableDatas())
        //    {
        //        tList.Add("{ \"csv\" : \"" + tObject.DGPRLinearization() + "\"}");
        //    }
        //    rExtract += string.Join(",\n\r", tList.ToArray());
        //    rExtract += "\n\r]\n\r}";
        //    return rExtract;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGDPR class is use to extract the informations of user and send it to user to be conform to the EU's GDPR directives. 
    /// </summary>
    public class NWDGDPR
    {
        //-------------------------------------------------------------------------------------------------------------
        static public void Log(string sInformations)
        {
            Debug.Log("<color=red>GDPR informations</color>: " + sInformations);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Extract the specified type.
        /// </summary>
        /// <returns>The extract.</returns>
        /// <param name="tListAddon">T list addon.</param>
        public static string Extract(List<Type> tListAddon = null)
        {
            Debug.Log("NWDGDPR Extract()");
            List<string> tList = new List<string>();
            // the list to use
            List<Type> tListClasses = new List<Type>();
            // add special object inside the list
            List<Type> tListAddInternal = new List<Type>();
            tListAddInternal.Add(typeof(NWDConsent));
            // Add account dependance class
            foreach (Type tClassType in NWDDataManager.SharedInstance().ClassAccountDependentList)
            {
                if (tListClasses.Contains(tClassType) == false)
                {
                    tListClasses.Add(tClassType);
                }
            }
            // Add special class for understand the export
            foreach (Type tClassType in tListAddInternal)
            {
                if (tListClasses.Contains(tClassType) == false)
                {
                    tListClasses.Add(tClassType);
                }
            }
            // Add developper class 
            if (tListAddon != null)
            {
                foreach (Type tClassType in tListAddon)
                {
                    if (tListClasses.Contains(tClassType) == false)
                    {
                        tListClasses.Add(tClassType);
                    }
                }
            }
            // ok prepare extract
            string rExtract = "[\n\r";
            // TODO : Add account informations ?
            tList.Add("{ \"PlayerAccountReference\" : \"" + NWDAccount.CurrentReference() + "\"}\n\r}");
            //tList.Add("{ \"AnonymousPlayerAccountReference\" : \"" + NWDAccount.CurrentAnonymousReference() + "\"}\n\r}");
            // TODO : Add email/password ?
            // TODO : Add account type ?
            foreach (Type tClassType in tListClasses)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tClassType);
                tHelper.DGPRExtract();
                tList.Add(tHelper.DGPRExtract());
            }
            rExtract += string.Join(",\n\r", tList.ToArray());
            rExtract += "\n\r]";
            return rExtract;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete account and datas.
        /// </summary>
        public static void DeleteAccountAndDatas()
        {
            //TODO : Delete and Sync ? but how sync account delete?
            Debug.Log("NWDGDPR DeleteAccountAndDatas()");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Extract and save by send in email.
        /// </summary>
        /// <returns>The and save.</returns>
        /// <param name="tListAddon">T list addon.</param>
        public static string ExtractAndSave(List<Type> tListAddon = null)
        {
            //Debug.Log("NWDGDPR ExtractAndSave()");
            string rReturn = Extract(tListAddon);
            string tEmail = "mailto:?subject=DGPR%20Export&body=" + UnityWebRequest.EscapeURL(rReturn.Replace(" ", "%20")).Replace("%20", " ");
            Application.OpenURL(tEmail);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
