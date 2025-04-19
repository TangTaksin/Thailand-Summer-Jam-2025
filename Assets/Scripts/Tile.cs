using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileState
    {
        Normal,
        Revealed,
        RevealedLocked
    }

    public int x, y;
    public TileState state = TileState.Normal;
    private GridManager gridManager;
    private SpriteRenderer sr;

    public void Init(int x, int y, GridManager grid)
    {
        this.x = x;
        this.y = y;
        gridManager = grid;
        sr = GetComponent<SpriteRenderer>();
        UpdateTileAppearance();
    }

    public void UpdateTileAppearance()
    {
        switch (state)
        {
            case TileState.Normal:
                sr.color = Color.black;
                break;
            case TileState.Revealed:
                sr.color = Color.gray;
                break;
            case TileState.RevealedLocked:
                sr.color = Color.yellow;
                break;
        }
    }

    public void Reveal()
    {
        if (state == TileState.Normal)
        {
            state = TileState.Revealed;
            UpdateTileAppearance();
        }
    }

    public void LockTile()
    {
        if (state == TileState.Revealed)
        {
            state = TileState.RevealedLocked;
            UpdateTileAppearance();
        }
    }

    public void SetAsGoal()
    {
        sr.color = Color.green;
    }

    private void OnMouseDown()
    {
        if ((state == TileState.Revealed || state == TileState.RevealedLocked) && GameManager.Instance.CanMoveTo(this))
        {
            GameManager.Instance.MovePlayerTo(this);
        }
        else if (state == TileState.Normal && GameManager.Instance.CanReveal(this))
        {
            Reveal();
            GameManager.Instance.AddRevealOption(this);
        }
    }
}