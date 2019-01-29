//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using BasicToolBox;
using System.Globalization;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        public static CultureInfo FormatCountry = CultureInfo.InvariantCulture;
        //-------------------------------------------------------------------------------------------------------------
        public static string FloatFormat = "F5";
        public static string FloatSQLFormat = "5";
        public static string DoubleFormat = "F5";
        public static string DoubleSQLFormat = "5";
        //-------------------------------------------------------------------------------------------------------------
        //// [Obsolete(NWDConstants.K_WILL_BE_REMOVED)] // used in NWD task
        //public const string K_WILL_BE_REMOVED = " WILL BE REMOVED ";
        //// [Obsolete(NWDConstants.K_OBSOLETE)] // used in NWD task
        //public const string K_OBSOLETE = "OBSOLETE";
        //// [Obsolete(NWDConstants.K_USE_REPRESENTATIVE_ITEM)] // used in NWD task
        //public const string K_USE_REPRESENTATIVE_ITEM = " USE REPRESENTATIVE ITEM ! ";
        //-------------------------------------------------------------------------------------------------------------
        //public static string K_DEVELOPMENT = "Development";
        //public static string K_PREPRODUCTION = "PreProduction";
        //public static string K_PRODUCTION = "Production";
        //-------------------------------------------------------------------------------------------------------------
        public static string K_DEVELOPMENT_NAME = "Dev";
        public static string K_PREPRODUCTION_NAME = "Preprod";
        public static string K_PRODUCTION_NAME = "Prod";
        //-------------------------------------------------------------------------------------------------------------
        //public static string K_VERSION_LABEL = "Version";
        //-------------------------------------------------------------------------------------------------------------
        public static string kStandardSeparator = "|";
        public static string kStandardSeparatorSubstitute = "@0#";
        //-------------------------------------------------------------------------------------------------------------
        public static string kFieldNone = "none";
        public static string kFieldEmpty = "empty";
        public static string kFieldNotEmpty = "not empty";
        public static string kFieldSeparatorA = "•";
        public static string kFieldSeparatorB = ":";
        public static string kFieldSeparatorC = "_";
        public static string kFieldSeparatorD = "∆";
        public static string kFieldSeparatorE = "∂";
        public static string kFieldSeparatorASubstitute = "@1#";
        public static string kFieldSeparatorBSubstitute = "@2#";
        public static string kFieldSeparatorCSubstitute = "@3#";
        public static string kFieldSeparatorDSubstitute = "@4#";
        public static string kFieldSeparatorESubstitute = "@5#";
        //-------------------------------------------------------------------------------------------------------------
        static public string kPrefSaltValidKey = "SaltValid";
        static public string kPrefSaltAKey = "SaltA";
        static public string kPrefSaltBKey = "SaltB";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================