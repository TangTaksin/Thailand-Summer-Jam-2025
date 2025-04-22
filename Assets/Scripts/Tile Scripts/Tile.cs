using UnityEngine;
using UnityEngine.Events;
using System;

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
    private bool isGoal = false;

    public Sprite sprite_back, sprite_front;

    public delegate void TileEvent();
    public TileEvent OnEnterTile, OnExitTile;

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
        if (isGoal)
        {
            sr.color = Color.green;
            return;
        }

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

    public void BecomeObscured()
    {
        state = TileState.Obscured;
        UpdateTileAppearance();
    }

    public void BecomeRevealed()
    {
        if (state == TileState.Obscured)
        {
            state = TileState.Revealed;
            UpdateTileAppearance();
        }
    }

    public void BecomeChecked()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.open_Tile_SFX);
        if (state == TileState.Revealed)
        {
            state = TileState.Checked;
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
        state = TileState.Obscured;
        isGoal = false;
        UpdateTileAppearance();
    }

    public void ExitTile()
    {
        OnExitTile?.Invoke();
        print("Exiting tile x " + x + ", y " + y);
    }

    private void OnMouseDown()
    {

        switch (state)
        {
            case TileState.Obscured:
                if (GameManager.Instance.CanReveal(this))
                {
                    BecomeRevealed();
                    GameManager.Instance.AddRevealOption(this);
                }
                break;

            case TileState.Revealed:
                if (GameManager.Instance.CanMoveTo(this))
                    BecomeChecked();
                break;

            case TileState.Checked:
                if (GameManager.Instance.CanMoveTo(this) && FuelSystem.Instance.UseFuel(1f))
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.player_Step_On_Tile);
                    GameManager.Instance.MovePlayerTo(this);

                    //ExecuteEvent
                    print("Entering tile x " + x + ", y " + y);
                    OnEnterTile?.Invoke();
                }
                break;
        }
    }
}
