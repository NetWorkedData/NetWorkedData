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

#if UNITY_EDITOR
using UnityEditor;

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
        //public static Sprite SpriteOfClass()
        //{
        //    Sprite rSprite = null;
        //    string[] sGUIDs = AssetDatabase.FindAssets(ClassNamePHP()+" t:texture2D");
        //    foreach( string tGUID in sGUIDs)
        //    {
        //        Debug.Log("SpriteOfClass GUID " + tGUID);
        //        string tPath = AssetDatabase.GUIDToAssetPath(tGUID);
        //        Debug.Log("SpriteOfClass " + tPath);
        //        rSprite = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(tGUID), typeof(Sprite)) as Sprite;
        //    }
        //    return rSprite;
        //}
        //-------------------------------------------------------------------------------------------------------------
        static Dictionary<string, Texture2D> kTextureOfClass = new Dictionary<string, Texture2D>();
        //-------------------------------------------------------------------------------------------------------------
        public static Texture2D TextureOfClass()
        {
            Texture2D rTexture = null;
            if (kTextureOfClass.ContainsKey(ClassNamePHP()) == false)
            {
                string[] sGUIDs = AssetDatabase.FindAssets(ClassNamePHP() + " t:texture2D");
                foreach (string tGUID in sGUIDs)
                {
                    Debug.Log("TextureOfClass GUID " + tGUID);
                    string tPath = AssetDatabase.GUIDToAssetPath(tGUID);
                    Debug.Log("TextureOfClass " + tPath);
                    rTexture = AssetDatabase.LoadAssetAtPath(tPath, typeof(Texture2D)) as Texture2D;
                }
                kTextureOfClass[ClassNamePHP()] = rTexture;
            }
            else
            {
                rTexture = kTextureOfClass[ClassNamePHP()];
            }
            return rTexture;
        }
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif