using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTosser : MonoBehaviour
{
    public GameObject DicePrefab;

    public int NumberOfDices = 1;

    List<Dice> Dices = new List<Dice>();

    Plane Plane;

    Dice SelectedDice;
    bool Dragging;

    void Start()
    {
        Plane = new Plane(Vector3.up, new Vector3(0.0f, 0.5f, 0.0f));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Dragging)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 50f, 1 << 6))
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
            int difference = Dices.Count - NumberOfDices;
            if (difference > 0)
            {
                for (int i = 0; i < difference; ++i)
                {
                    Destroy(Dices[Dices.Count - 1]);
                    Dices.RemoveAt(Dices.Count - 1);
                }
            }
            else if (difference < 0)
            {
                difference *= -1;
                for (int i = 0; i < difference; ++i)
                {
                    Dices.Add(Instantiate(DicePrefab).GetComponent<Dice>());
                }
            }

            for (int i = 0; i < Dices.Count; ++i)
                Dices[i].TossDice();
        }
    }

    void OnTriggerStay(Collider other)
    {
        other.transform.parent.GetComponent<Dice>().TriggerDetection(other.gameObject.name);
    }
}
