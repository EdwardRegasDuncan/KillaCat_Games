using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public bool Used = false;

    void Start()
    {
        GameManager.Instance.ResetGrid.AddListener(ResetGridNode);
    }

    void OnDestroy()
    {
        GameManager.Instance.ResetGrid.RemoveListener(ResetGridNode);
    }

    void ResetGridNode()
    {
        Used = false;
    }
}
