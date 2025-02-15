using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactIndicator;
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
    }

    private void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            DialogueManager.GetInstance().EnterDialogueMode_Quest(NPC_Dialogue, NPC_Quest);
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
