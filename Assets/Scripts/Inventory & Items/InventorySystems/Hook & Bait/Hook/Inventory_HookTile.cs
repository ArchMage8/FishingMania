using UnityEngine;

public class Inventory_HookTile : MonoBehaviour
{
    public int HookClass;
    private bool isUnlocked;
    private Sprite originalSprite;
    public Sprite Active_Effect;

    private void OnEnable()
    {
        isUnlocked = Inventory_HookManager.Instance.GetHookStatus(HookClass);
        originalSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
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
