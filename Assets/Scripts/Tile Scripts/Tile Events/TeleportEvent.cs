using System.Collections.Generic;
using UnityEngine;

public class TeleportEvent : TileEvents
{
    private Tile currentTile;
    private GridManager gridManager;

    private void Awake()
    {
        currentTile = GetComponent<Tile>();
        gridManager = FindFirstObjectByType<GridManager>(); // Locate GridManager in the scene
    }

    public override void Effect()
    {
        var validTargetTiles = new List<Tile>();

        if (!gridManager)
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }

        foreach (var tile in gridManager.tileObjects)
        {
            // Skip if null, self, or not checked
            if (tile == null || tile == currentTile || tile.state != Tile.TileState.Checked)
                continue;

            // Skip if the tile has a TeleportEvent component
            if (tile.GetComponent<TeleportEvent>() != null)
                continue;

            validTargetTiles.Add(tile);
        }

        if (validTargetTiles.Count > 0)
        {
            Tile targetTile = validTargetTiles[Random.Range(0, validTargetTiles.Count)];
            GameManager.Instance.MovePlayerTo(targetTile);
            targetTile.OnEnterTile?.Invoke(); // Trigger enter event
            Debug.Log($"✅ Teleported to tile at ({targetTile.x}, {targetTile.y})");
        }
        else
        {
            GameManager.Instance.MovePlayerTo(GameManager.Instance.lastTile); // Cancel move
            Debug.Log("❌ No valid non-teleport tiles to teleport to. Returned to last tile.");
        }
    }
}
