using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public DiceTosser DiceTosser;

    public int[] DiceAmounts;

    GAME_STATES GameState;

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = null;
    }

    void Start()
    {
        GameState = GAME_STATES.ROLL_STAGE;
        ChangeState();
    }

    void Update()
    {
        
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
}
