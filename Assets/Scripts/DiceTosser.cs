using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTosser : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        Dice dice = other.transform.parent.parent.GetComponent<Dice>();

        int aux = dice.TriggerDetection(other.gameObject.name);
        if (aux != -1)
            GameManager.Instance.DiceManager.DiceStopped(dice, aux);
    }
}
