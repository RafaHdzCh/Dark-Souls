using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerInventory playerInventory;

    [Header("Default Naked Model")]
    public GameObject nakedHeadModel;
    public GameObject nakedTorsoModel;
    public GameObject nakedHipModel;
    public GameObject nakedLeftLeg;
    public GameObject nakedRightLeg;

    [Header("Equipment Model Changers")]
    HelmetModelChanger helmetModelChanger;
    TorsoModelChanger torsoModelChanger;
    HipModelChanger hipModelChanger;
    LeftLegModelChanger leftLegModelChanger;
    RightLegModelChanger rightLegModelChanger;
    //Hands


    [SerializeField]BlockingCollider blockingCollider;

    private void Awake()
    {
        inputHandler = GetComponentInParent<InputHandler>();
        playerInventory = GetComponentInParent<PlayerInventory>();

        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
        hipModelChanger = GetComponentInChildren<HipModelChanger>();
        leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
        rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();
    }

    private void EquipAllEquipmentModelsOnStart()
    {
        helmetModelChanger.UnequipAllHelmetModels();
        torsoModelChanger.UnequipAllTorsoModels();
        hipModelChanger.UnequipAllHipModels();
        leftLegModelChanger.UnequipAllLegModels();
        rightLegModelChanger.UnequipAllLegModels();

        #region Helmet Equipment

        if (playerInventory.currentHelmentEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmentEquipment.helmetModelName);
        }
        else
        {
            nakedHeadModel.SetActive(true);
        }

        #endregion

        #region Torso Equipment

        if (playerInventory.currentTorsoEquipment != null)
        {
            nakedTorsoModel.SetActive(false);
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
        }
        else
        {
            nakedTorsoModel.SetActive(true);
        }

        #endregion

        #region Hip Equipment

        if (playerInventory.currentLegEquipment != null)
        {
            nakedLeftLeg.SetActive(false);
            nakedRightLeg.SetActive(false);
            nakedHipModel.SetActive(false);

            hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
            leftLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.leftLegName);
            rightLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.rightLegName);
        }
        else
        {
            nakedHipModel.SetActive(true);
            nakedLeftLeg.SetActive(true);
            nakedRightLeg.SetActive(true);
        }

        #endregion
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
