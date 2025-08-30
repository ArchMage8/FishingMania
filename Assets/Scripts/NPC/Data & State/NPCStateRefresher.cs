using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NPCStateRefresher : MonoBehaviour
{
    public static NPCStateRefresher Instance { get; private set; }

    private List<StateManager> npcStateManagers = new List<StateManager>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep alive across scenes
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
        // Optional: clear list on each scene load to ensure only active NPCs are tracked
        npcStateManagers.Clear();
    }

    public void Register(StateManager manager)
    {
        if (manager != null && !npcStateManagers.Contains(manager))
        {
            npcStateManagers.Add(manager);
        }
    }

    public void Unregister(StateManager manager)
    {
        if (manager != null && npcStateManagers.Contains(manager))
        {
            npcStateManagers.Remove(manager);
        }
    }

    public void RefreshAllNPCStates()
    {
        // Remove any destroyed or missing references
        npcStateManagers.RemoveAll(m => m == null);

        foreach (var manager in npcStateManagers)
        {
            if (manager != null)
            {
                NPCManager.Instance.ModifyIsFullState(manager.npcDataSO.npcName, false);
                manager.NewDayAdjust();
            }
        }
    }
}
