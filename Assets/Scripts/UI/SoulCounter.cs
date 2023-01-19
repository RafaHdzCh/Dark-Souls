using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SoulCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI soulCountTMP;

    public void SetSoulCountText(int soulCount)
    {
        soulCountTMP.text = soulCount.ToString();
    }
}
