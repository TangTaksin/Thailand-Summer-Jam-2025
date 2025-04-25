using UnityEngine;

public class BreakableEvent : TileEvents
{
    [Header("Break Settings")]
    public int maxSteps = 2;
    private int currentSteps = 0;
    private bool isBroken = false;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite crackedSprite;
    public Sprite brokenSprite;

    private SpriteRenderer spriteRenderer;
    private Tile tile;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tile = GetComponent<Tile>();

        if (spriteRenderer != null && normalSprite != null)
            spriteRenderer.sprite = normalSprite;
    }

    public override void Effect()
    {
        var gameManager = GameManager.Instance;

        if (isBroken)
        {
            gameManager.MovePlayerTo(tile);
            return;
        }

        currentSteps++;

        if (currentSteps == 1)
        {
            if (crackedSprite != null)
                spriteRenderer.sprite = crackedSprite;

            gameManager.MovePlayerTo(gameManager.lastTile); // Cancel move
        }
        else if (currentSteps >= maxSteps)
        {
            isBroken = true;

            if (brokenSprite != null)
                spriteRenderer.sprite = brokenSprite;

            gameManager.MovePlayerTo(tile); // Allow move
        }
    }
}
