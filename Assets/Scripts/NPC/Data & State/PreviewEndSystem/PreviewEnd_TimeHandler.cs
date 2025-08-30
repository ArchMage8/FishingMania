using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewEnd_TimeHandler : MonoBehaviour
{
    public bool RunTime = false;

    private void Start()
    {
        if (Daylight_Handler.Instance != null)
        {
            Daylight_Handler.Instance.TimeRunning = RunTime;

            if (RunTime == false)
            {
                InventoryManager.Instance.HUD.SetActive(false);
            }

            else
            {
                InventoryManager.Instance.HUD.SetActive(true);
            }
        }
    }
}
