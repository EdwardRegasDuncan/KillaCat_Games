using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DICES
{
    D4 = 0,
    D6 = 1,
    D10 = 2,
    COUNT = 3,
}

public class Dice : MonoBehaviour
{
    public DICES Type;
    public int DiceValue = -1;
    public GameObject Triggers;
    public Renderer Renderer;

    bool DiceSetup = false;

    Rigidbody Rigidbody;

    bool Enemy;

    bool Tossed = false;

    void Setup(bool enemy)
    {
        Rigidbody = GetComponent<Rigidbody>();

        Enemy = enemy;

        Renderer.material.SetColor("_Color", Color.red);
        ActivateDice(false);
        Tossed = false;
    }

    public void TossDice(bool enemy, Vector3 position, float force)
    {
        if (!DiceSetup)
            Setup(enemy);

        transform.position = position;
        transform.rotation = new Quaternion(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        Rigidbody.AddForce(force, Random.Range(0, 250), Random.Range(0, 250));
        Rigidbody.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));

        Renderer.material.SetColor("_Color", Color.red);
        ActivateDice(true);
        Tossed = true;
    }

    public int TriggerDetection(string triggerName)
    {
        if (Rigidbody.velocity != Vector3.zero)
            return -1;

        string[] splittedName = triggerName.Split('_');

        DiceValue = int.Parse(splittedName[1]);

        Debug.Log("Dice face:" + triggerName);

        Renderer.material.SetColor("_Color", Color.green);
        ActivateDice(false);
        Tossed = false;

        return DiceValue;
    }

    #region GETTERS
    public bool Selectable()
    {
        return !Tossed;
    }

    public bool IsFromTheEnemy()
    {
        return Enemy;
    }
    #endregion

    void ActivateDice(bool active)
    {
        if (active)
            Rigidbody.constraints = RigidbodyConstraints.None;
        else
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        Triggers.SetActive(active);
    }
}
