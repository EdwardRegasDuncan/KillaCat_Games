using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string _mainScene;

    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _settings;
    
    

    public void StartGame()
    {
        SceneManager.LoadScene(_mainScene);
    }
    
    public void OpenSettings()
    {
        //enable settings disable main menu
        _settings.SetActive(true);
        _mainMenu.SetActive(false);
    }
    public void CloseSettings()
    {
        //enable main menu disable settings
        _settings.SetActive(false);
        _mainMenu.SetActive(true);
    }


    public void Quit()
    {
        Application.Quit();
    }
}
