using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Transform cam;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
		transform.LookAt(transform.position + cam.forward);
    }
}
