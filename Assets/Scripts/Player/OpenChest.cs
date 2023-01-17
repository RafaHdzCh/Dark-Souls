using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : Interactable
{
    Animator animator;
    [SerializeField] Transform playerStandingPosition;
    [SerializeField] GameObject itemSpawner;
    [SerializeField] WeaponItem itemInChest;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(PlayerManager playerManager)
    { 
        //Rotate towards the chest
        playerManager.OpenChestInteraction(playerStandingPosition);

        Vector3 rotationDirection = transform.position - playerManager.transform.position;
        rotationDirection.y = 0;
        rotationDirection.Normalize();

        Quaternion tr = Quaternion.LookRotation(rotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;

        //Lock his transform infront of the chest

        //Open the chest
        animator.Play(DarkSoulsConsts.CHESTOPEN);
        StartCoroutine(nameof(SpawnItemInChest));
        //Spawn an item inside the chest
        WeaponPickUp weaponPickUp = itemSpawner.GetComponent<WeaponPickUp>();

        if(weaponPickUp != null)
        {
            weaponPickUp.weapon = itemInChest;
        }
    }

    private IEnumerator SpawnItemInChest()
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(itemSpawner, transform, false);

        Destroy(this);
    }
}
