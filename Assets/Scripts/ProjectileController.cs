using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Transform _target;
    public float _DestroyAtRange = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if (!_target)
        {
            Destroy(gameObject);
        }
        else if (Vector3.Distance(transform.position, _target.position) < _DestroyAtRange)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * 10);
            transform.LookAt(_target);
        }
    }
}
