//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:42
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDQuest : NWDBasis<NWDQuest>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 240.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect)
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            tStyle.richText = true;

            if (RequiredItems == null)
            {
                RequiredItems = new NWDReferencesConditionalType<NWDItem>();
            }

            if (RequiredItemGroups == null)
            {
                RequiredItemGroups = new NWDReferencesConditionalType<NWDItemGroup>();
            }

            if (RequiredItemsToRemove == null)
            {
                RequiredItemsToRemove = new NWDReferencesQuantityType<NWDItem>();
            }
            string tQuestTitle = Title.GetBaseString();
            string tQuestDescription = Description.GetBaseString();
            string tRequiredItemsDescription = RequiredItems.Description();
            string tRequiredItemGroupsDescription = RequiredItemGroups.Description();
            //string tRequiredItemToRemoveDescription = RequiredItemsToRemove.Description();
            string tText = "" + InternalDescription + "\n\n<b>Title : </b>\n" + tQuestTitle + "\n\n<b>Description : </b>\n"+ tQuestDescription+
                "\n <b>Required Items : </b>\n" + tRequiredItemsDescription + 
                "\n <b>Required Items Groups: </b>\n"+ tRequiredItemGroupsDescription+
                "\n ";
            GUI.Label(sRect, tText, tStyle);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif