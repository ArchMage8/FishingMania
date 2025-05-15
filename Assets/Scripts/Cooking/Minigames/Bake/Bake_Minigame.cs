using UnityEngine;
using UnityEngine.UI;

public class Bake_Minigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider playerSlider;         // Circular angle slider (0–360)
    [SerializeField] private Slider progressSlider;       // Vertical progress bar
    [SerializeField] private Bake_TargetMover targetMover; // Reference to get current target info

    [Header("Player Slider Settings")]
    [SerializeField] private float increaseRate = 60f;    // Degrees per second when holding F
    [SerializeField] private float decayRate = 30f;       // Degrees per second when not holding F
    [SerializeField] private float angleTolerance = 5f;   // +/- range around target angle

    [Header("Progress Settings")]
    [SerializeField] private float holdDurationRequired = 1.5f; // Seconds to hold before progress begins
    [SerializeField] private float progressFillRate = 10f;      // Progress per second
    [SerializeField] private float progressMaxValue = 100f;

    private float holdTimer = 0f;
    private bool canMakeProgress = false;

    private void Start()
    {
        playerSlider.minValue = 0f;
        playerSlider.maxValue = 360f;

        progressSlider.minValue = 0f;
        progressSlider.maxValue = progressMaxValue;
        progressSlider.value = 0f;
    }

    private void Update()
    {
        HandlePlayerInput();
        HandleProgress();
    }

    private void HandlePlayerInput()
    {
        float value = playerSlider.value;

        if (Input.GetKey(KeyCode.F))
        {
            value += increaseRate * Time.deltaTime;
        }
        else
        {
            value -= decayRate * Time.deltaTime;
        }

        value = Mathf.Clamp(value, 0f, 360f); // FIXED: Prevent reset to 0
        playerSlider.value = value;
    }

    private void HandleProgress()
    {
        float sliderAngle = playerSlider.value;
        int targetAngle = targetMover.currentAngle;
        bool targetMoving = targetMover.isMoving;

        // Calculate if within tolerance range
        float minAngle = (targetAngle - angleTolerance + 360f) % 360f;
        float maxAngle = (targetAngle + angleTolerance) % 360f;

        bool isInRange = IsAngleWithinRange(sliderAngle, minAngle, maxAngle);

        if (isInRange && !targetMoving)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdDurationRequired)
            {
                canMakeProgress = true;
            }
        }
        else
        {
            holdTimer = 0f;
            canMakeProgress = false;
        }

        if (canMakeProgress)
        {
            progressSlider.value += progressFillRate * Time.deltaTime;

            if (progressSlider.value >= progressMaxValue)
            {
                CompleteMinigame();
            }
        }
    }

    private bool IsAngleWithinRange(float value, float min, float max)
    {
        // Handle wraparound between 359° → 0°
        if (min < max)
        {
            return value >= min && value <= max;
        }
        else
        {
            return value >= min || value <= max;
        }
    }

    private void CompleteMinigame()
    {
        Debug.Log("Minigame Complete!");
        enabled = false;
        // Optional: Trigger events or callback here
    }
}
