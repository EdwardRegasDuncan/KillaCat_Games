using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTosser : MonoBehaviour
{
    public GameObject DicePrefab;

    public int NumberOfDices = 1;

    List<Dice> Dices = new List<Dice>();

    // Update is called once per frame
    void Update()
    {
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

    private void OnTriggerStay(Collider other)
    {
        other.transform.parent.GetComponent<Dice>().TriggerDetection(other.gameObject.name);
    }
}
