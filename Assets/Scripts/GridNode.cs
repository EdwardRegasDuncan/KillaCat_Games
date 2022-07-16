using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public bool Used = false;

    public int NumberPositions = 0;

    List<GameObject> InstantiatedUnits = new List<GameObject>();
    Vector3 OriginalPosition;

    void Start()
    {
        GameManager.Instance.ResetGrid.AddListener(ResetGridNode);

        NumberPositions = transform.childCount;
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

    public string Select(int amount)
    {
        OriginalPosition = transform.position;
        transform.position += new Vector3(0.0f, 2.0f, 0.0f);

        return (NumberPositions < amount) ? $"This tile can only have {NumberPositions} you will lose: <color=red>{amount - NumberPositions} units</color>" : null;
    }

    public void Unselect()
    {
        transform.position = OriginalPosition;
    }

    public void InstantiateUnits(GameObject prefab, int amount)
    {
        Used = true;
        int aux = Mathf.Min(amount, NumberPositions);
        for (int i = 0; i < aux; ++i)
        {
            InstantiatedUnits.Add(Instantiate(prefab, transform.GetChild(i)));
            InstantiatedUnits[InstantiatedUnits.Count - 1].transform.position += new Vector3(0.0f, 2.5f, 0.0f);
        }
    }
}
