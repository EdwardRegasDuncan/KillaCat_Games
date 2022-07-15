using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    bool DiceSetup = false;

    Rigidbody Rigidbody;
    Renderer Renderer;
    Transform[] Triggers;

    bool Tossed = false;

    void Setup()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<Renderer>();

        Triggers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
            Triggers[i] = transform.GetChild(i);

        Renderer.material.SetColor("_Color", Color.red);
        ActivateTriggers(false);
        Tossed = false;
    }

    public void TossDice()
    {
        if (!DiceSetup)
            Setup();

        transform.position = new Vector3(0.0f, 5.0f, 0.0f);
        transform.rotation = new Quaternion(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        Rigidbody.AddForce(Random.Range(0, 250), Random.Range(0, 250), Random.Range(0, 250));
        Rigidbody.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));

        Renderer.material.SetColor("_Color", Color.red);
        ActivateTriggers(true);
        Tossed = true;
    }

    public void TriggerDetection(string triggerName)
    {
        if (Rigidbody.velocity != Vector3.zero)
            return;

        string[] splittedName = triggerName.Split('_');

        Debug.Log("Dice face:" + triggerName);

        Renderer.material.SetColor("_Color", Color.green);
        ActivateTriggers(false);
        Tossed = false;
    }

    void ActivateTriggers(bool active)
    {
        for (int i = 0; i < Triggers.Length; ++i)
            Triggers[i].gameObject.SetActive(active);
    }
}
