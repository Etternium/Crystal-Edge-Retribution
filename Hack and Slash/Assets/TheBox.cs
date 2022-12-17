using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TheBox : MonoBehaviour
{
    public ParticleSystem[] particles;
    public Combat player;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeDamage()
    {
        foreach(ParticleSystem system in particles)
        {
            system.Play();
        }
        transform.DOShakeScale(0.1f, player.shakeStrength);
    }
}
