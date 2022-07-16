using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : MonoBehaviour
{
    public UnitCore.UNIT_TYPE UnitType;

    void OnTriggerEnter(Collider other)
    {
        Dice dice = other.transform.GetComponent<Dice>();
        GameManager.Instance.DiceManager.DiceInCard(true, UnitType, dice);
    }

    void OnTriggerExit(Collider other)
    {
        Dice dice = other.transform.GetComponent<Dice>();
        GameManager.Instance.DiceManager.DiceInCard(false, UnitType, dice);
    }
}
