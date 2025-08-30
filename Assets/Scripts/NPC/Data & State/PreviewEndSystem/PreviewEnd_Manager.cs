using UnityEngine;

public class PreviewEnd_Manager : MonoBehaviour
{
    public static PreviewEnd_Manager Instance { get; private set; }

    public bool isPreviewComplete = false;

    private void Awake()
    {
        // Enforce singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetPreviewComplete()
    {
        isPreviewComplete = true;
    }
}
