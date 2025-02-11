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
    public Animator NPCDialogueAnimator;

    [Space(15)]
    // Choices UI
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private bool canSkip = false;

    [Space(15)]
    [Header("Extras")]

    // Dialogue State
    private Story currentStory;
    public bool dialogueRunning;
    private bool canContinueToNextLine = false;
    private bool WantSkip = false;

    

    [Space(15)]
    // Typing and Animation
    public float typingSpeed = 0.0001f;
    private const string SPEAKER_TAG = "speaker";
    private const string ANIMATION_TRIGGER = "trigger";

    // Coroutine
    private Coroutine displayLineCoroutine;

    // Quest Stuffs
    private QuestSO DialogueQuest; // The quest of the NPC we are talking to


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than 1 manager found");
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
                    ContinueStory();
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

    public void EnterDialogueMode_Quest(TextAsset NPCDialogue, QuestSO PassedQuest)
    {
        //The dialogue trigger needs to also send the corresponding Quest Data

        DialogueQuest = PassedQuest;

        //Handling code side of the ink file
        SetVariables_Before();
        BindSubmit();

        currentStory = new Story(NPCDialogue.text);
        dialogueRunning = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    public void EnterDialogueMode_Default(TextAsset NPCDialogue)
    {
        //The dialogue trigger needs to also send the corresponding Quest Data

        currentStory = new Story(NPCDialogue.text);
        dialogueRunning = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueRunning = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
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
            Debug.Log("call test 4");

            if (canSkip && WantSkip)
            {
                Debug.Log("Call test 5");
                dialogueText.text = line;
                WantSkip = false;
                break; // Exit the loop early
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }


        DisplayChoices();
        canContinueToNextLine = true;
        canSkip = false;
    }

    public  void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
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
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
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
                    break;

                case ANIMATION_TRIGGER:
                    Debug.Log("Call animation switch");
                    NPCDialogueAnimator.SetTrigger("Next");
                    break;

                default:
                    Debug.LogWarning("Tag came in but isn't handled by the code: " + tag);
                    break;
            }
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
        Debug.Log("call test 1");

        canSkip = false;
        yield return new WaitForSeconds(0.05f);

        Debug.Log("call test 2");
        canSkip = true;
    }

    private void SetVariables_Before()
    {
        //This is used to determine how we start the dialogue
        //and bind bools to the INK file bools

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

    private void SubmitQuest()
    {
        //Called when we are sending items to the inventoryManager
        //So the ink file is the one triggering the submit

        //1. Is the transaction completed? (Yes or No)

        if (QuestManager.Instance.SubmitQuest(DialogueQuest.desiredItem, DialogueQuest.requiredQuantity) == true)
        {
            currentStory.variablesState["Success"] = true;
        }

        else if (QuestManager.Instance.SubmitQuest(DialogueQuest.desiredItem, DialogueQuest.requiredQuantity) == false)
        {
            currentStory.variablesState["Success"] = false;
        }
    }

    private void BindSubmit()
    {
        currentStory.BindExternalFunction("SubmitQuest", (string variableName, int value) => {
            SubmitQuest();
        });
    }
}
