using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsScreenWindow;
    void Update()
    {
        settingsScreenWindow.SetActive(false);
    }
    public void Continue()
    {
        
    }
    public void NewGame()
    {
        
    }
    public void System()
    {
        settingsScreenWindow.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
