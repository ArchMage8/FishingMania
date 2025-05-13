using UnityEngine;

public class Fry_Marker : MonoBehaviour
{
    public RectTransform markerTransform;

    private Fry_Minigame minigameRef;
    private float barHeight;
    private bool movingUp = true;
    private bool isMoving = false;

    [Header("Movement Settings")]
    public float MinSpeed = 2f;
    public float MaxSpeed = 4f;

    private float currentSpeed; // Internally used (scaled)

    public void Initialize(Fry_Minigame minigame)
    {
        minigameRef = minigame;
        barHeight = minigame.barTransform.rect.height;
        isMoving = true;

        // Pick a new speed from range and scale it
        float rawSpeed = Random.Range(MinSpeed, MaxSpeed);
        currentSpeed = rawSpeed * 100f;

        // Optionally reset marker position
        markerTransform.localPosition = new Vector3(markerTransform.localPosition.x, -barHeight / 2f, markerTransform.localPosition.z);
        movingUp = true;
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

    public float GetMarkerYPositionLocal()
    {
        return markerTransform.localPosition.y;
    }
}
