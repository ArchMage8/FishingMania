using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chop_Marker : MonoBehaviour
{
    private RectTransform barRect;
    public float speed = 200f; // Units per second
    private bool bounce = true;

    private List<RectTransform> sections;
    private float barWidth;
    private float direction = 1f;

    private RectTransform marker;
    private bool isMoving = false;

    // Section tracking
    private RectTransform currentSectionOver;
    private RectTransform lastValidSection;

    // Coyote time
    public float coyoteTime = 0.15f; // Seconds
    private float timeSinceLastValidSection = Mathf.Infinity;

    public void Initialize(List<RectTransform> sectionRects, RectTransform bar)
    {
        marker = this.GetComponent<RectTransform>();
        sections = sectionRects;
        barRect = bar;
        barWidth = bar.rect.width;

        isMoving = true;
        timeSinceLastValidSection = Mathf.Infinity;
        lastValidSection = null;
    }

    void Update()
    {
        if (isMoving)
            MoveMarker();

        // Track elapsed time since last section was touched
        timeSinceLastValidSection += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && timeSinceLastValidSection <= coyoteTime && lastValidSection != null)
        {
            HandleSuccessfulHit(lastValidSection.gameObject);
            timeSinceLastValidSection = Mathf.Infinity;
            lastValidSection = null;
        }
    }

    private void MoveMarker()
    {
        if (marker == null || barRect == null) return;

        Vector2 pos = marker.anchoredPosition;
        pos.x += speed * direction * Time.deltaTime;

        float halfBar = barWidth / 2f;
        if (pos.x > halfBar)
        {
            if (bounce) direction = -1f;
            else pos.x = -halfBar;
        }
        else if (pos.x < -halfBar)
        {
            if (bounce) direction = 1f;
            else pos.x = halfBar;
        }

        marker.anchoredPosition = pos;

        // Section detection using marker edges
        bool onSection = false;
        currentSectionOver = null;

        float markerLeft = marker.anchoredPosition.x - marker.rect.width / 2f;
        float markerRight = marker.anchoredPosition.x + marker.rect.width / 2f;

        foreach (var section in sections)
        {
            if (!section.gameObject.activeSelf) continue;

            float sectionX = section.anchoredPosition.x;
            float halfWidth = section.rect.width / 2f;

            float sectionLeft = sectionX - halfWidth;
            float sectionRight = sectionX + halfWidth;

            if (markerRight >= sectionLeft && markerLeft <= sectionRight)
            {
                onSection = true;
                currentSectionOver = section;

                // Start coyote timer and record valid section
                timeSinceLastValidSection = 0f;
                lastValidSection = section;
                break;
            }
        }

        // Visual feedback
        Image temp = this.GetComponent<Image>();
        temp.color = onSection ? Color.black : Color.red;
    }

    private void HandleSuccessfulHit(GameObject hitSection)
    {
        if (hitSection.activeSelf)
        {
            hitSection.SetActive(false);
            Debug.Log("Hit section: " + hitSection.name);
        }
    }

    // Optional controls
    public void Pause() => isMoving = false;
    public void Resume() => isMoving = true;
}
