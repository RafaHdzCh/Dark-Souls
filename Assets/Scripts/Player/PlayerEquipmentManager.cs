using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerInventory playerInventory;
    [SerializeField]BlockingCollider blockingCollider;

    private void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        playerInventory = GetComponentInParent<PlayerInventory>();
    }

    public void OpenBlockingCollider()
    {
        if(inputHandler.twoHandFlag)
        {
            blockingCollider.SetColliderDamageAbsortion(playerInventory.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsortion(playerInventory.leftWeapon);
        }
        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
