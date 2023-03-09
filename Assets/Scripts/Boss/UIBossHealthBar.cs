using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealthBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bossName;
    [SerializeField] Slider slider;

    private void Start()
    {
        slider.gameObject.SetActive(false);
    }

    public void SetBossName(string name)
    {
        bossName.text = name;
    }

    public void SetUIHealthBarToActive()
    {
        slider.gameObject.SetActive(true);
    }

    public void SetUIHealthBarToInactive()
    {
        slider.gameObject.SetActive(false);
    }

    public void SetBossMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetBossCurrentHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
