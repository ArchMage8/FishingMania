using System.Collections;
using TMPro;
using UnityEngine;

public class SignMessage_Trigger : MonoBehaviour
{
    public GameObject signPanel; // Assign the "Sign Message" GameObject in the Inspector
    public TMP_Text signText;
    
    [TextArea]
    public string Message;

    [Space(10)]

    public GameObject F_Indicator;



    private Animator animator;
    private bool playerInRange = false;
    private bool messageEnabled = false;
    private bool FullyEnabled = false;


    private void Start()
    {
        animator = signPanel.GetComponent<Animator>();
        signPanel.SetActive(false);
    }

    private void Update()
    {
        if (InventoryManager.Instance.SomeUIEnabled == false)
        {
            if (playerInRange && !messageEnabled && Input.GetKeyDown(KeyCode.F) && !messageEnabled)
            {
                EnableSignMessage();
            }
        }

       
        if (messageEnabled && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            StartCoroutine(DisableSignMessage());
            FullyEnabled = false;
        }
    }

    private void EnableSignMessage()
    {
        if (signPanel != null)
        {
            
            StartCoroutine(EnableBool());
            
            InventoryManager.Instance.SomeUIEnabled = true;

            signPanel.SetActive(true);
            signText.text = Message;
            messageEnabled = true;

            F_Indicator.SetActive(false);
        }
    }

    private IEnumerator DisableSignMessage()
    {
        if (signPanel != null)
        {
            animator.SetTrigger("Exit");

            yield return new WaitForSecondsRealtime(1f);

            signPanel.SetActive(false);
            InventoryManager.Instance.SomeUIEnabled = false;
            messageEnabled = false;
        }

        // In the future you can expand this method to include sounds, animations, delays, etc.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            F_Indicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            F_Indicator.SetActive(false);
        }
    }

    private IEnumerator EnableBool()
    {
        yield return new WaitForSeconds(1.5f);
        FullyEnabled = true;
    }
}
