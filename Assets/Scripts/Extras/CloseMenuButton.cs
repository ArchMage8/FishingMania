using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenuButton : MonoBehaviour
{
    public GameObject TargetObject;

    public void DisableTarget()
    {
        TargetObject.SetActive(false);
        InventoryManager.Instance.SomeUIEnabled = false;
        Time.timeScale = 1f;
    }
}
