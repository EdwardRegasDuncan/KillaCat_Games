using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SCREENS
{
    MENUS = 0,
    IN_GAME = 1,
    RESULT = 2
}

public enum GAME_STATES
{
    ROLL_STAGE = 0,
    PLACEMENT = 1,
    COMBAT = 2,
    DAMAGE_PHASE = 3,
    COUNT = 4
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UIManager UIManager;
    public DiceTosser DiceTosser;
    public UnitPlacer UnitPlacer;

    public int[] DiceAmounts;

    int Score = 0;

    SCREENS CurrentScreen;
    GAME_STATES GameState;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        GameState = GAME_STATES.ROLL_STAGE;
        ChangeState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Score += 10;
            UIManager.UpdateScore(Score);
        }
    }

    public void ChangeScreen(SCREENS newScreen)
    {
        CurrentScreen = newScreen;

        switch (newScreen)
        {
            case SCREENS.MENUS:
                break;
            case SCREENS.IN_GAME:
                break;
            case SCREENS.RESULT:
                break;
        }
        UIManager.ChangeScreen(newScreen);
    } 

    void EnterState()
    {
        switch (GameState)
        {
            case GAME_STATES.ROLL_STAGE:
                DiceTosser.TossDices(DiceAmounts);
                break;
            case GAME_STATES.PLACEMENT:
                break;
            case GAME_STATES.COMBAT:
                break;
            case GAME_STATES.DAMAGE_PHASE:
                break;
        }
    }

    void ChangeState()
    {
        GameState = (GAME_STATES)(((int)GameState + 1) % (int)GAME_STATES.COUNT);
        EnterState();
    }

    public void DiceValues(List<int>[] diceValues, List<int>[] enemyDiceValues)
    {

    }
}
