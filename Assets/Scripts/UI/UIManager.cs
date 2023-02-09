using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerInventory playerInventory;
    public EquipmentWindowUI equipmentWindowUI;

    [Header("Serializables")]

    [Header("UI Windows")]
    [SerializeField] GameObject equipmentScreenWindow;
    [SerializeField] GameObject weaponInventoryWindow;
    [SerializeField] GameObject settingsWindow;
    [SerializeField] GameObject selectWindow;
    [SerializeField] public GameObject hudWindow;
    [SerializeField] Image spellIcon;
    [SerializeField] Image consumableItemIcon;

    [Header("Weapon Inventory")]
    [SerializeField] GameObject weaponInventorySlotPrefab;
    [SerializeField] Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    [Header("Equipment Windows Slot Selected")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    private void Awake()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }
    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        equipmentWindowUI.LoadWeaponOnEquipmentSlot(playerInventory);
    }

    private void OnEnable()
    {
        UpdateSpellIcon();
        UpdateConsumableIcon();
    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots

        UpdateSpellIcon();
        UpdateConsumableIcon();

        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if(i < playerInventory.weaponsInventory.Count)
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
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        equipmentScreenWindow.SetActive(false);
        settingsWindow.SetActive(false);
    }

    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
    }

    private void UpdateSpellIcon()
    {
        if (playerInventory.currentSpell == null) return;

        spellIcon.sprite = playerInventory.currentSpell.itemIcon;
        spellIcon.enabled = true;
    }

    private void UpdateConsumableIcon()
    {
        if (playerInventory.currentConsumableItem == null) return;

        consumableItemIcon.sprite = playerInventory.currentConsumableItem.itemIcon;
        consumableItemIcon.enabled = true;
    }
}
