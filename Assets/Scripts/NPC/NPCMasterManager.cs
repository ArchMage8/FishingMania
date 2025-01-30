using System.Collections.Generic;
using UnityEngine;

public class NPCMasterManager : MonoBehaviour
{
    public static NPCMasterManager Instance { get; private set; }

    private List<NPC_Manager> npcManagers = new List<NPC_Manager>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
        FindAllNPCManagers();
    }

    /// <summary>
    /// Finds all NPC_Manager scripts in the scene and stores them.
    /// </summary>
    private void FindAllNPCManagers()
    {
        npcManagers.Clear();

        Debug.Log("Searching for NPCs");
        
        NPC_Manager[] foundNPCs = FindObjectsOfType<NPC_Manager>();

        foreach (var npc in foundNPCs)
        {
            npcManagers.Add(npc);
        }

        
    }

    /// <summary>
    /// Calls AdjustVersion() on all NPC_Manager instances in the scene.
    /// Ensures that all NPCs are displaying the correct version and mode.
    /// </summary>
    public void AdjustAllVersions()
    {
        NPCDataManager.Instance.LoadNPCData();

        Debug.Log("Adjust All");

        foreach (var npc in npcManagers)
        {
            npc.LoadNPCData();
            npc.AdjustVersion();
        }
    }

    /// <summary>
    /// Refreshes the NPC list when a scene changes and updates NPC versions.
    /// </summary>
    public void OnSceneChange()
    {
        FindAllNPCManagers();
        AdjustAllVersions();
    }
}
