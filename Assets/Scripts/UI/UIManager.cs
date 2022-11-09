using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    PlayerInventory playerInventory;
    EquipmentWindowUI equipmentWindowUI;

    [Header("Serializables")]

    [Header("UI Windows")]
    [SerializeField] GameObject selectWindow;
    [SerializeField] public GameObject hudWindow;
    [SerializeField] GameObject weaponInventoryWindow;

    [Header("Weapon Inventory")]
    [SerializeField] GameObject weaponInventorySlotPrefab;
    [SerializeField] Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    private void Awake()
    {
        equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
        playerInventory = FindObjectOfType<PlayerInventory>();
    }
    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        print(playerInventory);
        equipmentWindowUI.LoadWeaponOnEquipmentSlot(playerInventory);
    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots

        for(int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if(i<playerInventory.weaponsInventory.Count)
            {
                if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>(true);
                }
                weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }
        }

        #endregion
    }

    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
        selectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        weaponInventoryWindow.SetActive(false);

    }

}
