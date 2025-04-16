using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnAwake : MonoBehaviour
{
    private DialogueManager dialogueManager;

    public AutoAddItems autoAddItems;
    [Space(10)]
    public TextAsset Dialogue;
    public float DelayBeforeDialogue;


    [Space(20)]

    public Animator NPCDialogueAnimator;
    //public GameObject ChoicesHolder;


    private void Start()
    {
        dialogueManager = DialogueManager.GetInstance();
        StartCoroutine(StartDialogue());

        autoAddItems = GetComponent<AutoAddItems>();
    
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(DelayBeforeDialogue);


        if (NPCDialogueAnimator.gameObject.activeSelf == false)
        {
            NPCDialogueAnimator.gameObject.SetActive(true);
        }

        dialogueManager.EnterDialogueMode_Default(Dialogue, NPCDialogueAnimator);

        if (autoAddItems != null)
        {
            autoAddItems.GiveItems();
        }
        
    }
}
