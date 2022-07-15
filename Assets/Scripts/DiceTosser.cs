using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTosser : MonoBehaviour
{
    public GameObject Dice;

    public bool tossed = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TossDice();

            tossed = true;
        }
    }

    void TossDice()
    {
        Rigidbody rb = Dice.GetComponent<Rigidbody>();
        Dice.transform.position = new Vector3(0.0f, 5.0f, 0.0f);
        Dice.transform.rotation = new Quaternion(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        rb.AddForce(Random.Range(0, 250), Random.Range(0, 250), Random.Range(0, 250));
        rb.AddTorque(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
    }

    private void OnTriggerStay(Collider other)
    {
        if (!tossed) return;

        if (other.transform.parent.GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            string[] splittedName = other.gameObject.name.Split('_');

            Debug.Log("Dice face:" + other.gameObject.name);

            tossed = false;
        }
    }
}
