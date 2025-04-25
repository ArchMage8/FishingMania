using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactIndicator;
    [SerializeField] private Animator LocalDialogueAnimator;
    private bool playerInRange = false;

    [Space(15)]
    [Header("NPC System Requirements")]
    public TextAsset NPC_Dialogue;
    //public GameObject ChoicesHolder;

    [Header("NPC Info")]
    public QuestSO NPC_Quest;
    public GameObject Shop_NPC_UI;
    public GameObject Sell_To_NPC_UI;

 

    private void Start()
    {
        if (interactIndicator != null)
        {
            interactIndicator.SetActive(false);
        }

        if(Shop_NPC_UI != null)
        {
            Shop_NPC_UI.SetActive(false);
        }

        if (Sell_To_NPC_UI != null)
        {
            Sell_To_NPC_UI.SetActive(false);
        }

        if ((NPC_Quest != null ? 1 : 0) + (Shop_NPC_UI != null ? 1 : 0) + (Sell_To_NPC_UI != null ? 1 : 0) > 1)
        {
            Debug.LogError("Ur Setup Be broken");
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

            if(LocalDialogueAnimator.gameObject.activeSelf == false)
            {
                LocalDialogueAnimator.gameObject.SetActive(true);
            }

            if (NPC_Quest != null)
            {
              
                DialogueManager.GetInstance().EnterDialogueMode_Quest(NPC_Dialogue, NPC_Quest, LocalDialogueAnimator);
            }

            else if (Shop_NPC_UI != null)
            {
                //We are buying from the NPC
              
                DialogueManager.GetInstance().EnterDialogue_Buy(NPC_Dialogue, Shop_NPC_UI, LocalDialogueAnimator);
            }

            else if (Sell_To_NPC_UI != null)
            {
                //We are selling to the NPC
             
                DialogueManager.GetInstance().EnterDialogue_Sell(NPC_Dialogue, Sell_To_NPC_UI, LocalDialogueAnimator);
            }

            else if(!NPC_Quest && !Shop_NPC_UI && !Sell_To_NPC_UI)
            {
              
                DialogueManager.GetInstance().EnterDialogueMode_Default(NPC_Dialogue, LocalDialogueAnimator);
            }

            //LocalDialogueAnimator.gameObject.SetActive(true);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactIndicator != null)
            {
                Debug.Log("Pp");
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
