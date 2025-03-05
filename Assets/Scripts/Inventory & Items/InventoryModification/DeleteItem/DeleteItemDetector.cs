using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteItemDetector : MonoBehaviour
{
    public DeleteItemManager deleteItemManager;

    private void OnEnable()
    {
        deleteItemManager.UpdateUI();
    }
}
