using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnAwake : MonoBehaviour
{
    private DialogueManager dialogueManager;

    public TextAsset Dialogue;
    public int DelayBeforeDialogue;

    private void Awake()
    {
        Debug.Log("Potat");
        dialogueManager = DialogueManager.GetInstance();
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(DelayBeforeDialogue);
        Debug.Log("Run");
        dialogueManager.EnterDialogueMode_Default(Dialogue);
    }
}
