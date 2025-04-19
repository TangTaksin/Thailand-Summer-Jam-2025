using UnityEngine;
using System.Collections.Generic;

public class GridSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public Grid gridComp;

    List<Tile> _tileList = new List<Tile>();
    public List<Tile> tileList
    {
        get{ return _tileList; }
    }

    public int rows = 5;
    public int cols = 5;
    //public float spacing = 1.1f; // space between tiles

    void Start()
    {
        gridComp = GetComponent<Grid>();

        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //gridComp.GetCellCenterWorld(new Vector3Int(0, 1));
                Vector2 spawnPos = gridComp.GetCellCenterWorld(new Vector3Int(x, y));

                var spawned = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                spawned.transform.SetParent(this.transform);

                _tileList.Add(spawned.GetComponent<Tile>());
            }
        }
    }
}
