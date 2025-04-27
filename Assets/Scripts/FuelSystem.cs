using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelSystem : MonoBehaviour
{
    public static FuelSystem Instance;

    [Header("Fuel Settings")]
    public float maxFuel = 100f;
    public float currentFuel = 100f;
    public TextMeshProUGUI fuelText;

    [Header("Play Time UI")]
    public TextMeshProUGUI playTime;

    [Header("Needle UI")]
    public RectTransform fuelNeedle;
    public Image needleRenderer;
    public float fullAngle = 55f;
    public float emptyAngle = -55f;
    public float needleSpeed = 5f;

    [Header("Low Fuel Effects")]
    public float lowFuelThreshold = 20f;
    public float shakeIntensity = 1f;
    public AudioSource lowFuelSound;
    public AudioSource emptyFuelSound;

    [Header("Game Over Settings")]
    public GameObject losePanel;
    public GameObject[] uiGameplay;

    private float targetAngle;
    private bool lowFuelWarningPlayed = false;
    private bool emptyFuelPlayed = false;
    private float elapsedTime = 0f;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        losePanel.SetActive(false);
        SetTargetAngle();
    }

    private void Update()
    {
        SmoothRotateNeedle();
        UpdateNeedleColor();
        HandleLowFuelEffects();
        UpdateFuelText();
        UpdatePlayTime();
    }

    private void UpdateFuelText()
    {
        if (fuelText != null)
            fuelText.text = Mathf.CeilToInt(currentFuel).ToString();
    }

    private void UpdatePlayTime()
    {
        if (isGameOver || GameManager.Instance.IsGameWon) return;

        elapsedTime += Time.deltaTime;

        if (playTime != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            playTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public bool UseFuel(float amount)
    {
        if (currentFuel >= amount)
        {
            currentFuel -= amount;
            SetTargetAngle();
            return true;
        }

        return false;
    }

    public void RefillFuel(float amount)
    {
        currentFuel = Mathf.Clamp(currentFuel + amount, 0, maxFuel);
        lowFuelWarningPlayed = false;
        emptyFuelPlayed = false;
        isGameOver = false;
        SetTargetAngle();
    }

    private void SetTargetAngle()
    {
        float fuelPercent = currentFuel / maxFuel;
        targetAngle = Mathf.Lerp(emptyAngle, fullAngle, fuelPercent);
    }

    private void SmoothRotateNeedle()
    {
        float z = Mathf.LerpAngle(fuelNeedle.localEulerAngles.z, targetAngle, Time.deltaTime * needleSpeed);
        fuelNeedle.localEulerAngles = new Vector3(0f, 0f, z);
    }

    private void UpdateNeedleColor()
    {
        if (needleRenderer == null) return;

        float percent = currentFuel / maxFuel;
        if (percent > 0.5f)
            needleRenderer.color = Color.green;
        else if (percent > 0.2f)
            needleRenderer.color = Color.yellow;
        else
            needleRenderer.color = Color.red;
    }

    private void HandleLowFuelEffects()
    {
        if (currentFuel <= 0f && !GameManager.Instance.IsGameWon)
        {
            if (lowFuelSound != null && lowFuelSound.isPlaying)
                lowFuelSound.Stop();

            if (!emptyFuelPlayed && emptyFuelSound != null)
            {
                emptyFuelSound.Play();
                emptyFuelPlayed = true;
                isGameOver = true;

                if (losePanel != null)
                    uiGameplay[0].SetActive(false);
                uiGameplay[1].SetActive(false);
                losePanel.SetActive(true);
            }

            return;
        }

        if (currentFuel <= lowFuelThreshold)
        {
            float shake = Mathf.Sin(Time.time * 20f) * shakeIntensity;
            fuelNeedle.localRotation *= Quaternion.Euler(0f, 0f, shake);

            if (!lowFuelWarningPlayed && lowFuelSound != null)
            {
                lowFuelSound.Play();
                lowFuelWarningPlayed = true;
            }
        }
        else
        {
            // FUEL is above low threshold, stop the low fuel sound!
            if (lowFuelSound != null && lowFuelSound.isPlaying)
                lowFuelSound.Stop();

            lowFuelWarningPlayed = false; // allow it to play again when fuel drops later
        }
    }

    public void ResetFuel()
    {
        currentFuel = maxFuel;
        lowFuelWarningPlayed = false;
        emptyFuelPlayed = false;
        elapsedTime = 0f;
        isGameOver = false;

        if (lowFuelSound != null) lowFuelSound.Stop();
        if (emptyFuelSound != null) emptyFuelSound.Stop();

        SetTargetAngle();
    }
}
