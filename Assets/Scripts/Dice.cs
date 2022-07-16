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
    const float speedThreshHold = 0.000001f;

    public DICES Type;
    public UnitCore.UNIT_TYPE UnitType;
    public int DiceValue = -1;
    public GameObject Triggers;
    public Renderer Renderer;

    bool DiceSetup = false;

    Rigidbody Rigidbody;

    bool Enemy;

    float ElapsedTime = 0.0f;
    bool Tossed = false;

    void FixedUpdate()
    {
        if (Tossed && Rigidbody.velocity.x <= speedThreshHold && Rigidbody.velocity.x <= speedThreshHold && Rigidbody.velocity.x <= speedThreshHold)
        {
            ElapsedTime += Time.deltaTime;

            if (ElapsedTime >= 4.0f)
            {
                ElapsedTime = 0.0f;
                Rigidbody.AddForce(Random.Range(0, 250), Random.Range(0, 250), Random.Range(0, 250));
                Rigidbody.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
            }
        }
    }

    public void Setup(bool enemy)
    {
        Rigidbody = GetComponent<Rigidbody>();

        Enemy = enemy;

        Renderer.material.SetColor("_Color", Color.red);
        ActivateDice(false);
        Tossed = false;
    }

    public void TossDice(Vector3 position, float force)
    {
        transform.position = position;
        transform.rotation = new Quaternion(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        Rigidbody.AddForce(force, Random.Range(0, 250), Random.Range(0, 250));
        Rigidbody.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));

        Renderer.material.SetColor("_Color", Color.red);
        ActivateDice(true);
        Tossed = true;
        ElapsedTime = 0.0f;
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
