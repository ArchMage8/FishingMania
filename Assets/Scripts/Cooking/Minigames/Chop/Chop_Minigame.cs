using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chop_Minigame : MonoBehaviour
{
    public RectTransform barRect;
    public List<RectTransform> sectionPool;
    public int minSections = 3;
    public int maxSections = 6;
    public float minSpacing = 20f;

    public TMP_Text countdownText;
    public Chop_Marker marker;

    [Space(10)]

  

    private List<RectTransform> activeSections = new List<RectTransform>();

    private bool MinigameStarted = false;
    private Coroutine minigameRoutine;

    private void OnEnable()
    {
        StartCoroutine(Start_WithAnimation());
    }

    private IEnumerator Start_WithAnimation()
    {
        yield return new WaitForSeconds(0.5f);
  
        minigameRoutine = StartCoroutine(Start_Minigame());
        marker.HitCount = 0;
        MinigameStarted = false;
    }

    private void OnDisable()
    {
        ResetMinigame();
    }

    private void Update()
    {
        if (Cooking_Minigame_Manager.Instance.health == 0)
        {
            EndFail();
        }

        if (marker.HitCount == activeSections.Count && MinigameStarted)
        {
            EndSuccess();
        }
    }

    private IEnumerator Start_Minigame()
    {
        

        marker.EnableMovement(false);
        countdownText.gameObject.SetActive(true);

        float barWidth = barRect.rect.width;
        float halfBar = barWidth / 2f;

        yield return MoveMarkerTo(new Vector2(0f, 0f));
        yield return new WaitForSeconds(0.2f);
        yield return MoveMarkerTo(new Vector2(halfBar - 10f, 0f));
        yield return new WaitForSeconds(0.2f);
        yield return MoveMarkerTo(new Vector2(-halfBar + 10f, 0f));
        yield return new WaitForSeconds(0.2f);
        yield return MoveMarkerTo(new Vector2(0f, 0f));
        yield return new WaitForSeconds(0.2f);

        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.gameObject.SetActive(false);
        SetupMinigame();
        marker.Initialize(activeSections, barRect);
    }

    private IEnumerator MoveMarkerTo(Vector2 targetPos)
    {
        RectTransform markerRect = marker.GetComponent<RectTransform>();
        Vector2 startPos = markerRect.anchoredPosition;

        float distance = Vector2.Distance(startPos, targetPos);
        float duration = distance / marker.speed;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            marker.SetMarkerPosition(Vector2.Lerp(startPos, targetPos, t));
            yield return null;
        }

        marker.SetMarkerPosition(targetPos);
    }

    private void SetupMinigame()
    {
        foreach (var sec in activeSections)
            sec.gameObject.SetActive(false);
        activeSections.Clear();

        int numSections = Random.Range(minSections, maxSections + 1);
        float barWidth = barRect.rect.width;
        List<float> usedPositions = new List<float>();

        for (int i = 0; i < numSections; i++)
        {
            RectTransform section = sectionPool[i];
            float sectionWidth = section.rect.width;

            bool placed = false;
            int attempts = 100;
            while (!placed && attempts-- > 0)
            {
                float xPos = Random.Range(sectionWidth / 2f, barWidth - sectionWidth / 2f);

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
                    section.anchoredPosition = new Vector2(xPos - barWidth / 2f, 0f);
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

        MinigameStarted = true;
        marker.EnableMovement(true);
    }


    private void EndFail() //Losing Health is handled by the Chop_Marker
    {                      //This script is called when the player runs out of health

        

        StopMinigame();
        Cooking_Minigame_Manager.Instance.CookingMinigameComplete(); //Pay attention
                                                                     //This function ends the cooking phase

    }

    private void EndSuccess()
    {
        StopMinigame();
        StartCoroutine(Cooking_Minigame_Manager.Instance.OnChopMinigameComplete()); //While this one ends the chop phase
    }

    public void StopMinigame()
    {
        if (minigameRoutine != null)
        {
            StopCoroutine(minigameRoutine);
            minigameRoutine = null;
        }

        marker.EnableMovement(false);

        foreach (var sec in activeSections)
        {
            if (sec != null)
                sec.gameObject.SetActive(false);
        }
        activeSections.Clear();

        countdownText.gameObject.SetActive(false);

        // Optional: reset marker to center
        //marker.SetMarkerPosition(Vector2.zero);
    }

    public void ResetMinigame()
    {
        if (minigameRoutine != null)
        {
            StopCoroutine(minigameRoutine);
            minigameRoutine = null;
        }

        marker.EnableMovement(false);
        marker.HitCount = 0;
        MinigameStarted = false;

        foreach (var sec in activeSections)
        {
            if (sec != null)
                sec.gameObject.SetActive(false);
        }
        activeSections.Clear();

        countdownText.gameObject.SetActive(false);

        marker.SetMarkerPosition(Vector2.zero);
    }
}
