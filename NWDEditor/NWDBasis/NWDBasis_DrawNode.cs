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

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;
using System.IO;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public void GetAllDependByLevel(int sActual, Dictionary<int, List<object>> sReturn, List<object> sReturnObjects)
        {
            if (sReturn.ContainsKey(sActual))
            {
                sReturn.Remove(sActual);
            }
            List<object> tObjects = GetAllDepend();
            sReturn.Add(sActual, tObjects);
            List<object> tNextLevel = new List<object>();
            foreach (object tObject in tObjects)
            {
                if (sReturnObjects.Contains(tObject) == false)
                {
                    tNextLevel.Add(tObject);
                    sReturnObjects.Add(tObject);
                }
            }
            // I need to insert now the level afetr this 
            sActual++;
            foreach (object tObject in tNextLevel)
            {
                Type tTypeOfThis = tObject.GetType();
                var tMethodInfo = tTypeOfThis.GetMethod("GetAllDependByLevel", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(tObject, new object[] { sActual, sReturn, sReturnObjects});
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<object> GetAllDepend()
        {
            List<object> rReturn = new List<object>();
            Type tType = ClassType();
            foreach (PropertyInfo tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type tTypeOfThis = tProp.PropertyType;
                if (tTypeOfThis != null)
                {
                    if (tTypeOfThis.IsGenericType)
                    {
                        if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>)
                            || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesListType<>)
                            || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesQuantityType<>)
                            || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesArrayType<>))
                        {
                            //Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            var tMethodInfo = tTypeOfThis.GetMethod("GetObjects", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                            if (tMethodInfo != null)
                            {
                                object[] tObjects = tMethodInfo.Invoke(tProp.GetValue(this, null), null) as object[];
                                foreach (object tObject in tObjects)
                                {
                                    if (tObject != null)
                                    {
                                        rReturn.Add(tObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawNodeInformation(Vector2 sStart)
        {
            GUI.Label(new Rect(sStart.x, sStart.y, 100, 100), ClassNamePHP());
            GUI.Label(new Rect(sStart.x, sStart.y+10, 100, 100), Reference);
            GUI.Label(new Rect(sStart.x, sStart.y + 20, 100, 100), InternalKey);
            GUI.Label(new Rect(sStart.x, sStart.y + 30, 100, 100), InternalDescription);

            //GUI.Label(new Rect(sStart.x, sStart.y+30, 200, 200), "Level = " + tLevels.Count.ToString("0000"));
            //GUI.Label(new Rect(sStart.x, sStart.y+40, 200, 200), "connexion = " + tReturnObjects.Count.ToString("0000"));
        }
        //-------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------
        public void DrawNode()
        {

            //Dictionary<object, Rect> kObjectsAndRect = new Dictionary<object, Rect>();
            //Dictionary<object, int> kObjectsAndLevel = new Dictionary<object, int>();
            //List<object> kObjects = new List<object>();
            Dictionary<int, List<object>> tLevels = new Dictionary<int, List<object>>();
            List<object> tReturnObjects = new List<object>();

            List<object> tInitial = new List<object>();
            tInitial.Add(this);
            tLevels.Add(0,tInitial);

            GetAllDependByLevel(1, tLevels, tReturnObjects);
            float tWidth = 150.0F;
            float tHeight = 150.0F;
            foreach (KeyValuePair<int, List<object>> tEntry in tLevels)
            {
                int tX = tEntry.Key+1;
                int tY = 1;
                foreach (object tObject in tEntry.Value)
                {
                    tY++;
                    Type tTypeOfThis = tObject.GetType();
                    var tMethodInfo = tTypeOfThis.GetMethod("DrawNodeInformation", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        float tXa = tX * tWidth;
                        float tYa = tY * tHeight;
                        tMethodInfo.Invoke(tObject, new object[] { new Vector2(tXa,tYa) });
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================