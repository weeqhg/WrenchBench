using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParticles : MonoBehaviour
{
    public ParticleSystem particleSystem ; // Ссылка на вашу систему частиц
    private ParticleSystem.Particle[] particles; // Массив для хранения частиц

    void Start()
    {
        // Инициализация массива частиц
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }

    void Update()
    {
        // Получаем количество активных частиц
        int particleCount = particleSystem.GetParticles(particles);

        if (particleCount > 0)
        {
            Vector3 averagePosition = Vector3.zero;

            for (int i = 0; i < particleCount; i++)
            {
                averagePosition += particles[i].position;
            }

            averagePosition /= particleCount; 

            transform.position = averagePosition;
        }
    }
}
