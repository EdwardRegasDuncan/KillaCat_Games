using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTosser : MonoBehaviour
{
    [Header("Each element is a dice type (D4/D6/D10)")]
    public GameObject[] DicePrefabs;
    public int[] DiceAmounts;

    List<Dice>[] Dices = new List<Dice>[(int)DICES.COUNT];
    List<int>[] DiceValues = new List<int>[(int)DICES.COUNT];

    Plane Plane;

    Dice SelectedDice;
    bool Dragging;

    int DiceInMovement = 0;

    void Start()
    {
        Plane = new Plane(Vector3.up, new Vector3(0.0f, 2.5f, 0.0f));

        for (int i = 0; i < (int)DICES.COUNT; ++i)
        {
            Dices[i] = new List<Dice>();
            DiceValues[i] = new List<int>();
        }
    }

    void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TossDices(DiceAmounts);
        }
    }

    public void TossDices(int[] diceAmounts)
    {
        for (int i = 0; i < (int)DICES.COUNT; ++i)
        {
            DiceValues[i].Clear();

            int difference = Dices[i].Count - diceAmounts[i];
            if (difference > 0)
            {
                for (int j = 0; j < difference; ++j)
                {
                    Destroy(Dices[i][Dices[i].Count - 1]);
                    Dices[i].RemoveAt(Dices[i].Count - 1);
                }
            }
            else if (difference < 0)
            {
                difference *= -1;
                for (int j = 0; j < difference; ++j)
                {
                    Dices[i].Add(Instantiate(DicePrefabs[i]).GetComponent<Dice>());
                }
            }

            for (int j = 0; j < Dices[i].Count; ++j)
            {
                DiceInMovement += 1;
                Dices[i][j].TossDice();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        Dice dice = other.transform.parent.GetComponent<Dice>();

        int aux = dice.TriggerDetection(other.gameObject.name);
        if (aux != -1)
        {
            DiceValues[(int)dice.Type].Add(aux);
            DiceInMovement -= 1;

            if (DiceInMovement <= 0)
                GameManager.Instance.DiceValues(DiceValues);
        }
    }
}
