using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExitPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TransitionPositionManager.Instance.SetExitPosition(transform.position);
        }
    }
}
