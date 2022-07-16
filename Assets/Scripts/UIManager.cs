using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject InGame;

    public TextMeshProUGUI ScoreText;

    public void UpdateScore(int newScore)
    {
        ScoreText.text = newScore.ToString();
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
    #endregion
}
