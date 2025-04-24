using UnityEngine;

public class SpriteSwitchEvent : TileEvents
{
    public Sprite newSprite;
    SpriteRenderer spriteRen;

    private void Start()
    {
        spriteRen = GetComponent<SpriteRenderer>();
    }

    public override void Effect()
    {
        spriteRen.sprite = newSprite;
    }
}
