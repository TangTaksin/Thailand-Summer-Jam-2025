using UnityEngine;

public class SwitchInfoEvent : TileEvents
{
    public string newCursorInfo;
    Tile tile;

    private void Start()
    {
        tile = GetComponent<Tile>();
    }

    public override void Effect()
    {
        tile.infoCursor = newCursorInfo;
    }
}
