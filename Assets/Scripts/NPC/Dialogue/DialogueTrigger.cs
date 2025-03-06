using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactIndicator;
    [SerializeField] private Animator LocalDialogueAnimator;
    private bool playerInRange = false;

    [Space(15)]
    [Header("NPC System Requirements")]
    public TextAsset NPC_Dialogue;
    public QuestSO NPC_Quest;

    private void Start()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }

        LocalDialogueAnimator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 0f;

            PlayerManager.instance.SnapDirection(this.gameObject.transform);

            DialogueManager.GetInstance().NPCDialogueAnimator = LocalDialogueAnimator;
            DialogueManager.GetInstance().NpcInRange = playerInRange;

            if (NPC_Quest != null)
            {
                DialogueManager.GetInstance().EnterDialogueMode_Quest(NPC_Dialogue, NPC_Quest);
            }

            else
            {
                DialogueManager.GetInstance().EnterDialogueMode_Default(NPC_Dialogue);
            }

            LocalDialogueAnimator.gameObject.SetActive(true);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactIndicator != null)
            {
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
