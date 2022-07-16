using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform _target;

    // Update is called once per frame
    void Update()
    {
        if (!_target)
        {
            Destroy(gameObject);
        }
        else if (Vector3.Distance(transform.position, _target.position) < 1f)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * 10);
        }
    }
}
