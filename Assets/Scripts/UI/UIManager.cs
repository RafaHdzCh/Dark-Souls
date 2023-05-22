using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] PlayerInventoryManager playerInventoryManager;
    QuickSlotsUI quickSlotsUI;
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
        quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
    }

    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        equipmentWindowUI.LoadWeaponOnEquipmentSlot(playerInventoryManager);

        quickSlotsUI.UpdateCurrentConsumableIcon(playerInventoryManager.currentConsumableItem);
        quickSlotsUI.UpdateCurrentSpellIcon(playerInventoryManager.currentSpell);


    }

    public void UpdateUI()
    {
        #region Weapon Inventory Slots

        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            if(i < playerInventoryManager.weaponsInventory.Count)
            {
                if(weaponInventorySlots.Length < playerInventoryManager.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>(true);
                }
                weaponInventorySlots[i].AddItem(playerInventoryManager.weaponsInventory[i]);
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
}
