using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public TileChance[] tileList;
    public int width = 5;
    public int height = 6;
    public float spacing = 0.1f;

    public int tileNeeded;

    private Tile[,] grid;
    private List<int> tileLimitTrackers = new List<int>();
    private Dictionary<GameObject, bool> tilePassabilityCache = new Dictionary<GameObject, bool>();

    public delegate void GridManagerEvent();
    public static GridManagerEvent OnGenerated;

    void Awake()
    {
        CacheImpassableTiles();
        GetLimit();
    }

    void CacheImpassableTiles()
    {
        tilePassabilityCache.Clear();
        foreach (var entry in tileList)
        {
            GameObject prefab = entry.tilePrefab;
            if (!tilePassabilityCache.ContainsKey(prefab))
            {
                Tile tile = prefab.GetComponent<Tile>();
                tilePassabilityCache[prefab] = tile == null ? false : tile.isImpassable;
            }
        }
    }

    void GetLimit()
    {
        tileLimitTrackers.Clear();
        foreach (var tile in tileList)
            tileLimitTrackers.Add(tile.limitPerMap);
    }

    void ResetLimit()
    {
        for (int i = 0; i < tileLimitTrackers.Count; i++)
            tileLimitTrackers[i] = tileList[i].limitPerMap;
    }

    public void GenerateGrid()
    {
        ResetLimit();

        if (grid != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] != null)
                        Destroy(grid[x, y].gameObject);
                }
            }
        }

        grid = new Tile[width, height];

        var path = BuildPath();
        foreach (var pos in path)
        {
            SpawnTile(pos.x, pos.y, isPathTile: true);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                    SpawnTile(x, y, isPathTile: false);
            }
        }

        OnGenerated?.Invoke();
    }

    List<Vector2Int> BuildPath()
    {
        List<Vector2Int> path = new List<Vector2Int>();
        int x = 0, y = 0;
        path.Add(new Vector2Int(x, y));

        while (x < width - 1 || y < height - 1)
        {
            bool right = x < width - 1;
            bool up = y < height - 1;

            if (right && up)
                (Random.value < 0.5f ? ref x : ref y)++;
            else if (right)
                x++;
            else
                y++;

            path.Add(new Vector2Int(x, y));
        }

        return path;
    }

    void SpawnTile(int x, int y, bool isPathTile)
    {
        Vector3 pos = new Vector3(x + x * spacing, y + y * spacing, 0);
        GameObject prefab = GetRandomTilePrefab(isPathTile);
        GameObject go = Instantiate(prefab, pos, Quaternion.identity, transform);
        Tile tile = go.GetComponent<Tile>();
        tile.Init(x, y, this);

        if (isPathTile)
            tile.isImpassable = false;

        grid[x, y] = tile;
    }

    GameObject GetRandomTilePrefab(bool forcePassable)
    {
        float range = 0f;
        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileLimitTrackers[i] > 0)
                range += tileList[i].spawnChance;
        }

        float random = Random.Range(0f, range);
        float sum = 0f;

        for (int i = 0; i < tileList.Length; i++)
        {
            if (tileLimitTrackers[i] <= 0) continue;

            sum += tileList[i].spawnChance;
            if (random < sum)
            {
                GameObject prefab = tileList[i].tilePrefab;
                bool isImpassable = tilePassabilityCache[prefab];

                if (forcePassable && isImpassable)
                    continue;

                tileLimitTrackers[i]--;
                return prefab;
            }
        }

        // fallback
        return tileList[0].tilePrefab;
    }

    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return grid[x, y];
        return null;
    }

    public (int obscure, int revealed, int checkedTiles) TileStatusCounter()
    {
        int obscure = 0, revealed = 0, checkedTiles = 0;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Tile tile = grid[x, y];
                if (tile == null) continue;

                switch (tile.state)
                {
                    case Tile.TileState.Obscured: obscure++; break;
                    case Tile.TileState.Revealed: revealed++; break;
                    case Tile.TileState.Checked: checkedTiles++; break;
                }
            }
        }

        return (obscure, revealed, checkedTiles);
    }

    public IEnumerable<Tile> GetAllTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tile = grid[x, y];
                if (tile != null)
                    yield return tile;
            }
        }
    }

}
