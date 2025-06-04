using UnityEngine;
using UnityEngine.UI;

public class Inventory_BookManager : MonoBehaviour
{
    [Header("Book Pages")]
    [SerializeField] private GameObject[] pages; // Array of page GameObjects

    [Header("Book Root")]
    [SerializeField] private GameObject bookUI; // Parent UI GameObject for the book

    private int currentPageIndex = 0;
    private bool hasPlayedEntryAnimation = false; // Tracks if first page entry animation has been played

    public void OpenBook()
    {
        bookUI.SetActive(true);

        foreach (var page in pages)
            page.SetActive(false);

        currentPageIndex = 0;
        pages[0].SetActive(true);

        if (!hasPlayedEntryAnimation)
        {
            Animator pageAnimator = pages[0].GetComponent<Animator>();
            if (pageAnimator != null)
            {
                pageAnimator.SetTrigger("Entry"); // Assuming trigger is called "Entry"
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
        pages[currentPageIndex].SetActive(false);
        bookUI.SetActive(false);
    }
}
