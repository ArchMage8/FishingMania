using UnityEngine;
using System.Collections;

public class RestrictArea_Dialogue : MonoBehaviour
{

    //The script is meant to handle a situation where if the player enters an area that is restricted
    //They get teleported + some dialogue appears

    [Header("Delay")]
    public float stayDuration = 0.5f;
    public float cooldownDuration = 10f;

    [Space(10)]

    public Animator fadeObject;
    public Vector2 teleportCoordinates;

    [Space(10)]

    public DialogueTrigger trigger;

    private bool playerInside = false;
    private bool onCooldown = false;
    private Coroutine countdownCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (onCooldown)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            playerInside = true;
            countdownCoroutine = StartCoroutine(StayCountdown(other.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (countdownCoroutine != null)
                StopCoroutine(countdownCoroutine);
        }
    }

    private IEnumerator StayCountdown(GameObject player)
    {
        float timer = 0f;
        while (timer < stayDuration)
        {
            if (!playerInside)
                yield break;
            timer += Time.deltaTime;
            yield return null;
        }
        TriggerTeleport(player);
    }

    private void TriggerTeleport(GameObject player)
    {
      
        fadeObject.gameObject.SetActive(true);

        player.transform.position = teleportCoordinates;

        StartCoroutine(EnableDialogue());
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator EnableDialogue()
    {
        yield return new WaitForSecondsRealtime(1f);
        fadeObject.SetTrigger("Exit");

        fadeObject.gameObject.SetActive(false);

        TextAsset NPC_Dialogue = trigger.NPC_Dialogue;
        Animator LocalDialogueAnimator = trigger.LocalDialogueAnimator;

        DialogueManager.GetInstance().EnterDialogueMode_Default(NPC_Dialogue, LocalDialogueAnimator);

    }

    
    private IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        yield return new WaitForSecondsRealtime(cooldownDuration);
        onCooldown = false;
    }
}
