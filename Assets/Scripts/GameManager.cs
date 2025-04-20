using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GridManager gridManager;
    public GameObject playerPrefab;
    public Button startButton;

    private GameObject playerObject;
    private Tile currentTile;
    private Tile goalTile;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        if (playerObject != null)
            Destroy(playerObject);

        int goalX = Random.Range(0, gridManager.width);
        int goalY = gridManager.height - 1;
        goalTile = gridManager.GetTile(goalX, goalY);
        goalTile.BecomeRevealed();
        goalTile.SetAsGoal();

        int startX = Random.Range(0, gridManager.width);
        int startY = 0;
        Tile startTile = gridManager.GetTile(startX, startY);
        startTile.BecomeRevealed();

        playerObject = Instantiate(playerPrefab, startTile.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        currentTile = startTile;
        startTile.BecomeChecked();

        RevealAdjacentTiles(currentTile);
    }

    public bool CanReveal(Tile tile)
    {
        return IsAdjacent(tile, currentTile) && tile.state == Tile.TileState.Obscured;
    }

    public bool CanMoveTo(Tile tile)
    {
        return IsAdjacent(tile, currentTile) &&
               (tile.state == Tile.TileState.Revealed || tile.state == Tile.TileState.Checked);
    }

    public void AddRevealOption(Tile tile)
    {
        // You can track reveal options if needed
    }

    public void MovePlayerTo(Tile tile)
    {
        //currentTile.BecomeChecked();
        //Unreveal Last tile's Adjacent
        UnRevealLastAdjacentTiles(currentTile);

        currentTile = tile;
        playerObject.transform.position = tile.transform.position + new Vector3(0, 0, -1);

        if (tile == goalTile)
        {
            Debug.Log("🎉 You reached the goal!");
            return;
        }

        RevealAdjacentTiles(currentTile);
    }

    void RevealAdjacentTiles(Tile tile)
    {
        TryReveal(tile.x + 1, tile.y);
        TryReveal(tile.x - 1, tile.y);
        TryReveal(tile.x, tile.y + 1);
        TryReveal(tile.x, tile.y - 1);
    }

    void TryReveal(int x, int y)
    {
        Tile tile = gridManager.GetTile(x, y);
        if (tile != null)
            tile.BecomeRevealed();
    }

    void UnRevealLastAdjacentTiles(Tile tile)
    {
        TryUnReveal(tile.x + 1, tile.y);
        TryUnReveal(tile.x - 1, tile.y);
        TryUnReveal(tile.x, tile.y + 1);
        TryUnReveal(tile.x, tile.y - 1);
    }

    void TryUnReveal(int x, int y)
    {
        Tile tile = gridManager.GetTile(x, y);
        if (tile != null && tile.state == Tile.TileState.Revealed)
            tile.BecomeObcsure();
    }

    bool IsAdjacent(Tile a, Tile b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) == 1;
    }
}
