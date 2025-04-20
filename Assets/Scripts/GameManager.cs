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
    private Tile currentTile;
    private Tile goalTile;

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
        FuelSystem.Instance.ResetFuel();
        gridManager.ResetAllTiles();

        if (playerObject != null)
        {
            Destroy(playerObject);
            playerObject = null;
        }

        goalTile = null;

        // Set a random goal tile (top row)
        int goalX = Random.Range(0, gridManager.width);
        int goalY = gridManager.height - 1;
        goalTile = gridManager.GetTile(goalX, goalY);
        goalTile.Reveal();
        goalTile.SetAsGoal();

        // Set a random start tile (bottom row)
        int startX = Random.Range(0, gridManager.width);
        int startY = 0;
        Tile startTile = gridManager.GetTile(startX, startY);
        startTile.Reveal();

        // Spawn player
        playerObject = Instantiate(playerPrefab, startTile.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        currentTile = startTile;

        RevealAdjacentTiles(currentTile);
        RestartDecisionTimer(); // Start first decision timer
    }

    public bool CanReveal(Tile tile)
    {
        return IsAdjacent(tile, currentTile) && tile.state == Tile.TileState.Normal;
    }

    public bool CanMoveTo(Tile tile)
    {
        return IsAdjacent(tile, currentTile) &&
               (tile.state == Tile.TileState.Revealed || tile.state == Tile.TileState.RevealedLocked);
    }

    public void AddRevealOption(Tile tile) { }

    public void MovePlayerTo(Tile tile)
    {
        hasMoved = true;
        currentTile.LockTile();
        currentTile = tile;
        playerObject.transform.position = tile.transform.position + new Vector3(0, 0, -1);

        if (tile == goalTile)
        {
            Debug.Log("🎉 You reached the goal!");
            StopDecisionTimer();
            return;
        }

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
            tile.Reveal();
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
