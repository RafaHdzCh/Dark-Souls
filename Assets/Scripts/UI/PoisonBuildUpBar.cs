using UnityEngine;
using UnityEngine.UI;

public class PoisonBuildUpBar : MonoBehaviour
{
    public Slider slider;
    const int maxPoisonBuildUp = 100;

    private void Start()
    {
        slider.maxValue = maxPoisonBuildUp;
        slider.value = 0;
        gameObject.SetActive(false);
    }

    public void SetPoisonBuildUpAmount(int currentPoisonBuildUp)
    {
        slider.value = currentPoisonBuildUp;
    }
}