using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class Bake_Minigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Bake_TargetMover targetMover;
    [SerializeField] private TMP_Text Timer_Text;

    [Header("Player Slider Settings")]
    [SerializeField] private float increaseRate = 60f;
    [SerializeField] private float decayRate = 30f;
    [SerializeField] private float angleTolerance = 5f;

    [Header("Progress Settings")]
    [SerializeField] private float holdDurationRequired = 1.5f;
    [SerializeField] private float progressFillRate = 10f;
    [SerializeField] private float progressMaxValue = 100f;

    [Header("Timer Settings")]
    [SerializeField] private int timeLimit = 30;
    private int currentTime;
    private Coroutine timerCoroutine;

    private float holdTimer = 0f;
    private bool canMakeProgress = false;
    private bool gameEnded = false;
    private bool gameStarted = false;

    private Cooking_Minigame_Manager minigameManager;

    void OnEnable()
    {
        progressSlider.value = 0f;
        StartCoroutine(Start_WithAnimation());
    }

    private void OnDisable()
    {
        ResetVisuals();
    }

    private IEnumerator Start_WithAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PregameSequence());
    }


    private IEnumerator PregameSequence()
    {
        targetMover.enabled = true;

        gameStarted = false;
        gameEnded = false;

        // Reset all values
        playerSlider.minValue = 0f;
        playerSlider.maxValue = 360f;
        playerSlider.value = 0f;

        progressSlider.minValue = 0f;
        progressSlider.maxValue = progressMaxValue;
        progressSlider.value = 0f;

        currentTime = timeLimit;

        // Step 1: Reset timer text
        Timer_Text.text = "3";

        // Step 2: Force target to 0°
        targetMover.SetImmediateAngle(0);

        yield return new WaitForSeconds(0.5f);
        yield return targetMover.RotateToAngle(270);
        yield return targetMover.RotateToAngle(0);

        // Step 3: Countdown (3 → 1)
        for (int i = 3; i > 0; i--)
        {
            Timer_Text.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        targetMover.StartTargetMovement();
        StartMinigame();
    }

    private void StartMinigame()
    {
        minigameManager = Cooking_Minigame_Manager.Instance;

        Timer_Text.text = currentTime.ToString();
        gameStarted = true;
        timerCoroutine = StartCoroutine(TimerCountdown());
    }

    private void Update()
    {
        
        if (!gameStarted || gameEnded)
        {
            return;
        }

        else
        {
            HandlePlayerInput();
            HandleProgress();
        }

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

        value = Mathf.Clamp(value, 0f, 360f);
        playerSlider.value = value;
    }

    private void HandleProgress()
    {
        float sliderAngle = playerSlider.value;
        int targetAngle = targetMover.currentAngle;
        bool targetMoving = targetMover.isMoving;

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
                OnMinigameSuccess(); //Success before time runs out
            }
        }
    }

    private bool IsAngleWithinRange(float value, float min, float max)
    {
        return min < max ? value >= min && value <= max : value >= min || value <= max;
    }

    private IEnumerator TimerCountdown()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);

            if (gameEnded) yield break;

            currentTime--;
            Timer_Text.text = currentTime.ToString();
        }

       
            if (progressSlider.value >= progressMaxValue)
            {
                OnMinigameSuccess(); //Success exactly when timer runs out
            }

            else
            {
                OnMinigameFail(); //Timer ran out, progress not 100 yet
            }
        
    }

    public void StopMinigame()
    {
        Debug.Log("Stop");

        gameEnded = true;
        targetMover.StopTargetMovement();

        // Stop timer coroutine
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        // Reset gameplay variables
        holdTimer = 0f;
        canMakeProgress = false;
        //playerSlider.value = 0f;
        //progressSlider.value = 0f;
        
    }

    


    private void OnMinigameSuccess()
    {
        StopMinigame();
        minigameManager.CookingMinigameComplete();
    }

    private void OnMinigameFail()
    {
        StopMinigame();
        minigameManager.LoseHealth();
        minigameManager.CookingMinigameComplete();

    }

    private void ResetVisuals()
    {
        
        targetMover.ResetTarget();
        gameEnded = false;
        
        playerSlider.value = 0f;
        
        Timer_Text.text = "3";
    }
}
