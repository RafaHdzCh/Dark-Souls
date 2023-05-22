using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;
    public Image upItemIcon;
    public Image downItemIcon;

    public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
    {
        if (isLeft == false)
        {
            if (weapon.itemIcon != null)
            {
                rightWeaponIcon.sprite = weapon.itemIcon;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
        }
        else
        {
            if (weapon.itemIcon != null)
            {
                leftWeaponIcon.sprite = weapon.itemIcon;
                leftWeaponIcon.enabled = true;
            }
            else
            {
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }
        }
    }

    public void UpdateCurrentSpellIcon(SpellItem spell)
    {
        if(spell.itemIcon != null)
        {
            upItemIcon.sprite = spell.itemIcon;
            upItemIcon.enabled = true;
        }
        else
        {
            upItemIcon.sprite = null;
            upItemIcon.enabled = false;
        }
    }

    public void UpdateCurrentConsumableIcon(ConsumableItem consumableItem)
    {
        if (consumableItem.itemIcon != null)
        {
            downItemIcon.sprite = consumableItem.itemIcon;
            downItemIcon.enabled = true;
        }
        else
        {
            downItemIcon.sprite = null;
            downItemIcon.enabled = false;
        }
    }
}
