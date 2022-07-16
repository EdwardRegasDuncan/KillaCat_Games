using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    const float DistanceBetweenDices = 2.0f;


    [Header("Each element is a dice type (D4/D6/D10)")]
    public GameObject[] DicePrefabs;

    public Transform Inventory;

    public Camera cam;

    List<Dice>[] Dices = new List<Dice>[(int)DICES.COUNT];
    List<Dice>[] EnemyDices = new List<Dice>[(int)DICES.COUNT];
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
                    dices[i][dices[i].Count - 1].Setup(enemy);
                }
            }
        }

        if (enemy) EnemyDiceAmount = aux;
        else DiceAmount = aux;
    }

    public void TossDices(int[] diceAmounts, Vector3 position, float force, bool enemy)
    {
        List<Dice>[] dices = (!enemy) ? Dices : EnemyDices;

        DiceInMovement = DiceAmount;
        for (int i = 0; i < (int)DICES.COUNT; ++i)
        {
            for (int j = 0; j < diceAmounts[i]; ++j)
                dices[i][j].TossDice(position, force);
        }
    }

    public void DiceStopped(Dice dice, int face)
    {
        DiceInMovement -= 1;

        if (DiceInMovement <= 0)
            GameManager.Instance.ActionEnded();
    }

    public void DiceInCard(bool entering, UnitCore.UNIT_TYPE unitType, Dice dice)
    {
        if (entering)
        {
            dice.UnitType = unitType;
            DiceInMovement -= 1;
        }
        else
        {
            DiceInMovement += 1;
        }
    }

    public void PlaceDicesInInventory()
    {
        DiceInMovement = DiceAmount;

        Vector3 initialPos = Inventory.position + Inventory.right * (((DiceAmount - 1) *  DistanceBetweenDices) / -2.0f);
        for (int i = 0; i < (int)DICES.COUNT; ++i)
        {
            for (int j = 0; j < Dices[i].Count; ++j)
            {
                Dices[i][j].transform.position = initialPos;
                initialPos += Inventory.right * DistanceBetweenDices;
            }
        }
    }

    public void PlaceDicesInFrontOfCamera()
    {
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

    #region GETTERS
    public int GetDicesInMovement()
    {
        return DiceInMovement;
    }
    public List<Dice> GetDices()
    {
        List<Dice> dices = new List<Dice>();
        for (int i = 0; i < Dices.Length; ++i)
            dices.AddRange(Dices[i]);
        return dices;
    }
    public int GetDiceAmount()
    {
        return DiceAmount;
    }
    #endregion

    #region DICE_DRAGGING
    public void EnableDiceDragging(Vector3 up, Vector3 point)
    {
        Plane = new Plane(up, point);
        DraggingEnable = true;
    }
    public void DisableDiceDragging()
    {
        DraggingEnable = false;
        Dragging = false;
    }
    #endregion
}
