using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset inkJSON;
    public Animator NPCImages;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            BeginDialogue();
        }

        if (!DialogueManager.GetInstance().dialogueRunning)
        {
            NPCImages.gameObject.SetActive(false);
        }
        else
        {
            NPCImages.gameObject.SetActive(true);
        }
    }

    private void BeginDialogue()
    {
        DialogueManager.GetInstance().NPCDialogueAnimator = NPCImages;

        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }
}
