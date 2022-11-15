using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler animatorHandler;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    [HideInInspector] public string lastAttack;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        inputHandler = GetComponent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        //Si se activa el combo...
        if(inputHandler.comboFlag)
        {
            //Se desactiva de inmediato el bool que permite hacer un combo y se ejecuta la animacion del segundo ataque segun el tipo de ataque.
            animatorHandler.anim.SetBool(DarkSoulsConsts.CANDOCOMBO, false);
            //Si el ultimo ataque realizado fue el primer ataque ligero...

            if (lastAttack == weapon.OH_Light_Attack_0)
            {
                //Se ejecuta el segundo ataque ligero y se activa el bool isInteracting para evitar realizar otras acciones.
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            }
            //Si el ultimo ataque realizado fue el primer ataque pesado...
            if (lastAttack == weapon.OH_Heavy_Attack_0)
            {
                //Se ejecuta el segundo ataque pesado y se activa el bool isInteracting para evitar realizar otras acciones.
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            }

            if(lastAttack == weapon.TH_Light_Attack_0)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
            }

            if (lastAttack == weapon.TH_Heavy_Attack_0)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_1, true);
            }
        }
    }

    //Realizar un ataque ligero.
    public void HandleLightAttack(WeaponItem weapon)
    {
        //Establece el arma de ataque del slotManager como item de ataque.
        weaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_0, true);
            lastAttack = weapon.TH_Light_Attack_0;
        }
        else
        {
            //Ejecuta su animacion y se activa el bool isInteracting para evitar realizar otras acciones.
            animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_0, true);
            //El ultimo ataque es el ataque realizado.
            lastAttack = weapon.OH_Light_Attack_0;
        }
    }

    //Realizar un ataque pesado.
    public void HandleHeavyAttack(WeaponItem weapon)
    {
        //Establece el arma de ataque del slotManager como item de ataque.
        weaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_0, true);
            lastAttack = weapon.TH_Heavy_Attack_0;
        }
        else
        {
            //Ejecuta su animacion y se activa el bool isInteracting para evitar realizar otras acciones.
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_0, true);
            //El ultimo ataque es el ataque realizado.
            lastAttack = weapon.OH_Heavy_Attack_0;
        }
    }
}
