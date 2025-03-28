using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    public SpriteRenderer Reference;
    public Renderer TargetSprite;
    public int Difference;
    private int originalLayer;

    private void Start()
    {
        if (TargetSprite != null)
        {
            originalLayer = TargetSprite.sortingOrder;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AdjustLayer();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetLayer();
        }
    }

    private void AdjustLayer()
    {
        if (Reference != null && TargetSprite != null)
        {
            TargetSprite.sortingOrder = Reference.sortingOrder + Difference;
        }
    }

    private void ResetLayer()
    {
        if (TargetSprite != null)
        {
            TargetSprite.sortingOrder = originalLayer;
        }
    }
}
