using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    public TextMeshProUGUI displayNameText;
    public Animator NPCDialogueAnimator;

    private Story currentStory;
    private bool dialogueRunning;

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private const string SPEAKER_TAG = "speaker";
    private const string ANIMATION_TRIGGER = "trigger";


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
            if (currentStory.currentChoices.Count == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    ContinueStory();
                }
            }
        }
    }

    public void EnterDialogueMode(TextAsset NPCDialogue)
    {
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

    public  void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
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

        //StartCoroutine(SelectFirstChoice());
    } 

    private IEnumerator SelectFirstChoice()
    {
        //Event System requires we clear it first, then wait
        //for at least one frame before we set the current selected object
        
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
       
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
        EventSystem.current.SetSelectedGameObject(null);
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
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
}
