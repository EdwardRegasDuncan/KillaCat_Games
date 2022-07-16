using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacer : MonoBehaviour
{
    [Header("Each element is a unit type (Knight/Archer/Wizard)")]
    public GameObject[] UnitPrefabs;

    public Transform PlayerSide;
    public Transform EnemySide;

    List<GameObject> InstantiatedUnits = new List<GameObject>();

    Plane Plane;
    Transform GridNode;

    Vector3 SelectedArea;
    public bool Placing = false;

    void Start()
    {
        Plane = new Plane(Vector3.up, new Vector3(0.0f, 0.05f, 0.0f));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Placing = true;
        }

        if (Placing)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, 1 << 8))
            {
                if (hit.transform.parent == PlayerSide)
                {
                    if (GridNode != hit.transform)
                    {
                        if (GridNode != null)
                        {
                            GridNode.position = SelectedArea;
                            GridNode = null;
                        }
                        else
                        {
                            //SelectionAreaGO.SetActive(true);
                        }

                        GridNode = hit.transform;

                        SelectedArea = GridNode.position;

                        GridNode.position += new Vector3(0.0f, 2.0f, 0.0f);

                        //SelectionAreaGO.transform.position = GridNode.position + new Vector3(0.0f, 0.55f, 0.0f);
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        InstantiatedUnits.Add(Instantiate(UnitPrefabs[0]));

                        InstantiatedUnits[InstantiatedUnits.Count - 1].transform.position = SelectedArea + new Vector3(0.0f, 2.0f, 0.0f);

                        GridNode.position = SelectedArea;
                        GridNode = null;
                        //Placing = false;
                    }
                }
                else
                {
                    if (GridNode != null)
                    {
                        GridNode.position = SelectedArea;
                        GridNode = null;
                    }
                }
            }
            else if (GridNode != null)
            {
                GridNode.position = SelectedArea;
                GridNode = null;
            }
        }
    }

    /*IEnumerator ManageGridNode()
    {
        yield return null;
    }*/
}
