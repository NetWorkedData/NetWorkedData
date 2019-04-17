// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:55
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDItemSlotEvent : UnityEvent<NWDItemSlotScript>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDItemSlotScript : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public Button SlotButton;
        public Image SpritePlugged;
        public NWDItemSlotConnection ItemSlotConnection;
        public int SlotIndex = 0;
        public bool IsUsed;
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemSlotEvent EventAction;
        //-------------------------------------------------------------------------------------------------------------
        public void PlugAction()
        {
            //Debug.Log("NWDItemSlotScript PlugAction()");
            NWDItemSlot tItemSlot = ItemSlotConnection.GetData();
            if (tItemSlot != null && EventAction!=null)
            {
                EventAction.Invoke(this);
                PlugInstall();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PlugInstall()
        {
            //Debug.Log("NWDItemSlotScript PlugInstall()");
            NWDItemSlot tItemSlot = ItemSlotConnection.GetData();
            SpritePlugged.sprite = null;
            if (tItemSlot != null)
            {
                NWDUserItemSlot tUserItemSlot = NWDUserItemSlot.FindFirstByIndex(tItemSlot.Reference);
                if (tUserItemSlot!=null)
                {
                    List<NWDItem> tItemList = tUserItemSlot.ItemsUsed.GetObjectsList();
                    if (SlotIndex < tItemList.Count)
                    {
                        NWDItem tItem = tItemList[SlotIndex];
                        if (tItem != tItemSlot.ItemNone.GetData())
                        {
                            IsUsed = true;
                        }
                        else
                        {
                            IsUsed = false;
                        }
                        tItem.SecondarySprite.ToSpriteAsync(null, delegate (Sprite sInterim, Sprite sResult)
                        {
                            SpritePlugged.sprite = sResult;
                        });
                        tItem.PrimaryTexture.ToTexure2DAsync(null, delegate (Texture2D sInterim, Texture2D sResult)
                        {
                            //SpritePlugged.sprite = sResult;
                        });
                    }
                }
            };
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            //Debug.Log("NWDItemSlotScript Start()");
            PlugInstall();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
