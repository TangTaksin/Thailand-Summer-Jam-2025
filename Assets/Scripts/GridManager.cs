using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public TileChance[] tileList;
    public int width = 5;
    public int height = 6;
    public float spacing = 0.1f;

    public int tileNeeded;

    private Tile[,] grid;
    public List<Tile> tileObjects = new List<Tile>();
    List<int> tileLimitTrackers = new List<int>();

    public delegate void GridManagerEvent();
    public static GridManagerEvent OnGenerated; 

    void Start()
    {
        GetLimit();
    }

    void GetLimit()
    {
        foreach (var tile in tileList)
        {
            tileLimitTrackers.Add(tile.limitPerMap);
        }
    }

    void ResetLimit()
    {
        //print("reset limit");

        for (int i = 0; tileLimitTrackers.Count > i; i++)
        {
            tileLimitTrackers[i] = tileList[i].limitPerMap;
            //print("Reseted Limit:" + tileLimitTrackers[i]);
        }
    }

    public void GenerateGrid()
    {
        ResetLimit();

        if (tileObjects.Count > 0)
        {
            foreach (var tile in tileObjects)
            {
                Destroy(tile.gameObject);
            }

            tileObjects.Clear();
        }

        grid = new Tile[width, height];

        var gridNo = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridNo++;
                //print(gridNo);

                Vector3 pos = new Vector3(x + (x * spacing), y + (y * spacing), 0);

                GameObject go = Instantiate(RandomizeTile(), pos, Quaternion.identity, transform);
                tileObjects.Add(go.GetComponent<Tile>());

                Tile tile = go.GetComponent<Tile>();
                tile.Init(x, y, this);
                grid[x, y] = tile;
            }
        }

        OnGenerated?.Invoke();
    }

    GameObject RandomizeTile()
    {
        float range = 0;
        GameObject chosenTile = null;
        float top = 0;
        var ite = 0;

        for(int i = 0; tileList.Length > i; i++)
        {
            if (tileLimitTrackers[i] > 0)
                range += tileList[i].spawnChance;
        }

        float random = Random.Range(0, range);
        //print("random: " + random);

        foreach (var tile in tileList)
        {
            //print("checking " + tile.tilePrefab.name + " limits.");
            if (tileLimitTrackers[ite]<=0)
            {
                //print("skipping " + tile.tilePrefab.name);
                ite++;
                continue;
            }

            top += tile.spawnChance;

            //print("top: " + top + "> random: " + random + "?");
            //print(random < top);

            if (random < top)
            {
                chosenTile = tile.tilePrefab;
                tileLimitTrackers[ite]--;

                break;
            }

            ite++;
        }

        //print("generated " + chosenTile);
        return chosenTile;
    }

    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return grid[x, y];
        return null;
    }

    public void ResetAllTiles()
    {
        foreach (Tile tile in grid)
        {
            if (tile != null)
                tile.Reset();
        }
    }

    public (int,int,int) TileStatusCounter()
    {
        int obscure = 0;
        int reveal = 0;
        int check = 0;

        foreach(var tile in tileObjects)
        {
            switch(tile.state)
            {
                case Tile.TileState.Obscured:
                    obscure++;
                    break;
                case Tile.TileState.Revealed:
                    reveal++;
                    break;
                case Tile.TileState.Checked:
                    check++;
                    break;
            }
        }

        return (obscure, reveal, check);
    }
}
