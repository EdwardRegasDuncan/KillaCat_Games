using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;

    public void UpdateScore(int newScore)
    {
        ScoreText.text = newScore.ToString();
    }
}
