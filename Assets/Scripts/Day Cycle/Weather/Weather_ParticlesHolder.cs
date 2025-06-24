using System.Collections.Generic;
using UnityEngine;

public class Weather_ParticlesHolder : MonoBehaviour
{
    public List<ParticleSystem> sunnyParticles = new List<ParticleSystem>();

    public List<ParticleSystem> rainParticles = new List<ParticleSystem>();

    private void Start()
    {
        if (Weather_Handler.Instance != null)
        {
            Weather_Handler.Instance.RegisterSceneParticles(this);
        }
    }
}
