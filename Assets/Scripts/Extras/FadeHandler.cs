using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeHandler : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(waitForFadeComplete());
    }

    private IEnumerator waitForFadeComplete()
    {
        InventoryManager.Instance.SomeUIEnabled = true;
        yield return new WaitForSeconds(0.9f);
        Debug.Log("Bla");
        InventoryManager.Instance.SomeUIEnabled = false;
    }
}
