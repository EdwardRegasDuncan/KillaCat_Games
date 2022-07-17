using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{

    [SerializeField] GameObject[] _tiles;
    [SerializeField] GameObject _playerSide;
    [SerializeField] GameObject _enemySide;


    private void Awake()
    {
        Generate();
    }

    public void Generate()
    {
        List<Transform> childs = new List<Transform>();
        foreach (Transform child in _playerSide.transform)
            childs.Add(child);
        foreach (Transform child in childs)
        {
            // pick a random tile
            int tileIndex = Random.Range(0, _tiles.Length);
            // randome 90 degree rotation
            Quaternion rotationQuaternion = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
            // instantiate the tile
            GameObject tile = Instantiate(_tiles[tileIndex], child.transform.position, rotationQuaternion);
            // set the parent to the child
            tile.transform.parent = _playerSide.transform;
            // scale the tile
            tile.transform.localScale = child.transform.localScale;
        }

        childs.Clear();
        foreach (Transform child in _enemySide.transform)
            childs.Add(child);
        foreach (Transform child in childs)
        {
            // pick a random tile
            int tileIndex = Random.Range(0, _tiles.Length);
            // randome 90 degree rotation
            Quaternion rotationQuaternion = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
            // instantiate the tile
            GameObject tile = Instantiate(_tiles[tileIndex], child.transform.position, rotationQuaternion);
            // set the parent to the child
            tile.transform.parent = _enemySide.transform;
            // scale the tile
            tile.transform.localScale = child.transform.localScale;
        }
    }
    private void OnDrawGizmos()
    {
        foreach (Transform child in _playerSide.transform)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(child.position, 0.5f);
        }
        foreach (Transform child in _enemySide.transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(child.position, 0.5f);
        }
    }
}
