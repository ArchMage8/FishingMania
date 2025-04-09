using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnAwake : MonoBehaviour
{
    private DialogueManager dialogueManager;

    public TextAsset Dialogue;
    public float DelayBeforeDialogue;

    [Space(20)]

    public Animator NPCDialogueAnimator;
    public GameObject ChoicesHolder;

    private void Awake()
    {
        dialogueManager = DialogueManager.GetInstance();
        StartCoroutine(StartDialogue());
    
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(DelayBeforeDialogue);
    
        dialogueManager.EnterDialogueMode_Default(Dialogue, NPCDialogueAnimator);
    }
}
