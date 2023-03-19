using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerInventory playerInventory;

    [Header("Equipment Model Changers")]
    HelmetModelChanger helmetModelChanger;
    //Body
    //Legs
    //Hands


    [SerializeField]BlockingCollider blockingCollider;

    private void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
    }

    private void Start()
    {
        helmetModelChanger.UnequipAllHelmetModels();

        helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmentEquipment.helmetModelName);
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
