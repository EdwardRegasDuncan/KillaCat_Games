using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlacer : MonoBehaviour
{
    public GameObject Unit;

    public GameObject SelectionAreaGO;

    Plane Plane;
    Transform GridNode;

    Vector3 SelectedArea;
    bool Placing = false;

    void Start()
    {
        Plane = new Plane(Vector3.up, new Vector3(0.0f, 0.05f, 0.0f));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //SelectionAreaGO.SetActive(true);
            Placing = true;
        }

        if (Placing)
        {
#if NO_GRID
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (Plane.Raycast(ray, out distance))
            {
                SelectedArea = ray.origin + ray.direction * distance;

                SelectedArea.x = Mathf.Round(SelectedArea.x);
                SelectedArea.z = Mathf.Round(SelectedArea.z);

                SelectionAreaGO.transform.position = SelectedArea;

                if (Input.GetMouseButtonDown(0))
                {
                    SelectedArea.y = 0.5f;
                    Unit.transform.position = SelectedArea;
                    SelectionAreaGO.SetActive(false);

                    Placing = false;
                }
            }
#else
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, 1 << 8))
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
                    Unit.transform.position = SelectedArea + new Vector3(0.0f, 2.0f, 0.0f);
                    //SelectionAreaGO.SetActive(false);

                    GridNode.position = SelectedArea;
                    GridNode = null;
                    Placing = false;
                }
            }
            else if (GridNode != null)
            {
                GridNode.position = SelectedArea;
                GridNode = null;
                //SelectionAreaGO.SetActive(false);
            }
#endif
        }
    }

    /*IEnumerator ManageGridNode()
    {
        yield return null;
    }*/
}
