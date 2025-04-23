using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    //public SpriteRenderer Reference;
    public Renderer[] TargetSprite;
    public int Difference;
    private int[] originalLayer;

    private void Start()
    {
        if (TargetSprite != null && TargetSprite.Length > 0)
        {
            originalLayer = new int[TargetSprite.Length];
            for (int i = 0; i < TargetSprite.Length; i++)
            {
                if (TargetSprite[i] != null)
                {
                    originalLayer[i] = TargetSprite[i].sortingOrder;
                }
            }
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
        if (TargetSprite != null)
        {
            foreach (var sprite in TargetSprite)
            {
                if (sprite != null)
                {
                    sprite.sortingOrder += Difference;
                }
            }
        }
    }

    private void ResetLayer()
    {
        if (TargetSprite != null && originalLayer != null)
        {
            for (int i = 0; i < TargetSprite.Length; i++)
            {
                if (TargetSprite[i] != null)
                {
                    TargetSprite[i].sortingOrder = originalLayer[i];
                }
            }
        }
    }
}
