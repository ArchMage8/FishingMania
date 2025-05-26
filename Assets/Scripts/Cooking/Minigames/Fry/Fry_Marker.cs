using UnityEngine;
using System.Collections;

public class Fry_Marker : MonoBehaviour
{
    public RectTransform markerTransform;

    private Fry_Minigame minigameRef;
    private float barHeight;
    private bool movingUp = true;
    [HideInInspector] public bool isMoving = false;

    [Header("Movement Settings")]
    public float MinSpeed = 2f;
    public float MaxSpeed = 4f;

    private float currentSpeed;

    public void Initialize(Fry_Minigame minigame)
    {
        minigameRef = minigame;
        barHeight = minigame.barTransform.rect.height;

        float rawSpeed = Random.Range(MinSpeed, MaxSpeed);
        currentSpeed = rawSpeed * 100f;

        movingUp = true;
        isMoving = false;

        // Start at center
        markerTransform.localPosition = new Vector3(markerTransform.localPosition.x, 0f, markerTransform.localPosition.z);
    }

    void Update()
    {
        if (!isMoving || minigameRef == null) return;

        float delta = currentSpeed * Time.deltaTime;
        float maxY = barHeight / 2f;
        float minY = -maxY;

        Vector3 pos = markerTransform.localPosition;
        pos.y += movingUp ? delta : -delta;

        if (pos.y >= maxY)
        {
            pos.y = maxY;
            movingUp = false;
        }
        else if (pos.y <= minY)
        {
            pos.y = minY;
            movingUp = true;
        }

        markerTransform.localPosition = pos;
    }

    public void SetMovementActive(bool state)
    {
        isMoving = state;
    }

    public IEnumerator ScriptedMoveToY(float targetY, float speed = 300f)
    {
        isMoving = false;
        Vector3 pos = markerTransform.localPosition;

        while (Mathf.Abs(pos.y - targetY) > 1f)
        {
            pos.y = Mathf.MoveTowards(pos.y, targetY, speed * Time.deltaTime);
            markerTransform.localPosition = pos;
            yield return null;
        }

        pos.y = targetY;
        markerTransform.localPosition = pos;
    }

    public float GetMarkerYPositionLocal()
    {
        return markerTransform.localPosition.y;
    }
}
