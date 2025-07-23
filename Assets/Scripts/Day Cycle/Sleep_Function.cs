using System.Collections;
using UnityEngine;

public class Sleep_Function : MonoBehaviour
{
    public GameObject TransitionFade;
    public GameObject Prompt_Panel;

    [Space(10)]

    public GameObject F_Indicator;

    private bool playerInRange = false;
    private bool hasInteracted = false;

    private Animator FadeAnimator;
    private Animator PromptAnimator;

    private void Start()
    {
        TransitionFade.SetActive(false);
        Prompt_Panel.SetActive(false); // Hide panel at start
        FadeAnimator = TransitionFade.GetComponent<Animator>();
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

        PromptAnimator.SetTrigger("Exit");
        yield return new WaitForSeconds(1f);
        Prompt_Panel.SetActive(false);

        F_Indicator.SetActive(false);
        InventoryManager.Instance.SomeUIEnabled = false;
    }

    private IEnumerator ResetDay()
    {
        
        Prompt_Panel.SetActive(false); // Hide confirmation panel

        TransitionFade.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        FadeAnimator.SetTrigger("Exit");

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
