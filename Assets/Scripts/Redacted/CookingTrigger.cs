using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingTrigger : MonoBehaviour
{
    public GameObject interactIndicator;
    public GameObject CookingUI;

    private bool PlayerInRange;

    private void Start()
    {
        interactIndicator.SetActive(false);
        CookingUI.SetActive(false);
    }

    private void Update()
    {
        if(PlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (CookingUI.activeSelf == false)
            {
                OpenBook();
            }
        }
    }

    public void OpenBook()
    {
        interactIndicator.SetActive(false);
        CookingUI.SetActive(true);
        Time.timeScale = 0f;
        InventoryManager.Instance.SomeUIEnabled = true;
        //CookingManager.Instance.CloseButton.SetActive(true);
    }

    public void CloseRecipeBook()
    {
        Animator BookAnimator = CookingUI.GetComponent<Animator>();
        BookAnimator.SetTrigger("Exit");

        StartCoroutine(ExitCooking());

        InventoryManager.Instance.SomeUIEnabled = false;

    }

    private IEnumerator ExitCooking()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        InventoryManager.Instance.SomeUIEnabled = false;
        CookingUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))      
        {
            if (interactIndicator != null)
            {
                interactIndicator.SetActive(true);
            }
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactIndicator != null)
            {
                interactIndicator.SetActive(false);
            }
            PlayerInRange = false;
        }
    }
}
