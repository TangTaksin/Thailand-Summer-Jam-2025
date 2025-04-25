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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        attachedTile = GetComponent<Tile>();

        if (spriteRenderer != null && normalSprite != null)
        {
            base.attachedTile.sprite_front = normalSprite;
            spriteRenderer.sprite = normalSprite;
        }
            
    }

    public override void Effect()
    {
        var gameManager = GameManager.Instance;

        if (isBroken)
        {
            gameManager.MovePlayerTo(attachedTile);
            return;
        }

        currentSteps++;

        if (currentSteps == 1)
        {
            if (crackedSprite != null)
            {
                base.attachedTile.sprite_front = crackedSprite;
                spriteRenderer.sprite = crackedSprite;
            }
            gameManager.MovePlayerTo(gameManager.lastTile); // Cancel move
        }
        else if (currentSteps >= maxSteps)
        {
            isBroken = true;

            if (brokenSprite != null)
            {
                base.attachedTile.sprite_front = brokenSprite;
                spriteRenderer.sprite = brokenSprite;
            }

            gameManager.MovePlayerTo(attachedTile); // Allow move
        }
    }

    public override void Effect(Tile attachtile)
    {
        var gameManager = GameManager.Instance;

        if (isBroken)
        {
            gameManager.MovePlayerTo(attachtile);
            return;
        }

        currentSteps++;

        if (currentSteps == 1)
        {
            if (crackedSprite != null)
            {
                attachtile.sprite_front = crackedSprite;
                //spriteRenderer.sprite = crackedSprite;
            }
            gameManager.MovePlayerTo(gameManager.lastTile); // Cancel move
        }
        else if (currentSteps >= maxSteps)
        {
            isBroken = true;

            if (brokenSprite != null)
            {
                attachtile.sprite_front = brokenSprite;
                //spriteRenderer.sprite = brokenSprite;
            }

            gameManager.MovePlayerTo(attachedTile); // Allow move
        }
    }
}
