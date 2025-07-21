using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep_Function : MonoBehaviour
{

    public GameObject TransitionFade;
    public GameObject F_Indicator;
    
    private bool playerInRange = false;
    private bool hasInteracted = false;
    private Animator animator;

    private void Start()
    {
        TransitionFade.SetActive(false);
        animator = TransitionFade.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerInRange && !hasInteracted && Input.GetKeyDown(KeyCode.F) && InventoryManager.Instance.SomeUIEnabled == false)
        {
            StartCoroutine(ResetDay());
            hasInteracted = true;
            F_Indicator.SetActive(false);
        }
    }

    private IEnumerator ResetDay()
    {
        InventoryManager.Instance.SomeUIEnabled = true;
        
        TransitionFade.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("Exit");

        Daylight_Handler.Instance.CallNewDay();

        yield return new WaitForSeconds(0.7f);
        InventoryManager.Instance.SomeUIEnabled = false;

        TransitionFade.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            F_Indicator.SetActive(true);
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            F_Indicator.SetActive(false);
            playerInRange = false;
            hasInteracted = false;
        }
    }
}
