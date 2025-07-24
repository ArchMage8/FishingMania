using System.Collections;
using UnityEngine;

public class Sleep_Function : MonoBehaviour
{
    //public GameObject TransitionFade;
    public GameObject Prompt_Panel;

    [Space(10)]

    public GameObject F_Indicator;

    private bool playerInRange = false;
    private bool hasInteracted = false;

    private Animator FadeAnimator;
    private Animator PromptAnimator;

    private void Start()
    {
        Prompt_Panel.SetActive(false); // Hide panel at start
        PromptAnimator = Prompt_Panel.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerInRange && !hasInteracted && Input.GetKeyDown(KeyCode.F) && !InventoryManager.Instance.SomeUIEnabled)
        {
            InventoryManager.Instance.SomeUIEnabled = true;

            Prompt_Panel.SetActive(true); // Show confirmation panel
            hasInteracted = true;
            F_Indicator.SetActive(false);
        }
    }

    public void ConfirmSleep()
    {
        StartCoroutine(ResetDay());
    }

    public void CloseSleepPanel()
    {
       StartCoroutine(ClosePanel());
    }

    private IEnumerator ClosePanel()
    {
        hasInteracted = true;

        PromptAnimator.SetTrigger("Cancel_Close");
        yield return new WaitForSeconds(0.5f);
        Prompt_Panel.SetActive(false);

        F_Indicator.SetActive(false);
        InventoryManager.Instance.SomeUIEnabled = false;
    }

    private IEnumerator ResetDay()
    {
        PromptAnimator.SetTrigger("Exit");
        Daylight_Handler.Instance.CallNewDay();
        yield return new WaitForSeconds(0.7f);
        PromptAnimator.SetTrigger("Sleep_Close");

        yield return new WaitForSeconds(0.5f);

        Prompt_Panel.SetActive(false); // Hide confirmation panel

        InventoryManager.Instance.SomeUIEnabled = false;
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
