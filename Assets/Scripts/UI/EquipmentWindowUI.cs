using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    [NonSerialized] public bool rightHandSlot01Selected;
    [NonSerialized] public bool rightHandSlot02Selected;
    [NonSerialized] public bool leftHandSlot01Selected;
    [NonSerialized] public bool leftHandSlot02Selected;

    [SerializeField] HandEquipmentSlotUI[] handEquipmentSlotUI;


    public void LoadWeaponOnEquipmentSlot(PlayerInventory playerInventory)
    {
        for(int i = 0; i < handEquipmentSlotUI.Length; i++)
        {
            if (handEquipmentSlotUI[i].rightHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightHandSlots[0]);
            }
            else if(handEquipmentSlotUI[i].rightHandSlot02)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightHandSlots[1]);
            }
            else if (handEquipmentSlotUI[i].leftHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftHandSlots[0]);
            }
            else
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftHandSlots[1]);
            }
        }
    }
    public void SelectRightHandSlot01()
    {
        rightHandSlot01Selected = true;
    }
    public void SelectRightHandSlot02()
    {
        rightHandSlot02Selected = true;
    }
    public void SelectLeftHandSlot01()
    {
        leftHandSlot01Selected = true;
    }
    public void SelectLeftHandSlot02()
    {
        leftHandSlot02Selected = true;
    }
}
