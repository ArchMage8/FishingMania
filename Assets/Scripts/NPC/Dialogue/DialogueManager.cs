using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    // Singleton instance
    private static DialogueManager instance;


    [Header("Dialogue Components")]
    // UI Elements
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI displayNameText;

    [Space(10)]
    [Header("Auto moved Stuffs")]
    public GameObject SpeakerNameBG;
    public GameObject ChoicesHolder;

    private Animator NPCDialogueAnimator;
    private GameObject PlayerHUD;
    

    [Space(15)]
    // Choices UI
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private bool canSkip = false;

    [Space(15)]
    [Header("Dialogue State")]

    // Dialogue State
    [HideInInspector] public Story currentStory;
    public bool dialogueRunning;
    public bool canDialogue = true;
    private bool canContinueToNextLine = false;
    private bool WantSkip = false;
    private bool QuestCompleted;

    

    [Space(15)]
    // Typing and Animation
    public float typingSpeed = 0.0001f;
    private const string SPEAKER_TAG = "speaker";
    private const string ANIMATION_TRIGGER = "trigger";

    // Coroutine
    private Coroutine displayLineCoroutine;

    // NOC Stuffs
    private QuestSO DialogueQuest; // The quest of the NPC we are talking to
    [HideInInspector] public bool NpcInRange = false;
    private GameObject Temp_Shop;

    //Variables & Function needed to move Dialogue Objects when iris is speaking

    private RectTransform rectTransform;
    //Choices
    private Vector2 ChoicesOriginalPosition = new Vector2(-642f, -31f);
    private Vector2 ChoicesMovedPosition = new Vector2(25f, 0f); //Iris is talking

    //Speaker Name
    private Vector2 SpeakerNameOriginalPosition = new Vector2(740f, -459f);
    private Vector2 SpeakerNameMovedPosition = new Vector2(-697f, -459f); //Iris is talking


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than 1 manager found");
            Destroy(this.gameObject);
        }

        instance = this;

    }

    public static DialogueManager GetInstance()
    {
    
        return instance;
    }

    private void Start()
    {
        dialogueRunning = false;
        dialoguePanel.SetActive(false);

       

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueRunning)
        {
            return;
        }

        else
        {
            if (currentStory.currentChoices.Count == 0 && canContinueToNextLine)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    if (dialogueRunning)
                    {
                        ContinueStory();
                    }
                }
            }

            else if (!canContinueToNextLine)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                   
                    WantSkip = true;
                }
            }
        }
    }

    public void EnterDialogueMode_Default(TextAsset NPCDialogue, Animator PassedAnimator)
    {
        if (!InventoryManager.Instance.SomeUIEnabled)
        {
            Debug.Log("Call A");
            InventoryManager.Instance.HUD.SetActive(false);  

            NPCDialogueAnimator = PassedAnimator;

            currentStory = new Story(NPCDialogue.text);
            dialogueRunning = true;
            dialoguePanel.SetActive(true);

            ContinueStory();
            InventoryManager.Instance.SomeUIEnabled = true;
        }
    }

    public void EnterDialogueMode_Quest(TextAsset NPCDialogue, QuestSO PassedQuest, Animator PassedAnimator)
    {
        if (NpcInRange == true && canDialogue && !InventoryManager.Instance.SomeUIEnabled)
        {
            NPCDialogueAnimator = PassedAnimator;

            

            QuestCompleted = false;

            //The dialogue trigger needs to also send the corresponding Quest Data

            DialogueQuest = PassedQuest;
            currentStory = new Story(NPCDialogue.text);


            //Handling code side of the ink file
            BindVariables();
            BindSubmit();
            BindSetQuest();

         


            
            dialogueRunning = true;
            dialoguePanel.SetActive(true);

            ContinueStory();

            InventoryManager.Instance.SomeUIEnabled = true;
        }
    }

    public void EnterDialogue_Sell(TextAsset NPCDialogue, GameObject Sell_To_NPC_UI, Animator PassedAnimator)
    {
        if (NpcInRange == true && canDialogue && !InventoryManager.Instance.SomeUIEnabled)
        {

            NPCDialogueAnimator = PassedAnimator;

            InventoryManager.Instance.HUD.SetActive(false);

            //We are selling to the NPC
            currentStory = new Story(NPCDialogue.text);
            dialogueRunning = true;
            dialoguePanel.SetActive(true);


            BindPlayerShop();
            ContinueStory();
            Temp_Shop = Sell_To_NPC_UI;

            InventoryManager.Instance.SomeUIEnabled = true;

            
        }
    }

    public void EnterDialogue_Buy(TextAsset NPCDialogue, GameObject Sell_UI, Animator PassedAnimator)
    {
        if (!InventoryManager.Instance.SomeUIEnabled)
        {

            NPCDialogueAnimator = PassedAnimator;

            InventoryManager.Instance.HUD.SetActive(false);

            //We are buying from the NPC
            currentStory = new Story(NPCDialogue.text);
            dialogueRunning = true;
            dialoguePanel.SetActive(true);

            BindNPCShop();
            ContinueStory();

            Temp_Shop = Sell_UI;

            InventoryManager.Instance.SomeUIEnabled = true;
        }
    }

    private void ExitDialogueMode()
    {
        Time.timeScale = 1f;

        dialogueRunning = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        InventoryManager.Instance.SomeUIEnabled = false;

        NPCDialogueAnimator.gameObject.SetActive(false);
        InventoryManager.Instance.HUD.SetActive(true);

        NPCDialogueAnimator = null;

        currentStory = null;


        if (QuestCompleted)
        {
            QuestCompleteUpdate();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        //bool istyping = false;

        StartCoroutine(CanSkip());

        dialogueText.text = "";

        HideChoices();
        canContinueToNextLine = false;
        foreach (char letter in line.ToCharArray())
        {

            if (canSkip && WantSkip)
            {
              
                dialogueText.text = line;
                WantSkip = false;
                break; // Exit the loop early
            }

            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }


        DisplayChoices();
        canContinueToNextLine = true;
        canSkip = false;
    }

    public void ContinueStory()
    {  if (currentStory.canContinue)
            {
                if (displayLineCoroutine != null)
                {
                    StopCoroutine(displayLineCoroutine);
                }

                    if (currentStory != null)
                    {
                        string line = currentStory.Continue();
                        displayLineCoroutine = StartCoroutine(DisplayLine(line));
                    }

                if (currentStory.currentTags != null && currentStory.currentTags.Count > 0)
                {
                    HandleTags(currentStory.currentTags);
                }
            }
            else
            {
                ExitDialogueMode();
            }
        
    }



    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than possible");
        }

        int index = 0;

        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i< choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    } 


    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            
            ContinueStory();
            //EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        //Function needs to handle 2 types of tags
        //1. Speaker Name
        //2. Next Trigger
        
        
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.Log("Incorrect parsing on tag: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    HandleWhoseTalking(tagValue);
                    break;

                case ANIMATION_TRIGGER:
                    Debug.Log("Call animation switch");
                    NPCDialogueAnimator.SetTrigger(tagValue);
                    break;

                default:
                    Debug.LogWarning("Tag came in but isn't handled by the code: " + tag);
                    break;
            }
        }
    }

    private void HandleWhoseTalking(string SpeakerName) //Moves Objects based on the speaker
    {
        if(ChoicesHolder == null)
        {
            Debug.LogError("Ur setup be missing the ability to move choices");
        }

        if (SpeakerNameBG == null)
        {
            Debug.LogError("Ur setup be missing the ability to move the speaker name");
        }

        RectTransform ChoicesRectTransform = ChoicesHolder.GetComponent<RectTransform>();
        RectTransform SpeakerNameTransform = SpeakerNameBG.GetComponent<RectTransform>();


        if (ChoicesRectTransform == null) return;

        if (SpeakerName == "Iris")
        {
            ChoicesRectTransform.anchoredPosition = ChoicesMovedPosition;
            SpeakerNameTransform.anchoredPosition = SpeakerNameMovedPosition;
        }
        else
        {
            ChoicesRectTransform.anchoredPosition = ChoicesOriginalPosition;
            SpeakerNameTransform.anchoredPosition = SpeakerNameOriginalPosition;
        }
    }

    private void HideChoices()
    {
        foreach(GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private IEnumerator CanSkip()
    {
      

        canSkip = false;
        yield return new WaitForSecondsRealtime(0.05f);
        Debug.Log("Can Skip");
        canSkip = true;
    }

    //After this point it is used to handle the code aspect of the ink files
    //====================================================================================

    #region Quest

    private void BindVariables()
    {
        
        currentStory.BindExternalFunction("SetVariables", () => {
            SetVariables_Before();
        });
    }

    private void SetVariables_Before()
    {
        //This is used to determine how we start the dialogue
        //and bind bools to the INK file bools

        //*Note that we need to add the Ifs to branch the dialogues from
        //within the INK file

        //1. Do we have an active quest

        currentStory.variablesState["hasActiveQuest"] = QuestManager.Instance.activeQuestPresent;

        //2. If 1. is true, are we talking to the NPC that quested us?
        if(QuestManager.Instance.activeQuestID == DialogueQuest.questID)
        {
            currentStory.variablesState["correspondingNPC"] = true;
        }

        else
        {
            currentStory.variablesState["correspondingNPC"] = false;
        }
    }

//=====================================================================================

    private void BindSubmit()
    {
        //This is called when we enter dialogue mode
        
        currentStory.BindExternalFunction("SubmitQuest", () => {
            SubmitQuest();
        });
    }

    private void SubmitQuest()
    {
        //Called when we are sending items to the inventoryManager
        //So the ink file is the one triggering the submit

        //1. Is the transaction completed? (Yes or No)

        if (QuestManager.Instance.SubmitQuest(DialogueQuest.desiredItem, DialogueQuest.requiredQuantity) == true)
        {
            currentStory.variablesState["Success"] = true;
            QuestCompleted = true;
            
        }

        else if (QuestManager.Instance.SubmitQuest(DialogueQuest.desiredItem, DialogueQuest.requiredQuantity) == false)
        {
            currentStory.variablesState["Success"] = false;
            QuestCompleted = false;
        }

        //*Note that we need to add the Ifs to branch the dialogues from
        //within the INK file
    }

//=====================================================================================
    
     private void BindSetQuest()
    {
        //This is called when we enter dialogue mode as "Not Busy"
        
        currentStory.BindExternalFunction("SetActiveQuest", () => {
            SetActiveQuest();
        });
    }
    
    
    
    private void SetActiveQuest()
    {
        if (!QuestManager.Instance.activeQuestPresent)
        {
            QuestManager.Instance.SetActiveQuest(DialogueQuest);
        }
    }

//=====================================================================================

      private void QuestCompleteUpdate()
    {
        NPCData npcData = NPCManager.Instance?.npcTempList.Find(npc => npc.npcName == DialogueQuest.npcName);

        QuestManager.Instance.ResetActiveQuest();

        if (npcData != null)
        {
            npcData.friendshipLevel += 1;
            npcData.isFull = true;

            //StateHandler will transition to full state after x seconds
        }
        else
        {
            Debug.LogError("This NPC's data is not found :v");
        }
    }


    //List of stuffs to assign to the INK file

    //1. #speaker -> Tag
    //2. #trigger -> Tag
    //3. hasActiveQuest     -> bool
    //4. correspondingNPC   -> bool
    //5. Success            -> bool
    //6. SetVariables   -> function
    //6. SubmitQuest    -> function
    //7. SetActiveQuest -> function
    #endregion

    #region Buying from NPC

    public void BindNPCShop()
    {
        Debug.Log("cll");
        currentStory.BindExternalFunction("EnableShop", () => MakeShopUIAppear());
    }

    private void MakeShopUIAppear()
    {
        if(Temp_Shop != null)
        {
            ExitDialogueMode();
            Debug.Log("CAll");
            InventoryManager.Instance.SomeUIEnabled = true;
            Temp_Shop.SetActive(true);
            Time.timeScale = 0f;

            Temp_Shop = null;
            
        }
    }

    #endregion

    #region Selling to NPC

    private void BindPlayerShop()
    {
        currentStory.BindExternalFunction("EnablePlayerShop", () => MakePlayerShopAppear());
    }

    private void MakePlayerShopAppear()
    {
        if (Temp_Shop != null)
        {
            ExitDialogueMode();
            InventoryManager.Instance.SomeUIEnabled = true;
            Temp_Shop.SetActive(true);
            Time.timeScale = 0f;

            Temp_Shop = null;
        }
    }

    #endregion
}
