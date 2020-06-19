

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#if UNITY_MENU_IDEMOBI
//using UnityMenuIdemobi;
//#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWE constants. This file containt all constants of this package
	/// </summary>
	public class NWEConstants
	{
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The k MEN u BAS.
        /// </summary>
        //#if UNITY_MENU_IDEMOBI
        //public const string K_MENU_BASE = UMIConstants.K_MENU_IDEMOBI+"/Basic Toolbox";
        //#else
        //-------------------------------------------------------------------------------------------------------------
        public const string K_EMPTY_STRING = "";
        public const string K_MINUS = "-";
        public const string K_HASHTAG = "#";
        public const string K_A = "A";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_MENU_BASE = "Window/Basic Toolbox";
		//#endif
		//-------------------------------------------------------------------------------------------------------------
		public const string K_MENU_IDEMOBI = 						K_MENU_BASE+"/Developed by ideMobi©";
		// Idemobi alert Strings
		public const string K_ALERT_IDEMOBI_TITLE = 				"BasicToolBox";
		public const string K_ALERT_IDEMOBI_MESSAGE = 				"BasicToolBox is an idéMobi module to help your game's development.";
		public const string K_ALERT_IDEMOBI_OK = 					"Thanks!";
		public const string K_ALERT_IDEMOBI_SEE_DOC = 				"See online docs";
		public const string K_ALERT_IDEMOBI_DOC_HTTP = 				"http://www.idemobi.com/basictoolbox";
		//-------------------------------------------------------------------------------------------------------------
		public const string DEBUG_CHOOSER_TITLE = "Debug configurations";
		//-------------------------------------------------------------------------------------------------------------
		public const string DEBUG_CONFIGURATION_MENU = K_MENU_BASE+"Debug configurations";
		//-------------
		public const string DEBUG_CONFIGURATION_OVERLAY_LOG_LIMIT_TITLE = "Log number show in overlay in player mode";
		public const string DEBUG_CONFIGURATION_OVERLAY_LOG_LIMIT ="Limit to";
		//-------------
		public const string DEBUG_CONFIGURATION_LEVEL_TITLE = "Level activation";
		//-------------
		public const string DEBUG_CHOOSER_GROUP_LOG_TITLE = "Log type";
		public const string DEBUG_CONFIGURATION_VERBOSE_TITLE = "Verbose Log (0)";
		public const string DEBUG_CONFIGURATION_LEVEL_OPTIONAL_TITLE = "Optional Log (1)";
		public const string DEBUG_CONFIGURATION_LEVEL_NORMAL_TITLE = "Normal Log (2)";
		//-------------
		public const string DEBUG_CHOOSER_GROUP_WARNING_TITLE = "Warning type";
		public const string DEBUG_CONFIGURATION_WARNING_TITLE = "Warning Log (3)";
		public const string DEBUG_CONFIGURATION_ASSERT_TITLE = "Assert Log (?)";
		//-------------
		public const string DEBUG_CHOOSER_GROUP_ERROR_TITLE = "Error type";
		public const string DEBUG_CONFIGURATION_ERROR_TITLE = "Error Log (4)";
		public const string DEBUG_CONFIGURATION_ALERT_TITLE = "Alert Log (5)";
		public const string DEBUG_CONFIGURATION_EXCEPTION_TITLE = "Exception Log (?)";
		//-------------
		public const string DEBUG_CONFIGURATION_MODE_TITLE = "Mode preconfigured";
		//-------------
		public const string DEBUG_CONFIGURATION_MODE_DEVELOPMENT = "Development";
		public const string DEBUG_CONFIGURATION_MODE_TEST = "Test";
		public const string DEBUG_CONFIGURATION_MODE_RELEASE = "Release";
		public const string DEBUG_CONFIGURATION_MODE_DISTRIBUTION = "Distribution";
		//-------------
		public const string DEBUG_CONFIGURATION_LEVEL_TEST = "Log tests";
		//-------------
		public const string DEBUG_CONFIGURATION_VERBOSE_TEST = "Verbose test";
		public const string DEBUG_CONFIGURATION_LEVEL_OPTIONAL_TEST = "Optional test";
		public const string DEBUG_CONFIGURATION_LEVEL_NORMAL_TEST = "Normal test";
		public const string DEBUG_CONFIGURATION_WARNING_TEST = "Warning test";
		public const string DEBUG_CONFIGURATION_ERROR_TEST = "Error test";
		public const string DEBUG_CONFIGURATION_ALERT_TEST = "Alert test";
		public const string DEBUG_CONFIGURATION_EXCEPTION_TEST = "Exception test";
		public const string DEBUG_CONFIGURATION_ASSERT_TEST = "Assert test";
		public const string DEBUG_CONFIGURATION_LEVEL_PARAM = "Formats Parameters";
		public const string DEBUG_CONFIGURATION_ALL_TEST = "Test all cases";
		//-------------
		public const string DEBUG_CONFIGURATION_RESET_TITLE = "Reset to factory";
		public const string DEBUG_CONFIGURATION_RESET = "Reset all parameters to factory's settings";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_URL_SCHEME_NOTIFICATION = "K_URL_SCHEME_NOTIFICATION_jhf56v4";
        //-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================