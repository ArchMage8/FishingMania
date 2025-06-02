using UnityEngine;

public class Inventory_BaitTile : MonoBehaviour
{
    public int BaitClass;
    public Item BaitItem;
    private bool hasBait;
    private Sprite originalSprite;
    public Sprite Active_Effect;

    private void OnEnable()
    {
        int quantity = InventoryManager.Instance.GetTotalQuantity(BaitItem);
        hasBait = quantity > 0;

        originalSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public bool HasBait()
    {
        return hasBait;
    }

    public void EnableEffect()
    {
        GetComponent<SpriteRenderer>().sprite = Active_Effect;
    }

    public void DisableEffect()
    {
        GetComponent<SpriteRenderer>().sprite = originalSprite;
    }
}
