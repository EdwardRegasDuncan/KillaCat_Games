using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject InGame;

    [Header("InGameUI")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI PressKeyText;
    public Button ReadyButton;

    public void UpdateScore(int newScore)
    {
        ScoreText.text = newScore.ToString();
    }

    public void ShowPressKeyText(bool show, string key = null, string action = null)
    {
        if (show) PressKeyText.text = "Press [" + key + "] to " + action;
        PressKeyText.gameObject.SetActive(show);
    }

    public void ActiveReadyButton(bool active)
    {
        ReadyButton.gameObject.SetActive(active);
    }

    public void ChangeScreen(SCREENS newScreen)
    {
        Menu.SetActive(false);
        InGame.SetActive(false);

        switch (newScreen)
        {
            case SCREENS.MENUS:
                Menu.SetActive(true);
                break;
            case SCREENS.IN_GAME:
                InGame.SetActive(true);
                break;
            case SCREENS.RESULT:
                break;
        }
    }

    #region BUTTON_MANAGEMENT
    public void StartClicked()
    {
        GameManager.Instance.ChangeScreen(SCREENS.IN_GAME);
    }

    public void QuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReadyClicked()
    {
        GameManager.Instance.ActionEnded();
    }
    #endregion
}
