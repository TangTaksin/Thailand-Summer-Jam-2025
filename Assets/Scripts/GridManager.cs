using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public TileChance[] tileList;
    public int width = 5;
    public int height = 6;
    public float spacing = 0.1f;

    private Tile[,] grid;

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x + (x * spacing), y + (y * spacing), 0);

                GameObject go = Instantiate(RandomizeTile(), pos, Quaternion.identity, transform);
                Tile tile = go.GetComponent<Tile>();
                tile.Init(x, y, this);
                grid[x, y] = tile;
            }
        }
    }

    GameObject RandomizeTile()
    {
        float range = 0;
        GameObject chosenTile = null;

        foreach (var tile in tileList)
            range += tile.spawnChance;

        float random = Random.Range(0, range);
        //print("random: " + random);

        float top = 0;

        foreach (var tile in tileList)
        {
            top += tile.spawnChance;

            if (random < top)
            {
                print("top: " + top);
                chosenTile = tile.tilePrefab;

                break;
            }
        }

        return chosenTile;
    }

    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return grid[x, y];
        return null;
    }
}