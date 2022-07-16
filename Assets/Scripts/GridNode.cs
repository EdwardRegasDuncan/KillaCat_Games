using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public bool Used = false;

    public int numberPositions = 0;

    List<GameObject> InstantiatedUnits = new List<GameObject>();

    void Start()
    {
        GameManager.Instance.ResetGrid.AddListener(ResetGridNode);

        numberPositions = transform.childCount;
    }

    void OnDestroy()
    {
        GameManager.Instance.ResetGrid.RemoveListener(ResetGridNode);
    }

    void ResetGridNode()
    {
        Used = false;

        for (int i = 0; i < InstantiatedUnits.Count; ++i)
            Destroy(InstantiatedUnits[i]);

        InstantiatedUnits.Clear();
    }

    public void InstantiateUnits(GameObject prefab, int amount)
    {
        Used = true;
        int aux = Mathf.Min(amount, numberPositions);
        for (int i = 0; i < aux; ++i)
        {
            InstantiatedUnits.Add(Instantiate(prefab, transform.GetChild(i)));
            InstantiatedUnits[InstantiatedUnits.Count - 1].transform.position += new Vector3(0.0f, 2.5f, 0.0f);
        }
    }
}
