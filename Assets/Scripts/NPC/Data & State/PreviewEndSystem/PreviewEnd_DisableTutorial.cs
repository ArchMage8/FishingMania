using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewEnd_DisableTutorial : MonoBehaviour
{
    private void Start()
    {
        if (PreviewEnd_Manager.Instance != null)
        {
            if (PreviewEnd_Manager.Instance.isPreviewComplete)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
