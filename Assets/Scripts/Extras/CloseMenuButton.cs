using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenuButton : MonoBehaviour
{
    public GameObject TargetObject;

    private Animator animator;

    public void DisableTarget()
    {
        animator = TargetObject.GetComponent<Animator>();

        if (animator != null)
        {
            StartCoroutine(CloseWithAnimations());
        }

        else
        {
            TargetObject.SetActive(false);
            InventoryManager.Instance.SomeUIEnabled = false;
            Time.timeScale = 1f;
        }
    }

    private IEnumerator CloseWithAnimations()
    {
        animator.SetTrigger("Exit");
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        TargetObject.SetActive(false);
        InventoryManager.Instance.SomeUIEnabled = false;
    }
}
