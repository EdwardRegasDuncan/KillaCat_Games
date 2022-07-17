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
        // combine children of _playerside and _enemyside into one array
        List<GameObject> _allTiles = new List<GameObject>();
        foreach (Transform child in _playerSide.transform)
        {
            _allTiles.Add(child.gameObject);
        }
        foreach (Transform child in _enemySide.transform)
        {
            _allTiles.Add(child.gameObject);
        }

        // interate through each child of _playerside
        foreach (GameObject child in _allTiles)
        {
            // should be a tile?
            if (Random.Range(1, 100) < 25)
            {
                // pick a random tile
                int tileIndex = Random.Range(0, _tiles.Length);
                // randome 90 degree rotation
                Quaternion rotationQuaternion = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                // instantiate the tile
                GameObject tile = Instantiate(_tiles[tileIndex], child.transform.position, rotationQuaternion);
                // set the parent to the child
                tile.transform.parent = child.transform;
                // scale the tile
                tile.transform.localScale = child.transform.localScale;
            }
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
