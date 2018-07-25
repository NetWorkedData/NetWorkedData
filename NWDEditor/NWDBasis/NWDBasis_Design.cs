//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        // TODO : MUST MOVE IN NWDTypeInfos
        //-------------------------------------------------------------------------------------------------------------
        static Dictionary<string, Texture2D> kTextureOfClass = new Dictionary<string, Texture2D>();
        //-------------------------------------------------------------------------------------------------------------
        public static Texture2D TextureOfClass()
        {
            string tName = ClassNamePHP();
            Texture2D rTexture = null;
            if (kTextureOfClass.ContainsKey(tName) == false)
            {
                kTextureOfClass.Add(tName, null);
                string[] sGUIDs = AssetDatabase.FindAssets(""+tName + " t:texture2D");
                foreach (string tGUID in sGUIDs)
                {
                    //Debug.Log("TextureOfClass GUID " + tGUID);
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    //Debug.Log("tPathFilename = " + tPathFilename);
                    if ( tPathFilename.Equals( tName))
                    {
                        //Debug.Log("TextureOfClass " + tPath);
                        rTexture = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                    }
                }
                kTextureOfClass[tName] = rTexture;
            }
            else
            {
                rTexture = kTextureOfClass[tName];
            }
            return rTexture;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif