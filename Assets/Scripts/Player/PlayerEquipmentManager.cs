using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerInventory playerInventory;

    [Header("Default Naked Model")]
    public GameObject nakedHeadModel;
    public GameObject nakedTorsoModel;

    [Header("Equipment Model Changers")]
    HelmetModelChanger helmetModelChanger;
    TorsoModelChanger torsoModelChanger;
    //Legs
    //Hands


    [SerializeField]BlockingCollider blockingCollider;

    private void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        playerInventory = GetComponentInParent<PlayerInventory>();

        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();
    }

    private void EquipAllEquipmentModelsOnStart()
    {
        helmetModelChanger.UnequipAllHelmetModels();
        torsoModelChanger.UnequipAllTorsoModels();

        if(playerInventory.currentHelmentEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmentEquipment.helmetModelName);
        }
        else
        {
            nakedHeadModel.SetActive(true);
        }

        if (playerInventory.currentTorsoEquipment != null)
        {
            nakedTorsoModel.SetActive(false);
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
        }
        else
        {
            nakedTorsoModel.SetActive(true);
        }
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
