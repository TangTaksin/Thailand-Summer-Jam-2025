using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileState
    {
        Obscured,
        Revealed,
        Checked
    }

    public int x, y;
    public TileState state = TileState.Obscured;
    private GridManager gridManager;
    private SpriteRenderer sr;

    public Sprite sprite_back, sprite_front;

    Color defaultColor;

    public void Init(int x, int y, GridManager grid)
    {
        this.x = x;
        this.y = y;
        gridManager = grid;
        sr = GetComponent<SpriteRenderer>();

        defaultColor = sr.color;

        UpdateTileAppearance();
    }

    public void UpdateTileAppearance()
    {
        switch (state)
        {
            case TileState.Obscured:
                sr.sprite = sprite_back;
                sr.color = defaultColor;
                break;
            case TileState.Revealed:
                sr.color = defaultColor/2;
                break;
            case TileState.Checked:
                sr.sprite = sprite_front;
                sr.color = defaultColor;
                break;
        }
    }

    public void Reveal()
    {
        if (state == TileState.Obscured)
        {
            state = TileState.Revealed;
            UpdateTileAppearance();
        }
    }

    public void BecomeChecked()
    {
        if (state == TileState.Revealed)
        {
            state = TileState.Checked;
            UpdateTileAppearance();
        }
    }

    public void SetAsGoal()
    {
        sr.color = Color.green;
    }

    private void OnMouseDown()
    {

        switch (state)
        {
            case TileState.Obscured:
                if (GameManager.Instance.CanReveal(this))
                {
                    Reveal();
                    GameManager.Instance.AddRevealOption(this);
                }
                break;

            case TileState.Revealed:
                if (GameManager.Instance.CanMoveTo(this))
                    BecomeChecked();
                break;

            case TileState.Checked:
                if (GameManager.Instance.CanMoveTo(this))
                    GameManager.Instance.MovePlayerTo(this);
                break;
        }
    }
}