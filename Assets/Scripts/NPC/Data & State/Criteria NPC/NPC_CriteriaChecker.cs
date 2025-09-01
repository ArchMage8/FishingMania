using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NPC_CriteriaChecker : MonoBehaviour
{
    public static NPC_CriteriaChecker Instance { get; private set; }

    public List<ConditionalNPC> conditionalNPCs = new List<ConditionalNPC>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persistent across scenes
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Clear list each time a new scene is loaded
        conditionalNPCs.Clear();
    }

    public void Register(ConditionalNPC npc)
    {
        if (npc != null && !conditionalNPCs.Contains(npc))
        {
            conditionalNPCs.Add(npc);
        }
    }

    public void Unregister(ConditionalNPC npc)
    {
        if (npc != null && conditionalNPCs.Contains(npc))
        {
            conditionalNPCs.Remove(npc);
        }
    }

    public void RunChecksOnAll()
    {
        // Remove null references (destroyed/disabled objects)
        conditionalNPCs.RemoveAll(n => n == null);

        foreach (var npc in conditionalNPCs)
        {
            npc.RunCheck();
        }
    }
}
