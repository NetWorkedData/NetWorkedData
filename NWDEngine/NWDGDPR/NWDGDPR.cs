// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:4
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================


using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// DGPR Linearization of user data in database.
        /// </summary>
        /// <returns>The inearization.</returns>
        /// <param name="sAsssemblyAsCSV">If set to <c>true</c> s asssembly as csv.</param>
        public string DGPRLinearization(bool sAsssemblyAsCSV = true)
        {
            Debug.Log("NWDBasis<K> DGPRLinearization()");
            string rReturn = string.Empty;
            Type tType = ClassType();
            List<string> tPropertiesList = BasisHelper().New_PropertiesOrderArray();

            tPropertiesList.Remove("Integrity");
            tPropertiesList.Remove("Reference");
            tPropertiesList.Remove("ID");
            tPropertiesList.Remove("DM");
            tPropertiesList.Remove("DS");
            tPropertiesList.Remove("ServerHash");
            tPropertiesList.Remove("ServerLog");
            tPropertiesList.Remove("DevSync");
            tPropertiesList.Remove("PreprodSync");
            tPropertiesList.Remove("ProdSync");
            tPropertiesList.Remove("ProdSync");
            tPropertiesList.Remove("InError");

            // todo get the good version of assembly 
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            int tLastWebService = -1;
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in tApp.kWebBuildkDataAssemblyPropertiesList)
            {
                if (tKeyValue.Key <= WebModel && tKeyValue.Key > tLastWebService)
                {
                    if (tKeyValue.Value.ContainsKey(ClassID()))
                    {
                        tPropertiesList = tKeyValue.Value[ClassID()];
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
        public static string DGPRExtract()
        {
            Debug.Log("NWDBasis<K> DGPRExtract()");
            string rExtract = "{\"" + BasisHelper().ClassNamePHP + "\"" + " : [\n\r";
            List<string> tList = new List<string>();
            foreach (K tObject in GetDatas())
            {
                tList.Add("{ \"csv\" : \""+tObject.DGPRLinearization()+"\"}");
            }
            rExtract+= string.Join(",\n\r",tList.ToArray());
            rExtract += "\n\r]\n\r}";
            return rExtract;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDGDPR class is use to extract the informations of user and send it to user to be conform to the EU's GDPR directives. 
    /// </summary>
    public class NWDGDPR
    {
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
            foreach (Type tClassType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
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
            tList.Add("{ \"AnonymousPlayerAccountReference\" : \"" + NWDAccount.CurrentAnonymousReference() + "\"}\n\r}");
            // TODO : Add email/password ?
            // TODO : Add account type ?
            foreach (Type tClassType in tListClasses)
            {
                var tMethodInfo = tClassType.GetMethod("DGPRExtract", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                string rR = "{\"error\" : \"error\"}";
                if (tMethodInfo != null)
                {
                    rR = tMethodInfo.Invoke(null, null) as string;
                }
                tList.Add(rR);
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
            string tEmail = "mailto:?subject=DGPR%20Export&body=" + UnityWebRequest.EscapeURL(rReturn.Replace(" ","%20")).Replace("%20", " ");
            Application.OpenURL(tEmail);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
