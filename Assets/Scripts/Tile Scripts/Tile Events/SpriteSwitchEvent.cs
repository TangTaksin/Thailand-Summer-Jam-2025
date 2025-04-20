using UnityEngine;

public class SpriteSwitchEvent : TileEvents
{
    public Sprite newSprite;
    SpriteRenderer spriteRen;

    private void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
    }

    protected override void Effect()
    {
        spriteRen.sprite = newSprite;
    }
}
