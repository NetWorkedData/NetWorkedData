
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public string DGPRLinearization(bool sAsssemblyAsCSV = true)
        {
            Debug.Log("NWDBasis<K> DGPRLinearization()");
            string rReturn = "";
            Type tType = ClassType();
            List<string> tPropertiesList = DataAssemblyPropertiesList();

            // todo get the good version of assembly 
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            int tLastWebService = -1;
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in tApp.kWebBuildkDataAssemblyPropertiesList)
            {
                if (tKeyValue.Key <= WebServiceVersion && tKeyValue.Key > tLastWebService)
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

                    string tValueString = "";

                    object tValue = tProp.GetValue(this, null);
                    if (tValue == null)
                    {
                        tValue = "";
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
        public static string DGPRExtract()
        {
            Debug.Log("NWDBasis<K> DGPRExtract()");
            string rExtract = "{\"" + ClassNamePHP() + "\"" + " : [\n\r";
            List<string> tList = new List<string>();
            foreach (K tObject in GetAllObjects())
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
    public class NWDGDPR
    {
        //-------------------------------------------------------------------------------------------------------------
        // Use this for extract GDPR
        public static string Extract(List<Type> tListAddon = null)
        {
            Debug.Log("NWDGDPR Extract()");
            List<string> tList = new List<string>();
            // the list to use
            List<Type> tListClasses = new List<Type>();
            // add special object inside the list
            List<Type> tListAddInternal = new List<Type>();
            tListAddInternal.Add(typeof(NWDAppConsent));
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
            tList.Add("{ \"PlayerAccountReference\" : \"" + NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference + "\"}\n\r}");
            tList.Add("{ \"AnonymousPlayerAccountReference\" : \"" + NWDAppConfiguration.SharedInstance().SelectedEnvironment().AnonymousPlayerAccountReference + "\"}\n\r}");
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
        // Use this for delete account GDPR
        public static void DeleteAccountAndDatas()
        {
            Debug.Log("NWDGDPR DeleteAccountAndDatas()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string ExtractAndSave(List<Type> tListAddon = null)
        {
            Debug.Log("NWDGDPR ExtractAndSave()");
            string rReturn = Extract(tListAddon);
            string tEmail = "mailto:?subject=DGPR%20Export&body=" + WWW.EscapeURL(rReturn.Replace(" ","%20")).Replace("%20", " ");
            Application.OpenURL(tEmail);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
