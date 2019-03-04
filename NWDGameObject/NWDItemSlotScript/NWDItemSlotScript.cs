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
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemSlotEvent EventAction;
        //-------------------------------------------------------------------------------------------------------------
        public void PlugAction()
        {
            NWDItemSlot tItemSlot = ItemSlotConnection.GetObject();
            if (tItemSlot != null && EventAction!=null)
            {
                EventAction.Invoke(this);
                PlugInstall();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PlugInstall()
        {
            NWDItemSlot tItemSlot = ItemSlotConnection.GetObject();
            Sprite tSprite = null;
            if (tItemSlot != null)
            {
                NWDUserItemSlot tUserItemSlot = NWDUserItemSlot.FindFirstByIndex(tItemSlot.Reference);
                if (tUserItemSlot!=null)
                {
                    List<NWDItem> tItemList = tUserItemSlot.ItemsUsed.GetObjectsList();
                    if (SlotIndex < tItemList.Count)
                    {
                        NWDItem tItem = tItemList[SlotIndex];
                        tSprite = tItem.PrimaryTexture.ToSprite();
                    }
                }
            }
            SpritePlugged.sprite = tSprite;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            PlugInstall();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
