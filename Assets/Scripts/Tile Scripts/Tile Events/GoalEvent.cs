using UnityEngine;

public class GoalEvent : TileEvents
{
    public Sprite unlockedGoalSprite, lockedGoalSprite;
    int tileNeeded;
    bool canEnter;

    GridManager gridManager;
    SpriteRenderer spriteRend;

    public delegate void Event();
    public static Event OnGoal;

    protected override void OnEnable()
    {
        base.OnEnable();

        gridManager = GetComponentInParent<GridManager>();
        spriteRend = GetComponent<SpriteRenderer>();

        tileNeeded = gridManager.tileNeeded;
        Tile.OnChecked += TileCheck;
    }

    private void Start()
    {
        TileCheck();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        Tile.OnChecked -= TileCheck; 
    }

    protected override void Effect()
    {
        var GMInstance = GameManager.Instance;

        if (canEnter)
        {
            OnGoal?.Invoke();
            GameManager.Instance.StopDecisionTimer();
        }
        else
        {
            GMInstance.MovePlayerTo(GMInstance.lastTile);
        }
    }

    void TileCheck()
    {
        var counter = gridManager.TileStatusCounter();
        var checkedTile = counter.Item3;

        canEnter = checkedTile >= tileNeeded;

        if (canEnter)
        {
            spriteRend.sprite = unlockedGoalSprite;
        }
        else
        {
            spriteRend.sprite = lockedGoalSprite;
        }
    }
}
