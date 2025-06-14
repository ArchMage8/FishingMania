using UnityEngine;

public class Inventory_BookManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pages; // Array of page GameObjects
    public GameObject BG_Effect;

    private GameObject HUD;

    private int currentPageIndex = 0;
    private bool hasPlayedEntryAnimation = false; // Tracks if first page entry animation has been played

    private void Start()
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }
    }

    public void OpenBook()
    {
        if (InventoryManager.Instance.SomeUIEnabled)
        {
            return;
        }

        HUD = InventoryManager.Instance.HUD;
        BG_Effect.SetActive(true);
        HUD.SetActive(false);
        InventoryManager.Instance.SomeUIEnabled = true;
        
        foreach (var page in pages)
        {
            page.SetActive(false);
        }

        currentPageIndex = 0;
        pages[0].SetActive(true);

        if (!hasPlayedEntryAnimation)
        {
            Animator pageAnimator = pages[0].GetComponent<Animator>();
            if (pageAnimator != null)
            {
                pageAnimator.SetTrigger("Entry"); // Play entry animation only once
            }
            hasPlayedEntryAnimation = true;
        }

    }

    public void ChangePage(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= pages.Length)
        {
            Debug.LogWarning("Invalid page index: " + targetIndex);
            return;
        }

        pages[currentPageIndex].SetActive(false);
        pages[targetIndex].SetActive(true);

        currentPageIndex = targetIndex;
    }

    public void CloseBook()
    {
        Animator pageAnimator = pages[currentPageIndex].GetComponent<Animator>();
        if (pageAnimator != null)
        {
            pageAnimator.SetTrigger("Exit"); 
        }

        hasPlayedEntryAnimation = false;
        float exitAnimDuration = GetAnimationClipLength(pageAnimator, "Exit"); 
        StartCoroutine(DeactivatePageAfterDelay(exitAnimDuration, currentPageIndex));
    }

    private System.Collections.IEnumerator DeactivatePageAfterDelay(float delay, int pageIndex)
    {
        yield return new WaitForSeconds(delay);
        BG_Effect.SetActive(false);
        pages[pageIndex].SetActive(false);
        InventoryManager.Instance.SomeUIEnabled = false;
        HUD.SetActive(true);
    }

    private float GetAnimationClipLength(Animator animator, string clipName)
    {
        if (animator == null) return 0f;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        
        return 0f; // Default fallback
    }
}
