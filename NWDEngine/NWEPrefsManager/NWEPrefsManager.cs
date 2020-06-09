

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWE Prefs Manager. This class is an override of EditorPrefs and PlayerPrefs.
    /// </summary>
	public class NWEPrefsManager
    {
        //-------------------------------------------------------------------------------------------------------------
        private static NWEPrefsManager kSharedInstance = null;
        private static string kFirstLaunch = "IDEPrefsManager_kFirstLaunch";
        //-------------------------------------------------------------------------------------------------------------
        public static NWEPrefsManager ShareInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = new NWEPrefsManager();
            }

            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public bool HasKey(string sKey)
        {
            return NWDEditorPrefs.HasKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool isFirstLaunch()
        {
            if (NWDEditorPrefs.HasKey(kFirstLaunch))
            {
                return false;
            }

            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void setFirstLaunch()
        {
            NWDEditorPrefs.SetInt(kFirstLaunch, 1);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void set(string sKey, float sValue)
        {
            NWDEditorPrefs.SetFloat(sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void set(string sKey, int sValue)
        {
            NWDEditorPrefs.SetInt(sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void set(string sKey, string sValue)
        {
            NWDEditorPrefs.SetString(sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void set(string sKey, bool sValue)
        {
            if (sValue)
            {
                set(sKey, 1);
            }
            else
            {
                set(sKey, 0);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public float getFloat(string sKey, float sDefault = 0.0F)
        {
            if (NWDEditorPrefs.HasKey(sKey))
            {
                return NWDEditorPrefs.GetFloat(sKey, -1);
            }

            return sDefault;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int getInt(string sKey, int sDefault = 0)
        {
            if (NWDEditorPrefs.HasKey(sKey))
            {
                return NWDEditorPrefs.GetInt(sKey, -1);
            }

            return sDefault;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string getString(string sKey, string sDefault = NWEConstants.K_EMPTY_STRING)
        {
            if (NWDEditorPrefs.HasKey(sKey))
            {
                return NWDEditorPrefs.GetString(sKey, string.Empty);
            }

            return sDefault;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool getBool(string sKey, bool sDefault = false)
        {
            if (NWDEditorPrefs.HasKey(sKey))
            {
                int value = getInt(sKey);
                if (value == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return sDefault;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool deleteKey(string sKey)
        {
            if (NWDEditorPrefs.HasKey(sKey))
            {
                NWDEditorPrefs.DeleteKey(sKey);
                return true;
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Save()
        {
            // save not used in Editor mode
        }
        //-------------------------------------------------------------------------------------------------------------
#else
		public bool isFirstLaunch ()
		{
			if (PlayerPrefs.HasKey (kFirstLaunch)) {
				return false;
			}

			return true;
		}
        //-------------------------------------------------------------------------------------------------------------
		public void setFirstLaunch ()
		{
			PlayerPrefs.SetInt (kFirstLaunch, 1);
            Save();
		}
        //-------------------------------------------------------------------------------------------------------------
		public void set (string sKey, float sValue)
		{
			PlayerPrefs.SetFloat (sKey, sValue);
            Save();
		}
        //-------------------------------------------------------------------------------------------------------------
		public void set (string sKey, int sValue)
		{
			PlayerPrefs.SetInt (sKey, sValue);
            Save();
		}
        //-------------------------------------------------------------------------------------------------------------
		public void set (string sKey, string sValue)
		{
			PlayerPrefs.SetString (sKey, sValue);
            Save();
		}
        //-------------------------------------------------------------------------------------------------------------
		public void set (string sKey, bool sValue)
		{
			if (sValue) {
				set (sKey, 1);
			} else {
				set (sKey, 0);
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		public float getFloat (string sKey, float sDefault = 0f)
		{
			if (PlayerPrefs.HasKey (sKey)) {
				return PlayerPrefs.GetFloat (sKey, -1);
            }

            return sDefault;
		}
        //-------------------------------------------------------------------------------------------------------------
		public int getInt (string sKey, int sDefault = 0)
		{
			if (PlayerPrefs.HasKey (sKey)) {
				return PlayerPrefs.GetInt (sKey, -1);
            }

            return sDefault;
		}
        //-------------------------------------------------------------------------------------------------------------
		public string getString (string sKey, string sDefault = "")
		{
			if (PlayerPrefs.HasKey (sKey)) {
				return PlayerPrefs.GetString(sKey, "");
            }

            return sDefault;
		}
        //-------------------------------------------------------------------------------------------------------------
		public bool getBool (string sKey, bool sDefault=false)
		{
			if (PlayerPrefs.HasKey (sKey)) {
				int value = getInt (sKey);
				if (value == 1) {
					return true;
				} else {
					return false;
				}
			}
		    return sDefault;
		}
        //-------------------------------------------------------------------------------------------------------------
		public bool deleteKey (string sKey)
		{
			if (PlayerPrefs.HasKey (sKey)) {
				PlayerPrefs.DeleteKey (sKey);
				return true;
			}

			return false;
		}
        //-------------------------------------------------------------------------------------------------------------
		public void deleteAll ()
		{
			PlayerPrefs.DeleteAll ();
		}
        //-------------------------------------------------------------------------------------------------------------
		void Save()
		{
			PlayerPrefs.Save ();
		}
        //-------------------------------------------------------------------------------------------------------------
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================