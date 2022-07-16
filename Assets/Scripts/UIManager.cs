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
    public TextMeshProUGUI InfoText;

    Stack<string> InfoTexts = new Stack<string>();

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

    public void ShowInfoText(bool show, string text = null)
    {
        if (show)
        {
            InfoTexts.Push(text);
            InfoText.text = text;
        }
        else
        {
            if (InfoTexts.Count > 0)
            {
                InfoTexts.Pop();

                if (InfoTexts.Count > 0)
                {
                    show = true;
                    InfoText.text = InfoTexts.Peek();
                }
            }
        }
        InfoText.transform.parent.gameObject.SetActive(show);
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
        SoundManager.PlaySound(SoundManager.Sound.UISound);
        GameManager.Instance.ChangeScreen(SCREENS.IN_GAME);
    }

    public void QuitClicked()
    {
        SoundManager.PlaySound(SoundManager.Sound.UISound);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReadyClicked()
    {
        SoundManager.PlaySound(SoundManager.Sound.UISound);
        GameManager.Instance.ActionEnded();
    }
    #endregion
}
