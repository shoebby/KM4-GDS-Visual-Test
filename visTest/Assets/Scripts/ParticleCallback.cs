using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallback : MonoBehaviour
{
    ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particles.time > particles.main.duration - 0.01f)
        {
            particles.Pause();
        }
    }

}