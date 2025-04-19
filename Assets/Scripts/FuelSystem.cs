using UnityEngine;

public class FuelSystem : MonoBehaviour
{
    public static FuelSystem Instance;

    [Header("Fuel Settings")]
    public float maxFuel = 100f;
    public float currentFuel = 100f;

    [Header("Needle UI")]
    public Transform fuelNeedle;
    public SpriteRenderer needleRenderer;
    public float fullAngle = 55f;
    public float emptyAngle = -55f;
    public float needleSpeed = 5f;

    [Header("Low Fuel Effects")]
    public float lowFuelThreshold = 20f;
    public float shakeIntensity = 1f;
    public AudioSource lowFuelSound;
    public AudioSource emptyFuelSound;

    private float targetAngle;
    private bool lowFuelWarningPlayed = false;
    private bool emptyFuelPlayed = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SetTargetAngle();
    }

    private void Update()
    {
        SmoothRotateNeedle();
        UpdateNeedleColor();
        HandleLowFuelEffects();
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
        fuelNeedle.localRotation = Quaternion.Euler(0f, 0f, z);
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
        if (currentFuel <= 0f)
        {
            if (lowFuelSound != null && lowFuelSound.isPlaying)
                lowFuelSound.Stop();

            if (!emptyFuelPlayed && emptyFuelSound != null)
            {
                emptyFuelSound.Play();
                emptyFuelPlayed = true;
            }

            return; // Don't shake or warn if out of fuel
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
    }
}
