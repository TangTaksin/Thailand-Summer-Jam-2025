using UnityEngine;

public class GoalEvent : TileEvents
{
    public int tileNeeded;
    GridManager gridManager;

    private void OnEnable()
    {
        gridManager = GetComponentInParent<GridManager>();
    }



    protected override void Effect()
    {
        CheckedTileCounter();
    }

    void CheckedTileCounter()
    {
        int count = 0;
        //print("Checking...");
        foreach (var tile in gridManager.tileObjects)
        {
            if (tile.state == Tile.TileState.Checked)
                count++;
        }

        if (count >= tileNeeded)
        {

        }
    }
}
