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

    public override void Effect()
    {
        if (canEnter)
        {
            // Goal unlocked: win the game
            OnGoal?.Invoke();
            GameManager.Instance.StopDecisionTimer();
        }
        else
        {
            // Goal locked: do nothing special
            // (player can stand here but nothing happens)
        }
    }

    void TileCheck()
    {
        var counter = gridManager.TileStatusCounter();
        var checkedTile = counter.Item3;

        canEnter = checkedTile >= tileNeeded;

        spriteRend.sprite = canEnter ? unlockedGoalSprite : lockedGoalSprite;
    }
}
