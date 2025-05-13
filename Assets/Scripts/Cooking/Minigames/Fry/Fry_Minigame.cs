using UnityEngine;
using UnityEngine.UI;

public class Fry_Minigame : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform barTransform;
    public RectTransform sectionTransform;
    public Fry_Marker marker;

    [Header("Gameplay Settings")]
    public float sectionShrinkAmount = 10f;
    public int maxRounds = 5;

    private bool isTop = true;
    private int currentRound = 0;
    private float originalSectionY;

    void OnEnable()
    {
        currentRound = 0;
        isTop = true;

        // Store original section Y position (assumed placed on top)
        originalSectionY = sectionTransform.localPosition.y;

        // Reset section height if needed
        // sectionTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, initialHeight); // Optional

        if (marker != null)
            marker.Initialize(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (IsMarkerWithinSection())
            {
                HandleSuccess();
            }
        }
    }

    private bool IsMarkerWithinSection()
    {
        float markerY = marker.GetMarkerYPositionLocal();
        float sectionMin = sectionTransform.localPosition.y - sectionTransform.rect.height / 2;
        float sectionMax = sectionTransform.localPosition.y + sectionTransform.rect.height / 2;
        return markerY >= sectionMin && markerY <= sectionMax;
    }

    private void HandleSuccess()
    {
        // Shrink section
        float newHeight = Mathf.Max(sectionTransform.rect.height - sectionShrinkAmount, 5f);
        sectionTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

        currentRound++;
        if (currentRound < maxRounds)
        {
            isTop = !isTop;
            MoveSection(isTop);
        }
        else
        {
            Debug.Log("Minigame Complete!");
            // Trigger completion event here
        }
    }

    private void MoveSection(bool moveToTop)
    {
        float newY = moveToTop ? originalSectionY : -originalSectionY;
        sectionTransform.localPosition = new Vector2(sectionTransform.localPosition.x, newY);
    }
}
