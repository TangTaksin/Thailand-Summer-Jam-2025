using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public int rows = 5;
    public int cols = 5;
    public float spacing = 1.1f; // space between tiles

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector2 spawnPos = new Vector2(x * spacing, y * spacing);
                Instantiate(tilePrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
