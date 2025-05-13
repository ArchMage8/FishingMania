using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chop_Minigame : MonoBehaviour
{
    public RectTransform barRect;          // The bar to place sections on
    public List<RectTransform> sectionPool; // Pool of reusable section UI elements
    public int minSections = 3;
    public int maxSections = 6;
    public float minSpacing = 20f;         // Minimum spacing between sections (in pixels)

    private List<RectTransform> activeSections = new List<RectTransform>();

    [Space(20)]

    public Chop_Marker marker;


    private void Awake()
    {
        SetupMinigame();
        marker.Initialize(activeSections, barRect);
    }

  

    private void SetupMinigame()
    {
        // Clear previous sections
        foreach (var sec in activeSections)
            sec.gameObject.SetActive(false);
        activeSections.Clear();

        int numSections = Random.Range(minSections, maxSections + 1);
        float barWidth = barRect.rect.width;

        // Keep track of used positions
        List<float> usedPositions = new List<float>();

        for (int i = 0; i < numSections; i++)
        {
            RectTransform section = sectionPool[i];
            float sectionWidth = section.rect.width;

            // Try placing section in a valid spot
            bool placed = false;
            int attempts = 100;

            while (!placed && attempts-- > 0)
            {
                float xPos = Random.Range(sectionWidth / 2f, barWidth - sectionWidth / 2f);

                // Check for spacing with other placed sections
                bool tooClose = false;
                foreach (float other in usedPositions)
                {
                    if (Mathf.Abs(other - xPos) < (sectionWidth + minSpacing))
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    usedPositions.Add(xPos);
                    section.SetParent(barRect);
                    section.anchoredPosition = new Vector2(xPos - barWidth / 2f, 0f); // bar pivot assumed center
                    section.gameObject.SetActive(true);
                    activeSections.Add(section);
                    placed = true;
                }
            }

            if (!placed)
            {
                Debug.LogWarning("Failed to place all sections due to spacing constraints.");
                break;
            }
        }
    }
}