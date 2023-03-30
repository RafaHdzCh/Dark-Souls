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
        inputHandler = GetComponentInParent<InputHandler>();
        playerInventory = GetComponentInParent<PlayerInventory>();

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
            nakedUpperLeftArm.SetActive(false);
            nakedUpperRightArm.SetActive(false);
            nakedLowerLeftArm.SetActive(false);
            nakedLowerRightArm.SetActive(false);
            nakedLeftHand.SetActive(false);
            nakedRightHand.SetActive(false);
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
            upperLeftArmModelChanger.EquipModelByName(playerInventory.currentTorsoEquipment.upperLeftArmModelName);
            upperRightArmModelChanger.EquipModelByName(playerInventory.currentTorsoEquipment.upperRightArmModelName);
        }
        else
        {
            nakedTorsoModel.SetActive(true);
            nakedUpperLeftArm.SetActive(true);
            nakedUpperRightArm.SetActive(true);
        }

        #endregion

        #region Hand Equipment

        if(playerInventory.currentHandEquipment != null)
        {
            upperLeftArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.upperLeftArmModelName);
            upperRightArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.upperRightArmModelName);
            lowerLeftArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerLeftArmModelName);
            lowerRightArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerRightArmModelName);
            leftHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.leftHandModelName);
            rightHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.rightHandModelName);
        }
        else
        {
            nakedUpperLeftArm.SetActive(true);
            nakedUpperRightArm.SetActive(true);
            nakedLowerLeftArm.SetActive(true);
            nakedLowerRightArm.SetActive(true);
            nakedLeftHand.SetActive(true);
            nakedRightHand.SetActive(true);
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
