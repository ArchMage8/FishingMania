using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    //Documentation:
    //This script works with the NPC_SortingHandler
    //This side handles what happens if the player is within in range of the object that requires
    //a sorting layer adjustment

    //But this may cause the layer's to break should an NPC move in range of the object being adjusted
    //So the NPC sorting layer's job is to adjust the NPC's layer


    //public SpriteRenderer Reference;
    public Renderer[] TargetSprite;
    public int Difference;
    private int[] originalLayer;

    public NPC_SortingHandler npcSortingHandler; // Assign if needed

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
