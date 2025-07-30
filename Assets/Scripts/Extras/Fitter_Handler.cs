using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Fitter_Handler : MonoBehaviour
{
    private RectTransform targetLayout;

    void Awake()
    {
        var fitter = GetComponentInChildren<ContentSizeFitter>();
        if (fitter != null)
            targetLayout = fitter.GetComponent<RectTransform>();
    }

    public void RefreshLayout()
    {
        if (targetLayout != null)
        {
            Debug.LogWarning("Bubss");
            LayoutRebuilder.ForceRebuildLayoutImmediate(targetLayout);
        }
        else
        {
            Debug.LogWarning("Buns");
        }
    }
}
