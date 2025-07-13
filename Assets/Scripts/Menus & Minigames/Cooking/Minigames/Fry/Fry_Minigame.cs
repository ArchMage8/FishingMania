using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class Fry_Minigame : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform barTransform;
    public RectTransform sectionTransform;
    public Fry_Marker marker;

    [Space(10)]

    public Slider progressSlider;
    public TextMeshProUGUI countdownText;

    [Header("Gameplay Settings")]
    public float sectionShrinkAmount = 10f;
    public float progressMaxValue = 100f;
    public float progressPerHit = 20f;

    [Header("Coyote Time Settings")]
    public float coyoteTimeDuration = 0.05f;
    private float lastTimeInSection = -Mathf.Infinity;

    private bool isTop = true;
    private float originalSectionY;
    private bool isPregameActive = true;
    private float originalSectionHeight;
    private bool inProgress = false;

    void OnEnable()
    {
        StartCoroutine(Start_WithAnimation());
    }

    private IEnumerator Start_WithAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        Start_Minigame();
    }

    private void OnDisable()
    {
        Reset_Minigame();
    }

    public void Start_Minigame()
    {
        inProgress = true;

        

        isTop = true;
        lastTimeInSection = -Mathf.Infinity;
        isPregameActive = true;

        if (sectionTransform != null)
            originalSectionY = sectionTransform.localPosition.y;

        if (progressSlider != null)
        {
            progressSlider.maxValue = progressMaxValue;
            progressSlider.value = 0f;
        }

        if (marker != null)
            marker.Initialize(this);

        MoveSection(isTop);

        if (sectionTransform != null)
        {
            originalSectionY = sectionTransform.localPosition.y;
            originalSectionHeight = sectionTransform.rect.height; // Store initial height
        }

        StartCoroutine(RunPregamePhase());
    }

    void Update()
    {
        if (isPregameActive) return;

        UpdateCoyoteTime();

        if (Cooking_Minigame_Manager.Instance.health == 0 && inProgress == true)
        {
            inProgress = false;
            End_Fail();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (IsMarkerWithinSection() || IsWithinCoyoteTime())
            {
                HandleHit();
            }
            else
            {
                HandleMiss();
            }
        }
    }

    private void UpdateCoyoteTime()
    {
        if (IsMarkerWithinSection())
            lastTimeInSection = Time.time;
    }

    private bool IsMarkerWithinSection()
    {
        Bounds sectionBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(barTransform, sectionTransform);
        float markerY = marker.markerTransform.localPosition.y;
        return markerY >= sectionBounds.min.y && markerY <= sectionBounds.max.y;
    }

    private bool IsWithinCoyoteTime()
    {
        return Time.time - lastTimeInSection <= coyoteTimeDuration;
    }

    private void HandleHit()
    {
        float newHeight = Mathf.Max(sectionTransform.rect.height - sectionShrinkAmount, 5f);
        sectionTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        isTop = !isTop;
        MoveSection(isTop);

        if (progressSlider != null)
        {
            progressSlider.value += progressPerHit;
            if (progressSlider.value >= progressSlider.maxValue)
            {
                End_Success();
            }
        }
    }

    private void HandleMiss()
    {
        Cooking_Minigame_Manager.Instance.LoseHealth();
    }

    private void MoveSection(bool moveToTop)
    {
        float newY = moveToTop ? originalSectionY : -originalSectionY;
        sectionTransform.localPosition = new Vector2(sectionTransform.localPosition.x, newY);
    }

    private IEnumerator RunPregamePhase()
    {
        countdownText.gameObject.SetActive(true);

        float halfHeight = barTransform.rect.height / 2f;

        yield return marker.ScriptedMoveToY(-halfHeight);
        yield return new WaitForSeconds(0.1f);
        yield return marker.ScriptedMoveToY(halfHeight);
        yield return new WaitForSeconds(0.1f);
        yield return marker.ScriptedMoveToY(0f);
        yield return new WaitForSeconds(0.3f);

        for (int i = 3; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);
        sectionTransform.gameObject.SetActive(true);

        isPregameActive = false;
        marker.SetMovementActive(true);
    }

    private void End_Success()
    {
        Debug.Log("BB");

        marker.isMoving = false;
        Cooking_Minigame_Manager.Instance.CookingMinigameComplete();
    }

    private void End_Fail()
    {
        marker.isMoving = false;
        Cooking_Minigame_Manager.Instance.CookingMinigameComplete();
    }

    public void Reset_Minigame()
    {
        // Reset game state flags
        isPregameActive = true;
        inProgress = false;
        isTop = true;
        lastTimeInSection = -Mathf.Infinity;

        // Reset progress bar
        if (progressSlider != null)
        {
            progressSlider.value = 0f;
        }

        // Reset countdown text
        if (countdownText != null)
        {
            countdownText.text = "3";
            countdownText.gameObject.SetActive(false);
        }

        // Reset section height and position
        if (sectionTransform != null)
        {
            sectionTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSectionHeight);
            sectionTransform.localPosition = new Vector2(sectionTransform.localPosition.x, originalSectionY);
            sectionTransform.gameObject.SetActive(false); // Hide again before pregame
        }

        // Reset marker position (optional)
        if (marker != null)
        {
            marker.markerTransform.localPosition = new Vector3(marker.markerTransform.localPosition.x, 0f, marker.markerTransform.localPosition.z);
            marker.SetMovementActive(false);
        }
    }

}
