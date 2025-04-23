using UnityEngine;

public class SwitchDescriptionEvent : TileEvents
{
    [TextArea] public string newDescInfo;
    Tile tile;

    private void Start()
    {
        tile = GetComponent<Tile>();
    }

    protected override void Effect()
    {
        tile.infoDescription = newDescInfo;
    }
}
