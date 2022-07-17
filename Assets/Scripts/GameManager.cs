using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    const int MaxHealth = 20;

    public static GameManager Instance;

    [Header("Managers")]
    public UIManager UIManager;
    public CameraManager CameraManager;
    public DiceManager DiceManager;

    [Header("Controllers")]
    public DiceTosser DiceTosser;

    [Header("Each element is a unit type (Knight/Archer/Wizard)")]
    public GameObject[] UnitPrefabs;

    [Header("Misc")]
    public int[] DiceAmounts;
    public int[] EnemyDiceAmounts;
    public LifeCounter PlayerLifeCounter;
    public LifeCounter EnemyLifeCounter;
    public Transform PlayerSide;
    public Transform EnemySide;
    public UnityEvent ResetGrid;
    public Transform PlayerUnitContainer;
    public Transform EnemyUnitContainer;

    GridNode GridNode;

    [SerializeField] int PlayerHealth;
    [SerializeField] int EnemyHealth;
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

        SoundManager.Initilize();
        SoundManager.PlayMusic(SoundManager.Sound.BackGroundMusic, 0.1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Score += 10;
            UIManager.UpdateScore(Score);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerHealth -= 1;
            PlayerLifeCounter.SetHealth(PlayerHealth);
        }
        PlayerLifeCounter.SetHealth(PlayerHealth);
        EnemyLifeCounter.SetHealth(EnemyHealth);
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
                CameraManager.SetBoardCamera();

                PlayerHealth = MaxHealth;
                EnemyHealth = MaxHealth;
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
                StartCoroutine(Placement());
                break;
            case GAME_STATES.COMBAT:
                StartCoroutine(Combat());
                break;
            case GAME_STATES.DAMAGE_PHASE:
                StartCoroutine(DamagePhase());
                break;
        }
    }

    IEnumerator RollStage()
    {
        yield return new WaitForSeconds(1.0f);

        // Waiting first input to start
        UIManager.ShowPressKeyText(true, "Space", "start");
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
        UIManager.ShowPressKeyText(false);

        // Create dices
        DiceManager.CreateDices(false, DiceAmounts);
        DiceManager.CreateDices(true, EnemyDiceAmounts);

        // Show inventory dices
        DiceManager.PlaceDicesInInventory(true);
        DiceManager.EnableDiceDragging(Vector3.up, new Vector3(0.0f, 2.0f, 0.0f));
        CameraManager.SetPlayerInventoryCamera();
        yield return new WaitForSeconds(1.0f);

        // Wait all dice to be selected
        Waiting = true;
        bool readyButton = false;
        while (Waiting)
        {
            if (!readyButton && DiceManager.GetDicesInSelection() <= 0)
            {
                readyButton = true;
                UIManager.ActiveReadyButton(true);
            }
            else if (readyButton && DiceManager.GetDicesInSelection() > 0)
            {
                readyButton = false;
                UIManager.ActiveReadyButton(false);
            }

            yield return null;
        }
        UIManager.ActiveReadyButton(false);

        // Enemy dice selection
        DiceManager.EnemyDiceSelection();

        // Get back the board
        CameraManager.SetBoardCamera();
        DiceManager.DisableDiceDragging();
        yield return new WaitForSeconds(1.0f);

        // Waiting the first space to toss the dices
        UIManager.ShowPressKeyText(true, "Space", "toss the dices");
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
        UIManager.ShowPressKeyText(false);

        // Toss the dices
        DiceManager.TossDices(new Vector3(84f, 10f, -18f), -800, new Vector3(-60f, 10f, -18f), 1000);

        // Waiting the dices values
        Waiting = true;
        while (Waiting)
            yield return null;

        // Show the dices
        DiceManager.PlaceDicesInFrontOfCamera();

        // Waiting the space to continue with the next state
        UIManager.ShowPressKeyText(true, "Space", "continue");
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
        UIManager.ShowPressKeyText(false);

        // Return the dices to the inventory
        DiceManager.PlaceDicesInInventory();

        ChangeState();
    }

    IEnumerator Placement()
    {
        List<Dice> playerDices = DiceManager.GetDices(); 
        List<Dice> enemyDices = DiceManager.GetDices(true);
        int playerIdx = 0;
        int enemyIdx = 0;

        while (playerIdx < playerDices.Count)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, 1 << 8))
            {
                GridNode gridNode = hit.transform.GetComponent<GridNode>();
                if (hit.transform.parent == PlayerSide && !gridNode.Used)
                {
                    if (GridNode != gridNode)
                    {
                        if (GridNode != null)
                        {
                            GridNode.Unselect();
                            UIManager.ShowInfoText(false);
                        }
                        GridNode = gridNode;

                        string msg = GridNode.Select(playerDices[playerIdx].DiceValue);
                        if (msg != null) UIManager.ShowInfoText(true, msg);
                    }
                    if (Input.GetMouseButtonDown(0) && !GridNode.GetComponent<GridNode>().Used)
                    {
                        SoundManager.PlaySound(SoundManager.Sound.TropPlacing, hit.transform.localPosition);
                        UIManager.ShowInfoText(false);
                        GridNode.Unselect();
                        GridNode.GetComponent<GridNode>().InstantiateUnits(UnitPrefabs[(int)playerDices[playerIdx].UnitType], playerDices[playerIdx].DiceValue, UnitCore.Team.Player, PlayerUnitContainer);
                        GridNode = null;


                        playerIdx += 1;
                    }
                }
                else if (GridNode != null)
                {
                    UIManager.ShowInfoText(false);
                    GridNode.Unselect();
                    GridNode = null;
                }
            }
            else if (GridNode != null)
            {
                UIManager.ShowInfoText(false);
                GridNode.Unselect();
                GridNode = null;
            }

            yield return null;
        }

        while (enemyIdx < enemyDices.Count)
        {
            EnemyPlaceUnit(ref enemyDices, ref enemyIdx);
        }

        // Waiting the space to continue with the next state
        UIManager.ShowPressKeyText(true, "Space", "fight");
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
        UIManager.ShowPressKeyText(false);

        ChangeState();
    }

    IEnumerator Combat()
    {

        foreach (Transform playerUnit in PlayerUnitContainer)
            playerUnit.GetComponent<UnitCore>().UnPause();
        foreach (Transform enemyUnit in EnemyUnitContainer)
            enemyUnit.GetComponent<UnitCore>().UnPause();

        // TODO: make the units fight
        while (PlayerUnitContainer.transform.childCount > 0 && EnemyUnitContainer.transform.childCount > 0)
        {
            yield return null;
        }

        ChangeState();
    }

    IEnumerator DamagePhase()
    {
        // who has units left
        if (PlayerUnitContainer.transform.childCount > 0)
        {
            EnemyHealth -= PlayerUnitContainer.transform.childCount;
        }
        else if (EnemyUnitContainer.transform.childCount > 0)
        {
            PlayerHealth -= EnemyUnitContainer.transform.childCount;
        }
        else
        {
            Debug.Log($"Error Occured calculating winner; Player count: {PlayerUnitContainer.transform.childCount}, Enemy count: {EnemyUnitContainer.transform.childCount}");
            yield return null;
        }

        ResetGrid.Invoke();
        ChangeState();
    }

    void ChangeState()
    {
        GameState = (GAME_STATES)(((int)GameState + 1) % (int)GAME_STATES.COUNT);
        EnterState();
    }

    public void ActionEnded()
    {
        Waiting = false;
    }

    void EnemyPlaceUnit(ref List<Dice> enemyDices, ref int enemyIdx)
    {
        if (enemyIdx >= enemyDices.Count) return;

        int grid = Random.Range(0, EnemySide.childCount);
        GridNode gridNode = EnemySide.GetChild(grid).GetComponent<GridNode>();
        while (gridNode.Used)
        {
            grid = Random.Range(0, EnemySide.childCount);
            gridNode = EnemySide.GetChild(grid).GetComponent<GridNode>();
        }

        gridNode.InstantiateUnits(UnitPrefabs[(int)enemyDices[enemyIdx].UnitType], enemyDices[enemyIdx].DiceValue, UnitCore.Team.Enemy, EnemyUnitContainer);
        enemyIdx += 1;
    }

    /*IEnumerator ManageGridNode()
    {
        yield return null;
    }*/
}
