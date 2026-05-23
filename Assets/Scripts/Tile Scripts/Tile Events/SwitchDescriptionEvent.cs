using UnityEngine;

public class SwitchDescriptionEvent : TileEvents
{
    [TextArea] public string newDescInfo;
    Tile tile;

    private void Start()
    {
        tile = GetComponent<Tile>();
    }

    public override void Effect()
    {
        tile.infoDescription = newDescInfo;
    }
}
