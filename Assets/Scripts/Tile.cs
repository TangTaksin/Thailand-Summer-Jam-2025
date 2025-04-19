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
    private bool isGoal = false;

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
        if (isGoal)
        {
            sr.color = Color.green;
            return;
        }

        switch (state)
        {
            case TileState.Normal:
                sr.color = Color.white;
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
        isGoal = true;
        UpdateTileAppearance();
    }

    public void Reset()
    {
        state = TileState.Normal;
        isGoal = false;
        UpdateTileAppearance();
    }

    private void OnMouseDown()
    {
        if ((state == TileState.Revealed || state == TileState.RevealedLocked) && GameManager.Instance.CanMoveTo(this))
        {
            if (FuelSystem.Instance.UseFuel(1f))
            {
                GameManager.Instance.MovePlayerTo(this);
            }
        }
        else if (state == TileState.Normal && GameManager.Instance.CanReveal(this))
        {
            Reveal();
            GameManager.Instance.AddRevealOption(this);
        }
    }
}
