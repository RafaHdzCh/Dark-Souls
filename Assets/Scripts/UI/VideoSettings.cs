using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VideoSettings : MonoBehaviour
{
    List<int> playerWidths = new List<int>();
    List<int> playerHeights = new List<int>();

    private int resolutionIndex;
    private int fullscreenIndex;
    private int vsyncIndex;
    private int qualityIndex;
    private int shadowQualityIndex;

    [Header("Text references")]
    [SerializeField] TextMeshProUGUI resolutionTMP = null;
    [SerializeField] TextMeshProUGUI fullscreenTMP = null;
    [SerializeField] TextMeshProUGUI vsyncTMP = null;
    [SerializeField] TextMeshProUGUI qualityTMP = null;
    [SerializeField] TextMeshProUGUI shadowQualityTMP = null;

    private int _width;
    private int _height;
    private int _quality;

    private void OnEnable()
    {
        EnableResolutionOptions();
        LoadSettings();
    }
    private void OnDisable()
    {
        playerWidths.Clear();
        playerHeights.Clear();
    }
    private void LoadSettings()
    {
        if(PlayerPrefs.GetInt(DarkSoulsConsts.HASSAVEDSETTINGS) == 0 )
        {
            SetDefaultSettings();
        }
        else
        {
            resolutionIndex     = PlayerPrefs.GetInt(DarkSoulsConsts.RESOLUTIONINDEX);
            fullscreenIndex     = PlayerPrefs.GetInt(DarkSoulsConsts.FULLSCREENINDEX);
            vsyncIndex          = PlayerPrefs.GetInt(DarkSoulsConsts.VSYNCINDEX);
            qualityIndex        = PlayerPrefs.GetInt(DarkSoulsConsts.QUALITYINDEX);
            shadowQualityIndex  = PlayerPrefs.GetInt(DarkSoulsConsts.SHADOWQUALITYINDEX);

            resolutionTMP.text      = PlayerPrefs.GetString(DarkSoulsConsts.RESOLUTION);
            fullscreenTMP.text      = PlayerPrefs.GetString(DarkSoulsConsts.FULLSCREEN);
            vsyncTMP.text           = PlayerPrefs.GetString(DarkSoulsConsts.VSYNC);
            qualityTMP.text         = PlayerPrefs.GetString(DarkSoulsConsts.QUALITY);
            shadowQualityTMP.text   = PlayerPrefs.GetString(DarkSoulsConsts.SHADOWQUALITY);
        }
    }

    #region Resolution

    public void NextResolution()
    {
        if (playerHeights.Count == 0) return;

        resolutionIndex += 1;
        if (resolutionIndex >= playerWidths.Count)
        {
            resolutionIndex = 0;
        }
        ChangeResolution();
    }
    public void PreviousResolution()
    {
        if (playerHeights.Count == 0) return;

        resolutionIndex -= 1;
        if (resolutionIndex < 0)
        {
            resolutionIndex = playerWidths.Count - 1;
        }
        ChangeResolution();
    }
    private void ChangeResolution()
    {
        _width = playerWidths[resolutionIndex];
        _height = playerHeights[resolutionIndex];
        resolutionTMP.text = _width + "x" + _height;

        OnSettingsChanged();
    }
    private void EnableResolutionOptions()
    {
        Resolution[] resolutions = Screen.resolutions;
        foreach (var res in resolutions)
        {
            playerWidths.Add(res.width);
            playerHeights.Add(res.height);
        }
    }

    #endregion

    #region Fullscreen
    public void SetFullscreen()
    {
        if(fullscreenIndex == 0) 
        {
            Screen.fullScreen = false;
            fullscreenTMP.text = DarkSoulsConsts.OFF;
        }
        else
        {
            Screen.fullScreen = true;
            fullscreenTMP.text = DarkSoulsConsts.ON;
        }
        OnSettingsChanged();
    }
    public void SwitchFullscreen()
    {
        fullscreenIndex += 1;
        if (fullscreenIndex > 1)
        {
            fullscreenIndex = 0;
        }
        SetFullscreen();
    }
    #endregion

    #region VSync

    public void SetVsync()
    {
        if (vsyncIndex == 0)
        {
            QualitySettings.vSyncCount = 0;
            vsyncTMP.text = DarkSoulsConsts.OFF;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
            vsyncTMP.text = DarkSoulsConsts.ON;
        }
        OnSettingsChanged();
    }
    public void SwitchVsync()
    {
        vsyncIndex += 1;
        if (vsyncIndex > 1)
        {
            vsyncIndex = 0;
        }
        SetVsync();
    }

    #endregion

    #region Quality
    public void LowQuality()
    {
        _quality = 1;
        qualityTMP.text = DarkSoulsConsts.LOW;
        QualitySettings.SetQualityLevel(_quality, true);
        OnSettingsChanged();
    }

    public void MediumQuality()
    {
        _quality = 3;
        qualityTMP.text = DarkSoulsConsts.MEDIUM;
        QualitySettings.SetQualityLevel(_quality, true);
        OnSettingsChanged();
    }

    public void HighQuality()
    {
        _quality = 5;
        qualityTMP.text = DarkSoulsConsts.HIGH;
        QualitySettings.SetQualityLevel(_quality, true);
        OnSettingsChanged();
    }

    public void NextQuality()
    {
        qualityIndex += 1;
        if (qualityIndex > 2)
        {
            qualityIndex = 0;
        }
        ChangeQuality();
    }

    public void PreviousQuality()
    {
        qualityIndex -= 1;
        if (qualityIndex < 0)
        {
            qualityIndex = 2;
        }
        ChangeQuality();
    }

    private void ChangeQuality()
    {
        switch (qualityIndex)
        {
            case 0:
                LowQuality();
                break;
            case 1:
                MediumQuality();
                break;
            case 2:
                HighQuality();
                break;
        }
    }

    #endregion

    #region ShadowQuality
    public void LowShadowQuality()
    {
        QualitySettings.shadows = ShadowQuality.Disable;
        shadowQualityTMP.text = DarkSoulsConsts.LOW;

        OnSettingsChanged();
    }

    public void MediumShadowQuality()
    {
        QualitySettings.shadows = ShadowQuality.HardOnly;
        shadowQualityTMP.text = DarkSoulsConsts.MEDIUM;

        OnSettingsChanged();
    }

    public void HighShadowQuality()
    {
        QualitySettings.shadows = ShadowQuality.All;
        shadowQualityTMP.text = DarkSoulsConsts.HIGH;

        OnSettingsChanged();
    }

    public void NextShadowQuality()
    {
        shadowQualityIndex += 1;
        if (shadowQualityIndex > 2)
        {
            shadowQualityIndex = 0;
        }
        ChangeShadowQuality();
    }

    public void PreviousShadowQuality()
    {
        shadowQualityIndex -= 1;
        if (shadowQualityIndex < 0)
        {
            shadowQualityIndex = 2;
        }
        ChangeShadowQuality();
    }

    private void ChangeShadowQuality()
    {
        switch (shadowQualityIndex)
        {
            case 0:
                LowShadowQuality();
                break;
            case 1:
                MediumShadowQuality();
                break;
            case 2:
                HighShadowQuality();
                break;
        }
    }

    #endregion

    void OnSettingsChanged()
    {
        //Save Index
        PlayerPrefs.SetInt(DarkSoulsConsts.RESOLUTIONINDEX, resolutionIndex);
        PlayerPrefs.SetInt(DarkSoulsConsts.FULLSCREENINDEX, fullscreenIndex);
        PlayerPrefs.SetInt(DarkSoulsConsts.VSYNCINDEX, vsyncIndex);
        PlayerPrefs.SetInt(DarkSoulsConsts.QUALITYINDEX, qualityIndex);
        PlayerPrefs.SetInt(DarkSoulsConsts.SHADOWQUALITYINDEX, shadowQualityIndex);

        //Save Keywords
        PlayerPrefs.SetString(DarkSoulsConsts.RESOLUTION, resolutionTMP.text);
        PlayerPrefs.SetString(DarkSoulsConsts.FULLSCREEN, fullscreenTMP.text);
        PlayerPrefs.SetString(DarkSoulsConsts.VSYNC, vsyncTMP.text);
        PlayerPrefs.SetString(DarkSoulsConsts.QUALITY, qualityTMP.text);
        PlayerPrefs.SetString(DarkSoulsConsts.SHADOWQUALITY, shadowQualityTMP.text);

        PlayerPrefs.SetInt(DarkSoulsConsts.HASSAVEDSETTINGS, 1);
    }

    void SetDefaultSettings()
    {
        //Save Index
        PlayerPrefs.SetInt(DarkSoulsConsts.RESOLUTIONINDEX, 0);
        PlayerPrefs.SetInt(DarkSoulsConsts.FULLSCREENINDEX, 0);
        PlayerPrefs.SetInt(DarkSoulsConsts.VSYNCINDEX, 0);
        PlayerPrefs.SetInt(DarkSoulsConsts.QUALITYINDEX, 0);
        PlayerPrefs.SetInt(DarkSoulsConsts.SHADOWQUALITYINDEX, 0);

        //Save Keywords
        PlayerPrefs.SetString(DarkSoulsConsts.RESOLUTION, resolutionTMP.text);
        PlayerPrefs.SetString(DarkSoulsConsts.FULLSCREEN, fullscreenTMP.text);
        PlayerPrefs.SetString(DarkSoulsConsts.VSYNC, vsyncTMP.text);
        PlayerPrefs.SetString(DarkSoulsConsts.QUALITY, qualityTMP.text);
        PlayerPrefs.SetString(DarkSoulsConsts.SHADOWQUALITY, shadowQualityTMP.text);

        PlayerPrefs.SetInt(DarkSoulsConsts.HASSAVEDSETTINGS, 1);
    }

    public void DeleteSavedSettings()
    {
        PlayerPrefs.DeleteAll();
    }
}