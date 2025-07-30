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
        Debug.Log("potatoes");
        
        if (targetLayout != null)
        {
            Debug.Log("Bubss");
            LayoutRebuilder.ForceRebuildLayoutImmediate(targetLayout);
        }
        else
        {
            Debug.Log("Buns");
        }
    }
}
