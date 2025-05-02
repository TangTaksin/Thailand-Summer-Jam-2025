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

    void Awake()
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

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * (1 + spacing), y * (1 + spacing), 0);

                GameObject go = null;
                Tile tile = null;
                int safetyCounter = 100;

                do
                {
                    go = Instantiate(RandomizeTile(), pos, Quaternion.identity, transform);
                    tile = go.GetComponent<Tile>();
                    safetyCounter--;

                    if (safetyCounter <= 0)
                    {
                        Debug.LogWarning("Failed to find a valid tile within safety loop.");
                        break;
                    }

                    if (IsEdgeTile(x, y) && tile.isImpassable)
                    {
                        Destroy(go);
                        go = null;
                        tile = null;
                    }

                } while (go == null);

                tileObjects.Add(tile);
                tile.Init(x, y, this);
                grid[x, y] = tile;
            }
        }

        OnGenerated?.Invoke();
    }

    private bool IsEdgeTile(int x, int y)
    {
        return x == 0 || x == width - 1 || y == 0 || y == height - 1;
    }

    GameObject RandomizeTile()
    {
        float totalWeight = 0f;
        int validTileCount = 0;

        // First pass: count valid tiles and total weight
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileLimitTrackers[i] > 0)
            {
                totalWeight += tileList[i].spawnChance;
                validTileCount++;
            }
        }

        if (validTileCount == 0)
        {
            Debug.LogWarning("No tiles left to spawn (all limits reached). Returning null.");
            return null;
        }

        float rand = Random.Range(0, totalWeight);
        float cumulative = 0f;

        Debug.Log("Total Weight: " + totalWeight + " Random Value: " + rand);

        // Second pass: select a tile based on random value
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileLimitTrackers[i] <= 0)
                continue;

            cumulative += tileList[i].spawnChance;

            // Debug log for the cumulative value
            Debug.Log($"Tile {i}: {tileList[i].tilePrefab.name}, Cumulative: {cumulative}, Random: {rand}");

            if (rand <= cumulative)
            {
                Debug.Log($"Selected Tile: {tileList[i].tilePrefab.name}");
                tileLimitTrackers[i]--;
                return tileList[i].tilePrefab;
            }
        }

        // Fallback (shouldn't be reached)
        Debug.LogError("Tile selection failed. This should not happen.");
        return tileList[0].tilePrefab;
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

    public (int, int, int) TileStatusCounter()
    {
        int obscure = 0;
        int reveal = 0;
        int check = 0;

        foreach (var tile in tileObjects)
        {
            switch (tile.state)
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

    // Gizmos for visualizing the grid in the editor
    void OnDrawGizmos()
    {
        if (grid == null)
            return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * (1 + spacing), y * (1 + spacing), 0);
                Tile tile = GetTile(x, y);

                if (tile != null && tile.isImpassable)
                {
                    Gizmos.color = Color.red; // Red for impassable
                }
                else
                {
                    Gizmos.color = Color.gray; // Default
                }

                Gizmos.DrawWireCube(pos, new Vector3(1, 1, 0.1f));
#if UNITY_EDITOR
                UnityEditor.Handles.Label(pos + new Vector3(0, 0.4f, 0), $"({x},{y})");
#endif
            }
        }
    }


}
