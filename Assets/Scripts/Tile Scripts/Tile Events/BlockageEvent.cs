using UnityEngine;

public class BlockageEvent : TileEvents
{
    public override void Effect()
    {
        var GMInstance = GameManager.Instance;
        GMInstance.MovePlayerTo(GMInstance.lastTile);
    }
}
