using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTrigger : MonoBehaviour
{
    public DeleteItemManager deleteItemManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            deleteItemManager.PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            deleteItemManager.PlayerInRange = false;
        }
    }
}
