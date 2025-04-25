using UnityEngine;

public class SlideEvent : TileEvents
{
    GridManager gridManager;

    public enum DirectionType{
        ice,
        up,
        down,
        left,
        right
    }
    public DirectionType directionType;

    private void Awake()
    {
        gridManager = GetComponentInParent<GridManager>();
    }

    public override void Effect()
    {
        var gameManager = GameManager.Instance;

        if (!gridManager)
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }

        print("transform position: " + transform.position);

        var lastTile = gameManager.lastTile;
        var attachTile = new Vector2Int(attachedTile.x, attachedTile.y);
        var desPos = Vector2Int.zero;

        Tile Destination = null;

        switch (directionType)
        {
            case DirectionType.ice:
                var direction = new Vector2Int(attachedTile.x, attachedTile.y) 
                            - new Vector2Int(lastTile.x, lastTile.y);
                desPos = attachTile + direction;

                break;

            case DirectionType.up:
                desPos = attachTile + new Vector2Int(0, 1);
                break;

            case DirectionType.down:
                desPos = attachTile + new Vector2Int(0, -1);
                break;

            case DirectionType.left:
                desPos = attachTile + new Vector2Int(-1, 0);
                break;

            case DirectionType.right:
                desPos = attachTile + new Vector2Int(1, 0);
                break;
        }

        Destination = gridManager.GetTile(desPos.x, desPos.y);
        if (Destination)
        {
            gameManager.MovePlayerTo(Destination);
            Destination.EnterTile();
        }
        else
        {
            gameManager.MovePlayerTo(attachedTile);
        }

    }

    public override void Effect(Tile attachedTile)
    {
        var gameManager = GameManager.Instance;

        if (!gridManager)
        {
            gridManager = FindFirstObjectByType<GridManager>();
        }

        print("transform position: " + transform.position);

        var lastTile = gameManager.lastTile;
        var attachTile = new Vector2Int(attachedTile.x, attachedTile.y);
        var desPos = Vector2Int.zero;

        Tile Destination = null;

        switch (directionType)
        {
            case DirectionType.ice:
                var direction = new Vector2Int(attachedTile.x, attachedTile.y)
                            - new Vector2Int(lastTile.x, lastTile.y);
                desPos = attachTile + direction;

                break;

            case DirectionType.up:
                desPos = attachTile + new Vector2Int(0, 1);
                break;

            case DirectionType.down:
                desPos = attachTile + new Vector2Int(0, -1);
                break;

            case DirectionType.left:
                desPos = attachTile + new Vector2Int(-1, 0);
                break;

            case DirectionType.right:
                desPos = attachTile + new Vector2Int(1, 0);
                break;
        }

        Destination = gridManager.GetTile(desPos.x, desPos.y);
        if (Destination)
        {
            gameManager.MovePlayerTo(Destination);
            Destination.EnterTile();
        }
        else
        {
            gameManager.MovePlayerTo(attachedTile);
        }

    }
}
