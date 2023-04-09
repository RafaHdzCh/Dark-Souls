using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerInventoryManager playerInventoryManager;

    [Header("Default Naked Model")]
    public GameObject nakedHeadModel;
    public GameObject nakedTorsoModel;
    public GameObject nakedHipModel;
    public GameObject nakedLeftLeg;
    public GameObject nakedRightLeg;
    public GameObject nakedUpperRightArm;
    public GameObject nakedUpperLeftArm;
    public GameObject nakedLowerRightArm;
    public GameObject nakedLowerLeftArm;
    public GameObject nakedLeftHand;
    public GameObject nakedRightHand;

    [Header("Equipment Model Changers")]
    HelmetModelChanger helmetModelChanger;
    TorsoModelChanger torsoModelChanger;
    HipModelChanger hipModelChanger;
    LeftLegModelChanger leftLegModelChanger;
    RightLegModelChanger rightLegModelChanger;

    UpperLeftArmModelChanger upperLeftArmModelChanger;
    UpperRightArmModelChanger upperRightArmModelChanger;
    LowerLeftArmModelChanger lowerLeftArmModelChanger;
    LowerRightArmModelChanger lowerRightArmModelChanger;
    LeftHandModelChanger leftHandModelChanger;
    RightHandModelChanger rightHandModelChanger;

    [SerializeField]BlockingCollider blockingCollider;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();

        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
        hipModelChanger = GetComponentInChildren<HipModelChanger>();
        leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
        rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();

        upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
        upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
        lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
        lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
        leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
        rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
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
        upperLeftArmModelChanger.UnequipAllModels();
        upperRightArmModelChanger.UnequipAllModels();
        lowerRightArmModelChanger.UnequipAllModels();
        lowerLeftArmModelChanger.UnequipAllModels();
        leftHandModelChanger.UnequipAllModels();
        rightHandModelChanger.UnequipAllModels();

        #region Helmet Equipment

        if (playerInventoryManager.currentHelmentEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmentEquipment.helmetModelName);
        }
        else
        {
            nakedHeadModel.SetActive(true);
        }

        #endregion

        #region Torso Equipment

        if (playerInventoryManager.currentTorsoEquipment != null)
        {
            nakedTorsoModel.SetActive(false);
            nakedUpperLeftArm.SetActive(false);
            nakedUpperRightArm.SetActive(false);
            nakedLowerLeftArm.SetActive(false);
            nakedLowerRightArm.SetActive(false);
            nakedLeftHand.SetActive(false);
            nakedRightHand.SetActive(false);
            torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
            upperLeftArmModelChanger.EquipModelByName(playerInventoryManager.currentTorsoEquipment.upperLeftArmModelName);
            upperRightArmModelChanger.EquipModelByName(playerInventoryManager.currentTorsoEquipment.upperRightArmModelName);
        }
        else
        {
            nakedTorsoModel.SetActive(true);
            nakedUpperLeftArm.SetActive(true);
            nakedUpperRightArm.SetActive(true);
        }

        #endregion

        #region Hand Equipment

        if(playerInventoryManager.currentHandEquipment != null)
        {
            lowerLeftArmModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
            lowerRightArmModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
            leftHandModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.leftHandModelName);
            rightHandModelChanger.EquipModelByName(playerInventoryManager.currentHandEquipment.rightHandModelName);
        }
        else
        {
            nakedLowerLeftArm.SetActive(true);
            nakedLowerRightArm.SetActive(true);
            nakedLeftHand.SetActive(true);
            nakedRightHand.SetActive(true);
        }

        #endregion

        #region Hip Equipment

        if (playerInventoryManager.currentLegEquipment != null)
        {
            nakedLeftLeg.SetActive(false);
            nakedRightLeg.SetActive(false);
            nakedHipModel.SetActive(false);

            hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
            leftLegModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.leftLegName);
            rightLegModelChanger.EquipLegModelByName(playerInventoryManager.currentLegEquipment.rightLegName);
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
            blockingCollider.SetColliderDamageAbsortion(playerInventoryManager.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsortion(playerInventoryManager.leftWeapon);
        }
        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
