using UnityEngine;
using UnityEngine.UI;

public class PoisonAmountBar : MonoBehaviour
{
    public Slider slider;
    const int maxPoisonBuildUp = 100;

    private void Start()
    {
        slider.maxValue = maxPoisonBuildUp;
        slider.value = maxPoisonBuildUp;
        gameObject.SetActive(false);
    }

    public void SetPoisonAmount(int poisonAmount)
    {
        slider.value = poisonAmount;
    }
}
