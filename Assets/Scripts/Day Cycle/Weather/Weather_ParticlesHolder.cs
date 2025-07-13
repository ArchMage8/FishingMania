using System.Collections.Generic;
using UnityEngine;

public class Weather_ParticlesHolder : MonoBehaviour
{
    // Static instance for singleton
    public static Weather_ParticlesHolder Instance { get; private set; }

    public List<ParticleSystem> sunnyParticles = new List<ParticleSystem>();
    public List<ParticleSystem> rainSplashes = new List<ParticleSystem>();

    private void Awake()
    {
        // If instance already exists and it's not this, destroy this game object
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set instance to this and optionally make it persistent
        Instance = this;

        // If you want this object to persist across scenes, uncomment this line:
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (Weather_Handler.Instance != null)
        {
            Weather_Handler.Instance.RegisterSceneParticles(this);
        }
    }
}
