using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParticles : MonoBehaviour
{
    public ParticleSystem particleSystem ; // ������ �� ���� ������� ������
    private ParticleSystem.Particle[] particles; // ������ ��� �������� ������

    void Start()
    {
        // ������������� ������� ������
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }

    void Update()
    {
        // �������� ���������� �������� ������
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
