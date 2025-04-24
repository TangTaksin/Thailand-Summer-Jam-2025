using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GridManager gridManager;
    public GameObject playerPrefab;
    public Button startButton;

    private GameObject playerObject;
    [HideInInspector] public Tile lastTile;
    [HideInInspector] public Tile currentTile;

    [Header("Decision Timer")]
    public Image decisionTimerBar;      // UI fill bar (assign in Inspector)
    public float decisionTime = 5f;     // How long the player has to decide
    public float fuelPenalty = 5f;      // Fuel lost if timer runs out
    private Coroutine decisionTimerCoroutine;
    private bool hasMoved = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (startButton != null)
            startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        gridManager.GenerateGrid();

        FuelSystem.Instance.ResetFuel(); 
        // Reset all tiles before starting a new game
        //gridManager.ResetAllTiles();

        if (playerObject != null)
        {
            Destroy(playerObject);
            playerObject = null;
        }

        // Set a random start tile (bottom row)
        int startX = Random.Range(0, gridManager.width);
        int startY = Random.Range(0, gridManager.height);

        Tile startTile = gridManager.GetTile(startX, startY);
        startTile.BecomeRevealed();

        // Spawn player
        playerObject = Instantiate(playerPrefab, startTile.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        currentTile = startTile;
        startTile.BecomeChecked();

        RevealAdjacentTiles(currentTile);
        RestartDecisionTimer(); // Start first decision timer
    }

    public bool CanReveal(Tile tile)
    {
        return IsAdjacent(tile, currentTile) && tile.state == Tile.TileState.Obscured;
    }

    public bool CanMoveTo(Tile tile)
    {
        return IsAdjacent(tile, currentTile) &&
               (tile.state == Tile.TileState.Revealed || tile.state == Tile.TileState.Checked);
    }

    public void AddRevealOption(Tile tile) { }

    public void MovePlayerTo(Tile tile)
    {
        //currentTile.BecomeChecked();
        //Unreveal Last tile's Adjacent
        UnRevealLastAdjacentTiles(currentTile);
        currentTile.ExitTile();
        lastTile = currentTile;

        hasMoved = true;

        currentTile = tile;
        playerObject.transform.position = tile.transform.position + new Vector3(0, 0, -1);

        RevealAdjacentTiles(currentTile);
        RestartDecisionTimer(); // Restart timer for next move
    }


    void RevealAdjacentTiles(Tile tile)
    {
        TryReveal(tile.x + 1, tile.y);
        TryReveal(tile.x - 1, tile.y);
        TryReveal(tile.x, tile.y + 1);
        TryReveal(tile.x, tile.y - 1);
    }

    void TryReveal(int x, int y)
    {
        Tile tile = gridManager.GetTile(x, y);
        if (tile != null)
            tile.BecomeRevealed();
    }

    void UnRevealLastAdjacentTiles(Tile tile)
    {
        TryUnReveal(tile.x + 1, tile.y);
        TryUnReveal(tile.x - 1, tile.y);
        TryUnReveal(tile.x, tile.y + 1);
        TryUnReveal(tile.x, tile.y - 1);
    }

    void TryUnReveal(int x, int y)
    {
        Tile tile = gridManager.GetTile(x, y);
        if (tile != null && tile.state == Tile.TileState.Revealed)
            tile.BecomeObscured();
    }

    bool IsAdjacent(Tile a, Tile b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) == 1;
    }


    void UpdateTimerBar(float percent)
    {
        if (decisionTimerBar != null)
        {
            decisionTimerBar.fillAmount = percent;
        }
    }

    IEnumerator DecisionTimer()
    {
        hasMoved = false;

        while (!hasMoved)
        {
            float timer = decisionTime;

            while (timer > 0f && !hasMoved)
            {
                timer -= Time.deltaTime;
                UpdateTimerBar(timer / decisionTime);
                yield return null;
            }

            if (!hasMoved)
            {
                FuelSystem.Instance.UseFuel(fuelPenalty);
                Debug.Log("⛽ Time's up! Fuel lost.");
                UpdateTimerBar(0f);
            }
        }

        UpdateTimerBar(0f);
    }

    public void RestartDecisionTimer()
    {
        StopDecisionTimer();
        decisionTimerCoroutine = StartCoroutine(DecisionTimer());
    }

    public void StopDecisionTimer()
    {
        if (decisionTimerCoroutine != null)
        {
            StopCoroutine(decisionTimerCoroutine);
            decisionTimerCoroutine = null;
        }
    }
}
