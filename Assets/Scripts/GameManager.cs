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

    [Header("Managers")]
    public UIManager UIManager;
    public CameraManager CameraManager;
    public DiceManager DiceManager;

    [Header("Controllers")]
    public DiceTosser DiceTosser;
    public UnitPlacer UnitPlacer;

    [Header("Misc")]
    public int[] DiceAmounts;

    int Score = 0;
    bool Waiting = false;

    SCREENS CurrentScreen;
    GAME_STATES GameState;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

        CurrentScreen = SCREENS.MENUS;
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
        UIManager.ChangeScreen(newScreen);

        switch (newScreen)
        {
            case SCREENS.MENUS:
                break;
            case SCREENS.IN_GAME:
                GameState = GAME_STATES.ROLL_STAGE;
                EnterState();
                break;
            case SCREENS.RESULT:
                break;
        }
    } 

    void EnterState()
    {
        switch (GameState)
        {
            case GAME_STATES.ROLL_STAGE:
                StartCoroutine(RollStage());
                break;
            case GAME_STATES.PLACEMENT:
                UnitPlacer.Placing = true;
                break;
            case GAME_STATES.COMBAT:
                break;
            case GAME_STATES.DAMAGE_PHASE:
                break;
        }
    }

    IEnumerator RollStage()
    {
        // Create dices
        DiceManager.CreateDices(false, DiceAmounts);
        DiceManager.CreateDices(true, DiceAmounts);

        // Show inventory dices
        CameraManager.SetPlayerInventoryCamera();
        yield return new WaitForSeconds(1.0f);
        while (!Input.GetKeyDown(KeyCode.Escape))
            yield return null;
        CameraManager.SetBoardCamera();
        yield return new WaitForSeconds(1.0f);

        // Waiting the first space to toss the dices
        UIManager.ShowPressKeyText(true, "Space", "toss the dices");
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
        UIManager.ShowPressKeyText(false);

        // Toss the dices
        DiceManager.TossDices(DiceAmounts, new Vector3(85f, 10f, 6f), -800, false);
        DiceManager.TossDices(DiceAmounts, new Vector3(-60f, 10f, 6f), 1000, true);

        // Waiting the dices values
        Waiting = true;
        while (Waiting)
            yield return null;

        // Waiting the space to continue with the next state
        UIManager.ShowPressKeyText(true, "Space", "continue");
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
        UIManager.ShowPressKeyText(false);

        ChangeState();
    }

    void ChangeState()
    {
        GameState = (GAME_STATES)(((int)GameState + 1) % (int)GAME_STATES.COUNT);
        EnterState();
    }

    public void DiceValues(List<int>[] diceValues, List<int>[] enemyDiceValues)
    {
        DiceManager.PlaceDicesInFrontOfCamera();
        Waiting = false;
    }
}
