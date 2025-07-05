using UnityEngine;
using System.Collections;

public class NPC_IntroductionDialogue : MonoBehaviour
{

    [Header("Dialogue Requirements")]
    public GameObject interactIndicator;
    public Animator LocalDialogueAnimator;
    public TextAsset NPC_Dialogue;

    [Space(10)]

    [Header("NPC State")]
    public NPC npc_data;
    public StateManager stateManager;

    //public GameObject ChoicesHolder;

    private bool wasDialogueRunning = false;
    private bool monitoring = false;
    private bool playerInRange = false;

    private void Start()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }

        //LocalDialogueAnimator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !DialogueManager.GetInstance().dialogueRunning && !InventoryManager.Instance.SomeUIEnabled)
        {
            Debug.Log("Call");

            Time.timeScale = 0f;

            PlayerManager.instance.SnapDirection(this.gameObject.transform);

            interactIndicator.SetActive(false);


            DialogueManager.GetInstance().NpcInRange = playerInRange;

            if (LocalDialogueAnimator.gameObject.activeSelf == false)
            {
                LocalDialogueAnimator.gameObject.SetActive(true);
            }


            if (NPC_Dialogue != null)
            {
                DialogueManager.GetInstance().EnterDialogueMode_Default(NPC_Dialogue, LocalDialogueAnimator);
                StartCoroutine(WatchDialogueManager(DialogueManager.GetInstance()));
            }

            //LocalDialogueAnimator.gameObject.SetActive(true);

        }
    }

    private IEnumerator WatchDialogueManager(DialogueManager dialogueManager)
    {
        monitoring = true;

        while (!dialogueManager.dialogueRunning)
        {
            yield return null;
        }

        wasDialogueRunning = true;

        while (dialogueManager.dialogueRunning)
        {
            yield return null;
        }

        if (wasDialogueRunning && monitoring)
        {
            OnIntroComplete();
            monitoring = false;
        }
    }

    private void OnIntroComplete()
    {
        NPCManager.Instance.AddFriendshipLevel(npc_data.npcName);
        stateManager.AdjustVersion();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactIndicator != null)
            {
                //Debug.Log("Pp");
                interactIndicator.SetActive(true);
            }
            playerInRange = true;
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
            playerInRange = false;
        }
    }

}
