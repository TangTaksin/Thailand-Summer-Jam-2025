using UnityEngine;

public class BlockageEvent : TileEvents
{
    protected override void Effect()
    {
        var GMInstance = GameManager.Instance;
        GMInstance.MovePlayerTo(GMInstance.lastTile);
    }
}
