using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] Pages;

    private void OnEnable()
    {
        Pages[0].SetActive(true);
    }

    public void PageNavigate(int pageIndex)
    {
        // Disable all pages
        foreach (GameObject page in Pages)
        {
            if (page != null)
                page.SetActive(false);
        }

        // Enable the selected page if the index is valid
        if (pageIndex >= 0 && pageIndex < Pages.Length && Pages[pageIndex] != null)
        {
            Pages[pageIndex].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Invalid page index: " + pageIndex);
        }
    }
}
