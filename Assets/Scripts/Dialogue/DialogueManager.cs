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
    public bool dialogueRunning;

    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private bool canSkip = false;
    public float typingSpeed = 0.0001f;


    private const string SPEAKER_TAG = "speaker";
    private const string ANIMATION_TRIGGER = "trigger";

    private Coroutine displayLineCoroutine;

    private bool canContinueToNextLine = false;
    private bool WantSkip = false;

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
}
