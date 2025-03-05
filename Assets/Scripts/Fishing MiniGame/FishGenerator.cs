using UnityEngine;
using System.Collections;

public class FishGenerator : MonoBehaviour
{
    [Header("Bait Class Fish Pools")]
    public Item[] BaitClass_1 = new Item[5];
    public Item[] BaitClass_2 = new Item[5];
    public Item[] BaitClass_3 = new Item[5];
    public Item[] BaitClass_4 = new Item[5];
    public Item[] BaitClass_5 = new Item[5];

    [Header("Fishing Attributes")]
    public int BaitClass;
    public int HookClass;
    public string FishingSpotName;

    public void SelectFish()
    {
        // Get HookClass and BaitClass from BaitAndHookManager
        BaitClass = BaitAndHookManager.Instance.activeBait.baitClass;
        HookClass = BaitAndHookManager.Instance.activeHook.hookClass;

        int[] probabilities;
        switch (HookClass)
        {
            case 1:
                probabilities = new int[] { 70, 20, 8, 2, 0 };
                break;
            case 2:
                probabilities = new int[] { 25, 60, 10, 5, 0 };
                break;
            case 3:
                probabilities = new int[] { 15, 25, 50, 7, 3 };
                break;
            case 4:
                probabilities = new int[] { 15, 15, 20, 40, 10 };
                break;
            case 5:
                probabilities = new int[] { 10, 15, 20, 25, 30 };
                break;
            default:
                probabilities = new int[] { 70, 20, 8, 2, 0 };
                break;
        }

        int rand = Random.Range(0, 100);
        int cumulative = 0;
        int selectedIndex = 0;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulative += probabilities[i];
            if (rand < cumulative)
            {
                selectedIndex = i;
                break;
            }
        }

        Item selectedFish = null;
        switch (BaitClass)
        {
            case 1:
                selectedFish = BaitClass_1[selectedIndex];
                break;
            case 2:
                selectedFish = BaitClass_2[selectedIndex];
                break;
            case 3:
                selectedFish = BaitClass_3[selectedIndex];
                break;
            case 4:
                selectedFish = BaitClass_4[selectedIndex];
                break;
            case 5:
                selectedFish = BaitClass_5[selectedIndex];
                break;
            default:
                selectedFish = BaitClass_1[selectedIndex];
                break;
        }

        StartCoroutine(CatchFishUI(selectedFish));
        InventoryManager.Instance.AddItem(selectedFish, 1);
        DeductStock();
    }

    private void DeductStock()
    {
        FishingCoreSystems.instance.DeductStock(FishingSpotName);
    }

    private IEnumerator CatchFishUI(Item fish)
    {
        // Placeholder for UI logic when catching fish
        yield return null;
    }
}
