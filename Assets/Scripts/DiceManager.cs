using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [Header("Each element is a dice type (D4/D6/D10)")]
    public GameObject[] DicePrefabs;

    public Transform Inventory;

    public Camera cam;

    List<Dice>[] Dices = new List<Dice>[(int)DICES.COUNT];
    List<Dice>[] EnemyDices = new List<Dice>[(int)DICES.COUNT];
    List<int>[] DiceValues = new List<int>[(int)DICES.COUNT];
    List<int>[] EnemyDiceValues = new List<int>[(int)DICES.COUNT];
    int DiceAmount = 0;
    int EnemyDiceAmount = 0;

    int DiceInMovement = 0;

    bool DraggingEnable = false;

    Dice SelectedDice;
    bool Dragging = false;

    Plane Plane = new Plane(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));

    void Awake()
    {
        for (int i = 0; i < (int)DICES.COUNT; ++i)
        {
            Dices[i] = new List<Dice>();
            EnemyDices[i] = new List<Dice>();
            DiceValues[i] = new List<int>();
            EnemyDiceValues[i] = new List<int>();
        }
    }

    void Update()
    {
        if (!DraggingEnable) return;

        if (Input.GetMouseButtonDown(0) && !Dragging)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, 1 << 6))
            {
                SelectedDice = hit.transform.GetComponent<Dice>();
                if (SelectedDice.Selectable())
                    Dragging = true;
                else
                    SelectedDice = null;
            }
        }
        if (Dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (Plane.Raycast(ray, out distance))
                SelectedDice.transform.position = ray.origin + ray.direction * distance;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Dragging = false;
            SelectedDice = null;
        }
    }

    public void CreateDices(bool enemy, int[] diceAmounts)
    {
        List<Dice>[] dices = (!enemy) ? Dices : EnemyDices;

        int aux = 0;
        for (int i = 0; i < (int)DICES.COUNT; ++i)
        {
            aux += diceAmounts[i];

            int difference = dices[i].Count - diceAmounts[i];
            if (difference > 0)
            {
                for (int j = 0; j < difference; ++j)
                {
                    Destroy(dices[i][dices[i].Count - 1]);
                    dices[i].RemoveAt(dices[i].Count - 1);
                }
            }
            else if (difference < 0)
            {
                difference *= -1;
                for (int j = 0; j < difference; ++j)
                {
                    dices[i].Add(Instantiate(DicePrefabs[i]).GetComponent<Dice>());
                }
            }
        }

        if (enemy) EnemyDiceAmount = aux;
        else DiceAmount = aux;
    }

    public void TossDices(int[] diceAmounts, Vector3 position, float force, bool enemy)
    {
        List<Dice>[] dices = (!enemy) ? Dices : EnemyDices;
        List<int>[] diceValues = (!enemy) ? DiceValues : EnemyDiceValues;

        for (int i = 0; i < (int)DICES.COUNT; ++i)
        {
            diceValues[i].Clear();

            for (int j = 0; j < diceAmounts[i]; ++j)
            {
                DiceInMovement += 1;
                dices[i][j].TossDice(enemy, position, force);
            }
        }
    }

    public void DiceStopped(Dice dice, int face)
    {
        if (dice.IsFromTheEnemy())
            EnemyDiceValues[(int)dice.Type].Add(face);
        else
            DiceValues[(int)dice.Type].Add(face);

        DiceInMovement -= 1;

        if (DiceInMovement <= 0)
            GameManager.Instance.DiceValues(DiceValues, EnemyDiceValues);
    }

    public void PlaceDicesInInventory()
    {
        EnableDiceDragging(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));

        // PLACE DICES IN INVENTORY
    }

    public void PlaceDicesInFrontOfCamera()
    {
        DisableDiceDragging();

        const float distance = 10.0f;

        int diceAmount = 0;
        for (int i = 0; i < (int)DICES.COUNT; ++i)
            diceAmount += Dices[i].Count;

        Vector3 initialPos = cam.transform.position + cam.transform.forward * 15f + cam.transform.right * (-distance / 2.0f);
        float deltaDistance = distance / (diceAmount - 1);
        for (int i = 0; i < (int)DICES.COUNT; ++i)
        {
            for (int j = 0; j < Dices[i].Count; ++j)
            {
                Dices[i][j].transform.position = initialPos;

                initialPos += cam.transform.right * deltaDistance;
            }
        }
    }

    #region DICE_DRAGGING
    public void EnableDiceDragging(Vector3 up, Vector3 point)
    {
        Plane = new Plane(up, point);
        DraggingEnable = true;
    }
    public void DisableDiceDragging()
    {
        DraggingEnable = false;
    }
    #endregion
}
