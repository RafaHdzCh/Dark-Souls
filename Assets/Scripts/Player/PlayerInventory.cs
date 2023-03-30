using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    [Header("Current Equipment")]
    public HelmetEquipment currentHelmentEquipment;
    public TorsoEquipment currentTorsoEquipment;
    public LegEquipment currentLegEquipment;
    public HandEquipment currentHandEquipment;

    [Header("Current QuickSlot Items")]
    public SpellItem currentSpell;
    public ConsumableItem currentConsumableItem;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;

    [Header("Unnarmed ScriptableObject")]
    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponInLeftHandSlots = new WeaponItem[1];

    [System.NonSerialized] public int currentRightWeaponIndex;
    [System.NonSerialized] public int currentLeftWeaponIndex;

    public List<WeaponItem> weaponsInventory;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        //Al empezar, se indica que el arma actual (Mano izquierda y derecha) es la que se encuentra en la posicion 0 en la lista de armas.
        currentRightWeaponIndex = 0;
        currentLeftWeaponIndex = 0;
        rightWeapon = weaponInRightHandSlots[0];
        leftWeapon = weaponInLeftHandSlots[0];
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    //Se llama al presionar la Flecha Derecha (Teclado y Pad)
    public void ChangeRightWeapon()
    {
        //Se aumenta el indice en 1.
        currentRightWeaponIndex++;

        //Si el arma actual es la 0 y el arma no es nula...
        if(currentRightWeaponIndex == 0 && weaponInRightHandSlots[0] != null)
        {
            //El arma derecha es el arma en la posicion siguiente (0).
            rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
            //Se carga el arma a la mano del jugador.
            weaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
        }
        //O si la posicion actual es 0 y el arma es nula...
        else if(currentRightWeaponIndex == 0 && weaponInRightHandSlots[0] == null)
        {
            //Se vuelve a aumentar el indice.
            currentRightWeaponIndex++;
        }
        //O si la posicion es 1 y el arma no es nula...
        else if(currentRightWeaponIndex == 1 && weaponInRightHandSlots[1] != null)
        {
            //El arma derecha es el arma en la posicion siguiente (1).
            rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
            //Se carga el arma a la mano del jugador.
            weaponSlotManager.LoadWeaponOnSlot(weaponInRightHandSlots[currentRightWeaponIndex], false);
        }
        //De otra forma...
        else
        {
            //Se vuelve a aumentar el indice.
            currentRightWeaponIndex++;
        }

        //Si la posicion es superior a la cantidad de slots...
        if(currentRightWeaponIndex>weaponInRightHandSlots.Length-1)
        {
            //El indice es -1.
            currentRightWeaponIndex = -1;
            //El arma son las manos.
            rightWeapon = unarmedWeapon;
            //Se activa la mano como arma.
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }
    }

    public void ChangeLeftWeapon()
    {
        //Se aumenta el indice en 1.
        currentLeftWeaponIndex++;

        //Si el arma actual es la 0 y el arma no es nula...
        if (currentLeftWeaponIndex == 0 && weaponInLeftHandSlots[0] != null)
        {
            //El arma derecha es el arma en la posicion siguiente (0).
            leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
            //Se carga el arma a la mano del jugador.
            weaponSlotManager.LoadWeaponOnSlot(weaponInLeftHandSlots[currentLeftWeaponIndex], true);
        }
        //O si la posicion actual es 0 y el arma es nula...
        else if (currentLeftWeaponIndex == 0 && weaponInLeftHandSlots[0] == null)
        {
            //Se aumenta el indice en 1.
            currentLeftWeaponIndex++;
        }
        //O si la posicion es 1 y el arma no es nula...
        else if (currentLeftWeaponIndex == 1 && weaponInLeftHandSlots[1] != null)
        {
            leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
            //Se carga el arma a la mano del jugador.
            weaponSlotManager.LoadWeaponOnSlot(weaponInLeftHandSlots[currentLeftWeaponIndex], true);
        }
        //De otra forma...
        else
        {
            //Se aumenta el indice en 1.
            currentLeftWeaponIndex++;
        }
        //Si la posicion es superior a la cantidad de slots...
        if (currentLeftWeaponIndex > weaponInLeftHandSlots.Length - 1)
        {
            //El indice es -1.
            currentLeftWeaponIndex = -1;
            //El arma son las manos.
            leftWeapon = unarmedWeapon;
            //Se activa la mano como arma.
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
        }
    }
}
